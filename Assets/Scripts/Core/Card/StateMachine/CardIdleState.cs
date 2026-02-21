using UnityEngine;

public class CardIdleState : ICardState {

    private Card card;

    public CardIdleState(Card card) {
        this.card = card;
    }

    public void Enter() {
    }

    public void Exit() {
    }
}