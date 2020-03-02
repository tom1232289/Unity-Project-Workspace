using UnityEngine;

public class Movement : MonoBehaviour {
    public float m_fSpeed = 3;

    // Start is called before the first frame update
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        float fHorizontal = Input.GetAxis("Horizontal");
        float fVertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(fHorizontal, fVertical, 0) * Time.deltaTime * m_fSpeed);
    }
}