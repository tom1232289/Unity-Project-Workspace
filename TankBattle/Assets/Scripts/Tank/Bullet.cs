using UnityEngine;

public class Bullet : MonoBehaviour {
    public float m_fSpeed = 10f;
    public bool m_bIsPlayerBullet;
    public AudioClip m_FireClip;    // 玩家发射子弹的声音

    // Start is called before the first frame update
    private void Start() {
        // 玩家发射子弹的声音
        if (m_bIsPlayerBullet) {
            AudioSource.PlayClipAtPoint(m_FireClip, transform.position);
        }
    }

    private void FixedUpdate() {
        transform.Translate(transform.up * m_fSpeed * Time.deltaTime, Space.World);
    }

    // Update is called once per frame
    private void Update() {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Wall") {
            other.SendMessage("Die");
            Destroy(gameObject);
        }
        else if (other.tag == "Barrier") {
            Destroy(gameObject);
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
        else if (other.tag == "Tank") {
            if (!m_bIsPlayerBullet) {
                other.SendMessage("Die");
                Destroy(gameObject);
            }
        }
    }
}