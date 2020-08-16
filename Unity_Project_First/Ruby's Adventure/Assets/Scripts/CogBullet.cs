using UnityEngine;

public class CogBullet : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Rigidbody2D m_rigidbody2d;

    /// <summary>
    /// 公有变量
    /// </summary>
    public int m_iDestroyDis = 100;

    private void Awake()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.magnitude > m_iDestroyDis)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            enemyController.Fix();
        }
        Destroy(gameObject);
    }

    public void Launch(Vector2 dir, float fForce)
    {
        m_rigidbody2d.AddForce(dir * fForce);
    }
}