using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // 公有引用
    public GameObject m_ImgGameover;
    public Text m_TextScore;
    public Text m_TextLife;

    // 公有变量
    public bool m_bIsGameover;
    public int m_iLife = 3;

    // 私有变量
    private int m_iScore;

    // 单例
    private static GameManager m_Instance;

    public static GameManager Instance { get => m_Instance; set => m_Instance = value; }

    private void Awake() {
        m_Instance = this;
        m_TextLife.text = m_iLife.ToString();
    }

    public void GameOver() {
        m_bIsGameover = true;
        m_ImgGameover.SetActive(true);
        Invoke("ReturnToMainMenu",2f);
    }

    private void ReturnToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateScore() {
        m_TextScore.text = (++m_iScore).ToString();
    }

    public void UpdateLife() {
        m_TextLife.text = (--m_iLife).ToString();
        if (m_iLife <= 0) {
            GameOver();
        }
    }
}