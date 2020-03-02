using UnityEngine;

public class Shoot : MonoBehaviour {
    public GameObject m_bullet;
    public float m_fSpeed = 5;

    // Start is called before the first frame update
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {  // 0表示鼠标左键
            GameObject bullet = GameObject.Instantiate(m_bullet, transform.position, transform.rotation);
            Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();
            rigidBody.velocity = transform.forward * m_fSpeed;
        }
    }
}