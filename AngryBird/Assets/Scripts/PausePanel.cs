using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    // 私有引用
    private Animator m_anim;    // 动画状态机
    private GameObject m_btnPause;  // 暂停按钮

    private void Awake() {
        m_anim = GetComponent<Animator>();
        m_btnPause = GameObject.Find("Canvas").transform.Find("btnPause").gameObject;
    }

    public void OnBtnPauseClicked() {
        // 隐藏暂停按钮
        m_btnPause.SetActive(false);
        // 播放暂停动画
        m_anim.SetBool("bIsPause", true);
    }

    public void OnBtnResumeClicked() {
        // 解除游戏暂停
        Time.timeScale = 1;
        // 播放恢复动画
        m_anim.SetBool("bIsPause", false);
    }

    public void OnBtnRetryClicked() {
        // 解除游戏暂停
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void OnBtnHomeClicked() {
        // 解除游戏暂停
        Time.timeScale = 1;
        SceneManager.LoadScene("Level");
    }

    private void PauseAnimEnd() {
        // 暂停游戏
        Time.timeScale = 0;
    }

    private void ResumeAnimEnd() {
        // 显示暂停按钮
        m_btnPause.SetActive(true);
    }
}
