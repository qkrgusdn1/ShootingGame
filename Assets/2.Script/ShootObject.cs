using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootObject : MonoBehaviour
{
    [Header("Bullet Settings")]
    public Bullet bulletPrefab;
    public float bulletDamage;
    protected List<Bullet> bullets = new List<Bullet>();

    [Header("Shoot Points")]
    public List<GameObject> shootPoints = new List<GameObject>();
    protected int shootPointIndex;
    
    
    


    public virtual void DamageSetting()
    {
        Player player = GetComponentInParent<Player>();
        if (player != null)
            bulletDamage = player.atkDamage;
    }
    public virtual void Shoot()
    {
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
