using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public class CubeTest : MonoBehaviour
{
    // 公有变量
    public string m_sEventID;
    public float m_minScale = 0.5f;
    public float m_maxScale = 1.5f;

    private void Start() {
        Koreographer.Instance.RegisterForEventsWithTime(m_sEventID, ChangeCubeScale);
    }

    private void ChangeCubeScale(KoreographyEvent koreoEvent,
        int sampleTime,
        int sampleDelta,
        DeltaSlice deltaSlice) {
        if (koreoEvent.HasCurvePayload()) {
            float fCurValue = koreoEvent.GetValueOfCurveAtTime(sampleTime);  // 返回0~1之间的值
            transform.localScale = Vector3.one * Mathf.Lerp(m_minScale, m_maxScale, fCurValue);
        }
    }
}
