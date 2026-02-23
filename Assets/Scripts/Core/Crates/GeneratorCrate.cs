using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class GeneratorCrate : MonoBehaviour, ITappable, IPool {

    [Header("Generator Settings")]
    [SerializeField] private CardColor cardToGenerateColor;
    [SerializeField] private CardColor crateToGenerateColor;
    [SerializeField] private MeshRenderer crateRenderer;
    [SerializeField] private Collider crateCollider;
    [SerializeField] private float backwardOffsetZ = -1f;
    [SerializeField] private float forwardMoveDuration = 0.4f;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 0.15f;
    [SerializeField] private float releaseDelay = 0.15f;

    private bool isTapped;
    private bool isSpawningCards;
    private bool isReleasing;
    private Transform jumpTargetPoint;
    private int totalSpawnPoints;
    private List<Card> storedCards = new List<Card>();
    private MaterialPropertyBlock propertyBlock;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake() {

        propertyBlock = new MaterialPropertyBlock();
    }

    public void InitializeCrate(CrateConfig config) {

        isTapped = false;

        cardToGenerateColor = config.cardToGenerateColor;
        crateToGenerateColor = config.crateToGenerateColor;

        ApplyColor(crateToGenerateColor);

        Vector3 finalLocalPos = Vector3.zero;
        Vector3 startLocalPos = new Vector3(0f, 0f, backwardOffsetZ);

        transform.localPosition = startLocalPos;
        crateCollider.enabled = false;

        Sequence spawnSequence = DOTween.Sequence();

        spawnSequence.Append(transform.DOLocalMove(finalLocalPos, forwardMoveDuration).SetEase(Ease.OutBack));

        SoundManager.Instance.PlayOneShot(SoundType.GeneratorSpawn);

        spawnSequence.AppendCallback(() => {
            StartCoroutine(SpawnCardsCoroutine());
        });
    }

    public void OnSpawned() {

        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;

        jumpTargetPoint = ConveyorManager.Instance.JumpTargetPoint;

        if(spawnPoints != null) {
            totalSpawnPoints = spawnPoints.Count;
        }
    }

    public void OnDespawned() {

        StopAllCoroutines();
        storedCards.Clear();
        DOTween.Kill(transform);
    }

    public void ApplyColor(CardColor colorType) {

        Color color = ColorManager.Instance.GetColor(colorType);

        crateRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColor, color);
        crateRenderer.SetPropertyBlock(propertyBlock);
    }

    public void OnTap() {

        if(isSpawningCards || isReleasing || isTapped) {
            return;
        }

        isTapped = true;

        GameEvents.OnGeneratorTapped?.Invoke(this);
        SoundManager.Instance.PlayOneShot(SoundType.GeneratorTap);
        StartCoroutine(ReleaseCardsCoroutine());
    }

    private IEnumerator SpawnCardsCoroutine() {

        isSpawningCards = true;

        storedCards.Clear();

        for(int i = 0; i < totalSpawnPoints; i++) {

            Card card = PoolManager.Instance.GetCard();

            card.Initialize(cardToGenerateColor);

            card.transform.SetParent(spawnPoints[i]);

            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;

            storedCards.Add(card);

            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawningCards = false;
        crateCollider.enabled = true;
    }

    private IEnumerator ReleaseCardsCoroutine() {

        isReleasing = true;

        while(storedCards.Count > 0) {

            Card card = storedCards[0];
            storedCards.RemoveAt(0);

            card.transform.SetParent(null);

            card.ChangeToJumping(jumpTargetPoint.position, jumpTargetPoint.rotation, () => {
                card.ChangeToMoving();
            });

            yield return new WaitForSeconds(releaseDelay);
        }

        isReleasing = false;

        GameEvents.OnGeneratorEmpty?.Invoke(this);
        SoundManager.Instance.PlayOneShot(SoundType.GeneratorEmpty);
    }

    public IEnumerator ScaleDownAndReturn() {

        yield return transform.DOScale(0f, 0.3f).WaitForCompletion();

        PoolManager.Instance.ReturnGenerator(this);
    }
}