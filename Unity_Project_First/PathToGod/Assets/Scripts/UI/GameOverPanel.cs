using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Text m_textScore;
    private Text m_textBestScore;
    private Text m_textAddDiamondCount;
    private Button m_btnRank;
    private Button m_btnHome;
    private Button m_btnRetry;
    private Image m_imgNewBestScore;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGameOverPanel, ShowGameOverPanel);

        gameObject.SetActive(false);
        Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, ShowGameOverPanel);
    }

    private void Init()
    {
        m_textScore = transform.Find("textScore").GetComponent<Text>();
        m_textBestScore = transform.Find("textBestScore").GetComponent<Text>();
        m_textAddDiamondCount = transform.Find("AddDiamond/textAddDiamondCount").GetComponent<Text>();
        m_btnRank = transform.Find("btnRank").GetComponent<Button>();
        m_btnRank.onClick.AddListener(OnBtnRankClicked);
        m_btnHome = transform.Find("btnHome").GetComponent<Button>();
        m_btnHome.onClick.AddListener(OnBtnHomeClicked);
        m_btnRetry = transform.Find("btnRetry").GetComponent<Button>();
        m_btnRetry.onClick.AddListener(OnBtnRetryClicked);
        m_imgNewBestScore = transform.Find("imgNewBestScore").GetComponent<Image>();
    }

    private void ShowGameOverPanel()
    {
        m_textScore.text = GameManager.Instance.GetGameScore().ToString();

        if (GameManager.Instance.GetGameScore() > GameManager.Instance.GetBestScore())
        {
            m_textBestScore.text = "最高分  " + GameManager.Instance.GetGameScore();
            m_imgNewBestScore.gameObject.SetActive(true);
        }
        else
        {
            m_textBestScore.text = "最高分  " + GameManager.Instance.GetBestScore();
            m_imgNewBestScore.gameObject.SetActive(false);
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());

        m_textAddDiamondCount.text = "+" + GameManager.Instance.GetAddDiamondCount().ToString();

        // 更新总的钻石数量
        GameManager.Instance.UpdateTotalDiamond(GameManager.Instance.GetAddDiamondCount());

        gameObject.SetActive(true);
    }

    private void OnBtnRankClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    private void OnBtnHomeClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        GameData.m_bIsRetryGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnBtnRetryClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        GameData.m_bIsRetryGame = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}