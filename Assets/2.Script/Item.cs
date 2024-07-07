using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (itemType)
            {
                case ItemType.hp:
                    Player.Instance.hp = Player.Instance.maxHp;
                    Player.Instance.hpBar.fillAmount = Player.Instance.hp / Player.Instance.maxHp;
                    Player.Instance.hpBarText.text = Player.Instance.hp + "/" + Player.Instance.maxHp;
                    break;
                case ItemType.invincibility:
                    Player.Instance.StartInvincibility();
                    break;
                case ItemType.bomb:
                    if(GameMgr.Instance.enemyBullets.Count != 0)
                    {
                        for (int i = 0; i < GameMgr.Instance.enemyBullets.Count; i++)
                        {
                            GameMgr.Instance.enemyBullets[i].gameObject.SetActive(false);
                        }
                    }

                    break;
            }
            gameObject.SetActive(false);
        }
    }
}

public enum ItemType
{
    hp,
    invincibility,
    bomb
}
