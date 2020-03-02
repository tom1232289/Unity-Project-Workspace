using UnityEngine;

public class Pause : MonoBehaviour {

    // 公有引用
    public GameObject m_btnPause;

    // 私有引用
    private Animator m_anim;

    private void Awake() {
        m_anim = GetComponent<Animator>();
    }

    public void PauseAnimEnd() {
        Time.timeScale = 0;
    }

    public void ResumeAnimEnd() {
        m_btnPause.SetActive(true);
    }

    public void OnBtnPauseClicked() {
        m_btnPause.SetActive(false);
        m_anim.SetBool("bIsPause", true);
    }

    public void OnBtnResumeClicked() {
        Time.timeScale = 1;
        m_anim.SetBool("bIsPause", false);
    }
}