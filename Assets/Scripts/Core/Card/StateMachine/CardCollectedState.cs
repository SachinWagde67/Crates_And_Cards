using UnityEngine;

public class CardCollectedState : ICardState {

    private Card card;

    public CardCollectedState(Card card) {
        this.card = card;
    }

    public void Enter() {

        GameEvents.OnCardCollected?.Invoke(card);
    }

    public void Exit() { }
}