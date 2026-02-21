using UnityEngine;

public class CardStateMachine {

    private ICardState currentState;

    public void ChangeState(ICardState newState) {

        if(currentState == newState) {
            return;
        }

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}