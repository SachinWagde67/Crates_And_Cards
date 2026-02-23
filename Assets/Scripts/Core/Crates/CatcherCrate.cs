using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class CatcherCrate : MonoBehaviour, IPool {

    [SerializeField] private Transform root;
    [SerializeField] private MeshRenderer crateRenderer;
    [SerializeField] private float jumpPower = 0.5f;
    [SerializeField] private float upOffset = 0.5f;
    [SerializeField] private float scaleMagnitude = 1.1f;

    [Header("Catch Settings")]
    [SerializeField] private CardColor targetColor;

    [Header("Catch Points")]
    [SerializeField] private List<Transform> catchPoints;

    private int currentCatchIndex = 0;
    private bool isFull = false;
    private bool isDespawning = false;
    private CatcherSlot ownerSlot;
    private Vector3 originalScale;
    private MaterialPropertyBlock propertyBlock;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake() {

        originalScale = root.localScale;
        propertyBlock = new MaterialPropertyBlock();
    }

    public void Initialize(CardColor color, CatcherSlot slot) {

        targetColor = color;
        ownerSlot = slot;
        ApplyColor(color);
    }

    public void ApplyColor(CardColor colorType) {

        Color color = ColorManager.Instance.GetColor(colorType);

        crateRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColor, color);
        crateRenderer.SetPropertyBlock(propertyBlock);
    }

    public void OnSpawned() {

        currentCatchIndex = 0;
        isFull = false;
        isDespawning = false;

        root.localScale = Vector3.zero;

        root.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack).OnComplete(() => {
            ownerSlot.EnableTrigger();
        });
    }

    public void OnDespawned() {

        root.localScale = originalScale;
        transform.localScale = Vector3.one;
        DOTween.Kill(transform);
        DOTween.Kill(root);
    }

    public void TryCatchCard(Card card) {

        if(isFull || isDespawning) {
            Debug.Log($"is full or is despawning");
            return;
        }

        if(card.Color != targetColor) {
            Debug.Log($"target color not match");
            return;
        }

        if(currentCatchIndex >= catchPoints.Count) {
            Debug.Log($"catch index greater");
            return;
        }

        Transform targetPoint = catchPoints[currentCatchIndex];
        int catchIndex = currentCatchIndex;

        currentCatchIndex++;

        card.ChangeToJumping(targetPoint.position, targetPoint.rotation, () => {

            card.transform.SetParent(targetPoint);
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;

            card.ChangeToCollected();

            if(catchIndex == catchPoints.Count - 1) {

                isFull = true;
                PlayFullAnimation();
            }
        });
    }

    private void PlayFullAnimation() {

        isDespawning = true;

        ownerSlot.DisableTrigger();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOJump(transform.position + Vector3.up * upOffset, jumpPower, 1, 0.5f));

        sequence.Join(root.DOScale(originalScale * scaleMagnitude, 0.3f));

        sequence.Append(root.DOScale(0f, 0.2f));

        sequence.OnComplete(() => {

            foreach(Transform catchPoint in catchPoints) {

                if(catchPoint.childCount > 0) {

                    Card card = catchPoint.GetComponentInChildren<Card>();

                    if(card != null) {
                        PoolManager.Instance.ReturnCard(card);
                    }
                }
            }

            ownerSlot.NotifyCrateCompleted();
            PoolManager.Instance.ReturnCatcher(this);
        });
    }
}