using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // 单例
    private static GameManager m_instance;

    public static GameManager Instance {
        get { return m_instance; }
    }

    // 公有变量
    public int m_iPlayerMoveCount = 2;   // 玩家移动几次敌人才移动一次
    public int m_iLevel = 1;
    public AudioClip[] m_acDie;   // 玩家死亡的音效

    // 私有引用
    [HideInInspector]public List<Enemy> m_listEnmeys;    // 敌人的数组
    private Text m_textFood;
    private GameObject m_Lose;

    // 私有变量
    private int m_iCurPlayerMoveCount = 0;
    [HideInInspector]public Vector2 m_posPlayerTarget = new Vector2();  // 玩家要走的下一个位置
    private MapManager m_mapManager;
    private int m_iFoodCount = 100;   // 食物数量
    [HideInInspector]public bool m_bCanMove = true;

    private void Awake() {
        m_instance = this;
        m_mapManager = GetComponent<MapManager>();
        m_textFood = GameObject.Find("textFood").GetComponent<Text>();
        m_iLevel = PlayerPrefs.GetInt("Level", 1);
        m_iFoodCount = PlayerPrefs.GetInt("Food", 100);
        m_textFood.text = "Food:" + m_iFoodCount;
        m_Lose = GameObject.Find("Canvas").transform.Find("textLose").gameObject;
    }

    // 玩家移动了 的消息响应函数 
    public void OnPlayerMoved() {
        // 玩家是否到达终点
        if (m_posPlayerTarget.x == m_mapManager.m_iColumn - 2 && m_posPlayerTarget.y == m_mapManager.m_iRow - 2) {
            NextLevel();
        }

        ++m_iCurPlayerMoveCount;
        if (m_iCurPlayerMoveCount < m_iPlayerMoveCount) {
            return;
        }

        m_iCurPlayerMoveCount = 0;
        // 敌人移动
        foreach (var enemy in m_listEnmeys) {
            enemy.Action();
        }
    }

    public void AddFood(int count) {
        m_iFoodCount += count;
        m_textFood.text = "+" + count + " Food:" + m_iFoodCount;
    }

    public void ReduceFood(int count) {
        m_iFoodCount -= count;
        m_textFood.text = "-" + count + " Food:" + m_iFoodCount;
        if (m_iFoodCount <= 0) {
            // 玩家死亡
            Lose();
        }
    }

    // 下一关
    private void NextLevel() {
        // 存当前关卡和食物
        PlayerPrefs.SetInt("Food", m_iFoodCount);
        PlayerPrefs.SetInt("Level", ++m_iLevel);
        // 加载下一关
        SceneManager.LoadScene("Load");
    }

    // 失败
    private void Lose() {
        // 停止背景音乐
        GetComponent<AudioSource>().Stop();
        // 播放音效
        int iRandom = Random.Range(0, m_acDie.Length);
        AudioSource.PlayClipAtPoint(m_acDie[iRandom], Camera.main.transform.position);
        m_Lose.SetActive(true);
        m_bCanMove = false;
    }
}