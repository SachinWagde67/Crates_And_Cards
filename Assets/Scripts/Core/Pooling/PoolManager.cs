using UnityEngine;

public class PoolManager : MonoBehaviour {

    public static PoolManager Instance { get; private set; }

    [Header("Pool Config")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private int cardInitialSize = 30;

    [Header("Generator Config")]
    [SerializeField] private GeneratorCrate generatorPrefab;
    [SerializeField] private int generatorInitialSize = 5;

    private ObjectPool<Card> cardPool;
    private ObjectPool<GeneratorCrate> generatorPool;

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
        generatorPool = new ObjectPool<GeneratorCrate>(generatorPrefab, generatorInitialSize, transform);
    }

    public Card GetCard() {
        return cardPool.Get();
    }

    public void ReturnCard(Card card) {
        cardPool.ReturnToPool(card);
    }

    public GeneratorCrate GetGenerator() {
        return generatorPool.Get();
    }

    public void ReturnGenerator(GeneratorCrate generator) {
        generatorPool.ReturnToPool(generator);
    }
}