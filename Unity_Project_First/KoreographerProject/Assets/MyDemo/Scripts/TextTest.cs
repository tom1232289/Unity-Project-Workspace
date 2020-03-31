using SonicBloom.Koreo;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour {

    // 公有变量
    public string m_sEventID;

    // 私有引用
    private Text m_text;

    // 私有变量
    private KoreographyEvent m_keCurEvent;

    private void Awake() {
        m_text = GetComponent<Text>();
    }

    private void Start() {
        Koreographer.Instance.RegisterForEventsWithTime(m_sEventID, UpdateText);
    }

    private void UpdateText(KoreographyEvent koreoEvent,
        int sampleTime,
        int sampleDelta,
        DeltaSlice deltaSlice) {
        // 判断当前事件是否有文本负荷
        if (koreoEvent.HasTextPayload()) {
            // 更新文本的条件
            // 1.当前存储的事件为空
            // 2.事件叠加时取后面的事件
            if (m_keCurEvent == null || m_keCurEvent.StartSample < koreoEvent.StartSample) {
                // 更新文本
                m_text.text = koreoEvent.GetTextValue();
                // 保存此次检测到的事件
                m_keCurEvent = koreoEvent;
            }
            // 存储的最后一拍的时间 < 当前拍子的时间
            if (m_keCurEvent.EndSample < sampleTime) {
                // 滞空
                m_text.text = string.Empty;
                m_keCurEvent = null;
            }
        }
    }
}