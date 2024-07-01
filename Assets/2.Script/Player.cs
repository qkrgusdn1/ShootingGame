using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static Player instance;
    public static Player Instance
    {
        get { return instance; }
    }
    #endregion
    
    ShootObject[] shootObjects;
    ShootObject currentShootObject;

    [Header("Settings")]
    public float maxHp;
    public float hp;
    public float atkDamage;
    float pulsDamage;
    float pulsMaxExp;
    public const float SPEED = 2;
    public float maxExp;
    public float exp;
    public int level;

    [Header("Hit Effects")]
    public HitEffect hitEffectPrefab;
    List<HitEffect> hitEffects = new List<HitEffect>();

    [Header("UI")]
    public Image hpBar;
    public TMP_Text hpBarText;
    public Image expBar;
    public TMP_Text expBarText;
    public TMP_Text levelText;
    public TMP_Text damageText;


    void Awake()
    {
        instance = this;
    }

    void Equip(ShootObjectType shootObjectType)
    {
        currentShootObject?.gameObject.SetActive(false);
        for (int i = 0; i < shootObjects.Length; i++)
        {
            if(shootObjects[i].type == shootObjectType)
            {
                currentShootObject = shootObjects[i];
                currentShootObject.DamageSetting();
                damageText.text = "Damage : " + currentShootObject.bulletDamage;
                currentShootObject.gameObject.SetActive(true);
                currentShootObject.Equiped();
            }
        }
    }

    void Start()
    {
        levelText.text = "Lv - " + level;
        shootObjects = GetComponentsInChildren<ShootObject>(true);
        Equip(ShootObjectType.Basic);
        
        hp = maxHp;

        expBar.fillAmount = exp / maxExp;
        expBarText.text = exp + "/" + maxExp;
    }

    public void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(hor, 0, ver) * Time.deltaTime * SPEED;
        transform.Translate(move);

        currentShootObject.UpdateShoot();

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
        
        if(exp >= maxExp)
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
            
            
            level++;
            levelText.text = "Lv - " + level;
            pulsDamage++;
            atkDamage += pulsDamage;
            for(int i = 0; i< shootObjects.Length; i++)
            {
                shootObjects[i].DamageSetting();
            }
            damageText.text = "Damage : " + atkDamage;
            expBar.fillAmount = exp / maxExp;
            expBarText.text = exp + "/" + maxExp;
        }
    }
    public void TakeDamage(float damage)
    {

        if(hitEffects.Count == 0)
        {
            HitEffect spawnHitEffect = Instantiate(hitEffectPrefab, transform.position, transform.rotation);
            hitEffects.Add(spawnHitEffect);
        }
        else
        {
            HitEffect disableHitEffect = hitEffects.Find(b => !b.gameObject.activeSelf);

            if (disableHitEffect != null)
            {
                disableHitEffect.gameObject.SetActive(true);

                disableHitEffect.transform.position = transform.position;
                disableHitEffect.transform.rotation = transform.rotation;
            }
            else
            {
                HitEffect spawnHitEffect = Instantiate(hitEffectPrefab, transform.position, transform.rotation, GameMgr.Instance.saveEffectObj.transform);
                hitEffects.Add(spawnHitEffect);
            }
        }
        hp -= damage;
        hpBar.fillAmount = hp / maxHp;
        hpBarText.text = hp + "/" + maxHp;
    }

    public void Shoot()
    {
        currentShootObject.Shoot();
    }
}
