using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttr : MonoBehaviour {
    // 公有引用
    public GameObject m_FishDie;    // 鱼死亡
    public GameObject m_prefabGold;

    // 公有变量
    public int m_iMaxNum = 1;
    public int m_iMaxSpeed = 1;
    public int m_iHp = 0;
    public int m_iExp = 0;
    public int m_iGold = 0;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Border") {
            Destroy(gameObject);
        }
    }

    private void UnderAttack(int iDamage) {
        m_iHp -= iDamage;
        if (m_iHp <= 0) {
            Die();
        }
    }

    private void Die() {
        // 杀死奖励
        GameManager.Instance.m_iGold += m_iGold;
        GameManager.Instance.m_iExp += m_iExp;
        // 播放死亡动画
        GameObject goFishDie = Instantiate(m_FishDie);
        goFishDie.transform.SetParent(transform.parent, false);
        goFishDie.transform.position = transform.position;
        goFishDie.transform.rotation = transform.rotation;
        Destroy(gameObject);
        // 生成金币
        GameObject goGold = Instantiate(m_prefabGold, transform.position, Quaternion.identity);
        goGold.transform.SetParent(transform.parent);
        // 播放特效
        if (GetComponent<Ef_PlayEffect>() != null) {
            GetComponent<Ef_PlayEffect>().PlayEffect();
        }
    }
}
