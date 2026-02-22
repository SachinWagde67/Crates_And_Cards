using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorCrate : MonoBehaviour, ITappable {

    [Header("Generator Settings")]
    [SerializeField] private CardColor cardToGenerateColor;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 0.15f;
    [SerializeField] private float releaseDelay = 0.15f;

    private List<Card> storedCards = new List<Card>();
    private bool isReleasing;
    private Transform jumpTargetPoint;
    private int totalSpawnPoints;

    private void Start() {

        jumpTargetPoint = ConveyorManager.Instance.JumpTargetPoint;

        if(spawnPoints != null) {
            totalSpawnPoints = spawnPoints.Count;
        }

        StartCoroutine(PreloadCards());
    }

    public void OnTap() {

        if(isReleasing || storedCards.Count == 0) {
            return;
        }

        GameEvents.OnGeneratorTapped?.Invoke(this);
        StartCoroutine(ReleaseCardsRoutine());
    }

    private IEnumerator PreloadCards() {

        for(int i = 0; i < totalSpawnPoints; i++) {

            Card card = PoolManager.Instance.GetCard();

            card.Initialize(cardToGenerateColor);

            card.transform.SetParent(spawnPoints[i]);

            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;

            storedCards.Add(card);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator ReleaseCardsRoutine() {

        isReleasing = true;

        while(storedCards.Count > 0) {

            Card card = storedCards[storedCards.Count - 1];
            storedCards.RemoveAt(storedCards.Count - 1);

            card.transform.SetParent(null);

            card.ChangeToJumping(jumpTargetPoint.position, () => {
                card.ChangeToMoving();
            });

            yield return new WaitForSeconds(releaseDelay);
        }

        isReleasing = false;
    }
}