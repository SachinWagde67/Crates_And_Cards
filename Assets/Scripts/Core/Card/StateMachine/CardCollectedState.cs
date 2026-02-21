using DG.Tweening;

public class CardCollectedState : ICardState {

    private Card card;

    public CardCollectedState(Card card) {
        this.card = card;
    }

    public void Enter() {

        GameEvents.OnCardCollected?.Invoke(card);

        card.transform.DOScale(0f, 0.2f).OnComplete(() => {
            PoolManager.Instance.ReturnCard(card);
        });
    }

    public void Exit() {

    }
}