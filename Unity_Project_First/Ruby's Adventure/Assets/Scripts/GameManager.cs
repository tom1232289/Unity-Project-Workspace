using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// 公有变量
    /// </summary>
    // 是否接到任务了
    public bool m_bIsAcceptTask;

    // 任务是否完成了
    public bool m_bIsCompleteTask;

    // 需要修理的机器人的数量
    public int m_iNeedFixNum = 3;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 修理的机器人数量
    private int m_iFixedNum;

    private void Awake()
    {
        Instance = this;
    }

    public void AddFixedNum()
    {
        ++m_iFixedNum;
        if (m_iFixedNum >= m_iNeedFixNum)
        {
            m_bIsCompleteTask = true;
        }
    }
}