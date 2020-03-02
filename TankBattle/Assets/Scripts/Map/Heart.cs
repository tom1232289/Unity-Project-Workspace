using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour {
    // 公有引用
    public Sprite m_BrokenSprite;
    public GameObject m_ExplosionPrefab;

    // 私有引用
    private SpriteRenderer m_SpriteRenderer;
    private AudioSource m_HeartDamageAudio;

    private void Awake() {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_HeartDamageAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die() {
        // 老家爆炸
        m_SpriteRenderer.sprite = m_BrokenSprite;
        // 爆炸特效
        Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
        // 爆炸音效
        m_HeartDamageAudio.Play();
        // 游戏失败，返回主界面
        GameManager.Instance.GameOver();
    }
}
