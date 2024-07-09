using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawn : MonoBehaviour
{
    public Enemy[] enemyPrefabs;
    public bool cross;
    public List<GameObject> positions = new List<GameObject>();
    public EnemyType enemyType;
    Enemy disableEnemy;
    public int spawnCount;
    public float spawnTimer;
    public float crossSpawnTimer;



    public IEnumerator CoSpawn()
    {
        while (true)
        {
            if (!cross)
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    Debug.Log("spawnEnemy");
                    Spawn();
                    yield return new WaitForSeconds(spawnTimer);
                }
            }
            else
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    CrossSpawnEnemy(enemyPrefabs[0], positions[0]);
                    yield return new WaitForSeconds(spawnTimer);
                }
                yield return new WaitForSeconds(crossSpawnTimer);
                for (int i = 0; i < spawnCount; i++)
                {
                    CrossSpawnEnemy(enemyPrefabs[1], positions[1]);
                    yield return new WaitForSeconds(spawnTimer);
                }
                yield return new WaitForSeconds(spawnTimer);
            }

            break;
        }
    }

    void Spawn()
    {
        Debug.Log("spawnEnemy");
        if (!cross)
        {
            Debug.Log("spawnEnemy");
            foreach (GameObject position in positions)
            {
                Debug.Log("spawnEnemy");
                disableEnemy = null;
                for (int i = 0; i < GameMgr.Instance.enemies.Count; i++)
                {
                    if (GameMgr.Instance.enemies[i] != null && !GameMgr.Instance.enemies[i].gameObject.activeSelf && GameMgr.Instance.enemies[i].enemyType == enemyType)
                    {
                        disableEnemy = GameMgr.Instance.enemies[i];
                    }
                }
                if (disableEnemy != null && disableEnemy.enemyType == enemyType)
                {
                    disableEnemy.gameObject.SetActive(true);
                    disableEnemy.transform.position = position.transform.position;
                    disableEnemy.transform.rotation = position.transform.rotation;
                }
                else if (disableEnemy == null)
                {
                    disableEnemy = Instantiate(enemyPrefabs[0], position.transform.position, position.transform.rotation, GameMgr.Instance.saveEnemyObj.transform);
                    GameMgr.Instance.enemies.Add(disableEnemy);
                }
                disableEnemy.transform.position = EnemySpawnMgr.Instance.GetEnemySpawnRandomPosition();
            }
        }
        else
        {

        }
        

    }
    void CrossSpawnEnemy(Enemy enemyPrefab, GameObject position)
    {
        disableEnemy = null;
        for (int i = 0; i < GameMgr.Instance.enemies.Count; i++)
        {
            if (!GameMgr.Instance.enemies[i].gameObject.activeSelf && GameMgr.Instance.enemies[i].enemyType == enemyPrefab.enemyType)
            {
                disableEnemy = GameMgr.Instance.enemies[i];
            }
        }
        if (disableEnemy != null && disableEnemy.enemyType == enemyPrefab.enemyType)
        {
            disableEnemy.gameObject.SetActive(true);
            disableEnemy.transform.position = position.transform.position;
            disableEnemy.transform.rotation = position.transform.rotation;
        }
        else if (disableEnemy == null)
        {
            Enemy newEnemy = Instantiate(enemyPrefab, position.transform.position, position.transform.rotation, GameMgr.Instance.saveEnemyObj.transform);
            GameMgr.Instance.enemies.Add(newEnemy);
        }
    }


}
