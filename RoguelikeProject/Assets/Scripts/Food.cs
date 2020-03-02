using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // 公有引用
    public AudioClip[] m_acEatFood; // 吃食物的音效

    // 公有变量
    public int m_iScore = 10;   // 食物的分数

    private void EatFood() {
        // 播放音效
        int iRandom = Random.Range(0, m_acEatFood.Length);
        AudioSource.PlayClipAtPoint(m_acEatFood[iRandom], Camera.main.transform.position);
        GameManager.Instance.AddFood(m_iScore);
        Die();
    }

    private void Die() {
        Destroy(gameObject);
    }
}
