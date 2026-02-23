using UnityEngine;

public class CatcherSlot : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private Transform spawnPoint;

    private CatcherCrate activeCrate;

    public void SpawnCrate(CardColor color) {

        activeCrate = PoolManager.Instance.GetCatcher();

        activeCrate.transform.SetParent(spawnPoint);
        activeCrate.transform.localPosition = Vector3.zero;
        activeCrate.transform.localRotation = Quaternion.identity;

        activeCrate.Initialize(color, this);

        triggerCollider.enabled = false;
    }

    public void EnableTrigger() {
        triggerCollider.enabled = true;
    }

    public void DisableTrigger() {
        triggerCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {

        if(!triggerCollider.enabled) {
            return;
        }

        if(activeCrate == null) {
            return;
        }

        Card card = other.GetComponentInParent<Card>();

        if(card == null) {
            return;
        }

        activeCrate.TryCatchCard(card);
    }

    public void NotifyCrateCompleted() {

        DisableTrigger();
        activeCrate = null;

        CatcherCrateManager.Instance.RequestRespawn(this);
    }
}