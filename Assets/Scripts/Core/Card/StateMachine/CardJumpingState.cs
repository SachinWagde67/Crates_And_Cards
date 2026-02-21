using UnityEngine;
using DG.Tweening;

public class CardJumpingState : ICardState {

    private Card card;
    private Vector3 targetPosition;

    public CardJumpingState(Card card) {
        this.card = card;
    }

    public void SetTarget(Vector3 target) {
        targetPosition = target;
    }

    public void Enter() {

        Sequence cardSequence = card.transform.DOJump(targetPosition, 1.5f, 1, 0.4f).SetEase(Ease.OutQuad);

        cardSequence.OnComplete(() => {
            card.ChangeToCollected();
        });
    }

    public void Exit() {
        DOTween.Kill(card.transform);
    }
}