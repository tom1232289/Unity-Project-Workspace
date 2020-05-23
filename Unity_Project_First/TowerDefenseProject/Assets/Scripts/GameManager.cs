using System.Collections;
using System.Collections.Generic;
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

    // 公有引用
    public Text m_textMoney;
    public GameObject m_goGameOver;

    // 公有变量
    public int m_iMoney = 1000;

    // 私有引用
    [HideInInspector]public List<Transform> m_listRoadPos;

    private void Awake() {
        m_instance = this;
        // 初始化敌人行走的路径点
        GameObject goWayPoints = GameObject.Find("WayPoints");
        for (int i = 0; i < goWayPoints.transform.childCount; ++i) {
            m_listRoadPos.Add(goWayPoints.transform.GetChild(i));
        }
    }

    public void ChangeMoney(int money) {
        m_iMoney += money;
        // 更新UI
        m_textMoney.text = "￥ " + m_iMoney;
    }

    public void Win() {
        // 弹出胜利界面
        m_goGameOver.SetActive(true);
        m_goGameOver.transform.Find("Text").GetComponent<Text>().text = "YOU WIN!";
    }

    public void Lose() {
        // 停止生成
        GetComponent<EnemySpawner>().Stop();
        // 弹出失败界面
        m_goGameOver.SetActive(true);
        m_goGameOver.transform.Find("Text").GetComponent<Text>().text = "GAME OVER";
    }

    public void OnBtnRetryClicked() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBtnMenuClicked() {
        SceneManager.LoadScene("Start");
    }
}
