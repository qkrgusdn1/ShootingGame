using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public ShootObject[] crossShootObjects;
    public ShootObject bossBigRocketShootObject;
    Vector3 originalPos;
    public float crossShootSpeed;
    public HitEffect hitEffectPrefab;
    public GameObject root;
    public float maxHp;
    public float hp;
    private void Start()
    {
        originalPos = transform.position;
        StartCoroutine(CoShoot());
    }

    public void TakeDamage(float damage)
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

    IEnumerator CoShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));

            int randomChoice = Random.Range(0, 2);
            if (randomChoice == 0)
            {
                BigRocketShoot();
            }
            else
            {
                StartCoroutine(CoCrossShoot());
            }
        }
    }

    void BigRocketShoot()
    {
        bossBigRocketShootObject.Shoot();
    }

    IEnumerator CoCrossShoot()
    {
        for (int i = 0; i < crossShootObjects[0].shootPoints.Count; i++)
        {
            crossShootObjects[0].Shoot();
            yield return new WaitForSeconds(crossShootSpeed);
        }
        yield return new WaitForSeconds(crossShootSpeed);
        for (int i = 0; i < crossShootObjects[1].shootPoints.Count; i++)
        {
            crossShootObjects[1].Shoot();
            yield return new WaitForSeconds(crossShootSpeed);
        }
    }
}
