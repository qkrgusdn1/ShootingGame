using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class ShootObjectItem : MonoBehaviour
{
    public LayerMask PlayerLayer;
    public float range;
    Collider[] playerInRange;

    public ShootObjectType shootObjectType;

    public TMP_Text text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (shootObjectType)
            {
                case ShootObjectType.Basic:
                    ChangeShootObject("Basic");
                    break;
                case ShootObjectType.Quadruple:
                    ChangeShootObject("Quadruple");
                    break;
                case ShootObjectType.Cross:
                    ChangeShootObject("Cross");
                    break;
            }
            gameObject.SetActive(false);
        }
    }

    void ChangeShootObject(string shootObjectName)
    {
        
        if (Player.Instance.currentShootObject.type == shootObjectType)
        {
            for (int i = 0; i < GameMgr.Instance.skillPowerCounts.Length; i++)
            {
                if (!GameMgr.Instance.skillPowerCounts[i].gameObject.activeSelf)
                {
                    GameMgr.Instance.skillPowerCounts[i].gameObject.SetActive(true);
                    Player.Instance.damagePowerUp++;
                    Player.Instance.atkDamage = Player.Instance.atkDamage * Player.Instance.damagePowerUp;
                    for (int j = 0; j < Player.Instance.shootObjects.Length; j++)
                    {
                        Player.Instance.shootObjects[i].DamageSetting();
                    }
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < GameMgr.Instance.skillPowerCounts.Length; i++)
            {
                Player.Instance.atkDamage = Player.Instance.defaultAtkDamage;
                Player.Instance.damagePowerUp = 1;
                for (int j = 0; j < Player.Instance.shootObjects.Length; j++)
                {
                    Player.Instance.shootObjects[i].DamageSetting();
                }
                GameMgr.Instance.skillPowerCounts[i].gameObject.SetActive(false);
            }
        }
        Player.Instance.ChanageShootObject(shootObjectName);
    }

    public virtual void Range()
    {
        playerInRange = Physics.OverlapSphere(transform.position, range, PlayerLayer);

        if (playerInRange.Length <= 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
        }
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update()
    {
        Range();
    }
}
