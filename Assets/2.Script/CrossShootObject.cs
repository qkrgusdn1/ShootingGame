using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShootObject : ShootObject
{
    public override void ShootSpeedSetting()
    {
        Player player = GetComponentInParent<Player>();
        if (player != null)
            shootMaxTimer = player.shootSpeed * 0.5f;
    }

    public override void Shoot()
    {

        if (!enemy)
        {
            if (shootTimer > 0)
                return;
            animator.Play("Shoot");
        }
        shootTimer = shootMaxTimer;
        disableBullet = null;
        if (!enemy)
        {
            disableBullet = bullets.Find(b => !b.gameObject.activeSelf);
        }
        else
        {
            for (int i = 0; i < GameMgr.Instance.enemyBullets.Count; i++)
            {
                if (!GameMgr.Instance.enemyBullets[i].gameObject.activeSelf && GameMgr.Instance.enemyBullets[i].enemyBulletType == enemyBulletType)
                {
                    disableBullet = GameMgr.Instance.enemyBullets[i];
                }
            }
        }

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

            shootPointIndex = (shootPointIndex + 1) % shootPoints.Count;
        }
    }

    public override void DamageSetting()
    {
        Player player = GetComponentInParent<Player>();
        if (player != null)
            bulletDamage = player.atkDamage * 2 * powerUpDamage;
    }
}
