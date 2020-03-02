using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerStart : MonoBehaviour {

    // 公有引用
    public Text m_textLast;
    public Text m_textBest;
    public Toggle[] m_toggleSkin = new Toggle[2];
    public Toggle[] m_toggleMode = new Toggle[2];

    private void Awake() {
        int lastScore = PlayerPrefs.GetInt("lastScore", 0);
        int lastLength = PlayerPrefs.GetInt("lastLength", 0);
        int bestScore = PlayerPrefs.GetInt("bestScore", 0);
        int bestLength = PlayerPrefs.GetInt("bestLength", 0);
        m_textLast.text = "上次：长度" + lastLength + "，分数" + lastScore;
        m_textBest.text = "最好：长度" + bestLength + "，分数" + bestScore;
    }

    private void Start() {
        if (PlayerPrefs.GetInt("skin", 0) == 0) {
            m_toggleSkin[0].isOn = true;
        }
        else {
            m_toggleSkin[1].isOn = true;
        }
        if (PlayerPrefs.GetInt("mode", 0) == 0) {
            m_toggleMode[0].isOn = true;
        }
        else {
            m_toggleMode[1].isOn = true;
        }
    }

    public void OnBtnStartClicked() {
        // 传递用户参数
        LoadUserParam();
        // 加载主场景
        SceneManager.LoadScene("Main");
    }

    private void LoadUserParam() {
        if (m_toggleSkin[0].isOn) {
            PlayerPrefs.SetInt("skin", 0);
        }
        else {
            PlayerPrefs.SetInt("skin", 1);
        }

        if (m_toggleMode[0].isOn) {
            PlayerPrefs.SetInt("mode", 0);
        }
        else {
            PlayerPrefs.SetInt("mode", 1);
        }
    }
}