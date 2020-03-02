using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {
    public float m_fMoveSpeed = 3f;
    public Sprite[] m_TankSprites; // 上、右、下、左
    public GameObject m_BulletPrefab;
    public GameObject m_ExplosionPrefab;
    public float m_fChangeDirctionTime; // 地方坦克转向的时间

    private float m_fHorizontal;
    private float m_fVertical;
    private SpriteRenderer m_SpriteRenderer;
    private Vector3 m_BulletEulerAngles;
    private float m_fTimeInterval;

    private const float TIME_INTERVAL = 4f;

    private void Awake() {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        Move();
    }

    private void Start() {
        m_fChangeDirctionTime = TIME_INTERVAL;
    }

    private void Update() {
        // 攻击间隔
        if (m_fTimeInterval > TIME_INTERVAL) {
            Attack();
        }
        else {
            m_fTimeInterval += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "Enemy") {
            m_fChangeDirctionTime = TIME_INTERVAL;
        }
    }

    private void Move() {
        if (m_fChangeDirctionTime >= TIME_INTERVAL) {
            int iRangeNum = Random.Range(0, 5);
            if (iRangeNum == 0) {
                m_fVertical = 1;
                m_fHorizontal = 0;
            }
            else if (iRangeNum == 1) {
                m_fVertical = 0;
                m_fHorizontal = 1;
            }
            else if (iRangeNum == 2) {
                m_fVertical = -1;
                m_fHorizontal = 0;
            }
            else if (iRangeNum == 3) {
                m_fVertical = 0;
                m_fHorizontal = -1;
            }
            else if (iRangeNum >= 4) {
                m_fVertical = -1;
                m_fHorizontal = 0;
            }

            m_fChangeDirctionTime = 0;
        }

        m_fChangeDirctionTime += Time.fixedDeltaTime;

        if (m_fVertical > 0) {
            m_SpriteRenderer.sprite = m_TankSprites[0];
            m_BulletEulerAngles = new Vector3(0, 0, 0);
        }
        else if (m_fVertical < 0) {
            m_SpriteRenderer.sprite = m_TankSprites[2];
            m_BulletEulerAngles = new Vector3(0, 0, 180);
        }
        transform.Translate(Vector3.up * m_fVertical * m_fMoveSpeed * Time.fixedDeltaTime);

        if (m_fHorizontal > 0) {
            m_SpriteRenderer.sprite = m_TankSprites[1];
            m_BulletEulerAngles = new Vector3(0, 0, -90);
        }
        else if (m_fHorizontal < 0) {
            m_SpriteRenderer.sprite = m_TankSprites[3];
            m_BulletEulerAngles = new Vector3(0, 0, 90);
        }
        transform.Translate(Vector3.right * m_fHorizontal * m_fMoveSpeed * Time.fixedDeltaTime);
    }

    private void Attack() {
        // 子弹产生的角度：当前坦克的角度+子弹应该旋转的角度
        Instantiate(m_BulletPrefab, transform.position, Quaternion.Euler( /*transform.eulerAngles + */m_BulletEulerAngles));
        m_fTimeInterval = 0;
    }

    private void Die() {
        // 增加杀敌数
        ++PlayerManager.Instanse.m_iPlayerScore;

        // 产生爆炸特效
        Instantiate(m_ExplosionPrefab, transform.position, transform.rotation);

        // 死亡
        Destroy(gameObject);
    }
}