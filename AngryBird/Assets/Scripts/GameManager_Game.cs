using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_Game : MonoBehaviour {

    // 单例
    private static GameManager_Game m_instance;

    public static GameManager_Game Instance {
        get { return m_instance; }
    }

    // 公有引用
    public List<Bird> m_Birds = new List<Bird>(); // 小鸟的数组
    public List<Enemy> m_Pigs = new List<Enemy>(); // 小猪的数组

    // 公有变量
    public int m_iScore;   // 获得的分数

    // 私有变量
    private Vector3 m_posBirdOrigin = new Vector3();

    private void Awake() {
        m_instance = this;
        m_posBirdOrigin = m_Birds[0].transform.position;
    }

    private void Start() {
        // 初始化小鸟的状态
        InitBirdState();
    }

    private void InitBirdState() {
        for (int i = 0; i < m_Birds.Count; ++i) {
            // 要上战场的小鸟
            if (i == 0) {
                m_Birds[i].transform.position = m_posBirdOrigin;
                m_Birds[i].enabled = true;
                m_Birds[i].GetComponent<SpringJoint2D>().enabled = true;
                for (int j = 0; j < m_Birds[i].GetComponents<CircleCollider2D>().Length; ++j) {
                    m_Birds[i].GetComponents<CircleCollider2D>()[j].enabled = true;
                }
            }
            // 趴地上吃瓜的小鸟
            else {
                m_Birds[i].enabled = false;
                m_Birds[i].GetComponent<SpringJoint2D>().enabled = false;
                for (int j = 0; j < m_Birds[i].GetComponents<CircleCollider2D>().Length; ++j) {
                    m_Birds[i].GetComponents<CircleCollider2D>()[j].enabled = false;
                }
            }
        }
    }

    public void NextBird() {
        // 猪死光了 => 获胜
        if (m_Pigs.Count <= 0) {
            Win();
        }
        // 还有猪，但没有小鸟了 => 失败
        else if (m_Birds.Count <= 0) {
            Lose();
        }
        // 猪和鸟都还有 => 下一只上！
        else {
            InitBirdState();
        }
    }

    private void Win() {
        // 隐藏暂停按钮
        GameObject.Find("Canvas").transform.Find("btnPause").gameObject.SetActive(false);
        // 计算获得几颗星星
        int iStarCount = m_Birds.Count + 1;
        if (iStarCount > 3) {
            iStarCount = 3;
        }
        // 当此次记录最好时，更新记录
        int iRecodeStarCount = PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel"), 0);
        int iRecodeScore = PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel") + "_score", 0);
        if (iStarCount > iRecodeStarCount) {
            // 更新当前关卡星星个数
            string sNowLevel = PlayerPrefs.GetString("nowLevel");
            PlayerPrefs.SetInt(sNowLevel, iStarCount > 3 ? 3 : iStarCount);
            // 更新当前Map总的星星个数
            int iIndex = sNowLevel.IndexOf('_') + 1;
            string sMapIndex = sNowLevel.Substring(iIndex, 1);
            int iMapStarCount = PlayerPrefs.GetInt("Map_" + sMapIndex);
            iMapStarCount += (iStarCount - iRecodeStarCount);
            PlayerPrefs.SetInt("Map_" + sMapIndex, iMapStarCount);
        }

        if (m_iScore > iRecodeScore) {
            // 更新当前关卡最高分
            PlayerPrefs.SetInt(PlayerPrefs.GetString("nowLevel") + "_score", m_iScore);
        }

        // 显示获胜面板
        StartCoroutine("showStars", iStarCount);
    }

    // 星星一颗一颗显示
    private IEnumerator showStars(int iStarCount) {
        GameObject goWinPanel = GameObject.Find("Canvas").transform.Find("WinPanel").gameObject;
        goWinPanel.SetActive(true);
        const float iWaitSecond = 0.4f; // 延迟多少秒显示一颗星星
        if (iStarCount >= 1) {
            yield return new WaitForSeconds(iWaitSecond);
            goWinPanel.transform.Find("leftStar").gameObject.SetActive(true);
        }
        if (iStarCount >= 2) {
            yield return new WaitForSeconds(iWaitSecond);
            goWinPanel.transform.Find("midStar").gameObject.SetActive(true);
        }
        if (iStarCount >= 3) {
            yield return new WaitForSeconds(iWaitSecond);
            goWinPanel.transform.Find("rightStar").gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(iWaitSecond);
        GameObject go = goWinPanel.transform.Find("Text").gameObject;
        go.GetComponent<Text>().text = ("得分：" + m_iScore + "\n最高分：" + PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel") + "_score", 0));
        go.SetActive(true);
    }

    private void Lose() {
        // 隐藏暂停按钮
        GameObject.Find("Canvas").transform.Find("btnPause").gameObject.SetActive(false);
        // 显示失败面板
        GameObject.Find("Canvas").transform.Find("LosePanel").gameObject.SetActive(true);
    }

    public void OnBtnNextLevelClicked() {
        // 取当前关卡名
        string sNowLevel = PlayerPrefs.GetString("nowLevel");
        int iLevel = Int32.Parse(sNowLevel[sNowLevel.Length - 1].ToString());
        // 取下一关卡名
        ++iLevel;
        sNowLevel = sNowLevel.Remove(sNowLevel.Length - 1);
        sNowLevel = sNowLevel.Insert(sNowLevel.Length, iLevel.ToString());
        // 加载下一关卡
        PlayerPrefs.SetString("nowLevel", sNowLevel);
        SceneManager.LoadScene("Game");
    }

    public void WaitNextBird() {
        Invoke("NextBird", 3f);
    }
}