using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherCrateManager : MonoBehaviour {

    public static CatcherCrateManager Instance { get; private set; }

    [SerializeField] private float respawnDelay = 0.5f;
    [SerializeField] private List<CatcherSlot> slots = new List<CatcherSlot>();
    [SerializeField] private List<CardColor> possibleColors = new List<CardColor>();

    private List<CardColor> colorsList = new List<CardColor>();

    private void Awake() {

        if(Instance != this && Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {

        ShuffleColors();

        foreach(CatcherSlot slot in slots) {
            SpawnAtSlot(slot);
        }
    }

    private void SpawnAtSlot(CatcherSlot slot) {

        if(colorsList.Count == 0) {
            ShuffleColors();
        }

        CardColor selectedColor = colorsList[0];
        colorsList.RemoveAt(0);

        slot.SpawnCrate(selectedColor);
    }

    public void RequestRespawn(CatcherSlot slot) {
        StartCoroutine(RespawnCoroutine(slot));
    }

    private IEnumerator RespawnCoroutine(CatcherSlot slot) {

        yield return new WaitForSeconds(respawnDelay);
        SpawnAtSlot(slot);
    }

    private void ShuffleColors() {

        colorsList.Clear();
        colorsList.AddRange(possibleColors);

        for(int i = 0; i < colorsList.Count; i++) {

            int randomIndex = Random.Range(i, colorsList.Count);

            CardColor temp = colorsList[i];
            colorsList[i] = colorsList[randomIndex];
            colorsList[randomIndex] = temp;
        }
    }
}