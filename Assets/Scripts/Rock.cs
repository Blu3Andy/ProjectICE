using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private GameObject primarySpawn;
    [SerializeField] private GameObject secondarySpawn;

    [SerializeField] private int minIce, maxIce;
    [SerializeField] private int minRubble, maxRubble;

    public void IntiateSpawn()
    {
        int iceCount = Random.Range(minIce, maxIce);
        int rubbleCount = Random.Range(minRubble, maxRubble);

        SpawnAmountOf(iceCount, primarySpawn);
        SpawnAmountOf(rubbleCount, secondarySpawn);

    }

    private void SpawnAmountOf(int amount, GameObject spawnObject)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(spawnObject, transform.position, Quaternion.identity);
        }
    }
}
