using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameMgr : MonoBehaviour
{
    #region ½Ì±ÛÅæ
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

    [Header("Setting")]
    public int bestScore;
    public int score;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text bestScoreText;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = "BestScore : " + bestScore;
        scoreText.text = "Score : " + score;
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
