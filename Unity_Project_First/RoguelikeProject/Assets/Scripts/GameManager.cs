using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 单例
    private static GameManager m_instance;

    public static GameManager Instance {
        get { return m_instance; }
    }

    // 公有变量
    public int m_iLevel = 1;
    public int m_iFoodCount = 100;
    public List<Enemy> m_Enemys;
    public int m_iPlayerMoveCount = 2;  // 玩家移动几次敌人才移动一次
    public Vector2 m_posPlayerTarget = new Vector2(1, 1);
    public AudioClip[] m_acDies;        // 玩家死亡的音效
    public AudioClip[] m_acEatFruits;   // 玩家吃苹果的音效
    public AudioClip[] m_acEatSodas;    // 玩家喝苏打水的音效

    // 私有引用
    private Text m_textFood;
    private Text m_textFail;
    private MapManager m_mapManager;
    private GameObject m_imgDay;

    // 私有变量
    private int m_iCurPlayerMoveCount = 0;
    [HideInInspector]public bool m_bCanMove = false;

    private void Awake() {
        m_instance = this;
        m_textFood = GameObject.Find("textFood").GetComponent<Text>();
        m_textFail = GameObject.Find("Canvas").transform.Find("textFail").GetComponent<Text>();
        m_mapManager = GetComponent<MapManager>();
        m_imgDay = GameObject.Find("Canvas").transform.Find("imgDay").gameObject;
        m_iLevel = PlayerPrefs.GetInt("level", 1);
        m_iFoodCount = PlayerPrefs.GetInt("food", 100);
        m_textFood.text = "Food:" + m_iFoodCount;
    }

    private void Start() {
        m_imgDay.transform.Find("textDay").GetComponent<Text>().text = "Day " + m_iLevel;
        Invoke("HideDayInfo", 1);
    }

    private void HideDayInfo() {
        m_imgDay.SetActive(false);
        m_bCanMove = true;
    }

    // 主角移动了
    public void OnPlayerMove() {
        ++m_iCurPlayerMoveCount;
        // 判断是否到达终点
        if (m_posPlayerTarget.x == m_mapManager.m_iColumns - 2 && m_posPlayerTarget.y == m_mapManager.m_iRows - 2) {
            NextLevel();
        }

        if (m_iCurPlayerMoveCount < m_iPlayerMoveCount) {
            return;
        }

        // 敌人行动
        foreach (var enemy in m_Enemys) {
            enemy.Action();
        }

        m_iCurPlayerMoveCount = 0;
    }

    // 增加食物
    public void AddFood(int count) {
        m_iFoodCount += count;
        // 更新UI
        m_textFood.text = "+" + count + " Food:" + m_iFoodCount;
        // 播放吃食物音效
        if (count == 10) {
            int iRandom = Random.Range(0, m_acEatFruits.Length);
            AudioSource.PlayClipAtPoint(m_acEatFruits[iRandom], Camera.main.transform.position);
        }
        else {
            int iRandom = Random.Range(0, m_acEatSodas.Length);
            AudioSource.PlayClipAtPoint(m_acEatSodas[iRandom], Camera.main.transform.position);
        }
    }

    // 减少食物
    public void ReduceFood(int count) {
        m_iFoodCount -= count;
        // 更新UI
        m_textFood.text = "-" + count + " Food:" + m_iFoodCount;
        // 没得吃了
        if (m_iFoodCount <= 0) {
            Lose();
        }
    }

    private void Lose() {
        // 停止播放主音乐
        GetComponent<AudioSource>().Stop();
        // 播放死亡音效
        int iRandom = Random.Range(0, m_acDies.Length);
        AudioSource.PlayClipAtPoint(m_acDies[iRandom], Camera.main.transform.position);
        m_textFail.gameObject.SetActive(true);
        m_bCanMove = false;
    }

    private void NextLevel() {
        PlayerPrefs.SetInt("level", ++m_iLevel);
        PlayerPrefs.SetInt("food", m_iFoodCount);
        SceneManager.LoadScene("Game");
    }
}
