using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 公有引用
    public GameObject m_ExplosionPrefab;

    private void Die() {
        // 爆炸特效
        Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
        // 销毁自身
        Destroy(gameObject);
        // 更新分数
        GameManager.Instance.UpdateScore();
    }
}
