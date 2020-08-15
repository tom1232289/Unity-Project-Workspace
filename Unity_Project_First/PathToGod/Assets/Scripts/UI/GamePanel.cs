using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    // 私有引用
    private Button m_btnPause;
    private Button m_btnResume;
    private Text m_textScore;
    private Text m_textDiamondCount;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGamePanel, Show);
        EventCenter.AddListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.AddListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);

        Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.RemoveListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
    }

    private void Init()
    {
        m_btnPause = transform.Find("btnPause").GetComponent<Button>();
        m_btnPause.onClick.AddListener(OnBtnPauseClicked);
        m_btnResume = transform.Find("btnResume").GetComponent<Button>();
        m_btnResume.onClick.AddListener(OnBtnResumeClicked);
        m_textScore = transform.Find("textScore").GetComponent<Text>();
        m_textDiamondCount = transform.Find("imgDiamond/textDiamondCount").GetComponent<Text>();

        m_textScore.text = 0.ToString();
        m_textDiamondCount.text = 0.ToString();

        m_btnResume.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnBtnPauseClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        m_btnPause.gameObject.SetActive(false);
        m_btnResume.gameObject.SetActive(true);

        // 暂停游戏
        Time.timeScale = 0;
        GameManager.Instance.m_bIsGamePause = true;
    }

    private void OnBtnResumeClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        m_btnResume.gameObject.SetActive(false);
        m_btnPause.gameObject.SetActive(true);

        // 继续游戏
        Time.timeScale = 1;
        GameManager.Instance.m_bIsGamePause = false;
    }

    private void UpdateScoreText(int iScore)
    {
        m_textScore.text = iScore.ToString();
    }

    private void UpdateDiamondText(int iDiamond)
    {
        m_textDiamondCount.text = iDiamond.ToString();
    }
}