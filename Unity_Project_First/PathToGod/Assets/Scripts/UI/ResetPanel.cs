using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetPanel : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Button m_btnYes;

    private Button m_btnNo;

    private Image m_imgBg;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowResetPanel, ShowResetPanel);

        m_imgBg = transform.Find("bg").GetComponent<Image>();
        m_btnYes = transform.Find("bg/btnYes").GetComponent<Button>();
        m_btnYes.onClick.AddListener(OnBtnYesClicked);
        m_btnNo = transform.Find("bg/btnNo").GetComponent<Button>();
        m_btnNo.onClick.AddListener(OnBtnNoClicked);

        // 一开始看不见
        m_imgBg.color = new Color(m_imgBg.color.r, m_imgBg.color.g, m_imgBg.color.b, 0);
        m_imgBg.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowResetPanel, ShowResetPanel);
    }

    private void ShowResetPanel()
    {
        gameObject.SetActive(true);
        m_imgBg.DOColor(new Color(m_imgBg.color.r, m_imgBg.color.g, m_imgBg.color.b, 0.5f), 0.3f);
        m_imgBg.transform.DOScale(Vector3.one, 0.3f);
    }

    /// <summary>
    /// 点击了按钮 是
    /// </summary>
    private void OnBtnYesClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        GameManager.Instance.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 点击了按钮 否
    /// </summary>
    private void OnBtnNoClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        m_imgBg.DOColor(new Color(m_imgBg.color.r, m_imgBg.color.g, m_imgBg.color.b, 0f), 0.3f);
        m_imgBg.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}