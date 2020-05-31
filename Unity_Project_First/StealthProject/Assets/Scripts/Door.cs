using UnityEngine;

public class Door : MonoBehaviour {

    // 公有引用
    public AudioSource m_asAccessDenied;    // 拒绝进入的音效

    // 公有变量
    public bool m_bRequireKey;     // 是否需要钥匙才能打开

    // 私有引用
    private Animator m_anim;
    private AudioSource m_as;   // 开门、关门的音效

    // 私有变量
    private int m_iCount;   // 进入区域的数量
    
    private void Awake() {
        m_anim = GetComponent<Animator>();
        m_as = GetComponent<AudioSource>();
    }

    private void Update() {
        // 开门 或 关门
        m_anim.SetBool("Open", m_iCount > 0);
        // 门在切换状态中 时 播放音效
        if (m_anim.IsInTransition(0)) {
            m_as.Play();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" || (other.tag == "Enemy" && !other.isTrigger)) {
            // 不需要钥匙
            if (!m_bRequireKey) {
                ++m_iCount;
            }
            // 需要钥匙
            else {
                if (other.tag == "Player") {
                    // 玩家有钥匙 => 开门
                    if (other.GetComponent<Player>().m_bHasKey) {
                        ++m_iCount;
                    }
                    // 玩家没钥匙 => 滚(播放拒绝音效)
                    else {
                        m_asAccessDenied.Play();
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player" || (other.tag == "Enemy" && !other.isTrigger)) {
            // 不需要钥匙
            if (!m_bRequireKey) {
                --m_iCount;
            }
            // 需要钥匙
            else {
                if (other.tag == "Player") {
                    // 玩家有钥匙 => 关门
                    if (other.GetComponent<Player>().m_bHasKey) {
                        --m_iCount;
                    }
                }
            }
        }
    }
}