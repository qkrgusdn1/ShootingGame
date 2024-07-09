using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static Player instance;
    public static Player Instance
    {
        get { return instance; }
    }
    #endregion

    [Header("Settings")]
    public float maxHp;
    public float hp;
    public float atkDamage;
    public float defaultAtkDamage;
    public float damagePowerUp;
    float pulsMaxExp;
    public const float SPEED = 1;
    public float maxExp;
    public float exp;
    public int level;
    public float shootSpeed;
    public ShootObject[] shootObjects;
    public ShootObject currentShootObject;
    public Bullet skillBulletPrefab;
    List<Bullet> skillBullets = new List<Bullet>();
    public bool invincibility;
    public GameObject invincibilityBarrier;
    

    [Header("Hit Effects")]
    public HitEffect hitEffectPrefab;
    List<HitEffect> hitEffects = new List<HitEffect>();
    int currentPlayerIndex;
    GameObject currentPlayerBody;
    public GameObject[] playerBodys;

    [Header("UI")]
    public Image hpBar;
    public TMP_Text hpBarText;
    public Image expBar;
    public TMP_Text expBarText;
    public TMP_Text levelText;
    public TMP_Text damageText;
    public TMP_Text shootObjectTypeText;
    public TMP_Text bodyTypeText;




    void Awake()
    {
        instance = this;
        shootObjects = GetComponentsInChildren<ShootObject>(true);
        Equip(ShootObjectType.Basic);
    }

   

    void Start() 
    {

        levelText.text = "Lv - " + level;
        defaultAtkDamage = atkDamage;
        currentPlayerBody = playerBodys[currentPlayerIndex];
        bodyTypeText.text = "Body : " + currentPlayerBody.name;
        hp = maxHp;

        expBar.fillAmount = exp / maxExp;
        expBarText.text = exp + "/" + maxExp;
    }

    public void ChanageShootObject(string shootObjectName)
    {
        if (Enum.TryParse<ShootObjectType>(shootObjectName, out ShootObjectType objectType))
        {
            Equip(objectType);
        }
    }

    public void Equip(ShootObjectType shootObjectType)
    {
        currentShootObject?.gameObject.SetActive(false);
        for (int i = 0; i < shootObjects.Length; i++)
        {
            if (shootObjects[i].type == shootObjectType)
            {
                currentShootObject = shootObjects[i];
                currentShootObject.DamageSetting();
                damageText.text = "Damage : " + currentShootObject.bulletDamage;
                currentShootObject.gameObject.SetActive(true);
                currentShootObject.Equiped();
            }
        }
        shootObjectTypeText.text = "Type : " + shootObjectType.ToString();
    }

    void Move()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(hor, 0, ver) * Time.deltaTime * SPEED;
        transform.Translate(move);

        Vector2 leftWorldPos = Camera.main.ScreenToViewportPoint(GameMgr.Instance.leftLimitTr.position);
        Vector2 rightWorldPos = Camera.main.ScreenToViewportPoint(GameMgr.Instance.rightLimitTr.position);


        Vector3 viewPoint = Camera.main.WorldToViewportPoint(transform.position);

        viewPoint.x = Mathf.Clamp(viewPoint.x, leftWorldPos.x, rightWorldPos.x);
        viewPoint.y = Mathf.Clamp(viewPoint.y, 0, 1);

        //∫‰∆˜∆Æ ¡¬«•∏¶ ø˘µÂ ¡¬«•∑Œ ∫Ø»Ø
        viewPoint = Camera.main.ViewportToWorldPoint(viewPoint);

        transform.position = new Vector3(viewPoint.x, transform.position.y, viewPoint.z);
    }


    public void Update()
    {
        Move();
        currentShootObject.UpdateShoot();

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Q) && GameMgr.Instance.skillTimerImage.fillAmount == 1)
        {
            SkillShoot();
        }

        if (exp >= maxExp)
        {
            pulsMaxExp += 10 * level;
            if (maxExp < exp)
            {
                exp = exp - maxExp;
            }
            else
            {
                exp = 0;
            }
            maxExp += pulsMaxExp;
            expBar.fillAmount = exp / maxExp;
            expBarText.text = exp + "/" + maxExp;
            
            if(currentPlayerIndex < playerBodys.Length - 1)
            {
                currentPlayerBody.gameObject.SetActive(false);
                currentPlayerIndex++;
                currentPlayerBody = playerBodys[currentPlayerIndex];
                currentPlayerBody.gameObject.SetActive(true);
                bodyTypeText.text = "Body : " + currentPlayerBody.name;
                if(currentPlayerIndex >= playerBodys.Length - 1)
                {
                    bodyTypeText.color = Color.red;
                }
            }
            
            maxHp = maxHp * 1.1f;
            hp = maxHp;
            hpBar.fillAmount = hp / maxHp;
            hpBarText.text = hp + "/" + maxHp;

            level++;
            levelText.text = "Lv - " + level;

            defaultAtkDamage = defaultAtkDamage * 1.1f;
            atkDamage = atkDamage * 1.1f;
            for (int i = 0; i < shootObjects.Length; i++)
            {
                shootObjects[i].DamageSetting();
            }
            damageText.text = "Damage : " + currentShootObject.bulletDamage;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChanageShootObject("Basic");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChanageShootObject("Quadruple");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChanageShootObject("Cross");
        }
    }

    public void StartInvincibility()
    {
        if (!invincibility)
        {
            invincibility = true;
            invincibilityBarrier.SetActive(true);
            StartCoroutine(CoInvincibilityTimer());
        }
    }


    IEnumerator CoInvincibilityTimer()
    {
        yield return new WaitForSeconds(7f);
        invincibilityBarrier.SetActive(false);
        invincibility = false;
    }

    public void TakeDamage(float damage)
    {
        

        

        if (GameMgr.Instance.hitEffects.Count == 0)
        {
            HitEffect spawnHitEffect = Instantiate(hitEffectPrefab, transform.position, transform.rotation, GameMgr.Instance.saveEffectObj.transform);
            GameMgr.Instance.hitEffects.Add(spawnHitEffect);
        }
        else
        {
            HitEffect disableHitEffect = GameMgr.Instance.hitEffects.Find(b => !b.gameObject.activeSelf);

            if (disableHitEffect != null)
            {
                disableHitEffect.gameObject.SetActive(true);

                disableHitEffect.transform.position = transform.position;
                disableHitEffect.transform.rotation = transform.rotation;
            }
            else
            {
                HitEffect spawnHitEffect = Instantiate(hitEffectPrefab, transform.position, transform.rotation, GameMgr.Instance.saveEffectObj.transform);
                GameMgr.Instance.hitEffects.Add(spawnHitEffect);
            }
        }
        if (invincibility)
            return;

        StartCoroutine(CameraPos.Instance.Shake());
        hp -= damage;
        hpBar.fillAmount = hp / maxHp;
        hpBarText.text = hp + "/" + maxHp;
    }

    public void SkillShoot()
    {
        if (skillBullets.Count == 0)
        {
            Bullet bullet = Instantiate(skillBulletPrefab, transform.position, Quaternion.identity);
            bullet.damage = atkDamage * 20;
            skillBullets.Add(bullet);
        }
        else
        {
            bool foundSkillBullet = false;
            for (int i = 0; i < skillBullets.Count; i++)
            {
                if (!skillBullets[i].gameObject.activeSelf)
                {
                    skillBullets[i].gameObject.SetActive(true);
                    skillBullets[i].transform.position = transform.position;
                    skillBullets[i].damage = atkDamage * 20;
                    foundSkillBullet = true;
                    break;
                }
            }
            if (!foundSkillBullet)
            {
                Bullet bullet = Instantiate(skillBulletPrefab, transform.position, Quaternion.identity);
                bullet.damage = atkDamage * 20;
                skillBullets.Add(bullet);
            }
        }
        GameMgr.Instance.skillTimer = 0;
        GameMgr.Instance.skillTimerImage.fillAmount = 0;
        StartCoroutine(GameMgr.Instance.CoSkillUpdate());
    }

    public void Shoot()
    {
        currentShootObject.Shoot();
    }
}
