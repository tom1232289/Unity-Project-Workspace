using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    /// <summary>
    /// 公有引用
    /// </summary>
    public AudioClip m_acCompleteTask;

    /// <summary>
    /// 私有引用
    /// </summary>
    private Text m_textDialog;

    /// <summary>
    /// 公有变量
    /// </summary>
    // 对话框持续时间
    public float m_fDuringTime = 4;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 对话框持续时间计时器
    private float m_fTimerDuring;

    // 是否播放过交任务音效了
    private bool m_bIsPlayedSound;

    private void Awake()
    {
        m_textDialog = transform.Find("imgDialog/textDialog").GetComponent<Text>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_fTimerDuring > 0)
        {
            m_fTimerDuring -= Time.deltaTime;
            if (m_fTimerDuring < 0)
            {
                gameObject.SetActive(false);
                m_fTimerDuring = 0;
            }
        }
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    public void DisplayDialog()
    {
        gameObject.SetActive(true);
        m_fTimerDuring = m_fDuringTime;
        GameManager.Instance.m_bIsAcceptTask = true;

        // 任务完成的对话
        if (GameManager.Instance.m_bIsCompleteTask)
        {
            if (!m_bIsPlayedSound)
            {
                m_textDialog.text = "Ruby你真是太棒了！";
                AudioSource.PlayClipAtPoint(m_acCompleteTask, transform.position);
                m_bIsPlayedSound = true;
            }
        }
    }
}