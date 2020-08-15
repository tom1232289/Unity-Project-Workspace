using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Transform m_Parent;

    private Text m_textName;

    private Text m_textTotalDiamondCount;

    private Button m_btnBack;

    private Button m_btnSelect;

    private Button m_btnBuy;

    /// <summary>
    /// 私有变量
    /// </summary>
    private ManagerVars m_managerVars;
    
    // 当前选中的人物下标
    private int m_iSelectIndex;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowShopPanel, ShowShopPanel);

        // 初始化成员变量
        m_managerVars = ManagerVars.GetManagerVars();
        m_Parent = transform.Find("ScrollRect/parent");
        m_textName = transform.Find("textName").GetComponent<Text>();
        m_textTotalDiamondCount = transform.Find("TotalDiamond/textTotalDiamondCount").GetComponent<Text>();
        m_btnBack = transform.Find("btnBack").GetComponent<Button>();
        m_btnBack.onClick.AddListener(OnBtnBackClicked);
        m_btnSelect = transform.Find("btnSelect").GetComponent<Button>();
        m_btnSelect.onClick.AddListener(OnBtnSelectClicked);
        m_btnBuy = transform.Find("btnBuy").GetComponent<Button>();
        m_btnBuy.onClick.AddListener(OnBtnBuyClicked);
    }

    private void Start()
    {
        Init();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPanel, ShowShopPanel);
    }

    private void Update()
    {
        /// <summary>
        /// 设置人物大小
        /// </summary>
        // 获取当前选中人物的下标
        m_iSelectIndex = (int)Mathf.Round(m_Parent.localPosition.x / -160);
        m_iSelectIndex = Mathf.Clamp(m_iSelectIndex, 0, m_managerVars.m_listSkinSprite.Count - 1);

        // 鼠标抬起后，让人物缓缓定位
        if (Input.GetMouseButtonUp(0))
        {
            m_Parent.DOLocalMoveX(m_iSelectIndex * -160, 0.2f);
        }

        SetItemSize();

        // 刷新UI
        RefreshUI();
    }

    private void Init()
    {
        // 设置存放皮肤的宽度
        m_Parent.GetComponent<RectTransform>().sizeDelta = new Vector2((m_managerVars.m_listSkinSprite.Count + 2) * 160, 302);

        // 生成皮肤
        for (int i = 0; i < m_managerVars.m_listSkinSprite.Count; ++i)
        {
            GameObject go = Instantiate(m_managerVars.m_SkinPre, m_Parent);
            // 未解锁
            if (GameManager.Instance.GetSkinUnlocked(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            // 已解锁
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            go.GetComponentInChildren<Image>().sprite = m_managerVars.m_listSkinSprite[i];
            go.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);
        }

        // 打开商店页面直接定位到选中的皮肤
        m_Parent.transform.localPosition = new Vector3(GameManager.Instance.GetSelectedSkin() * -160, 0, 0);
    }

    /// <summary>
    /// 设置人物图像大小，当前选中的比较大，没选中的比较小
    /// </summary>
    /// <param name="m_iSelectIndex"></param>
    private void SetItemSize()
    {
        for (int i = 0; i < m_Parent.childCount; ++i)
        {
            // 选中的人物下标
            if (i == m_iSelectIndex)
            {
                m_Parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            // 没选中的人物下标
            else
            {
                m_Parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    private void RefreshUI()
    {
        m_textName.text = m_managerVars.m_listCharacterName[m_iSelectIndex];
        m_textTotalDiamondCount.text = GameManager.Instance.GetTotalDiamond().ToString();

        // 未解锁
        if (GameManager.Instance.GetSkinUnlocked(m_iSelectIndex) == false)
        {
            m_btnSelect.gameObject.SetActive(false);
            m_btnBuy.gameObject.SetActive(true);
            m_btnBuy.GetComponentInChildren<Text>().text = m_managerVars.m_listSkinPrice[m_iSelectIndex].ToString();
        }
        // 已解锁
        else
        {
            m_btnSelect.gameObject.SetActive(true);
            m_btnBuy.gameObject.SetActive(false);
        }
    }

    private void OnBtnBackClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    private void ShowShopPanel()
    {
        gameObject.SetActive(true);
    }

    private void OnBtnSelectClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        GameManager.Instance.SetSelectedSkin(m_iSelectIndex);
        // 更换人物皮肤
        EventCenter.Broadcast(EventDefine.ChangeSkin);
        // 保存
        GameManager.Instance.Save();
        // 显示主面板
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    private void OnBtnBuyClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        int iPrice = int.Parse(m_btnBuy.GetComponentInChildren<Text>().text);
        // 钻石不足
        if (iPrice > GameManager.Instance.GetTotalDiamond())
        {
            // 总钻石闪烁 => 提示你是个穷人
            m_textTotalDiamondCount.GetComponent<Animator>().SetTrigger("Flicker");
            // 弹出提示信息 => 再次提示你是个穷人
            EventCenter.Broadcast(EventDefine.ShowHint, "钻石不足");

            return;
        }
        // 可以购买
        else
        {
            // 结算钻石
            GameManager.Instance.UpdateTotalDiamond(-iPrice);
            // 当前皮肤解锁
            GameManager.Instance.SetSkinUnlocked(m_iSelectIndex);
            // 皮肤颜色变亮
            m_Parent.GetChild(m_iSelectIndex).GetChild(0).GetComponent<Image>().color = Color.white;
            
            // 保存
            GameManager.Instance.Save();
        }
    }
}