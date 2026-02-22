using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class GeneratorCrate : MonoBehaviour, ITappable, IPool {

    [Header("Generator Settings")]
    [SerializeField] private CardColor cardToGenerateColor;
    [SerializeField] private CardColor crateToGenerateColor;
    [SerializeField] private MeshRenderer crateRenderer;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 0.15f;
    [SerializeField] private float releaseDelay = 0.15f;

    private List<Card> storedCards = new List<Card>();
    private bool isTapped;
    private bool isSpawningCards;
    private bool isReleasing;
    private Transform jumpTargetPoint;
    private int totalSpawnPoints;
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

        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);

        StartCoroutine(SpawnCardsCoroutine());
    }

    public void OnSpawned() {

        jumpTargetPoint = ConveyorManager.Instance.JumpTargetPoint;

        if(spawnPoints != null) {
            totalSpawnPoints = spawnPoints.Count;
        }
    }

    public void OnDespawned() {

        StopAllCoroutines();
        storedCards.Clear();
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
    }

    private IEnumerator ReleaseCardsCoroutine() {

        isReleasing = true;

        while(storedCards.Count > 0) {

            Card card = storedCards[storedCards.Count - 1];
            storedCards.RemoveAt(storedCards.Count - 1);

            card.transform.SetParent(null);

            card.ChangeToJumping(jumpTargetPoint.position, () => {
                card.ChangeToMoving();
            });

            yield return new WaitForSeconds(releaseDelay);
        }

        isReleasing = false;

        if(storedCards.Count == 0) {
            GameEvents.OnGeneratorEmpty?.Invoke(this);
        }
    }

    public IEnumerator ScaleDownAndReturn() {

        yield return transform.DOScale(0f, 0.3f).WaitForCompletion();

        PoolManager.Instance.ReturnGenerator(this);
    }
}