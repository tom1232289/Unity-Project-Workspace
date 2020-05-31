using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    // 公有变量
    public float m_fMinDamage = 50;

    // 私有引用
    private Animator m_anim;
    private PlayerHealth m_PlayerHealth;

    // 私有变量
    private bool m_bIsShoot;

    private void Awake() {
        m_anim = GetComponent<Animator>();
        m_PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Update() {
        if (m_anim.GetFloat("Shot") > 0.5f && m_PlayerHealth.m_fHealth > 0) {
            Shoot();
            m_bIsShoot = true;
        }
        else {
            m_bIsShoot = false;
        }
    }

    // 射击
    private void Shoot() {
        if (m_bIsShoot == false) {
            float fDamage = m_fMinDamage + 100 - 10 * (transform.position - m_PlayerHealth.transform.position).magnitude;
            m_PlayerHealth.UnderAttack(fDamage);
        }
    }
}