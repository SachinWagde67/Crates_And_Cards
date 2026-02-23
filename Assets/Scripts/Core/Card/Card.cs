using UnityEngine;
using DG.Tweening;
using System;

public class Card : MonoBehaviour, IPool {

    [SerializeField] private CardColor cardColor;
    [SerializeField] private Transform root;
    [SerializeField] private MeshRenderer meshRenderer;

    private ICardState idleState;
    private ICardState jumpingState;
    private ICardState movingState;
    private ICardState collectedState;
    private ICardState currentState;

    private Vector3 originalScale;
    private MaterialPropertyBlock propertyBlock;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    public CardColor Color => cardColor;

    private void Awake() {

        originalScale = root.localScale;
        propertyBlock = new MaterialPropertyBlock();

        idleState = new CardIdleState(this);
        jumpingState = new CardJumpingState(this);
        movingState = new CardMovingState(this);
        collectedState = new CardCollectedState(this);
        currentState = idleState;
    }

    public void Initialize(CardColor color) {

        cardColor = color;
        ApplyColor(color);
    }

    public void ApplyColor(CardColor colorType) {

        Color color = ColorManager.Instance.GetColor(colorType);

        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColor, color);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void OnSpawned() {

        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;

        root.localScale = Vector3.zero;

        root.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack);

        GameEvents.OnCardSpawned?.Invoke(this);

        ChangeToIdle();
    }

    public void OnDespawned() {

        DOTween.Kill(transform);
        ChangeToIdle();
    }

    private void ChangeState(ICardState newState) {

        if(currentState == newState) {
            return;
        }

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void ChangeToIdle() {
        ChangeState(idleState);
    }

    public void ChangeToMoving() {

        ChangeState(movingState);
        GameEvents.OnCardEnteredConveyor?.Invoke(this);
    }

    public void ChangeToJumping(Vector3 targetPosition, Quaternion targetRotation, Action onComplete = null) {

        ((CardJumpingState)jumpingState).SetTarget(targetPosition, targetRotation, onComplete);
        ChangeState(jumpingState);
    }

    public void ChangeToCollected() {
        ChangeState(collectedState);
    }
}