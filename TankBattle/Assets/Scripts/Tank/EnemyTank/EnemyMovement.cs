using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    // 公有变量
    public float m_fSpeed = 2f;             // 坦克移动的速率

    public const float m_fChangeDirection = 4f;   // 坦克改变方向的时间

    // 私有变量
    private Vector3 m_Direction;

    private Vector3 m_Rotation;
    private float m_fCurrChangeDirection;

    private void Awake() {
        m_fCurrChangeDirection = m_fChangeDirection;
    }

    private void FixedUpdate() {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "Enemy") {
            m_fCurrChangeDirection = m_fChangeDirection;
        }
    }

    private void Move() {
        if (m_fCurrChangeDirection >= m_fChangeDirection) {
            int iRandom = Random.Range(0, 5);
            if (iRandom == 0) {
                m_Direction = Vector3.up;
                m_Rotation = new Vector3(0, 0, 0);
            }
            else if (iRandom == 1) {
                m_Direction = Vector3.right;
                m_Rotation = new Vector3(0, 0, -90);
            }
            else if (iRandom == 2) {
                m_Direction = Vector3.down;
                m_Rotation = new Vector3(0, 0, 180);
            }
            else if (iRandom == 3) {
                m_Direction = Vector3.left;
                m_Rotation = new Vector3(0, 0, 90);
            }
            else if (iRandom >= 4) {
                m_Direction = Vector3.down;
                m_Rotation = new Vector3(0, 0, 180);
            }

            m_fCurrChangeDirection = 0;
        }
        else {
            m_fCurrChangeDirection += Time.fixedDeltaTime;
        }

        transform.rotation = Quaternion.Euler(m_Rotation);
        transform.Translate(m_Direction * m_fSpeed * Time.fixedDeltaTime, Space.World);
    }
}