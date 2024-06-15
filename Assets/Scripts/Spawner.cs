using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Random = UnityEngine.Random;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Collider spawnArea;

    public GameObject[] bugPrefabs;

    public GameObject bombPrefab;

    [Range(0f, 1f)]
    public float bombChance = 0.05f;

    public float minSpawnTime = 0.25f;
    public float maxSpawnTime = 1f;

    public float minAngle = -15f;
    public float maxAngle = 15f;

    public float minForce = 18f;
    public float maxForce = 22f;

    public float lifetime = 5f;

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);

        while (enabled)
        {
            GameObject prefab = bugPrefabs[Random.Range(0, bugPrefabs.Length)];

            if (Random.value < bombChance)
            {
                prefab = bombPrefab;
            }

            Vector3 position = new Vector3();
            position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));

            GameObject bug = Instantiate(prefab, position, rotation);   
            Destroy(bug, lifetime);

            float force = Random.Range(minForce, maxForce);
            bug.GetComponent<Rigidbody>().AddForce(bug.transform.up * force, ForceMode.Impulse);

            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
}
