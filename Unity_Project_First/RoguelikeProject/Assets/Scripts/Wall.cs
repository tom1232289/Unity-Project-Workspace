using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // 公有引用
    public Sprite m_spriteHurt;     // 受伤时的图片
    public AudioClip[] m_acChops;      // 受伤的音效

    // 公有变量
    public int m_iHp = 2;   // 生命值

    // 受到攻击
    private void UnderAttack() {
        m_iHp -= 1;
        // 播放受伤音效
        int iRandom = Random.Range(0, m_acChops.Length);
        AudioSource.PlayClipAtPoint(m_acChops[iRandom], Camera.main.transform.position);
        // 受伤
        if (m_iHp == 1) {
            GetComponent<SpriteRenderer>().sprite = m_spriteHurt;
        }
        // 死亡
        else {
            Die();
        }
    }

    private void Die() {
        Destroy(gameObject);
    }
}
