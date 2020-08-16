using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static UIHealthBar Instance { get; private set; }

    /// <summary>
    /// 私有引用
    /// </summary>
    private RectTransform m_rectTransform;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 血条开始的大小
    private float m_fOriginalSize;

    private void Awake()
    {
        Instance = this;
        m_rectTransform = GetComponent<RectTransform>();
        m_fOriginalSize = m_rectTransform.rect.width;
    }

    /// <summary>
    /// 设置血条的百分比
    /// </summary>
    /// <param name="百分比"></param>
    public void SetValue(float fFillPercent)
    {
        m_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_fOriginalSize * fFillPercent);
    }
}