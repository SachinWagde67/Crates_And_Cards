using UnityEngine;
using Dreamteck.Splines;

public class ConveyorManager : MonoBehaviour {

    public static ConveyorManager Instance { get; private set; }

    [SerializeField] private SplineComputer conveyorSpline;
    [SerializeField] private float conveyorSpeed = 4f;

    private void Awake() {

        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable() {
        GameEvents.OnCardEnteredConveyor += HandleCardEnteredConveyor;
    }

    private void OnDisable() {
        GameEvents.OnCardEnteredConveyor -= HandleCardEnteredConveyor;
    }

    private void HandleCardEnteredConveyor(Card card) {
        ConfigureCardFollower(card);
    }

    private void ConfigureCardFollower(Card card) {

        SplineFollower follower = card.GetComponent<SplineFollower>();

        follower.spline = conveyorSpline;
        follower.followSpeed = conveyorSpeed;
        follower.wrapMode = SplineFollower.Wrap.Loop;
        follower.autoStartPosition = true;

        follower.Restart();
    }
}