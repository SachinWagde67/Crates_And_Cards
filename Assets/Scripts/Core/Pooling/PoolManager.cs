using UnityEngine;

public class PoolManager : MonoBehaviour {

    public static PoolManager Instance { get; private set; }

    [Header("Pool Config")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform cardPoolParent;
    [SerializeField] private int cardInitialSize = 30;

    [Header("Generator Config")]
    [SerializeField] private GeneratorCrate generatorPrefab;
    [SerializeField] private Transform generatorPoolParent;
    [SerializeField] private int generatorInitialSize = 5;

    [Header("Catcher Config")]
    [SerializeField] private CatcherCrate catcherPrefab;
    [SerializeField] private Transform catcherPoolParent;
    [SerializeField] private int catcherInitialSize = 5;

    [Header("Audio Source Config")]
    [SerializeField] private AudioSourcePool audioPrefab;
    [SerializeField] private Transform audioParent;
    [SerializeField] private int audioInitialSize = 10;

    private ObjectPool<Card> cardPool;
    private ObjectPool<GeneratorCrate> generatorPool;
    private ObjectPool<CatcherCrate> catcherPool;
    private ObjectPool<AudioSourcePool> audioPool;

    private void Awake() {

        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializePools();
    }

    private void InitializePools() {

        cardPool = new ObjectPool<Card>(cardPrefab, cardInitialSize, cardPoolParent);
        generatorPool = new ObjectPool<GeneratorCrate>(generatorPrefab, generatorInitialSize, generatorPoolParent);
        catcherPool = new ObjectPool<CatcherCrate>(catcherPrefab, catcherInitialSize, catcherPoolParent);
        audioPool = new ObjectPool<AudioSourcePool>(audioPrefab, audioInitialSize, audioParent);
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

    public CatcherCrate GetCatcher() {
        return catcherPool.Get();
    }

    public void ReturnCatcher(CatcherCrate catcher) {
        catcherPool.ReturnToPool(catcher);
    }

    public AudioSourcePool GetAudio() {
        return audioPool.Get();
    }

    public void ReturnAudio(AudioSourcePool audio) {
        audioPool.ReturnToPool(audio);
    }
}