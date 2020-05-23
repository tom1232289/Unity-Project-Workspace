using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    // 公有引用
    public GameObject m_effectDie;

    // 公有变量
    public float m_fSpeed = 5f;
    public float m_fHp = 150;

    // 私有引用
    private Slider m_sliderHp;

    // 私有变量
    private int m_iPos = 0;
    private float m_fTotalHp;

    private void Awake() {
        m_sliderHp = GetComponentInChildren<Slider>();
        m_fTotalHp = m_fHp;
    }

    private void Update() {
        Move();
    }

    private void OnDestroy() {
        --EnemySpawner.m_iAliveCount;
    }

    private void Move() {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.m_listRoadPos[m_iPos].position, m_fSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, GameManager.Instance.m_listRoadPos[m_iPos].position) < 0.2f)
            ++m_iPos;

        if (m_iPos > GameManager.Instance.m_listRoadPos.Count - 1) {
            ReachDestination();
        }
    }

    // 到达终点
    private void ReachDestination() {
        GameManager.Instance.Lose();
        Destroy(gameObject);
    }

    public void UnderAttack(float iDamage) {
        if (m_fHp <= 0)
            return;

        m_fHp -= iDamage;
        // 更新血条显示
        m_sliderHp.value = (float)m_fHp / m_fTotalHp;

        if (m_fHp <= 0) {
            Die();
        }
    }

    private void Die() {
        GameObject.Instantiate(m_effectDie, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}