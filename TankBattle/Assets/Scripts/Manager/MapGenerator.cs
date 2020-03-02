using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    // 公有引用
    public GameObject[] m_MapPrefabs;   // 0：空气墙；1：砖块；2：草；3：老家；4：河；5：墙
    public GameObject m_BornPlayerPrefabs;
    public GameObject m_BornEnemyPrefabs;

    // 公有变量
    public float m_fGeneratorEnemyInterval = 5f;      // 生成敌人的时间间隔

    // 私有引用
    private GameObject m_Map;

    // 私有变量
    private List<Vector3> m_PosList = new List<Vector3>();    // 已经占有的位置列表
    private float m_fCurrGeneEnemyInterval;

    // 单例
    private static MapGenerator m_Instance;

    public static MapGenerator Instance { get => m_Instance; set => m_Instance = value; }

    private void Awake() {
        m_Instance = this;
        m_Map = GameObject.Find("Map");
    }

    private void Start() {
        // 生成老家
        GenerateHome();
        // 生成空气墙
        GenerateAirBarrier();
        // 随机生成其他地图资源
        RandomGenerateMapResource();
        // 生成玩家
        GeneratePlayer();
        // 生成敌人
        GenerateEnemy();
    }

    private void Update() {
        if (m_fCurrGeneEnemyInterval > m_fGeneratorEnemyInterval) {
            GenerateEnemy();
            m_fCurrGeneEnemyInterval = 0;
        }
        else {
            m_fCurrGeneEnemyInterval += Time.deltaTime;
        }
    }

    private void CreateItems(GameObject original, Vector3 position, Quaternion rotation) {
        GameObject go = Instantiate(original, position, rotation);
        go.transform.parent = m_Map.transform;
        m_PosList.Add(position);
    }

    private void GenerateHome() {
        // 生成大本营
        CreateItems(m_MapPrefabs[3], new Vector3(0, -7.5f, 0), Quaternion.identity);
        // 生成老家周围的墙
        CreateItems(m_MapPrefabs[5], new Vector3(-1, -7.5f, 0), Quaternion.identity);
        CreateItems(m_MapPrefabs[5], new Vector3(1, -7.5f, 0), Quaternion.identity);
        for (int i = -1; i < 2; ++i) {
            CreateItems(m_MapPrefabs[5], new Vector3(i, -6.5f, 0), Quaternion.identity);
        }
    }

    private void GenerateAirBarrier() {
        // 最上方
        for (float i = -9.5f; i < 10.5; ++i) {
            CreateItems(m_MapPrefabs[0], new Vector3(i, 8.5f, 0), Quaternion.identity);
        }
        // 最右方
        for (float i = -7.5f; i < 8.5; ++i) {
            CreateItems(m_MapPrefabs[0], new Vector3(10.5f, i, 0), Quaternion.identity);
        }
        // 最下方
        for (float i = -9.5f; i < 10.5; ++i) {
            CreateItems(m_MapPrefabs[0], new Vector3(i, -8.5f, 0), Quaternion.identity);
        }
        // 最左方
        for (float i = -7.5f; i < 8.5; ++i) {
            CreateItems(m_MapPrefabs[0], new Vector3(-10.5f, i, 0), Quaternion.identity);
        }
    }

    private void RandomGenerateMapResource() {
        // 生成砖块
        for (int i = 0; i < 20; ++i) {
            // 随机生成一个位置
            Vector3 randomPos = RandomPos();
            CreateItems(m_MapPrefabs[1], randomPos, Quaternion.identity);
        }
        // 生成草
        for (int i = 0; i < 20; ++i) {
            // 随机生成一个位置
            Vector3 randomPos = RandomPos();
            CreateItems(m_MapPrefabs[2], randomPos, Quaternion.identity);
        }
        // 生成河
        for (int i = 0; i < 20; ++i) {
            // 随机生成一个位置
            Vector3 randomPos = RandomPos();
            CreateItems(m_MapPrefabs[4], randomPos, Quaternion.identity);
        }
        // 生成墙
        for (int i = 0; i < 60; ++i) {
            // 随机生成一个位置
            Vector3 randomPos = RandomPos();
            CreateItems(m_MapPrefabs[5], randomPos, Quaternion.identity);
        }
    }

    private Vector3 RandomPos() {
        while (true) {
            // 最外圈不生成
            Vector3 random = new Vector3(Random.Range(-9, 10), Random.Range(-7, 7) + 0.5f, 0);
            // 查找
            if (FindInList(random, ref m_PosList) == false) {
                return random;
            }
        }
    }

    private bool FindInList(Vector3 random, ref List<Vector3> list) {   // 用二分查找需要排序，可能更慢
        for (int i = 0; i < list.Count; ++i) {
            if (m_PosList[i] == random) {
                return true;
            }
        }

        return false;
    }

    public void GeneratePlayer() {
        GameObject go = Instantiate(m_BornPlayerPrefabs, new Vector3(-2, -7.5f, 0), Quaternion.identity);
        go.GetComponent<Born>().m_bIsPlayer = true;
    }

    private void GenerateEnemy() {
        CreateItems(m_BornEnemyPrefabs, new Vector3(-9, 7.5f, 0), Quaternion.identity);
        CreateItems(m_BornEnemyPrefabs, new Vector3(0, 7.5f, 0), Quaternion.identity);
        CreateItems(m_BornEnemyPrefabs, new Vector3(9, 7.5f, 0), Quaternion.identity);
    }
}