using UnityEngine;

public class Pig : MonoBehaviour {

    // 公有引用
    public Sprite m_spriteHurt;     // 猪受伤的图片
    public GameObject m_effectBoom;
    public GameObject m_effectScore;
    public AudioClip m_clipBirdCollision;
    public AudioClip m_clipPigDie;
    public AudioClip m_clipWoodDie;
    public AudioClip m_clipPigHurt;
    public AudioClip m_clipWoodHurt;

    // 公有变量
    public float m_fMaxVelocity = 10;
    public float m_fMinVelocity = 5;
    public bool m_bIsPig;

    // 私有变量
    private SpriteRenderer m_sr;

    private void Awake() {
        m_sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // 如果碰撞到的是小鸟，则播放小鸟碰撞的音效
        if (other.transform.tag == "Player") {
            AudioSource.PlayClipAtPoint(m_clipBirdCollision, transform.position);
            // 小鸟受伤
            other.transform.GetComponent<Bird>().Hurt();
        }

        if (other.relativeVelocity.magnitude > m_fMaxVelocity) {
            Die();
        }
        else if (other.relativeVelocity.magnitude > m_fMinVelocity && other.relativeVelocity.magnitude < m_fMaxVelocity) {
            m_sr.sprite = m_spriteHurt;
            // 播放受伤音效
            if (m_bIsPig) {
                AudioSource.PlayClipAtPoint(m_clipPigHurt, transform.position);
            }
            else {
                AudioSource.PlayClipAtPoint(m_clipWoodHurt, transform.position);
            }
        }
    }

    public void Die() {
        if (m_bIsPig) {
            AudioSource.PlayClipAtPoint(m_clipPigDie, Camera.main.transform.position);
            GameManager.Instance.m_listPigs.Remove(this);
        }
        else {
            AudioSource.PlayClipAtPoint(m_clipWoodDie, transform.position);
        }
        Destroy(gameObject);
        // 播放爆炸特效
        Instantiate(m_effectBoom, transform.position, Quaternion.identity);
        // 播放加分特效
        GameObject go = Instantiate(m_effectScore, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Destroy(go, 1.5f);
    }
}