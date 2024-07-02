using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShootObject : ShootObject
{
    public override void Shoot()
    {
        if (!enemy)
        {
            if (shootTimer > 0)
                return;
            animator.Play("Shoot");
        }
        shootTimer = shootMaxTimer;
        Bullet disableBullet = bullets.Find(b => !b.gameObject.activeSelf);

        if (disableBullet != null)
        {

            disableBullet.gameObject.SetActive(true);

            disableBullet.transform.position = shootPoints[shootPointIndex].transform.position;
            disableBullet.transform.rotation = shootPoints[shootPointIndex].transform.rotation;

            shootPointIndex = (shootPointIndex + 1) % shootPoints.Count;
        }
        else
        {
            GameObject shootPoint = shootPoints[shootPointIndex];
            Bullet bullet = Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation, GameMgr.Instance.saveBulletObj.transform);
            bullet.damage = bulletDamage;
            bullets.Add(bullet);
            
            shootPointIndex = (shootPointIndex + 1) % shootPoints.Count;
        }
    }

    public override void DamageSetting()
    {
        Player player = GetComponentInParent<Player>();
        if (player != null)
            bulletDamage = player.atkDamage * 2;
    }
}
