using UnityEngine;

public enum PlatformType
{
    Grass,
    Winter
}

public class PlatformSpawner : MonoBehaviour
{
    /// <summary>
    /// 公有变量
    /// </summary>
    // 一开始的生成位置
    public Vector3 m_posStartSpawn;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 平台生成的位置
    private Vector3 m_posSpawnPlatform;

    // 是否是向左生成
    private bool m_bIsLeftSpawn;

    // 生成的平台数量
    private int m_iSpawnPlatformCount;

    // 本次生成的平台的主题
    private Sprite m_CurPlatformTheme;

    // 本次生成的平台的主题的枚举类型
    private PlatformType m_CurPlatformType;

    private ManagerVars m_managerVars;

    private void Awake()
    {
        m_managerVars = ManagerVars.GetManagerVars();

        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }

    private void Start()
    {
        /// <summary>
        /// 生成平台
        /// </summary>
        m_posSpawnPlatform = m_posStartSpawn;

        m_CurPlatformTheme = RandomPlatformTheme();
        GameManager.Instance.m_CurPlatformTheme = m_CurPlatformTheme;

        // 一开始创建5个平台，都是向左的
        for (int i = 0; i < 5; ++i)
        {
            m_iSpawnPlatformCount = 5;
            DecidePath();
        }

        /// <summary>
        /// 生成人物
        /// </summary>
        Instantiate(m_managerVars.m_prefabCharacter, new Vector3(0, -1.8f, 0), Quaternion.identity);
    }

    /// <summary>
    /// 随机平台的主题
    /// </summary>
    /// <returns></returns>
    private Sprite RandomPlatformTheme()
    {
        int iRandom = Random.Range(0, m_managerVars.m_listPlatformTheme.Count);

        if (iRandom == 2)
        {
            m_CurPlatformType = PlatformType.Winter;
        }
        else
        {
            m_CurPlatformType = PlatformType.Grass;
        }

        return m_managerVars.m_listPlatformTheme[iRandom];
    }

    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        // 连续生成m_iSpawnPlatformCount个平台
        if (m_iSpawnPlatformCount > 0)
        {
            --m_iSpawnPlatformCount;
            SpawnPlatform();
        }
        // 转换方向，随机生成1~3个平台
        else
        {
            m_bIsLeftSpawn = !m_bIsLeftSpawn;
            m_iSpawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }

    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        /// <summary>
        /// 生成平台
        /// </summary>
        // 生成单个平台
        if (m_iSpawnPlatformCount > 0)
        {
            SpawnNormalPlatform();
        }
        // 在拐角前一处生成组合平台
        else if (m_iSpawnPlatformCount == 0)
        {
            int iRandom = Random.Range(0, 3);
            int iRandomDir = Random.Range(0, 2);
            // 生成通用组合平台
            if (iRandom == 0)
            {
                SpawnCommonPlatformGroup(iRandomDir);
            }
            // 生成主题组合平台
            else if (iRandom == 1)
            {
                switch (m_CurPlatformType)
                {
                    case PlatformType.Grass:
                        SpawnGrassPlatformGroup(iRandomDir);
                        break;

                    case PlatformType.Winter:
                        SpawnWinterPlatformGroup(iRandomDir);
                        break;

                    default:
                        break;
                }
            }
            // 生成钉子组合平台
            else
            {
                SpawnSpikePlatformGroup();
            }
        }

        /// <summary>
        /// 生成钻石
        /// </summary>
        int iRandomDiamond = Random.Range(0, 10);
        if (iRandomDiamond == 5 && GameManager.Instance.m_bPlayerIsMoving)
        {
            GameObject go = ObjectPool.Instance.GetDiamond();
            go.transform.SetParent(ObjectPool.Instance.transform);
            go.transform.position = new Vector3(m_posSpawnPlatform.x, m_posSpawnPlatform.y + 0.5f, 0);
            go.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            go.SetActive(true);
        }

        /// <summary>
        /// 调整下一次生成的位置
        /// </summary>
        // 要向 左 生成
        if (m_bIsLeftSpawn)
        {
            m_posSpawnPlatform = new Vector3(m_posSpawnPlatform.x - m_managerVars.fNextXPos, m_posSpawnPlatform.y + m_managerVars.fNextYPos, 0);
        }
        // 要向 右 生成
        else
        {
            m_posSpawnPlatform = new Vector3(m_posSpawnPlatform.x + m_managerVars.fNextXPos, m_posSpawnPlatform.y + m_managerVars.fNextYPos, 0);
        }
    }

    /// <summary>
    /// 生成普通平台（单个）
    /// </summary>
    private void SpawnNormalPlatform()
    {
        // 从对象池中取物体
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        go.SetActive(true);

        // 设置平台的位置
        go.transform.position = m_posSpawnPlatform;

        // 改变平台Sprite
        go.GetComponent<Platform>().Init(m_CurPlatformTheme, 0);
    }

    /// <summary>
    /// 生成通用组合平台
    /// </summary>
    private void SpawnCommonPlatformGroup(int iDir)
    {
        // 从对象池中取物体
        GameObject go = ObjectPool.Instance.GetCommonPlatformGroup();

        // 设置平台的位置
        go.transform.position = m_posSpawnPlatform;

        // 改变平台Sprite
        go.GetComponent<Platform>().Init(m_CurPlatformTheme, iDir);

        go.SetActive(true);
    }

    /// <summary>
    /// 生成草地组合平台
    /// </summary>
    private void SpawnGrassPlatformGroup(int iDir)
    {
        // 从对象池中取物体
        GameObject go = ObjectPool.Instance.GetGrassPlatformGroup();

        // 设置平台的位置
        go.transform.position = m_posSpawnPlatform;

        // 改变平台Sprite
        go.GetComponent<Platform>().Init(m_CurPlatformTheme, iDir);

        go.SetActive(true);
    }

    /// <summary>
    /// 生成冬季组合平台
    /// </summary>
    private void SpawnWinterPlatformGroup(int iDir)
    {
        // 从对象池中取物体
        GameObject go = ObjectPool.Instance.GetWinterPlatformGroup();

        // 设置平台的位置
        go.transform.position = m_posSpawnPlatform;

        // 改变平台Sprite
        go.GetComponent<Platform>().Init(m_CurPlatformTheme, iDir);

        go.SetActive(true);
    }

    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    private void SpawnSpikePlatformGroup()
    {
        GameObject go;
        if (m_bIsLeftSpawn)
        {
            // 从对象池中取物体
            go = ObjectPool.Instance.GetSpikePlatformRight();
        }
        else
        {
            // 从对象池中取物体
            go = ObjectPool.Instance.GetSpikePlatformLeft();
        }
        go.SetActive(true);

        // 设置平台的位置
        go.transform.position = m_posSpawnPlatform;

        // 改变平台Sprite
        go.GetComponent<Platform>().Init(m_CurPlatformTheme, 0);
    }
}