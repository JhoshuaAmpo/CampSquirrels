using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField]
    GameObject objectToPool;

    [SerializeField]
    bool doesObjectMove = false;
    [SerializeField]
    float initialSpawnCount = 0;

    [SerializeField]
    int numberOfObjects;

    [SerializeField]
    Transform targetTransform;

    [SerializeField]
    [Tooltip("Number of seconds between spawns")]
    float spawnRate = 1f;

    [SerializeField]
    float spawnYOffset;
    [SerializeField]
    GameObject spawnPointObject;

    List<GameObject> objectPool;
    List<Transform> spawnPoints;
    List<Tuple<GameObject, bool>> spawnPointsTaken;
    private float spawnTimer = 0f;

    void Awake()
    {
        objectPool = new();
        spawnPoints = new();
        spawnPointsTaken = new();

        // Initialize objectPool
        for(int i = 0; i < numberOfObjects; i++) {
            objectPool.Add(Instantiate(objectToPool,transform));
            if(objectToPool.CompareTag("Squirrel")) {
                objectPool[i].GetComponent<SquirrelBehavior>().targetTransform = targetTransform;
            }
            objectPool[i].SetActive(false);
        }

        // Initialize spawnPoints
        foreach(Transform sp in spawnPointObject.transform) {
            spawnPoints.Add(sp);
            sp.gameObject.SetActive(false);
        }

        // Spawn an item at each position
        for(int i = 0; i < initialSpawnCount; i++) {
            int ind = Spawn(spawnPoints[i].transform.position);
            spawnPointsTaken.Add(new(objectPool[ind], true));
        }
        while (spawnPointsTaken.Count < spawnPoints.Count) {
            spawnPointsTaken.Add(new(null, false));
        }

        spawnTimer = spawnRate;
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        UpdateSpotTaken();
        if (spawnTimer > 0f) { return;}
        if (doesObjectMove) {
            ProcessMoversSpawn();
        } else {
            ProcessStationarySpawn();
        }
    }
    
    private void ProcessMoversSpawn() {
        Spawn(spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].position);
    }

    private void ProcessStationarySpawn() {
        int spawnIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        int originalSI = spawnIndex;
        while(spawnPointsTaken[spawnIndex].Item2) {
            spawnIndex = (spawnIndex + 1 >= spawnPoints.Count) ? 0 : spawnIndex + 1;
            if (spawnIndex == originalSI) {
                // All spots taken
                spawnTimer = spawnRate;
                return;
            }
        }
        int ind = Spawn(spawnPoints[spawnIndex].position);
        spawnPointsTaken[spawnIndex] = new(objectPool[ind], true);
    }

    // Spawns object at pos, returns the index of the spawned object
    private int Spawn(Vector3 pos) {
        int i = 0;
        while(objectPool[i].activeSelf) {
            i++;
        }
        pos.y += spawnYOffset;
        objectPool[i].transform.position = pos;
        objectPool[i].SetActive(true);
        spawnTimer = spawnRate;
        // Debug.Log("Spawn " + objectPool[i] + " at " + pos);
        return i;
    }

    private void UpdateSpotTaken() {
        for( int i = 0; i < spawnPointsTaken.Count; i++) {
            if (spawnPointsTaken[i].Item1 && !spawnPointsTaken[i].Item1.activeSelf) {
                spawnPointsTaken[i] = new(null, false);
            }
        }
    }
}
