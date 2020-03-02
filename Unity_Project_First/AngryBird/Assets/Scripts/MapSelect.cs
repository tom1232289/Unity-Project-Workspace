using System;
using UnityEngine;

public class MapSelect : MonoBehaviour {

    // 公有变量
    public int m_iStarNum = 0;

    // 私有引用
    private GameObject m_Lock;
    private GameObject m_Star;
    private GameObject m_Panel;

    // 私有变量
    private bool m_bOptional = false;

    private void Awake() {
        m_Lock = transform.Find("lock").gameObject;
        m_Star = transform.Find("star").gameObject;

        string sName = transform.name;
        sName = sName.Substring(sName.IndexOf('_') + 1);
        int index = 0;
        Int32.TryParse(sName, out index);
        m_Panel = transform.parent.parent.Find("Panel_" + index).gameObject;
    }

    private void Start() {
        if (PlayerPrefs.GetInt("totalNum", 0) >= m_iStarNum) {
            m_bOptional = true;
        }

        if (m_bOptional) {
            m_Lock.SetActive(false);
            m_Star.SetActive(true);
        }
    }

    // 鼠标点击事件
    public void OnBtnClicked() {
        transform.parent.gameObject.SetActive(false);
        m_Panel.SetActive(true);
    }
}