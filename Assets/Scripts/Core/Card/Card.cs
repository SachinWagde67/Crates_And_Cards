using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour, IPool {

    [SerializeField] private CardColor cardColor;
    [SerializeField] private Transform cardTransform;

    private CardStateMachine stateMachine;

    private ICardState idleState;
    private ICardState jumpingState;
    private ICardState movingState;
    private ICardState collectedState;

    public CardColor Color => cardColor;

    private void Awake() {

        stateMachine = new CardStateMachine();

        idleState = new CardIdleState(this);
        jumpingState = new CardJumpingState(this);
        movingState = new CardMovingState(this);
        collectedState = new CardCollectedState(this);
    }

    public void OnSpawned() {

        base.transform.localScale = Vector3.zero;
        cardTransform.DOScale(1f, 0.25f).From(0f);

        GameEvents.OnCardSpawned?.Invoke(this);

        ChangeToIdle();
    }

    public void OnDespawned() {

        DOTween.Kill(cardTransform);
        ChangeToIdle();
    }

    public void ChangeToIdle() {
        stateMachine.ChangeState(idleState);
    }

    public void ChangeToMoving() {

        stateMachine.ChangeState(movingState);
        GameEvents.OnCardEnteredConveyor?.Invoke(this);
    }

    public void ChangeToJumping(Vector3 targetPosition) {

        ((CardJumpingState)jumpingState).SetTarget(targetPosition);
        stateMachine.ChangeState(jumpingState);
    }

    public void ChangeToCollected() {
        stateMachine.ChangeState(collectedState);
    }
}