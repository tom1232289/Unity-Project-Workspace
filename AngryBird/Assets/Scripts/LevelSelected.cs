using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelected : MonoBehaviour {

    // 公有引用
    public Sprite m_spriteLevel;

    // 私有变量
    private int m_iIndex1;  // Level_i_j中i的值
    private int m_iIndex2;  // Level_i_j中j的值
    private bool m_bIsOptional;     // 当前Level是否可选

    private void Start() {
        // 获取m_iIndex1的值
        string sName = transform.name;
        int iIndex1 = sName.IndexOf('_') + 1;
        int iIndex2 = sName.LastIndexOf('_');
        m_iIndex1 = Int32.Parse(sName.Substring(iIndex1, iIndex2 - iIndex1));
        // 获取m_iIndex2的值
        m_iIndex2 = Int32.Parse(sName.Substring(iIndex2 + 1));

        // 第一关默认可选
        if (m_iIndex2 == 1) {
            m_bIsOptional = true;
        }
        else {
            // 检测当前Level是否可选（上一关至少获得一个星星）
            int iPreLevelStarNum = PlayerPrefs.GetInt("Level_" + m_iIndex1 + "_" + (m_iIndex2 - 1), 0);
            if (iPreLevelStarNum > 0) {
                m_bIsOptional = true;
            }
        }

        if (m_bIsOptional) {
            // 更换图片
            transform.GetComponent<Image>().sprite = m_spriteLevel;
            // 显示星星
            GameObject go = transform.Find("Text").gameObject;
            go.SetActive(true);
            go.GetComponent<Text>().text = m_iIndex2.ToString();

            go = transform.Find("Stars").gameObject;
            go.SetActive(true);
            int iLevelStarNum = PlayerPrefs.GetInt("Level_" + m_iIndex1 + "_" + m_iIndex2, 0);
            if (iLevelStarNum > 0) {
                go.transform.Find("leftStar").gameObject.SetActive(true);
            }
            if (iLevelStarNum > 1) {
                go.transform.Find("MidStar").gameObject.SetActive(true);
            }
            if (iLevelStarNum > 2) {
                go.transform.Find("RightStar").gameObject.SetActive(true);
            }
        }
    }

    public void OnBtnLevelClicked() {
        if (m_bIsOptional) {
            // 存储需要加载的关卡名
            PlayerPrefs.SetString("nowLevel", transform.name);
            // 加载Game场景
            SceneManager.LoadScene("Game");
        }
    }
}