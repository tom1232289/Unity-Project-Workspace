using UnityEngine;

public class Bullet : MonoBehaviour {
    public float m_fSpeed = 10f;
    public bool m_bIsPlayerBullet;

    // Start is called before the first frame update
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        transform.Translate(transform.up * m_fSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Tank") {
            if (!m_bIsPlayerBullet) {
                other.SendMessage("Die");
                Destroy(gameObject);
            }
        }
        else if (other.tag == "Heart") {
            other.SendMessage("Die");
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy") {
            if (m_bIsPlayerBullet) {
                other.SendMessage("Die");
                Destroy(gameObject);
            }
        }
        else if (other.tag == "Wall") {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Barrier") {
            if (m_bIsPlayerBullet) {
                other.SendMessage("PlayAudio");
            }
            Destroy(gameObject);
        }
    }
}