using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnMgr : MonoBehaviour
{
    public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
    public float minTime;
    public float maxTime;
    public Camera mainCamera;

    #region ΩÃ±€≈Ê
    private static EnemySpawnMgr instance;
    public static EnemySpawnMgr Instance
    {
        get { return instance; }
    }
    #endregion
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        enemySpawns = new List<EnemySpawn>(GetComponentsInChildren<EnemySpawn>());
        StartCoroutine(CoSpawn());
    }

    public Vector3 GetEnemySpawnRandomPosition()
    {
        float viewPortLeftX = mainCamera.ScreenToViewportPoint(GameMgr.Instance.leftLimitTr.position).x;
        float viewPortRightX = mainCamera.ScreenToViewportPoint(GameMgr.Instance.rightLimitTr.position).x;

        float randomX = Random.Range(viewPortLeftX, viewPortRightX);

        Vector3 viewPoint = mainCamera.WorldToViewportPoint(new Vector3(randomX, 1.1f));

        return new Vector3(viewPoint.x, 9.20f, viewPoint.z);
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            StartCoroutine(enemySpawns[Random.Range(0, enemySpawns.Count)].CoSpawn());
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}
