using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButtonAudio : MonoBehaviour
{
    /// <summary>
    /// 私有引用 
    /// </summary>
    private AudioSource m_as;

    /// <summary>
    /// 私有变量
    /// </summary>
    private ManagerVars m_managerVars;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ClickButtonAudio, ClickBtnAudio);

        m_managerVars = ManagerVars.GetManagerVars();
        m_as = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ClickButtonAudio, ClickBtnAudio);
    }

    private void ClickBtnAudio()
    {
        if (GameManager.Instance.GetMusicOn())
        {
            m_as.PlayOneShot(m_managerVars.m_acButton);
        }
    }
}
