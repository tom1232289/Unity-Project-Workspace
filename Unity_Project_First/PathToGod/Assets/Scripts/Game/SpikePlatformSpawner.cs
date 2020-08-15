using UnityEngine;

public class SpikePlatformSpawner : MonoBehaviour
{
    /// <summary>
    /// 公有变量
    /// </summary>
    // 是否往左边生成
    public bool m_bIsLeft;

    // 生成几个
    public int m_iCount = 4;

    /// <summary>
    /// 私有变量
    /// </summary>
    private ManagerVars m_managerVars;

    // 下一个平台生成的位置
    private Vector3 m_posSpawnPlatform;

    private void Awake()
    {
        m_managerVars = ManagerVars.GetManagerVars();
    }

    private void Start()
    {
        /// <summary>
        /// 初始化第一个平台生成位置
        /// </summary>
        if (m_bIsLeft)
        {
            m_posSpawnPlatform = new Vector3(transform.position.x - m_managerVars.fNextXPos, transform.position.y + m_managerVars.fNextYPos, 0);
        }
        else
        {
            m_posSpawnPlatform = new Vector3(transform.position.x + m_managerVars.fNextXPos, transform.position.y + m_managerVars.fNextYPos, 0);
        }

        for (int i = 0; i < m_iCount; ++i)
        {
            SpawnNormalPlatform();
            // 改变下一次生成的位置
            if (m_bIsLeft)
            {
                m_posSpawnPlatform = new Vector3(m_posSpawnPlatform.x - m_managerVars.fNextXPos, m_posSpawnPlatform.y + m_managerVars.fNextYPos, 0);
            }
            else
            {
                m_posSpawnPlatform = new Vector3(m_posSpawnPlatform.x + m_managerVars.fNextXPos, m_posSpawnPlatform.y + m_managerVars.fNextYPos, 0);
            }
        }
    }

    private void SpawnNormalPlatform()
    {
        // 从对象池中取物体
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.SetActive(true);

        // 设置平台的位置
        go.transform.position = m_posSpawnPlatform;

        // 改变平台Sprite
        go.GetComponent<Platform>().Init(GameManager.Instance.m_CurPlatformTheme, 0);
    }
}