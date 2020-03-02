using UnityEngine;

public class GunFollow : MonoBehaviour {

    // 私有引用
    private RectTransform m_rectTrans;

    private void Awake() {
        m_rectTrans = GameObject.Find("Order90Canvas").GetComponent<RectTransform>();
    }

    private void Update() {
        /*炮口根据鼠标转*/
        Vector3 mousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rectTrans, Input.mousePosition, Camera.main, out mousePos);
        mousePos.z = 0;
        float z = 0;
        if (mousePos.x > transform.position.x) {
            z = -Vector3.Angle(Vector3.up, mousePos - transform.position);
        }
        else {
            z = Vector3.Angle(Vector3.up, mousePos - transform.position);
        }

        transform.localRotation = Quaternion.Euler(0, 0, z);
    }
}