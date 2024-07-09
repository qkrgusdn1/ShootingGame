using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float moveShootTimer;
    public GameObject[] bossMovePos;
    public Image bossHpBar;

    private void Start()
    {
        hp = maxHp;
        originalPos = transform.position;
        StartCoroutine(CoShoot());
        StartCoroutine(CoMoveShoot());
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
        bossHpBar.fillAmount = hp/maxHp;
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator MovePosition(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
    IEnumerator CoMoveShoot()
    {
        while (true)
        {
            yield return StartCoroutine(MovePosition(bossMovePos[0].transform.position, Quaternion.Euler(0, 145, 0), moveShootTimer));
            yield return new WaitForSeconds(moveShootTimer);

            yield return StartCoroutine(MovePosition(originalPos, Quaternion.Euler(0, 0, 0), moveShootTimer));
            yield return new WaitForSeconds(moveShootTimer);

            yield return StartCoroutine(MovePosition(bossMovePos[1].transform.position, Quaternion.Euler(0, -145, 0), moveShootTimer));
            yield return new WaitForSeconds(moveShootTimer);

            yield return StartCoroutine(MovePosition(originalPos, Quaternion.Euler(0, 0, 0), moveShootTimer));
            yield return new WaitForSeconds(moveShootTimer);


        }
    }

    IEnumerator CoShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));

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
