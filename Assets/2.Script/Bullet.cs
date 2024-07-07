using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    public float damage;
    public float disableTime;

    [Header("Type")]
    public BulletType bulletType;
    public EnemyBulletType enemyBulletType;

    private void OnEnable()
    {
        CancelInvoke("DisableBullet");
        Invoke("DisableBullet", disableTime);
    }

    private void OnDisable()
    {
        CancelInvoke("DisableBullet");
    }
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(bulletType == BulletType.mine)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    gameObject.SetActive(false);
                }
            }
        }
        else if(bulletType == BulletType.enemy)
        {
            if (other.CompareTag("Player"))
            {
                Player.Instance.TakeDamage(damage);
                gameObject.SetActive(false);
                Debug.Log("Player");
            }
        }
       
    }

    void DisableBullet()
    {
        gameObject.SetActive(false);
    }
}

public enum BulletType
{
    mine,
    enemy
}
public enum EnemyBulletType
{
    none,
    gray,
    pink
}
