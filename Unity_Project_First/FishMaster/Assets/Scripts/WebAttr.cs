using UnityEngine;

public class WebAttr : MonoBehaviour {

    // 公有变量
    public float m_fDuration = 0;    // 网的持续时间
    public int m_iDamage = 0;

    private void Start() {
        Destroy(gameObject, m_fDuration);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Fish") {
            other.SendMessage("UnderAttack", m_iDamage);
        }
    }
}