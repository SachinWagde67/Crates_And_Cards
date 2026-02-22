using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour, IPool {

    [SerializeField] private CardColor cardColor;
    [SerializeField] private Transform root;
    [SerializeField] private MeshRenderer meshRenderer;

    private CardStateMachine stateMachine;

    private ICardState idleState;
    private ICardState jumpingState;
    private ICardState movingState;
    private ICardState collectedState;
    private Vector3 originalScale;
    private MaterialPropertyBlock propertyBlock;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    public CardColor Color => cardColor;

    private void Awake() {

        originalScale = root.localScale;
        propertyBlock = new MaterialPropertyBlock();

        stateMachine = new CardStateMachine();
        idleState = new CardIdleState(this);
        jumpingState = new CardJumpingState(this);
        movingState = new CardMovingState(this);
        collectedState = new CardCollectedState(this);
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

        root.localScale = Vector3.zero;

        root.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack);

        GameEvents.OnCardSpawned?.Invoke(this);

        ChangeToIdle();
    }

    public void OnDespawned() {

        DOTween.Kill(transform);
        ChangeToIdle();
    }

    public void ChangeToIdle() {
        stateMachine.ChangeState(idleState);
    }

    public void ChangeToMoving() {

        stateMachine.ChangeState(movingState);
        GameEvents.OnCardEnteredConveyor?.Invoke(this);
    }

    public void ChangeToJumping(Vector3 targetPosition, System.Action onComplete = null) {

        ((CardJumpingState)jumpingState).SetTarget(targetPosition, onComplete);
        stateMachine.ChangeState(jumpingState);
    }

    public void ChangeToCollected() {
        stateMachine.ChangeState(collectedState);
    }
}