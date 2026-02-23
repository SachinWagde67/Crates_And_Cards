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

    private void OnEnable() {
        GameEvents.OnGeneratorEmpty += HandleGeneratorEmpty;
    }

    private void OnDisable() {
        GameEvents.OnGeneratorEmpty -= HandleGeneratorEmpty;
    }

    private void Start() {

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

        int randomIndex = Random.Range(0, crateConfigs.Count);

        CrateConfig config = crateConfigs[randomIndex];

        GeneratorCrate crate = PoolManager.Instance.GetGenerator();

        crate.transform.SetParent(parent);
        crate.transform.localPosition = Vector3.zero;
        crate.transform.localRotation = Quaternion.identity;

        crate.InitializeCrate(config);
    }
}