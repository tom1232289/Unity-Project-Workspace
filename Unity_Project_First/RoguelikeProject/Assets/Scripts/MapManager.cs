using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // 公有引用
    public GameObject[] m_HardWalls;    // 硬墙的数组
    public GameObject[] m_Floors;       // 地板的数组
    public GameObject[] m_Walls;        // 普通墙的数组
    public GameObject[] m_Foods;        // 食物的数组
    public GameObject[] m_Enemys;       // 敌人的数组
    public GameObject m_ExitPrefab;     // 出口的预制体

    // 公有变量
    public int m_iMinWallCount;         // 普通墙的最小个数
    public int m_iMaxWallCount;         // 普通墙的最大个数

    // 私有变量
    [HideInInspector] public int m_iRows = 10;     // 地图的行数
    [HideInInspector] public int m_iColumns = 10;  // 地图的列数
    private Transform m_Map;
    private List<Vector2> m_Positions = new List<Vector2>();

    private void Start() {
        InitMap();
    }

    private void InitMap() {
        m_Map = new GameObject("Map").transform;
        // 生成外围墙和地板
        for (int x = 0; x < m_iColumns; ++x) {
            for (int y = 0; y < m_iRows; ++y) {
                if (x == 0 || x == m_iColumns - 1 || y == 0 || y == m_iRows - 1) {
                    int iRandom = Random.Range(0, m_HardWalls.Length);
                    GameObject go = Instantiate(m_HardWalls[iRandom], new Vector3(x, y, 0), Quaternion.identity);
                    go.transform.SetParent(m_Map);
                }
                else {
                    int iRandom = Random.Range(0, m_Floors.Length);
                    GameObject go = Instantiate(m_Floors[iRandom], new Vector3(x, y, 0), Quaternion.identity);
                    go.transform.SetParent(m_Map);
                }
            }
        }
        // 生成中间的位置数组
        m_Positions.Clear();
        for (int x = 2; x < m_iColumns - 2; ++x) {
            for (int y = 2; y < m_iRows - 2; ++y) {
                m_Positions.Add(new Vector2(x, y));
            }
        }
        // 生成普通墙
        RandomGenerate(m_iMinWallCount, m_iMaxWallCount, m_Walls);
        // 生成食物（最大数量为当前关卡等级）
        int iCount1 = GameManager.Instance.m_iLevel / 2;    // 当前等级应该生成的数量
        int iCount2 = ((m_iRows - 4) * (m_iColumns - 4) - m_iMaxWallCount) / 2;     // 可生成的最大的数量
        int iMaxCount = iCount1 < iCount2 ? iCount1 : iCount2;
        RandomGenerate(1, iMaxCount, m_Foods);
        // 生成敌人
        RandomGenerate(1, iMaxCount, m_Enemys);
        // 创建出口
        GameObject goExit = Instantiate(m_ExitPrefab, new Vector3(m_iColumns - 2, m_iRows - 2, 0), Quaternion.identity);
        goExit.transform.SetParent(m_Map);
    }

    private void RandomGenerate(int iMinCount, int iMaxCount, GameObject[] goPrefabs) {
        int iCount = Random.Range(iMinCount, iMaxCount + 1);
        for (int i = 0; i < iCount; ++i) {
            // 随机获取位置
            int iRandom = Random.Range(0, m_Positions.Count);
            Vector2 pos = m_Positions[iRandom];
            m_Positions.Remove(pos);
            // 随机创建
            iRandom = Random.Range(0, goPrefabs.Length);
            GameObject go = Instantiate(goPrefabs[iRandom], pos, Quaternion.identity);
            go.transform.SetParent(m_Map);
        }
    }
}
