using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {
    // 公有成员变量
    public int m_iLife = 3;
    public int m_iPlayerScore = 0;
    public bool m_bIsDead;
    public bool m_bIsGameover;

    // 引用
    public GameObject m_Born;
    public Text m_TextPlayerScore;
    public Text m_TextPlayerLifeValue;
    public GameObject m_ImgGameover;

    // 单例
    private static PlayerManager m_Instanse;

    public static PlayerManager Instanse { get => m_Instanse; set => m_Instanse = value; }

    private void Awake() {
        m_Instanse = this;
    }

    private void Update() {
        if (m_bIsGameover) {
            m_ImgGameover.SetActive(true);
            Invoke("ReturnToTheMainMenu", 3);
            return;
        }
        if (m_bIsDead) {
            ReBorn();
        }
        // 更新计分UI
        m_TextPlayerScore.text = m_iPlayerScore.ToString();
        m_TextPlayerLifeValue.text = m_iLife.ToString();
    }

    private void ReBorn() {
        if (m_iLife <= 0) {
            // 游戏失败，返回主界面
            m_bIsGameover = true;
        }
        else {
            --m_iLife;
            GameObject go = Instantiate(m_Born, new Vector3(-2, -8, 0), Quaternion.identity);
            go.GetComponent<Born>().m_bCreatePlayer = true;
            m_bIsDead = false;
        }
    }

    private void ReturnToTheMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
