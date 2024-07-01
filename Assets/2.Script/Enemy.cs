using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public float maxHp;
    public float hp;
    public float moveSpeed;
    public float expAmount;
    public EnemyMoveType enemyMoveType;

    [Header("Hit Effects")]
    public HitEffect hitEffectPrefab;
    List<HitEffect> hitEffects = new List<HitEffect>();

    [Header("Root")]
    public GameObject root;

    private void OnEnable()
    {
        hp = maxHp;
    }

    private void Update()
    {
        if(hp <= 0)
        {
            Player.Instance.exp += expAmount;
            Player.Instance.expBar.fillAmount = Player.Instance.exp / Player.Instance.maxExp;
            Player.Instance.expBarText.text = Player.Instance.exp + "/" + Player.Instance.maxExp;
            Destroy(gameObject);
        }

        if(enemyMoveType == EnemyMoveType.verticality)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DestoryWall"))
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
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

public enum EnemyMoveType
{
    verticality,
    horizontal,
    circle
}
