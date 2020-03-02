using UnityEngine;

public class MoveToEndPos : MonoBehaviour {
    public float m_fSpeed = 20f;

    private Transform m_EndPos;

    private void Awake() {
        m_EndPos = GameObject.Find("EndPos").transform;
    }

    // Update is called once per frame
    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, m_EndPos.position, m_fSpeed * Time.deltaTime);
    }
}