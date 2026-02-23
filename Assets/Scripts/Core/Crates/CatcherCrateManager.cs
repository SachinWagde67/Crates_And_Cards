using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherCrateManager : MonoBehaviour {

    public static CatcherCrateManager Instance { get; private set; }

    [SerializeField] private float respawnDelay = 0.5f;
    [SerializeField] private List<CatcherSlot> slots = new List<CatcherSlot>();
    [SerializeField] private List<CardColor> possibleColors = new List<CardColor>();

    private void Awake() {

        if(Instance != this && Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {

        foreach(CatcherSlot slot in slots) {
            SpawnAtSlot(slot);
        }
    }

    private void SpawnAtSlot(CatcherSlot slot) {

        int randomIndex = Random.Range(0, possibleColors.Count);
        slot.SpawnCrate(possibleColors[randomIndex]);
    }

    public void RequestRespawn(CatcherSlot slot) {
        StartCoroutine(RespawnCoroutine(slot));
    }

    private IEnumerator RespawnCoroutine(CatcherSlot slot) {

        yield return new WaitForSeconds(respawnDelay);
        SpawnAtSlot(slot);
    }
}