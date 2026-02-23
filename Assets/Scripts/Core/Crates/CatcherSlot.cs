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
            Debug.Log($"Trigger Not enabled");
            return;
        }

        if(activeCrate == null) {
            Debug.Log($"Active crate is null");
            return;
        }

        Card card = other.GetComponentInParent<Card>();

        if(card == null) {
            Debug.Log($"Card is null");
            return;
        }

        Debug.Log($"TryCatchCard");
        activeCrate.TryCatchCard(card);
    }

    public void NotifyCrateCompleted() {

        DisableTrigger();
        activeCrate = null;

        CatcherCrateManager.Instance.RequestRespawn(this);
    }
}