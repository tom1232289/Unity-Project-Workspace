using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelBtnScrollRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

    // 常量
    private const int ICOUNT = 4;

    // 公有引用
    public Toggle[] m_toggles;

    // 公有变量
    public float m_fSlideSpeed = 4f;

    // 私有引用
    private ScrollRect m_scrollRect;

    // 私有变量
    private float[] m_fPageArray = new float[ICOUNT];

    private bool m_bIsDraging;
    private float m_fTargetHorizontalPos;

    private void Awake() {
        m_scrollRect = GetComponent<ScrollRect>();
    }

    private void Start() {
        m_fPageArray = SliptNum(0, 1, ICOUNT);
    }

    private void Update() {
        if (m_bIsDraging == false) {
            m_scrollRect.horizontalNormalizedPosition = Mathf.Lerp(m_scrollRect.horizontalNormalizedPosition, m_fTargetHorizontalPos, Time.deltaTime * m_fSlideSpeed);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        m_bIsDraging = true;
    }

    public void OnEndDrag(PointerEventData eventData) {
        m_bIsDraging = false;
        // 取当前拖拽的值（0~1之间;水平方向）
        float fPagePos = m_scrollRect.horizontalNormalizedPosition;
        // 循环查找距离 拖拽值最小的页面
        int iMinIndex = 0;
        float fMinDist = Single.PositiveInfinity;
        for (int i = 0; i < ICOUNT; ++i) {
            float fDist = Mathf.Abs(fPagePos - m_fPageArray[i]);
            if (fDist < fMinDist) {
                fMinDist = fDist;
                iMinIndex = i;
            }
        }
        // 设置当前值为距离 拖拽值最小的页面
        m_fTargetHorizontalPos = m_fPageArray[iMinIndex];
        // 刷新Toggle的显示
        m_toggles[iMinIndex].isOn = true;
    }

    // 将0~1切成4份
    private float[] SliptNum(int i1, int i2, int iCount) {
        float fInterval = (float)Mathf.Abs(i2 - i1) / (iCount - 1);
        float[] ret = new float[iCount];
        for (int i = 0; i < iCount; ++i) {
            ret[i] = i1 + fInterval * i;
        }

        return ret;
    }

    // Toggle点击事件
    public void MoveToPage1(bool bIsOn) {
        if (bIsOn) {
            m_fTargetHorizontalPos = m_fPageArray[0];
        }
    }

    public void MoveToPage2(bool bIsOn) {
        if (bIsOn) {
            m_fTargetHorizontalPos = m_fPageArray[1];
        }
    }

    public void MoveToPage3(bool bIsOn) {
        if (bIsOn) {
            m_fTargetHorizontalPos = m_fPageArray[2];
        }
    }

    public void MoveToPage4(bool bIsOn) {
        if (bIsOn) {
            m_fTargetHorizontalPos = m_fPageArray[3];
        }
    }
}