using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private GameObject primarySpawn;
    [SerializeField] private GameObject secondarySpawn;

    private Vector3 displacePosition;

    [SerializeField] private int minIce, maxIce;
    [SerializeField] private int minRubble, maxRubble;


    [SerializeField] private float displaceDistance = 0.5f;
    private float displaceAngle;

    [SerializeField] private bool debug = false;

    public void IntiateSpawn()
    {
        int iceCount = Random.Range(minIce, maxIce);
        int rubbleCount = Random.Range(minRubble, maxRubble);

        displaceAngle = 360 / (iceCount + rubbleCount);

        displacePosition = transform.position + new Vector3(displaceDistance, 0, 0);

        if (debug) print("Ice Rubbles spawned: " + iceCount);
        if (debug) print("Rubbles spawned: " + rubbleCount);

        SpawnAmountOf(iceCount, primarySpawn);
        SpawnAmountOf(rubbleCount, secondarySpawn);

    }

    private void SpawnAmountOf(int amount, GameObject spawnObject)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(spawnObject, rotateSpawnPoint(), Quaternion.identity);
        }
    }

    private Vector3 rotateSpawnPoint()
    {
        Vector3 eq;

        eq.x = transform.position.x + (Mathf.Cos(displaceAngle * displacePosition.x) - Mathf.Sin(displaceAngle * displacePosition.z));
        eq.y = transform.position.y;
        eq.z = transform.position.z + (Mathf.Sin(displaceAngle * displacePosition.x) + Mathf.Cos(displaceAngle * displacePosition.z));

        displacePosition = eq;
        return displacePosition;
    }
}