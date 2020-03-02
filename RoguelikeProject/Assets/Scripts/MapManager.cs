using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    // 公有引用
    public GameObject[] m_HardWalls;    // 外墙的数组
    public GameObject[] m_Floors;       // 地板
    public GameObject[] m_Walls;        // 墙
    public GameObject[] m_Foods;        // 食物
    public GameObject[] m_Enemys;       // 敌人
    public GameObject m_Exit;           // 出口

    // 公有变量
    public int m_iMinWallCount = 2;
    public int m_iMaxWallCount = 10;
    public int m_iRow = 10;    // 地图的行数
    public int m_iColumn = 10; // 地图的列数

    // 私有变量
    private List<Vector2> m_listRandomPos = new List<Vector2>();    // 可以随机生成物体的地板位置
    private GameObject m_Map;   // Map空物体，用于存放地图相关的物体

    private void Start() {
        InitMap();
    }

    private void InitMap() {
        // 创建Map空物体存放地图下的所有物体
        m_Map = new GameObject("Map");
        for (int x = 0; x < m_iColumn; ++x) {
            for (int y = 0; y < m_iRow; ++y) {
                // 初始化外墙
                if (x == 0 || x == m_iColumn - 1 || y == 0 || y == m_iRow - 1) {
                    int iRandom = Random.Range(0, m_HardWalls.Length);
                    GameObject go = Instantiate(m_HardWalls[iRandom], new Vector2(x, y), Quaternion.identity);
                    go.transform.SetParent(m_Map.transform);
                }
                // 初始化地板
                else {
                    int iRandom = Random.Range(0, m_Floors.Length);
                    GameObject go = Instantiate(m_Floors[iRandom], new Vector2(x, y), Quaternion.identity);
                    go.transform.SetParent(m_Map.transform);
                }
            }
        }
        // 获取可以随机生成物体的地板位置
        m_listRandomPos.Clear();
        for (int x = 2; x < m_iColumn - 2; ++x) {
            for (int y = 2; y < m_iRow - 2; ++y) {
                m_listRandomPos.Add(new Vector2(x, y)); 
            }
        }
        // 随机生成墙
        int iRandomWallCount = Random.Range(m_iMinWallCount, m_iMaxWallCount + 1);
        RandomGenerate(iRandomWallCount, m_Walls);
        // 随机生成敌人
        int iEnemyCount = Random.Range(1, GameManager.Instance.m_iLevel);
        // 敌人的数量 不能比 可以存放的方格 多
        if (iEnemyCount > m_listRandomPos.Count) {
            iEnemyCount = m_listRandomPos.Count;
        }
        RandomGenerate(iEnemyCount, m_Enemys);
        // 随机生成食物
        int iFoodCount = Random.Range(1, GameManager.Instance.m_iLevel);
        // 食物的数量 不能比 可以存放的方格 多
        if (iFoodCount > m_listRandomPos.Count) {
            iFoodCount = m_listRandomPos.Count;
        }
        RandomGenerate(iFoodCount, m_Foods);
        // 创建出口
        GameObject goExit = Instantiate(m_Exit, new Vector2(m_iColumn - 2, m_iRow - 2), Quaternion.identity);
        goExit.transform.SetParent(m_Map.transform);
    }

    private void RandomGenerate(int count, GameObject[] prefabs) {
        for (int i = 0; i < count; ++i) {
            int iRandom = Random.Range(0, prefabs.Length);
            Vector2 pos = m_listRandomPos[Random.Range(0, m_listRandomPos.Count)];
            GameObject go = Instantiate(prefabs[iRandom], pos, Quaternion.identity);
            go.transform.SetParent(m_Map.transform);
            m_listRandomPos.Remove(pos);
        }
    }
}