using UnityEngine;

public class TankManager : MonoBehaviour {

    // 公有引用
    public GameObject m_ExplosionPrefab;    // 爆炸特效
    public GameObject m_ShieldPrefab;       // 护盾特效
    public AudioClip m_DieClip;             // 死亡声音

    // 公有变量
    public float m_fShieldTime = 2f;        // 护盾持续时间

    // 私有变量
    private bool m_bIsShield = true;        // 是否有护盾
    private GameObject m_Shield;            // 实例化的护盾

    private void Start() {
        m_ShieldPrefab.SetActive(true);
        Invoke("EndShield", m_fShieldTime);
    }

    private void Die() {
        if (m_bIsShield)
            return;

        // 爆炸特效
        Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
        // 播放死亡声音
        AudioSource.PlayClipAtPoint(m_DieClip,transform.position);
        // 销毁自身
        Destroy(gameObject);
        // 更新生命条
        GameManager.Instance.UpdateLife();

        if (GameManager.Instance.m_iLife > 0) {
            MapGenerator.Instance.GeneratePlayer();
        }
    }

    private void EndShield() {
        m_bIsShield = false;
        m_ShieldPrefab.SetActive(false);
    }
}