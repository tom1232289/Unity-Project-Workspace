using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float m_fForce = 10;
    private Rigidbody m_rd;
    public Text m_text;
    public int m_iSorce;
    public GameObject m_winText;

    // Start is called before the first frame update
    private void Start() {
        m_rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update() {
        var fHorizontal = Input.GetAxis("Horizontal");
        var fVertical = Input.GetAxis("Vertical");
        m_rd.AddForce(new Vector3(fHorizontal, 0, fVertical) * m_fForce);
    }

    // 碰撞检测
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "PickUp")
            Destroy(collision.collider.gameObject);
    }

    // 触发检测
    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "PickUp") {
            ++m_iSorce;
            m_text.text = "得分：" + m_iSorce.ToString();
            Destroy(collider.gameObject);
            if (m_iSorce == 10) {
                m_winText.SetActive(true);
            }
        }
    }
}