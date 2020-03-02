using System;
using UnityEngine;
using UnityEngine.UI;

public class MapSelected : MonoBehaviour {

    // 私有引用
    private Text m_TextStar;

    // 私有变量
    private bool m_bIsOptional;     // 当前Map是否可选
    private int m_iLevels;          // 当前Map下有多少个小关
    private int m_iIndex;           // 当前Map_i的i的值

    private void Awake() {
        m_TextStar = transform.Find("star").Find("Text").GetComponent<Text>();
        m_iLevels = GameObject.Find("Canvas").transform.Find("Level_" + GetIndex()).Find("ScorllPanel").childCount;
        m_iIndex = GetIndex();
    }

    private void Start() {
        // 第一张图默认为可选
        if (m_iIndex == 1) {
            m_bIsOptional = true;
        }
        else {
            // 检测当前Map是否可选（上一张Map获取的星星总数达到一半）
            int iPreMapStarNum = PlayerPrefs.GetInt("Map_" + (m_iIndex - 1).ToString());
            if (iPreMapStarNum >= m_iLevels * 3 / 2) {
                m_bIsOptional = true;
            }
        }

        if (m_bIsOptional) {
            // 显示星星
            transform.Find("lock").gameObject.SetActive(false);
            transform.Find("star").gameObject.SetActive(true);
            // 计算显示星星个数的TEXT
            // 显示每一张Map获得的星星总数
            int iMapStarNum = PlayerPrefs.GetInt("Map_" + GetIndex(), 0);
            string sText1 = iMapStarNum.ToString();           // 当前Map获得星星数量
            string sText2 = (m_iLevels * 3).ToString();     // 当前Map总共的星星数量
            m_TextStar.text = sText1 + "/" + sText2;
        }
    }

    public void OnBtnMapSelected() {
        if (!m_bIsOptional) {
            return;
        }

        // 隐藏Maps
        GameObject.Find("Maps").SetActive(false);
        // 显示对应的Levels
        GameObject.Find("Canvas").transform.Find("Level_" + GetIndex()).gameObject.SetActive(true);
    }

    // 获得当前Map_i 的i的值
    private int GetIndex() {
        string sName = transform.name;
        int iIndex = sName.IndexOf('_') + 1;
        sName = sName.Substring(iIndex);
        return Int32.Parse(sName);
    }
}