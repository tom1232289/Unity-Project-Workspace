using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    // 私有引用
    private GameObject m_panelSetting;

    private void Awake() {
        m_panelSetting = GameObject.Find("Order180Canvas").transform.Find("panelSetting").gameObject;
    }

    public void SwitchMute(bool isOn) {
        AudioManager.Instance.SwitchMuteParam(isOn);
    }

    public void OnBtnBackDown() {
        GameManager.Instance.SaveData();
        SceneManager.LoadScene("Start");
    }

    public void OnBtnSettingDown() {
        m_panelSetting.SetActive(true);
    }

    public void OnBtnCloseDown() {
        m_panelSetting.SetActive(false);
    }
}
