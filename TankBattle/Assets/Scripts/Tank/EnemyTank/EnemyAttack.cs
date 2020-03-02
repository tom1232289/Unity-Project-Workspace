using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // 公有引用
    public GameObject m_BulletPrefab;

    // 公有变量
    public const float m_fAttackInterval = 3f;

    // 私有变量
    private float m_fCurrAttackInterval;

    private void Update() {
        if (m_fCurrAttackInterval > m_fAttackInterval) {
            Instantiate(m_BulletPrefab, transform.position, transform.rotation);

            m_fCurrAttackInterval = 0;
        }
        else {
            m_fCurrAttackInterval += Time.deltaTime;
        }
    }
}
