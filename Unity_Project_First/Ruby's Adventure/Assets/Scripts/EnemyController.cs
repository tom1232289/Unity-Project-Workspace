using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// 私有引用
    /// </summary>
    private Rigidbody2D m_rigidbody2d;

    private Animator m_anim;

    /// <summary>
    /// 公有变量
    /// </summary>
    // 移动的方向（true：水平方向；false：垂直方向）
    public bool m_bIsHorizontal;
    // 移动的速度
    public float m_fMoveSpeed = 1;
    // 经过__时间改变方向
    public float m_fChangeDirTime = 3;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 移动的方向（1：正方向；-1：反方向）
    private int m_iDirection = 1;
    // 改变方向计时器
    private float m_fChangeDirTimer;

    private void Awake()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //m_anim.SetFloat("fMove", m_iDirection);
        //m_anim.SetBool("bIsHorizontal", m_bIsHorizontal);
        PlayMoveAnimation();
    }

    private void Update()
    {
        Move();

        ChangeDirection();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "player")
        {
            RubyController rubyController = other.gameObject.GetComponent<RubyController>();
            rubyController.ChangeHp(-1);
        }
    }

    private void Move()
    {
        Vector2 pos = transform.position;
        if (m_bIsHorizontal)
        {
            pos.x += Time.deltaTime * m_fMoveSpeed* m_iDirection;
        }
        else
        {
            pos.y += Time.deltaTime * m_fMoveSpeed* m_iDirection;
        }
        m_rigidbody2d.MovePosition(pos);
    }

    private void ChangeDirection()
    {
        m_fChangeDirTimer += Time.deltaTime;
        if (m_fChangeDirTimer >= m_fChangeDirTime)
        {
            m_iDirection *= -1;
            //m_anim.SetFloat("fMove", m_iDirection);
            PlayMoveAnimation();
            m_fChangeDirTimer = 0;
        }
    }

    private void PlayMoveAnimation()
    {
        if (m_bIsHorizontal)
        {
            m_anim.SetFloat("fMoveX", m_iDirection);
            m_anim.SetFloat("fMoveY", 0);
        }
        else
        {
            m_anim.SetFloat("fMoveX", 0);
            m_anim.SetFloat("fMoveY", m_iDirection);
        }
    }
}
