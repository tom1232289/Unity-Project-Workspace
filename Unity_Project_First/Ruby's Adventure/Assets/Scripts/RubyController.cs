using UnityEngine;

public class RubyController : MonoBehaviour
{
    /// <summary>
    /// 公有引用
    /// </summary>
    // 齿轮飞镖的预制体
    public GameObject m_goCogBullet;

    // 受伤的音效
    public AudioClip m_acHurt;

    // 发射齿轮飞镖的音效
    public AudioClip m_acAttack;

    // 走路的音效
    public AudioClip m_acMove;

    /// <summary>
    /// 私有引用
    /// </summary>
    private Rigidbody2D m_rigidbody2d;

    private Animator m_anim;

    private AudioSource m_audioSource;

    /// <summary>
    /// 公有变量
    /// </summary>
    // Ruby移动的速度
    public float m_fSpeed = 1;

    // Ruby的最大生命值
    public float m_fMaxHp = 5;

    // Ruby受伤后的无敌时间
    public float m_fInvincibleTime = 2f;

    // 发射齿轮飞镖的力度
    public float m_fAttackForce = 300f;

    /// <summary>
    /// 私有变量
    /// </summary>
    // Ruby当前的生命值
    private float m_fCurHp;

    // Ruby是否是无敌状态
    private bool m_bIsInvincible;

    // 无敌状态下的计时器
    private float m_fInvincibleTimer;

    // Ruby当前看向
    private Vector2 m_LookDirction = new Vector2(1, 0);

    // Ruby出生点
    private Vector2 m_posBorn;

    /// <summary>
    /// 初始化组件
    /// </summary>
    private void InitComponent()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 初始化成员变量
    /// </summary>
    private void InitMemberVariable()
    {
        m_fCurHp = m_fMaxHp;
        m_posBorn = transform.position;
    }

    private void Awake()
    {
        InitComponent();
        InitMemberVariable();
    }

    private void Update()
    {
        Move();

        // 无敌状态下 => 累进无敌时间
        if (m_bIsInvincible)
        {
            m_fInvincibleTimer -= Time.deltaTime;
            // 无敌时间到
            if (m_fInvincibleTimer <= 0)
            {
                m_bIsInvincible = false;
            }
        }

        // 攻击
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }

        // 与NPC对话
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.2f, 0), m_LookDirction, 1.5f, LayerMask.GetMask("NPC"));
            if (hit != null)
            {
                NPCDialog npcDialog = hit.collider.GetComponentInChildren(typeof(NPCDialog), true) as NPCDialog;
                if (npcDialog != null)
                {
                    npcDialog.DisplayDialog();
                }
            }
        }
    }

    private void Move()
    {
        float fH = Input.GetAxis("Horizontal");
        float fV = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(fH, fV);
        // 当玩家输入的某个轴向值不为0
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            m_LookDirction = move;
            m_LookDirction.Normalize();
            // 播放走路音效
            if (!m_audioSource.isPlaying)
            {
                m_audioSource.clip = m_acMove;
                m_audioSource.Play();
            }
        }
        else
        {
            m_audioSource.Stop();
        }
        m_anim.SetFloat("Look X", m_LookDirction.x);
        m_anim.SetFloat("Look Y", m_LookDirction.y);
        m_anim.SetFloat("Speed", move.magnitude);

        Vector2 pos = transform.position;
        //pos.x = pos.x + fH * Time.deltaTime * m_fSpeed;
        //pos.y = pos.y + fV * Time.deltaTime * m_fSpeed;
        pos += (move * m_fSpeed * Time.deltaTime);
        //transform.position = pos;
        m_rigidbody2d.MovePosition(pos);
    }

    public void ChangeHp(float fAmount)
    {
        // 无敌状态下不受伤
        if (m_bIsInvincible)
        {
            return;
        }
        // 受伤一次进入无敌状态
        m_bIsInvincible = true;
        m_fInvincibleTimer = m_fInvincibleTime;

        // 受伤
        m_fCurHp = Mathf.Clamp(m_fCurHp + fAmount, 0, m_fMaxHp);
        UIHealthBar.Instance.SetValue(m_fCurHp / m_fMaxHp);
        if (fAmount < 0)
        {
            PlaySound(m_acHurt);
            m_anim.SetTrigger("Hit");
        }

        // 重生
        if (m_fCurHp <= 0)
        {
            Reborn();
        }
    }

    public float GetCurHp()
    {
        return m_fCurHp;
    }

    /// <summary>
    /// 发射齿轮飞镖
    /// </summary>
    private void Launch()
    {
        if (!GameManager.Instance.m_bIsAcceptTask)
        {
            return;
        }

        GameObject goCogBullet = Instantiate(m_goCogBullet, transform.position + Vector3.up * 0.5f + (Vector3)m_LookDirction * 0.5f, Quaternion.identity);
        CogBullet cogBullet = goCogBullet.GetComponent<CogBullet>();
        cogBullet.Launch(m_LookDirction, m_fAttackForce);
        // 播放攻击动画
        m_anim.SetTrigger("Launch");
        // 播放攻击音效
        AudioSource.PlayClipAtPoint(m_acAttack, transform.position);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    /// <summary>
    /// 重生
    /// </summary>
    private void Reborn()
    {
        m_bIsInvincible = false;
        ChangeHp(m_fMaxHp);
        transform.position = m_posBorn;
    }
}