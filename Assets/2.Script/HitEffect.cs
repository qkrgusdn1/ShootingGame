using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("DisableBullet", 1, 1);
    }

    void DisableBullet()
    {
        gameObject.SetActive(false);
    }
}
