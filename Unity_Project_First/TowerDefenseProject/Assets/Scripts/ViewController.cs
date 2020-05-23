using UnityEngine;

public class ViewController : MonoBehaviour {

    // 公有变量
    public float m_fMoveSpeed = 1;
    public float m_fWheelSpeed = 1;

    private void Update() {
        float fHorizontal = Input.GetAxis("Horizontal");
        float fVertical = Input.GetAxis("Vertical");
        float fWheel = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(new Vector3(-fVertical * m_fMoveSpeed, -fWheel * m_fWheelSpeed, fHorizontal * m_fMoveSpeed) * Time.deltaTime, Space.World);
    }
}