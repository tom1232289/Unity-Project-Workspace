using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池：将平台一次性创建出来，要用平台就从这个对象池中取，如果池子里的平台都被取完了，就再创建一个平台，塞到池子里
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static ObjectPool m_instance;

    public static ObjectPool Instance
    {
        get { return m_instance; }
    }

    /// <summary>
    /// 公有变量
    /// </summary>
    // 初始时池子的大小
    public int m_iInitSpawnCount = 5;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 普通平台的池子
    private List<GameObject> m_listNormalPlatform = new List<GameObject>();

    // 通用组合平台的池子
    private List<GameObject> m_listCommonPlatformGroup = new List<GameObject>();

    // 草地组合平台的池子
    private List<GameObject> m_listGrassPlatformGroup = new List<GameObject>();

    // 冬季组合平台的池子
    private List<GameObject> m_listWinterPlatformGroup = new List<GameObject>();

    // 左钉子平台的池子
    private List<GameObject> m_listSpikePlatformLeft = new List<GameObject>();

    // 右钉子平台的池子
    private List<GameObject> m_listSpikePlatformRight = new List<GameObject>();

    // 死亡时爆炸特效的池子
    private List<GameObject> m_listExplosionEffect = new List<GameObject>();

    // 钻石的池子
    private List<GameObject> m_listDiamond = new List<GameObject>();

    private ManagerVars m_managerVars;

    private void Awake()
    {
        m_instance = this;
        m_managerVars = ManagerVars.GetManagerVars();

        Init();
    }

    private GameObject InstantiateGO(GameObject goPlatform, ref List<GameObject> listPlatform)
    {
        GameObject go = Instantiate(goPlatform);
        go.transform.SetParent(transform);
        go.SetActive(false);
        listPlatform.Add(go);
        return go;
    }

    private GameObject TakeAway(List<GameObject> listPlatform)
    {
        for (int i = 0; i < listPlatform.Count; ++i)
        {
            // 池子里还有没用的 => 直接拿走
            if (listPlatform[i].activeInHierarchy == false)
            {
                return listPlatform[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 初始化池子
    /// </summary>
    private void Init()
    {
        // 初始化普通平台的池子
        for (int i = 0; i < m_iInitSpawnCount; ++i)
        {
            InstantiateGO(m_managerVars.m_NormalPlatform, ref m_listNormalPlatform);
        }

        // 初始化通用平台的池子
        for (int i = 0; i < m_iInitSpawnCount; ++i)
        {
            for (int j = 0; j < m_managerVars.m_CommonPlatformGroup.Count; ++j)
            {
                InstantiateGO(m_managerVars.m_CommonPlatformGroup[j], ref m_listCommonPlatformGroup);
            }
        }

        // 初始化草地平台的池子
        for (int i = 0; i < m_iInitSpawnCount; ++i)
        {
            for (int j = 0; j < m_managerVars.m_GrassPlatformGroup.Count; ++j)
            {
                InstantiateGO(m_managerVars.m_GrassPlatformGroup[j], ref m_listGrassPlatformGroup);
            }
        }

        // 初始化冬季平台的池子
        for (int i = 0; i < m_iInitSpawnCount; ++i)
        {
            for (int j = 0; j < m_managerVars.m_WinterPlatformGroup.Count; ++j)
            {
                InstantiateGO(m_managerVars.m_WinterPlatformGroup[j], ref m_listWinterPlatformGroup);
            }
        }

        // 初始化左钉子平台的池子
        for (int i = 0; i < m_iInitSpawnCount; ++i)
        {
            InstantiateGO(m_managerVars.m_SpikePlatformLeft, ref m_listSpikePlatformLeft);
        }

        // 初始化右钉子平台的池子
        for (int i = 0; i < m_iInitSpawnCount; ++i)
        {
            InstantiateGO(m_managerVars.m_SpikePlatformRight, ref m_listSpikePlatformRight);
        }

        // 初始化死亡时爆炸特效的池子
        for (int i = 0; i < m_iInitSpawnCount; ++i)
        {
            InstantiateGO(m_managerVars.m_ExplisionEffect, ref m_listExplosionEffect);
        }

        // 初始化钻石的池子
        for (int i = 0; i < m_iInitSpawnCount; ++i)
        {
            InstantiateGO(m_managerVars.m_Diamond, ref m_listDiamond);
        }
    }

    /// <summary>
    /// 从 普通平台池子里 取 一个普通平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetNormalPlatform()
    {
        // 池子里还有没用的 => 直接拿走
        GameObject go = TakeAway(m_listNormalPlatform);
        if (go != null)
        {
            return go;
        }

        // 池子里没有了 => 创建一个新的，并且加到池子里
        go = InstantiateGO(m_managerVars.m_NormalPlatform, ref m_listNormalPlatform);
        return go;
    }

    /// <summary>
    /// 从 通用组合平台池子里 取 一个通用组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetCommonPlatformGroup()
    {
        // 池子里还有没用的 => 直接拿走
        GameObject go = TakeAway(m_listCommonPlatformGroup);
        if (go != null)
        {
            return go;
        }

        // 池子里没有了 => 随机创建一个新的，并且加到池子里
        int iRandom = Random.Range(0, m_managerVars.m_CommonPlatformGroup.Count);
        go = InstantiateGO(m_managerVars.m_CommonPlatformGroup[iRandom], ref m_listCommonPlatformGroup);
        return go;
    }

    /// <summary>
    /// 从 草地组合平台池子里 取 一个草地组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetGrassPlatformGroup()
    {
        // 池子里还有没用的 => 直接拿走
        GameObject go = TakeAway(m_listGrassPlatformGroup);
        if (go != null)
        {
            return go;
        }

        // 池子里没有了 => 随机创建一个新的，并且加到池子里
        int iRandom = Random.Range(0, m_managerVars.m_GrassPlatformGroup.Count);
        go = InstantiateGO(m_managerVars.m_GrassPlatformGroup[iRandom], ref m_listGrassPlatformGroup);
        return go;
    }

    /// <summary>
    /// 从 冬季组合平台池子里 取 一个冬季组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetWinterPlatformGroup()
    {
        // 池子里还有没用的 => 直接拿走
        GameObject go = TakeAway(m_listWinterPlatformGroup);
        if (go != null)
        {
            return go;
        }

        // 池子里没有了 => 随机创建一个新的，并且加到池子里
        int iRandom = Random.Range(0, m_managerVars.m_WinterPlatformGroup.Count);
        go = InstantiateGO(m_managerVars.m_WinterPlatformGroup[iRandom], ref m_listWinterPlatformGroup);
        return go;
    }

    /// <summary>
    /// 从 左钉子平台池子里 取 一个左钉子平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpikePlatformLeft()
    {
        // 池子里还有没用的 => 直接拿走
        GameObject go = TakeAway(m_listSpikePlatformLeft);
        if (go != null)
        {
            return go;
        }

        // 池子里没有了 => 创建一个新的，并且加到池子里
        go = InstantiateGO(m_managerVars.m_SpikePlatformLeft, ref m_listSpikePlatformLeft);
        return go;
    }

    /// <summary>
    /// 从 右钉子池子里 取 一个右钉子平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetSpikePlatformRight()
    {
        // 池子里还有没用的 => 直接拿走
        GameObject go = TakeAway(m_listSpikePlatformRight);
        if (go != null)
        {
            return go;
        }

        // 池子里没有了 => 创建一个新的，并且加到池子里
        go = InstantiateGO(m_managerVars.m_SpikePlatformRight, ref m_listSpikePlatformRight);
        return go;
    }

    /// <summary>
    /// 从 死亡时爆炸特效的池子里 取 死亡时爆炸特效的池子
    /// </summary>
    /// <returns></returns>
    public GameObject GetExplosionEffect()
    {
        // 池子里还有没用的 => 直接拿走
        GameObject go = TakeAway(m_listExplosionEffect);
        if (go != null)
        {
            return go;
        }

        // 池子里没有了 => 随机创建一个新的，并且加到池子里
        go = InstantiateGO(m_managerVars.m_ExplisionEffect, ref m_listExplosionEffect);
        return go;
    }

    /// <summary>
    /// 从 钻石的池子里 取 钻石的池子
    /// </summary>
    public GameObject GetDiamond()
    {
        // 池子里还有没用的 => 直接拿走
        GameObject go = TakeAway(m_listDiamond);
        if (go != null)
        {
            return go;
        }

        // 池子里没有了 => 随机创建一个新的，并且加到池子里
        go = InstantiateGO(m_managerVars.m_Diamond, ref m_listDiamond);
        return go;
    }
}