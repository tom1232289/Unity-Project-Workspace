using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // 单例
    private static GameManager m_instance;

    public static GameManager Instance {
        get { return m_instance; }
    }

    // 公有引用
    public Text m_textMsg;
    public Text m_textScore;
    public Text m_textLength;
    public Image m_imageBg;
    public Image m_imagePause;
    public Sprite[] m_spritePauses = new Sprite[2];
    public GameObject m_Borders;

    // 公有变量
    public bool m_bIsPause;
    public int m_iScore;
    public int m_iLength;
    public bool m_bHasBorder;

    private void Awake() {
        m_instance = this;

        // 加载模式：是否有边界
        if (PlayerPrefs.GetInt("mode", 0) == 0) {
            m_bHasBorder = true;
        }
       
        // 没有边界需要隐藏墙
        if (!m_bHasBorder) {
            foreach (Transform wall in m_Borders.transform) {
                wall.GetComponent<Image>().color = Color.clear;
            }
        }
    }

    private void Update() {
        if (m_iScore >= 100 && m_iScore < 500) {
            m_textMsg.text = "阶段2";
            m_imageBg.color = new Color(14 / 255f, 75 / 255f, 41 / 255f);
        }
        else if (m_iScore >= 500 && m_iScore < 1000) {
            m_textMsg.text = "阶段3";
            m_imageBg.color = new Color(35 / 255f, 243 / 255f, 40 / 255f);
        }
        else if (m_iScore >= 1000 && m_iScore < 5000) {
            m_textMsg.text = "阶段4";
            m_imageBg.color = new Color(211 / 255f, 154 / 255f, 145 / 255f);
        }
        else if (m_iScore >= 5000) {
            m_textMsg.text = "无尽模式";
            m_imageBg.color = new Color(145 / 255f, 45 / 255f, 200 / 255f);
        }
    }

    public void UpdateUI(int s = 5) {
        m_iScore += s;
        ++m_iLength;
        m_textScore.text = "得  分：\n" + m_iScore;
        m_textLength.text = "长  度：\n" + m_iLength;
    }

    public void OnBtnPauseClicked() {
        m_bIsPause = !m_bIsPause;
        if (m_bIsPause) {
            Time.timeScale = 0;
            m_imagePause.sprite = m_spritePauses[1];
        }
        else {
            Time.timeScale = 1;
            m_imagePause.sprite = m_spritePauses[0];
        }
    }

    public void OnBtnHomeClicked() {
        SceneManager.LoadScene("Start");
    }
}