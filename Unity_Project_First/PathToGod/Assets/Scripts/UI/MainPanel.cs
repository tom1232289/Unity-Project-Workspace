using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Button m_btnStart;
    private Button m_btnShop;
    private Button m_btnRank;
    private Button m_btnSound;
    private Button m_btnReset;

    /// <summary>
    /// 私有变量
    /// </summary>
    private ManagerVars m_managerVars;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowMainPanel, ShowMainPanel);
        EventCenter.AddListener(EventDefine.ChangeSkin, ChangeSkin);

        Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowMainPanel, ShowMainPanel);
        EventCenter.RemoveListener(EventDefine.ChangeSkin, ChangeSkin);
    }

    private void Start()
    {
        if (GameData.m_bIsRetryGame)
        {
            OnBtnStartClicked();
        }

        ChangeSkin();
        // 改变音效按钮的图片
        ChangeSoundBtnSprite();
    }

    private void Init()
    {
        m_managerVars = ManagerVars.GetManagerVars();
        
        m_btnStart = transform.Find("btnStart").GetComponent<Button>();
        m_btnStart.onClick.AddListener(OnBtnStartClicked);      // 注册点击事件
        m_btnShop = transform.Find("Btns/btnShop").GetComponent<Button>();
        m_btnShop.onClick.AddListener(OnBtnShopClicked);
        m_btnRank = transform.Find("Btns/btnRank").GetComponent<Button>();
        m_btnRank.onClick.AddListener(OnBtnRankClicked);
        m_btnSound = transform.Find("Btns/btnSound").GetComponent<Button>();
        m_btnSound.onClick.AddListener(OnBtnSoundClicked);
        m_btnReset = transform.Find("Btns/btnReset").GetComponent<Button>();
        m_btnReset.onClick.AddListener(OnBtnResetClicked);
    }

    private void OnBtnStartClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        GameManager.Instance.m_bIsGameStarted = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }

    private void OnBtnShopClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        EventCenter.Broadcast(EventDefine.ShowShopPanel);
        gameObject.SetActive(false);
    }

    private void OnBtnRankClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    private void OnBtnSoundClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);
        // 改变音效开关状态
        GameManager.Instance.SetMusicOn(!GameManager.Instance.GetMusicOn());
        // 改变音效按钮的图片
        ChangeSoundBtnSprite();
    }

    /// <summary>
    /// 改变音效按钮的图片
    /// </summary>
    private void ChangeSoundBtnSprite()
    {
        if (GameManager.Instance.GetMusicOn())
        {
            m_btnSound.transform.GetChild(0).GetComponent<Image>().sprite = m_managerVars.m_spriteOpenMusic;
        }
        else
        {
            m_btnSound.transform.GetChild(0).GetComponent<Image>().sprite = m_managerVars.m_spriteCloseMusic;
        }
    }

    private void OnBtnResetClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        EventCenter.Broadcast(EventDefine.ShowResetPanel);
    }

    private void ShowMainPanel()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 更换 主界面的商店按钮上的 皮肤
    /// </summary>
    private void ChangeSkin()
    {
        m_btnShop.transform.GetChild(0).GetComponent<Image>().sprite = m_managerVars.m_listSkinSprite[GameManager.Instance.GetSelectedSkin()];
    }
}