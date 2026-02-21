using UnityEngine;

public class PoolManager : MonoBehaviour {

    public static PoolManager Instance { get; private set; }

    [Header("Pool Config")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private int cardInitialSize = 30;

    private ObjectPool<Card> cardPool;

    private void Awake() {

        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializePools();
    }

    private void InitializePools() {

        cardPool = new ObjectPool<Card>(cardPrefab, cardInitialSize, transform);
    }

    public Card GetCard() {

        return cardPool.Get();
    }

    public void ReturnCard(Card card) {

        cardPool.ReturnToPool(card);
    }
}