using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerStart : MonoBehaviour {

    // 公有引用
    public Toggle[] m_toggleSkin = new Toggle[2];      // 皮肤的Toggle
    public Toggle[] m_toggleMode = new Toggle[2];      // 模式的Toggle
    public Text m_textLast;
    public Text m_textBest;

    private void Awake() {
        LoadUserParams();
    }

    public void OnBtnStartClicked() {
        SetUserParams();
        SceneManager.LoadScene("Main");
    }

    private void SetUserParams() {
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

    private void LoadUserParams() {
        if (PlayerPrefs.GetInt("skin", 0) == 0) {
            m_toggleSkin[0].isOn = true;
            m_toggleSkin[1].isOn = false;
        }
        else {
            m_toggleSkin[0].isOn = false;
            m_toggleSkin[1].isOn = true;
        }

        if (PlayerPrefs.GetInt("mode", 0) == 0) {
            m_toggleMode[0].isOn = true;
            m_toggleMode[1].isOn = false;
        }
        else {
            m_toggleMode[0].isOn = false;
            m_toggleMode[1].isOn = true;
        }

        int iLastLength = PlayerPrefs.GetInt("lastLength", 0);
        int iLastScore = PlayerPrefs.GetInt("lastScore", 0);
        m_textLast.text = "上次：长度" + iLastLength + "，分数" + iLastScore;
        int iBestLength = PlayerPrefs.GetInt("bestLength", 0);
        int iBestScore = PlayerPrefs.GetInt("bestScore", 0);
        m_textBest.text = "最好：长度" + iBestLength + "，分数" + iBestScore;
    }
}