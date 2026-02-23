using UnityEngine;
using DG.Tweening;
using System;

public class CardJumpingState : ICardState {

    private Card card;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Action onCompleteAction;
    private float duration = 0.4f;

    public CardJumpingState(Card card) {
        this.card = card;
    }

    public void SetTarget(Vector3 position, Quaternion rotation, Action onComplete = null) {

        targetPosition = position;
        targetRotation = rotation;
        onCompleteAction = onComplete;
    }

    public void Enter() {

        SoundManager.Instance.PlayOneShot(SoundType.CardJump);

        Sequence seq = DOTween.Sequence();

        seq.Join(card.transform.DOJump(targetPosition, 0.5f, 1, duration).SetEase(Ease.OutQuad));

        seq.Join(card.transform.DORotateQuaternion(targetRotation, duration));

        seq.OnComplete(() => {
            onCompleteAction?.Invoke();
        });
    }

    public void Exit() { }
}