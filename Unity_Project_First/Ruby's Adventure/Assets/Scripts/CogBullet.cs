using UnityEngine;

public class CogBullet : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Rigidbody2D m_rigidbody2d;

    private void Awake()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 dir, float fForce)
    {
        m_rigidbody2d.AddForce(dir * fForce);
    }
}