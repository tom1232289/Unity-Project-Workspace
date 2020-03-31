using SonicBloom.Koreo;
using UnityEngine;

public class ColorJump : MonoBehaviour {
    // 公有引用
    public GameObject[] m_go;

    // 公有变量
    public string m_sEventID;

    private void Start() {
        Koreographer.Instance.RegisterForEventsWithTime(m_sEventID, ChangeColor);
    }

    private void ChangeColor(KoreographyEvent koreoEvent,
        int sampleTime,
        int sampleDelta,
        DeltaSlice deltaSlice) {
        if (koreoEvent.HasColorPayload()) {
            Color targetColor = koreoEvent.GetColorValue();
            ApplyColorToObjects(targetColor);
        }
        else if (koreoEvent.HasGradientPayload()) {
            Color targetColor = koreoEvent.GetColorOfGradientAtTime(sampleTime);
            ApplyColorToObjects(targetColor);
        }
    }

    private void ApplyColorToObjects(Color color) {
        foreach (var go in m_go) {
            go.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}