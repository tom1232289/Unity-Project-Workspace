using UnityEngine;

public class Lift : MonoBehaviour {

    // 公有引用
    public Transform m_transOuterLeft;      // 左边的外侧门
    public Transform m_transOuterRight;     // 右边的外侧门
    public Transform m_transInnerLeft;      // 左边的内侧门
    public Transform m_transInnerRight;     // 左边的内侧门

    // 公有变量
    public float m_fSpeed = 1;  // 内侧门开 的 速度
    public float m_fExitTime = 2;   // 在电梯内呆这么长时间后 电梯上升

    // 私有变量
    private const float m_fDiff = 0.4f;
    private bool m_bPlayerIsIn;
    private float m_fCurExitTime;

    private void Update() {
        float fX = Mathf.Lerp(m_transInnerLeft.position.x, m_transOuterLeft.position.x - m_fDiff, m_fSpeed * Time.deltaTime);
        m_transInnerLeft.position = new Vector3(fX, m_transInnerLeft.position.y, m_transInnerLeft.position.z);
        fX = Mathf.Lerp(m_transInnerRight.position.x, m_transOuterRight.position.x + m_fDiff, m_fSpeed * Time.deltaTime);
        m_transInnerRight.position = new Vector3(fX, m_transInnerRight.position.y, m_transInnerRight.position.z);

        if (m_bPlayerIsIn) {
            m_fCurExitTime += Time.deltaTime;
            if (m_fCurExitTime >= m_fExitTime) {
                transform.Translate(Vector3.up * Time.deltaTime);
                if (m_fCurExitTime >= 5) {
                    GameManager.Instance.Win();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            m_bPlayerIsIn = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            m_bPlayerIsIn = false;
        }
    }
}