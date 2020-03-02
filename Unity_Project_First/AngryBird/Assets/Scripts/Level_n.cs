using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_n : MonoBehaviour {
    // 公有引用
    public Sprite m_spriteLevel;

    // 私有变量
    private bool m_bOptional = false;
    private List<GameObject> m_Stars = new List<GameObject>();

    private void Awake() {
        Transform transText = transform.Find("Text");
        foreach (Transform child in transText) {
            m_Stars.Add(child.gameObject);
        }
    }

    private void Start() {
        string sName = transform.name;
        sName = sName.Substring(sName.IndexOf('_') + 1);
        // 第一个Level默认可选
        if (sName == "1") {
            m_bOptional = true;
        }
        // 判断前一关是否有星星，有则此关可选
        else {
            // 取前一关获得的星星数量
            int iIndex = Int32.Parse(sName) - 1;
            int iPreStarNum = PlayerPrefs.GetInt("level_" + iIndex, 0);
            if (iPreStarNum > 0) {
                m_bOptional = true;
            }
        }

        if (m_bOptional) {
            // 更换level图标
            transform.GetComponent<Image>().sprite = m_spriteLevel;

            transform.Find("Text").gameObject.SetActive(true);

            // 显示要显示几个星星
            int iStarNum = PlayerPrefs.GetInt(transform.name, 0);
            for (int i = 0; i < iStarNum; ++i) {
                m_Stars[i].SetActive(true);
            }
        }
    }

    public void OnBtnLevelClicked() {
        if (m_bOptional) {
            // 保存当前选择的关卡名字
            PlayerPrefs.SetString("nowLevel", transform.name);
            // 加载相应的关卡场景
            SceneManager.LoadScene("Game");
        }
    }
}