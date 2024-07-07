using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public float maxHp;
    public float hp;
    public float moveSpeed;
    public float expAmount;
    public int scoreAmount;
    Camera mainCamera;
    public float shootTime;
    public float shootCount;
    public float shootLoad;
    [SerializeField] bool canShoot;
    ShootObject shootObject;
    public ShootObjectType itemType;
    public float itemSpawnProbability;
    public bool spawnShootObjectEnemy;

    [Header("Item")]
    public ShootObjectItem shootObjectItem;
    public Item[] items;

    [Header("Hit Effects")]
    public HitEffect hitEffectPrefab;

    [Header("Root")]
    public GameObject root;

    private void OnEnable()
    {
        hp = maxHp;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        shootObject = GetComponent<ShootObject>();
        StartCoroutine(CoShoot());
    }



    private void Update()
    {
        Vector3 viewPoint = mainCamera.WorldToViewportPoint(transform.position);
        shootObject.UpdateShoot();
        if (hp <= 0)
        {
            Player.Instance.exp += expAmount;
            Player.Instance.expBar.fillAmount = Player.Instance.exp / Player.Instance.maxExp;
            Player.Instance.expBarText.text = Player.Instance.exp + "/" + Player.Instance.maxExp;
            GameMgr.Instance.AddScore(scoreAmount);
            if (Random.Range(0, 100) < itemSpawnProbability)
            {
                Quaternion spawnRotation = Quaternion.Euler(90, 0, 0);
                if (GameMgr.Instance.items.Count == 0)
                {
                    Item enemyItem = Instantiate(items[Random.Range(0, 3)], transform.position, spawnRotation);
                    GameMgr.Instance.items.Add(enemyItem);
                }
                else
                {
                    bool foundItem = false;
                    for (int i = 0; i < GameMgr.Instance.items.Count; i++)
                    {
                        if (!GameMgr.Instance.items[i].gameObject.activeSelf && GameMgr.Instance.items[i].itemType == items[Random.Range(0, 3)].itemType)
                        {
                            GameMgr.Instance.items[i].gameObject.SetActive(true);
                            GameMgr.Instance.items[i].transform.position = transform.position;
                            foundItem = true;
                            break;
                        }
                    }
                    if (!foundItem)
                    {
                        Item enemyItem = Instantiate(items[Random.Range(0, 3)], transform.position, spawnRotation);
                        GameMgr.Instance.items.Add(enemyItem);
                    }
                }
            }
            if (spawnShootObjectEnemy)
            {
                if (GameMgr.Instance.shootObjectItems.Count == 0)
                {
                    ShootObjectItem enemyItem = Instantiate(shootObjectItem, transform.position, Quaternion.identity);
                    GameMgr.Instance.shootObjectItems.Add(enemyItem);
                }
                else
                {
                    bool foundItem = false;
                    for (int i = 0; i < GameMgr.Instance.shootObjectItems.Count; i++)
                    {
                        if (!GameMgr.Instance.shootObjectItems[i].gameObject.activeSelf && GameMgr.Instance.shootObjectItems[i].shootObjectType == shootObjectItem.shootObjectType)
                        {
                            GameMgr.Instance.shootObjectItems[i].gameObject.SetActive(true);
                            GameMgr.Instance.shootObjectItems[i].transform.position = transform.position;
                            foundItem = true;
                            break;
                        }
                    }
                    if (!foundItem)
                    {
                        ShootObjectItem enemyItem = Instantiate(shootObjectItem, transform.position, Quaternion.identity);
                        GameMgr.Instance.shootObjectItems.Add(enemyItem);
                    }
                }
            }




            Destroy(gameObject);
        }
        if (viewPoint.x > 0 && viewPoint.x < 1 && viewPoint.y > 0 && viewPoint.y < 1)
        {
            canShoot = true;
            
        }
        else
        {
            canShoot = false;
        }
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DestoryWall"))
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator CoShoot()
    {
        while (true)
        {
            if (canShoot)
            {
                for (int i = 0; i < shootCount; i++)
                {
                    shootObject.Shoot();
                    yield return new WaitForSeconds(shootTime);
                }
            }
            yield return new WaitForSeconds(shootLoad);
        }
    }

    public void TakeDamage(float damage)
    {
        if (canShoot)
        {
            if (GameMgr.Instance.hitEffects.Count == 0)
            {
                HitEffect spawnHitEffect = Instantiate(hitEffectPrefab, root.transform.position, root.transform.rotation, GameMgr.Instance.saveEffectObj.transform);
                GameMgr.Instance.hitEffects.Add(spawnHitEffect);
            }
            else
            {
                HitEffect disableHitEffect = GameMgr.Instance.hitEffects.Find(b => !b.gameObject.activeSelf);

                if (disableHitEffect != null)
                {
                    disableHitEffect.gameObject.SetActive(true);

                    disableHitEffect.transform.position = root.transform.position;
                    disableHitEffect.transform.rotation = root.transform.rotation;
                }
                else
                {
                    HitEffect spawnHitEffect = Instantiate(hitEffectPrefab, root.transform.position, root.transform.rotation, GameMgr.Instance.saveEffectObj.transform);
                    GameMgr.Instance.hitEffects.Add(spawnHitEffect);
                }
            }
            hp -= damage;
        }
        

    }
}
