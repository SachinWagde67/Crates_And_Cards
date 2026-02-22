using UnityEngine;
using DG.Tweening;
using System;

public class CardJumpingState : ICardState {

    private Card card;
    private Vector3 targetPosition;
    private Action onCompleteAction;

    public CardJumpingState(Card card) {
        this.card = card;
    }

    public void SetTarget(Vector3 target, Action onComplete = null) {

        targetPosition = target;
        onCompleteAction = onComplete;
    }

    public void Enter() {

        Sequence cardSequence = card.transform.DOJump(targetPosition, 1.5f, 1, 0.4f).SetEase(Ease.OutQuad);

        cardSequence.OnComplete(() => {
            onCompleteAction?.Invoke();
        });
    }

    public void Exit() {
        DOTween.Kill(card.transform);
    }
}