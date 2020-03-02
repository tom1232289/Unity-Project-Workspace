using UnityEngine;

public class Enemy : MonoBehaviour {

    // 公有引用
    public AudioClip m_acDie;   // 死亡音效
    public AudioClip m_acHurt;  // 受伤音效
    public Sprite m_spriteHurt; // 受伤图片
    public GameObject m_effectDeath;  // 死亡特效
    public GameObject m_effectScore;  // 分数特效

    // 公有变量
    public float m_fDeadVelocity = 10;  // 死亡速度
    public float m_fHurtVelocity = 5;   // 受伤速度
    public bool m_bIsPig = false;
    public int m_iScore;    // 当前敌人的分数

    private void OnCollisionEnter2D(Collision2D other) {
        // 如果碰撞到的是小鸟
        if (other.transform.tag == "Bird") {
            // 小鸟受伤
            other.transform.GetComponent<Bird>().Hurt();
        }

        // 碰撞到的物体与自身的相对速度大于死亡速度，则自身 死亡
        if (other.relativeVelocity.magnitude > m_fDeadVelocity) {
            Die();
        }
        // 碰撞到的物体与自身的相对速度大于受伤速度，则自身 受伤
        else if (other.relativeVelocity.magnitude > m_fHurtVelocity) {
            Hurt();
        }
    }

    public void Die() {
        if (m_bIsPig) {
            // 从猪的数组中移除自身
            GameManager_Game.Instance.m_Pigs.Remove(this);
        }
        // 增加分数
        GameManager_Game.Instance.m_iScore += m_iScore;
        // 播放死亡音效
        AudioSource.PlayClipAtPoint(m_acDie, transform.position);
        // 销毁物体
        Destroy(gameObject);
        // 播放死亡特效
        Instantiate(m_effectDeath, transform.position, Quaternion.identity);
        // 播放得分特效
        GameObject go = Instantiate(m_effectScore, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Destroy(go, 1.5f);
    }

    private void Hurt() {
        // 播放受伤音效
        AudioSource.PlayClipAtPoint(m_acHurt, transform.position);
        // 替换受伤图片
        transform.GetComponent<SpriteRenderer>().sprite = m_spriteHurt;
    }
}