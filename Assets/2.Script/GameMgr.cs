using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static GameMgr instance;
    public static GameMgr Instance
    {
        get { return instance; }
    }
    #endregion
    void Awake()
    {
        instance = this;
    }
    [Header("Save")]
    public GameObject saveBulletObj;
    public GameObject saveEffectObj;

}
