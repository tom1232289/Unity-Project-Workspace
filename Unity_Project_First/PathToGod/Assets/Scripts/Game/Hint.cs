using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Image m_imgHint;

    private Text m_textHint;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 防止多次点击
    private bool m_bIsShowing;

    private void Awake()
    {
        EventCenter.AddListener<string>(EventDefine.ShowHint, ShowHint);

        m_imgHint = GetComponent<Image>();
        m_textHint = GetComponentInChildren<Text>();

        // 一开始不显示
        m_imgHint.color = new Color(m_imgHint.color.r, m_imgHint.color.g, m_imgHint.color.b, 0);
        m_textHint.color = new Color(m_textHint.color.r, m_textHint.color.g, m_textHint.color.b, 0);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.ShowHint, ShowHint);
    }

    private void ShowHint(string sHint)
    {
        // 防止多次点击
        if (m_bIsShowing)
            return;

        m_bIsShowing = true;
        transform.localPosition = new Vector3(0, -100, 0);
        m_imgHint.DOColor(new Color(m_imgHint.color.r, m_imgHint.color.g, m_imgHint.color.b, 0.4f), 0.1f);
        m_textHint.DOColor(new Color(m_textHint.color.r, m_textHint.color.g, m_textHint.color.b, 1f), 0.1f);
        transform.DOLocalMoveY(0, 0.3f).OnComplete(() => {
            StartCoroutine("DealyMoveUp");
        });
    }

    private IEnumerator DealyMoveUp()
    {
        yield return new WaitForSeconds(1);
        m_imgHint.DOColor(new Color(m_imgHint.color.r, m_imgHint.color.g, m_imgHint.color.b, 0f), 0.1f);
        m_textHint.DOColor(new Color(m_textHint.color.r, m_textHint.color.g, m_textHint.color.b, 0f), 0.1f);
        transform.DOLocalMoveY(100, 0.3f).OnComplete(() => {
            m_bIsShowing = false;
        });
    }
}