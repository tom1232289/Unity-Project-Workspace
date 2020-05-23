using UnityEngine;

public class Bullet : MonoBehaviour {

    // 公有引用
    public GameObject m_effectExplosion;    // 子弹爆炸特效

    // 公有变量
    public int m_iDamage = 50;
    public float m_fSpeed = 40;

    // 私有变量
    private Transform m_posTarget;

    private void Update() {
        if (m_posTarget == null) {
            Die();
            return;
        }

        transform.LookAt(m_posTarget);
        transform.Translate(Vector3.forward * m_fSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            other.GetComponent<Enemy>().UnderAttack(m_iDamage);
            Die();
        }
    }

    public void SetTarget(Transform targetPos) {
        m_posTarget = targetPos;
    }

    private void Die() {
        GameObject.Instantiate(m_effectExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}