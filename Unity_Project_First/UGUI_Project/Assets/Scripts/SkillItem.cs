using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour {
    // 公有变量
    public float m_fCDTime = 2f;
    public KeyCode keyCode;

    // 私有引用
    private Image m_imgCD;

    // 私有变量
    private float m_fCurrTime = 0f;
    private bool m_bIsStartTimer;

    private void Awake() {
        m_imgCD = transform.Find("ImgCD").GetComponent<Image>();
    }

    private void Update() {
        // 按下快捷键，开启冷却
        if (Input.GetKeyDown(keyCode)) {
            m_bIsStartTimer = true;
        }

        // 技能冷却，技能图标随时间变化
        if (m_bIsStartTimer) {
            m_fCurrTime += Time.deltaTime;
            m_imgCD.fillAmount = (m_fCDTime - m_fCurrTime) / m_fCDTime;
        }

        // 冷却结束
        if (m_fCurrTime >= m_fCDTime) {
            m_imgCD.fillAmount = 0;
            m_fCurrTime = 0;
            m_bIsStartTimer = false;
        }
    }

    public void OnClicked() {
        m_bIsStartTimer = true;
    }
}
