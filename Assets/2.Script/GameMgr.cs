using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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
    public GameObject saveEnemyObj;
    public GameObject saveBulletObj;
    public GameObject saveEffectObj;

    [Header("Setting")]
    public int bestScore;
    public int score;
    bool isSkillUpdating;
    public List<HitEffect> hitEffects = new List<HitEffect>();
    public List<Bullet> enemyBullets = new List<Bullet>();
    Camera mainCamera;
    public GameObject[] enemySpawnPositions;
    public List<Enemy> enemies = new List<Enemy>();

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text bestScoreText;
    public float skillTimer;
    public float maxSkillTimer;
    public TMP_Text skillTimerText;
    public Image skillTimerImage;
    public Image[] skillPowerCounts;
    public Transform leftLimitTr;
    public Transform rightLimitTr;


    [Header("Pooling")]
    public List<Item> items = new List<Item>();
    public List<ShootObjectItem> shootObjectItems = new List<ShootObjectItem>();
    private void Start()
    {
        mainCamera = Camera.main;
        PlayerPrefs.DeleteAll();
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = "BestScore : " + bestScore;
        scoreText.text = "Score : " + score;
        StartCoroutine(CoSkillUpdate());
    }


    public IEnumerator CoSkillUpdate()
    {
        if (isSkillUpdating) 
            yield break; 
        isSkillUpdating = true;
        skillTimerText.gameObject.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(0.15f);
            if (skillTimer < maxSkillTimer)
            {
                skillTimer += 0.3f;
                skillTimerImage.fillAmount = skillTimer / maxSkillTimer;
                skillTimerText.text = skillTimer.ToString("F0");
            }
            else
            {
                skillTimerText.gameObject.SetActive(false);
                skillTimer = maxSkillTimer;
                skillTimerImage.fillAmount = skillTimer / maxSkillTimer;
                break;
            }
        }
       
        isSkillUpdating = false;
    }

    public bool CheckInMap(Vector3 pos)
    {
        Vector2 leftWorldPos = Camera.main.ScreenToViewportPoint(leftLimitTr.position);
        Vector2 rightWorldPos = Camera.main.ScreenToViewportPoint(rightLimitTr.position);

        Vector3 viewPoint = mainCamera.WorldToViewportPoint(pos);

        if (viewPoint.x < leftWorldPos.x || viewPoint.x > rightWorldPos.x || viewPoint.y < 0 || viewPoint.y > 1)
        {
            return false;
        }
        return true;
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        if(score >= bestScore)
        {
            bestScore = score;
            bestScoreText.text = "BestScore : " + bestScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
        scoreText.text = "Score : " + score;
    }
}
