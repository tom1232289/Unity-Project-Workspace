using UnityEngine;

public class TankAttack : MonoBehaviour {
    // 公有引用
    public GameObject m_BulletPrefab;

    // 公有变量
    public const float m_fTimeInterval = 0.4f;
    public float m_fFireInterval;

    private void Awake() {
        m_fFireInterval = m_fTimeInterval;
    }

    // Update is called once per frame
    private void Update() {
        if (GameManager.Instance.m_bIsGameover)
            return;

        if (m_fFireInterval > m_fTimeInterval) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameObject go = Instantiate(m_BulletPrefab, transform.position, transform.rotation);
                go.GetComponent<Bullet>().m_bIsPlayerBullet = true;
                m_fFireInterval = 0;
            }
        }
        else {
            m_fFireInterval += Time.deltaTime;
        }
    }
}