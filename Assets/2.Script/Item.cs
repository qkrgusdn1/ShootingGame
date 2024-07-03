using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class Item : MonoBehaviour
{
    public LayerMask PlayerLayer;
    public float range;
    Collider[] playerInRange;

    public TMP_Text text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
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
