using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnMgr : MonoBehaviour
{
    public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

    private void Start()
    {
        enemySpawns = new List<EnemySpawn>(GetComponentsInChildren<EnemySpawn>());
        StartCoroutine(CoSpawn());
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            StartCoroutine(enemySpawns[Random.Range(0, enemySpawns.Count)].CoSpawn());
            yield return new WaitForSeconds(Random.Range(10, 20));
        }
    }
}
