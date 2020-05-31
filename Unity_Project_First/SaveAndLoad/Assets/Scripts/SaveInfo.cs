using System.Collections.Generic;

[System.Serializable]
public class SaveInfo {
    // 怪物在哪几个九宫格里存活
    public List<int> m_listLivingPos = new List<int>();
    // 怪物的类型
    public List<int> m_listLivingMonsterType = new List<int>();
    // 射击数
    public int m_iShootNum;
    // 得分
    public int m_iScore;
}