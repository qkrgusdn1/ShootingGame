using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootObject : MonoBehaviour
{
    [Header("Bullet Settings")]
    public Bullet bulletPrefab;
    public float bulletDamage;
    public ShootObjectType type;
    protected List<Bullet> bullets = new List<Bullet>();
    public float shootMaxTimer;
    public float shootTimer;
    public Animator animator;
    public float powerUpDamage;
    public bool enemy;
    Bullet disableBullet;
    public EnemyBulletType enemyBulletType;

    [Header("Shoot Points")]
    public List<GameObject> shootPoints = new List<GameObject>();
    protected int shootPointIndex;

    public void Start()
    {
        shootTimer = shootMaxTimer;
        ShootSpeedSetting();
    }

    public virtual void DamageSetting()
    {
        Player player = GetComponentInParent<Player>();
        if (player != null)
            bulletDamage = player.atkDamage * powerUpDamage;
    }

    public virtual void ShootSpeedSetting()
    {
        Player player = GetComponentInParent<Player>();
        if (player != null)
            shootMaxTimer = player.shootSpeed;
    }

    public virtual void Equiped()
    {

    }

    public virtual void UpdateShoot()
    {
        shootTimer -= Time.deltaTime;
    }

    

    public virtual void Shoot()
    {
        if (!enemy)
        {
            if (shootTimer > 0)
                return;
            animator.Play("Shoot");
        }

        shootTimer = shootMaxTimer;
        foreach (GameObject shootPoint in shootPoints)
        {
            if (!enemy)
            {
                disableBullet = bullets.Find(b => !b.gameObject.activeSelf);
            }
            else
            {
                for(int i = 0; i < GameMgr.Instance.enemyBullets.Count; i++)
                {
                    if (!GameMgr.Instance.enemyBullets[i].gameObject.activeSelf && GameMgr.Instance.enemyBullets[i].enemyBulletType == enemyBulletType)
                    {
                        disableBullet = GameMgr.Instance.enemyBullets[i];
                    }
                }
            }

            

            if (disableBullet != null && disableBullet.enemyBulletType == enemyBulletType)
            {
                disableBullet.gameObject.SetActive(true);

                disableBullet.transform.position = shootPoint.transform.position;
                disableBullet.transform.rotation = shootPoint.transform.rotation;
            }
            else if(disableBullet == null || disableBullet.gameObject.activeInHierarchy)
            {
                Bullet newBullet = Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation, GameMgr.Instance.saveBulletObj.transform);
                newBullet.damage = bulletDamage;
                if (enemy)
                {
                    GameMgr.Instance.enemyBullets.Add(newBullet);
                }
                else
                {
                    bullets.Add(newBullet);
                }
               
            }
        }
    }

    
}

public enum ShootObjectType
{
    Basic,
    Quadruple,
    Cross
}
