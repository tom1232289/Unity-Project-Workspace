using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static GameManager m_instance;

    public static GameManager Instance
    {
        get { return m_instance; }
    }

    /// <summary>
    /// 公有变量
    /// </summary>
    // 游戏是否开始
    [HideInInspector]
    public bool m_bIsGameStarted;

    // 游戏是否结束
    [HideInInspector]
    public bool m_bIsGameOver;

    // 本次生成的平台的主题
    [HideInInspector]
    public Sprite m_CurPlatformTheme;

    // 游戏是否是暂停的
    [HideInInspector]
    public bool m_bIsGamePause;

    // 玩家是否开始移动了
    [HideInInspector]
    public bool m_bPlayerIsMoving;

    // 得分临界值
    public float m_fScoreCriticalValue = 50;

    // 平台掉落时间减少的系数
    public float m_fReducedCoeffient = 0.9f;

    // 平台掉落时间最小值
    public float m_fMinPlatformFallTime = 0.5f;

    // 初始平台掉落时间
    public float m_fInitPlatformFallTime = 2f;

    /// <summary>
    /// 私有变量
    /// </summary>
    private ManagerVars m_managerVars;

    // 得分
    private int m_iScore;

    // 当前吃到的钻石
    private int m_iCurDiamond;

    // 平台掉落时间
    private float m_fPlatformFallTime;

    /// <summary>
    /// 数据储存信息
    /// </summary>
    private GameData m_GameData;

    // 是否是第一次游戏
    private bool m_bIsFirstGame;

    // 音乐是否打开
    private bool m_bIsMusicOn;

    // 最好成绩的前三名
    private int[] m_BestScoreArray;

    // 当前选中的皮肤
    private int m_iSelectSkin;

    // 皮肤是否解锁
    private bool[] m_SkinUnlockedArray;

    // 钻石的数量
    private int m_iDiamondCount;

    private void Awake()
    {
        m_instance = this;
        m_managerVars = ManagerVars.GetManagerVars();

        InitGameData();
    }

    private void Start()
    {
        m_fPlatformFallTime = m_fInitPlatformFallTime;
    }

    private void Update()
    {
        /// <summary>
        /// 计算平台掉落时间
        /// </summary>
        if (m_iScore > m_fScoreCriticalValue)
        {
            m_fScoreCriticalValue *= 1.5f;
            m_fPlatformFallTime *= m_fReducedCoeffient;

            if (m_fPlatformFallTime < m_fMinPlatformFallTime)
            {
                m_fPlatformFallTime = m_fMinPlatformFallTime;
            }

            EventCenter.Broadcast(EventDefine.UpdatePaltformFallTime, m_fPlatformFallTime);
        }
    }

    public void AddScore()
    {
        if (m_bIsGameOver || m_bIsGameStarted == false || m_bIsGamePause)
            return;

        ++m_iScore;

        EventCenter.Broadcast(EventDefine.UpdateScoreText, m_iScore);
    }

    public void AddDiamond()
    {
        if (m_bIsGameOver || m_bIsGameStarted == false || m_bIsGamePause)
            return;

        ++m_iCurDiamond;

        EventCenter.Broadcast(EventDefine.UpdateDiamondText, m_iCurDiamond);
    }

    public int GetGameScore()
    {
        return m_iScore;
    }

    public int GetAddDiamondCount()
    {
        return m_iCurDiamond;
    }

    /// <summary>
    /// 保存游戏
    /// </summary>
    public void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/PathToGod.save"))
            {
                m_GameData.SetIsFirstGame(m_bIsFirstGame);
                m_GameData.SetIsMusicOn(m_bIsMusicOn);
                m_GameData.SetBestScoreArray(m_BestScoreArray);
                m_GameData.SetSelectSkin(m_iSelectSkin);
                m_GameData.SetSkinUnlockedArray(m_SkinUnlockedArray);
                m_GameData.SetDiamondCount(m_iDiamondCount);

                bf.Serialize(fs, m_GameData);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 加载游戏
    /// </summary>
    public void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/PathToGod.save", FileMode.Open))
            {
                m_GameData = (GameData)bf.Deserialize(fs);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitGameData()
    {
        Load();
        // 读到数据了
        if (m_GameData != null)
        {
            m_bIsFirstGame = false;
        }
        // 没读到数据
        else
        {
            m_bIsFirstGame = true;
        }

        // 第一次游玩
        if (m_bIsFirstGame)
        {
            m_bIsFirstGame = false;
            m_bIsMusicOn = true;
            m_BestScoreArray = new int[3];
            m_iSelectSkin = 0;
            m_SkinUnlockedArray = new bool[m_managerVars.m_listSkinSprite.Count];
            m_SkinUnlockedArray[0] = true;
            m_iDiamondCount = 0;

            m_GameData = new GameData();
            Save();
        }
        // 老玩家了
        else
        {
            m_bIsMusicOn = m_GameData.GetIsMusicOn();
            m_BestScoreArray = m_GameData.GetBestScoreArray();
            m_iSelectSkin = m_GameData.GetSelectSkin();
            m_SkinUnlockedArray = m_GameData.GetSkinUnlockedArray();
            m_iDiamondCount = m_GameData.GetDiamondCount();
        }
    }

    /// <summary>
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public bool GetSkinUnlocked(int iIndex)
    {
        return m_SkinUnlockedArray[iIndex];
    }

    /// <summary>
    /// 设置皮肤解锁
    /// </summary>
    /// <param name="iIndex"></param>
    public void SetSkinUnlocked(int iIndex)
    {
        m_SkinUnlockedArray[iIndex] = true;
    }

    /// <summary>
    /// 获取总钻石数
    /// </summary>
    /// <returns></returns>
    public int GetTotalDiamond()
    {
        return m_iDiamondCount;
    }

    /// <summary>
    /// 更新总钻石数
    /// </summary>
    /// <param name="iCount"></param>
    public void UpdateTotalDiamond(int iCount)
    {
        m_iDiamondCount += iCount;
        Save();
    }

    /// <summary>
    /// 获取当前选中的皮肤
    /// </summary>
    /// <returns></returns>
    public int GetSelectedSkin()
    {
        return m_iSelectSkin;
    }

    /// <summary>
    /// 设置当前选中的皮肤
    /// </summary>
    /// <param name="iIndex"></param>
    public void SetSelectedSkin(int iIndex)
    {
        m_iSelectSkin = iIndex;
    }

    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        m_bIsFirstGame = false;
        m_bIsMusicOn = true;
        m_BestScoreArray = new int[3];
        m_iSelectSkin = 0;
        m_SkinUnlockedArray = new bool[m_managerVars.m_listSkinSprite.Count];
        m_SkinUnlockedArray[0] = true;
        m_iDiamondCount = 0;

        Save();
    }

    /// <summary>
    /// 保存成绩
    /// </summary>
    /// <param name="score"></param>
    public void SaveScore(int score)
    {
        // 数组转容器，方便操作
        List<int> iListBestScore = m_BestScoreArray.ToList();
        // 将当前分数加到最高分排列中
        iListBestScore.Add(score);
        // 将最高分从大到小排序
        iListBestScore.Sort((x, y) => (-x.CompareTo(y)));
        // 删除最低分
        iListBestScore.RemoveAt(iListBestScore.Count - 1);
        // 还给数组
        m_BestScoreArray = iListBestScore.ToArray();
        // 保存起来
        Save();
    }

    /// <summary>
    /// 获取最高分
    /// </summary>
    /// <returns></returns>
    public int GetBestScore()
    {
        return m_BestScoreArray.Max();
    }

    /// <summary>
    /// 获取排行榜
    /// </summary>
    /// <returns></returns>
    public int[] GetBestScoreArray()
    {
        return m_BestScoreArray;
    }

    /// <summary>
    /// 设置音效开关
    /// </summary>
    /// <param name="bIsOn"></param>
    public void SetMusicOn(bool bIsOn)
    {
        m_bIsMusicOn = bIsOn;
        Save();
    }

    /// <summary>
    /// 获取音效是否打开
    /// </summary>
    /// <returns></returns>
    public bool GetMusicOn()
    {
        return m_bIsMusicOn;
    }
}