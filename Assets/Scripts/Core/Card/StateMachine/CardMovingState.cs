using UnityEngine;
using Dreamteck.Splines;

public class CardMovingState : ICardState {

    private Card card;
    private SplineFollower splineFollower;

    public CardMovingState(Card card) {
        this.card = card;
        splineFollower = card.GetComponent<SplineFollower>();
    }

    public void Enter() {
        splineFollower.enabled = true;
    }

    public void Exit() {
        splineFollower.enabled = false;
    }
}