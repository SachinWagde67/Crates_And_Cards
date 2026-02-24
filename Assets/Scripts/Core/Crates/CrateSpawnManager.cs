using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct CrateConfig {

    public CardColor cardToGenerateColor;
    public CardColor crateToGenerateColor;
}

public class CrateSpawnManager : MonoBehaviour {

    [Header("Crate Settings")]
    [SerializeField] private List<CrateConfig> crateConfigs = new List<CrateConfig>();
    [SerializeField] private List<Transform> spawnLocations = new List<Transform>();
    [SerializeField] private float respawnDelay = 1.0f;

    private List<CrateConfig> spawnCratesList = new List<CrateConfig>();

    private void OnEnable() {
        GameEvents.OnGeneratorEmpty += HandleGeneratorEmpty;
    }

    private void OnDisable() {
        GameEvents.OnGeneratorEmpty -= HandleGeneratorEmpty;
    }

    private void Start() {

        ShuffleCrates();

        foreach(Transform location in spawnLocations) {
            SpawnRandomCrate(location);
        }
    }

    private void HandleGeneratorEmpty(GeneratorCrate generator) {
        StartCoroutine(ReplaceCrateCoroutine(generator));
    }

    private IEnumerator ReplaceCrateCoroutine(GeneratorCrate oldCrate) {

        Transform spawnPoint = oldCrate.transform.parent;

        yield return oldCrate.ScaleDownAndReturn();

        yield return new WaitForSeconds(respawnDelay);

        SpawnRandomCrate(spawnPoint);
    }

    private void SpawnRandomCrate(Transform parent) {

        if(spawnCratesList.Count == 0) {
            ShuffleCrates();
        }

        CrateConfig config = spawnCratesList[0];
        spawnCratesList.RemoveAt(0);

        GeneratorCrate crate = PoolManager.Instance.GetGenerator();

        crate.transform.SetParent(parent);
        crate.transform.localPosition = Vector3.zero;
        crate.transform.localRotation = Quaternion.identity;

        crate.InitializeCrate(config);
    }

    private void ShuffleCrates() {

        spawnCratesList.Clear();
        spawnCratesList.AddRange(crateConfigs);

        for(int i = 0; i < spawnCratesList.Count; i++) {

            int randomIndex = Random.Range(i, spawnCratesList.Count);

            CrateConfig temp = spawnCratesList[i];
            spawnCratesList[i] = spawnCratesList[randomIndex];
            spawnCratesList[randomIndex] = temp;
        }
    }
}