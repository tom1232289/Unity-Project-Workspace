[System.Serializable]
public class GameData
{
    public static bool m_bIsRetryGame;

    /// <summary>
    /// 存储的数据
    /// </summary>
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

    public void SetIsFirstGame(bool bIsFirstGame)
    {
        m_bIsFirstGame = bIsFirstGame;
    }

    public void SetIsMusicOn(bool bIsMusicOn)
    {
        m_bIsMusicOn = bIsMusicOn;
    }

    public void SetBestScoreArray(int[] bestScoreArray)
    {
        m_BestScoreArray = bestScoreArray;
    }

    public void SetSelectSkin(int iSelectSkin)
    {
        m_iSelectSkin = iSelectSkin;
    }

    public void SetSkinUnlockedArray(bool[] skinUnlockedArray)
    {
        m_SkinUnlockedArray = skinUnlockedArray;
    }

    public void SetDiamondCount(int iDiamondCount)
    {
        m_iDiamondCount = iDiamondCount;
    }

    public bool GetIsFirstGame()
    {
        return m_bIsFirstGame;
    }

    public bool GetIsMusicOn()
    {
        return m_bIsMusicOn;
    }

    public int[] GetBestScoreArray()
    {
        return m_BestScoreArray;
    }

    public int GetSelectSkin()
    {
        return m_iSelectSkin;
    }

    public bool[] GetSkinUnlockedArray()
    {
        return m_SkinUnlockedArray;
    }

    public int GetDiamondCount()
    {
        return m_iDiamondCount;
    }
}