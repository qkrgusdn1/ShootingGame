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
    public Item item;
    public ShootObjectType itemType;

    [Header("Hit Effects")]
    public HitEffect hitEffectPrefab;
    List<HitEffect> hitEffects = new List<HitEffect>();

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
            if (GameMgr.Instance.items.Count == 0)
            {
                Debug.Log("enemyItem");
                Item enemyItem = Instantiate(item, transform.position, Quaternion.identity);
                GameMgr.Instance.items.Add(enemyItem);
            }
            else
            {
                for (int i = 0; i < GameMgr.Instance.items.Count; i++)
                {

                    if (!GameMgr.Instance.items[i].gameObject.activeSelf && GameMgr.Instance.items[i].shootObjectType == item.shootObjectType)
                    {
                        GameMgr.Instance.items[i].gameObject.SetActive(true);

                    }
                    else if (GameMgr.Instance.items[i].gameObject.activeSelf && GameMgr.Instance.items[i].shootObjectType == item.shootObjectType)
                    {
                        Item enemyItem = Instantiate(item, transform.position, Quaternion.identity);
                        GameMgr.Instance.items.Add(enemyItem);
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
            if (hitEffects.Count == 0)
            {
                HitEffect spawnHitEffect = Instantiate(hitEffectPrefab, root.transform.position, root.transform.rotation);
                hitEffects.Add(spawnHitEffect);
            }
            else
            {
                HitEffect disableHitEffect = hitEffects.Find(b => !b.gameObject.activeSelf);

                if (disableHitEffect != null)
                {
                    disableHitEffect.gameObject.SetActive(true);

                    disableHitEffect.transform.position = root.transform.position;
                    disableHitEffect.transform.rotation = root.transform.rotation;
                }
                else
                {
                    HitEffect spawnHitEffect = Instantiate(hitEffectPrefab, root.transform.position, root.transform.rotation, GameMgr.Instance.saveEffectObj.transform);
                    hitEffects.Add(spawnHitEffect);
                }
            }
            hp -= damage;
        }
        

    }
}
