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
    protected Animator animator;

    public bool enemy;

    [Header("Shoot Points")]
    public List<GameObject> shootPoints = new List<GameObject>();
    protected int shootPointIndex;

    public void Start()
    {
        animator = GetComponent<Animator>();
        shootTimer = shootMaxTimer;
        ShootSpeedSetting();
    }

    public virtual void DamageSetting()
    {
        Player player = GetComponentInParent<Player>();
        if (player != null)
            bulletDamage = player.atkDamage;
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
            Bullet disableBullet = bullets.Find(b => !b.gameObject.activeSelf);

            if (disableBullet != null)
            {
                disableBullet.gameObject.SetActive(true);

                disableBullet.transform.position = shootPoint.transform.position;
                disableBullet.transform.rotation = shootPoint.transform.rotation;
            }
            else
            {
                Bullet newBullet = Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation, GameMgr.Instance.saveBulletObj.transform);
                newBullet.damage = bulletDamage;
                bullets.Add(newBullet);
            }
        }
    }
}

public enum ShootObjectType
{
    Basic,
    QuadrupleBasic,
    Cross
}
