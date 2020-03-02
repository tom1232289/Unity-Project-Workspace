using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Rigidbody m_rd;
    public float m_fForce = 1;
    private float m_fScores;
    public Text m_textScore;
    public GameObject m_textWinText;
 
    // Start is called before the first frame update
    private void Start() {
        m_rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update() {
        float fHorizontal = Input.GetAxis("Horizontal");
        float fVertical = Input.GetAxis("Vertical");
        m_rd.AddForce(new Vector3(fHorizontal, 0, fVertical) * m_fForce);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "PickUp") {
            Destroy(collider.gameObject);
            ++m_fScores;
            m_textScore.text = "Score:" + m_fScores.ToString();
            if (m_fScores == 12) {
                m_textWinText.SetActive(true);
            }
        }
    }
}