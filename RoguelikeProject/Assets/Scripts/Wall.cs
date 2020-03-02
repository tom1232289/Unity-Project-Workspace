using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // 公有引用 
    public Sprite m_spriteHurt; // 墙受伤的图片
    public AudioClip[] m_acChops;   // 墙受到攻击的声音

    // 公有变量
    public int m_iHp = 2;   // 墙的生命值

    // 私有引用
    private SpriteRenderer m_sr;

    private void Awake() {
        m_sr = GetComponent<SpriteRenderer>();
    }

    private void UnderAttack() {
        // 播放音效
        int iRandom = Random.Range(0, m_acChops.Length);
        AudioSource.PlayClipAtPoint(m_acChops[iRandom],Camera.main.transform.position);
        // 掉血
        m_iHp -= 1;
        // 受伤
        if (m_iHp == 1) {
            m_sr.sprite = m_spriteHurt;
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
