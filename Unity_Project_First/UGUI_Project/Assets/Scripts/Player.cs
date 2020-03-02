using UnityEngine;

public class Player : MonoBehaviour {
    public float m_fSpeed = 90;    // 旋转的度数

    // Update is called once per frame
    private void Update() {
        transform.Rotate(Vector3.up * m_fSpeed * Time.deltaTime, Space.Self);
    }

    public void ChangeSpeed(float fNewSpeed) {
        m_fSpeed = fNewSpeed;
    }
}