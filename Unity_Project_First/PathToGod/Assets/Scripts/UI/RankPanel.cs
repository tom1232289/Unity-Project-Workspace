using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Button m_btnClose;
    private Text m_textGold;
    private Text m_textSliver;
    private Text m_textBronze;
    private Image m_imgBg;
    private GameObject m_goScoreList;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRankPanel, ShowRankPanel);

        m_btnClose = transform.Find("btnClose").GetComponent<Button>();
        m_btnClose.onClick.AddListener(OnBtnCloseClicked);
        m_textGold = transform.Find("ScoreList/Gold/textGold").GetComponent<Text>();
        m_textSliver = transform.Find("ScoreList/Sliver/textSliver").GetComponent<Text>();
        m_textBronze = transform.Find("ScoreList/Bronze/textBronze").GetComponent<Text>();
        m_imgBg = m_btnClose.GetComponent<Image>();
        m_goScoreList = transform.Find("ScoreList").gameObject;

        // 一开始不显示
        m_imgBg.color = new Color(m_imgBg.color.r, m_imgBg.color.g, m_imgBg.color.b, 0);
        m_goScoreList.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, ShowRankPanel);
    }

    private void ShowRankPanel()
    {
        gameObject.SetActive(true);
        m_imgBg.DOColor(new Color(m_imgBg.color.r, m_imgBg.color.g, m_imgBg.color.b, 0.3f), 0.3f);
        m_goScoreList.transform.DOScale(Vector3.one, 0.3f);

        // 显示分数
        int[] bestScoreArray = GameManager.Instance.GetBestScoreArray();
        if (bestScoreArray.Length != 3)
        {
            m_textGold.text = "数据不正确";
            m_textSliver.text = "数据不正确";
            m_textBronze.text = "数据不正确";
            return;
        }
        m_textGold.text = bestScoreArray[0].ToString();
        m_textSliver.text = bestScoreArray[1].ToString();
        m_textBronze.text = bestScoreArray[2].ToString();
    }

    private void OnBtnCloseClicked()
    {
        // 播放音效
        EventCenter.Broadcast(EventDefine.ClickButtonAudio);

        m_imgBg.DOColor(new Color(m_imgBg.color.r, m_imgBg.color.g, m_imgBg.color.b, 0f), 0.3f);
        m_goScoreList.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}