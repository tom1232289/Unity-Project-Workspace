using UnityEngine;
using UnityEngine.UI;

public class MyToggle : MonoBehaviour {

    // 公有引用
    public GameObject m_isOnGameObject;

    public GameObject m_isOffGameObject;

    // 私有引用
    private Toggle m_toggle;

    private void Awake() {
        m_toggle = GetComponent<Toggle>();
    }

    private void Start() {
        OnValueChanged(m_toggle.isOn);
    }

    public void OnValueChanged(bool isOn) {
        m_isOnGameObject.SetActive(isOn);
        m_isOffGameObject.SetActive(!isOn);
    }
}