using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MapCreation : MonoBehaviour {

    // 用来装饰初始化地图所需物体的数组
    // 0：老家；1：墙；2：障碍；3：出生效果；4：河流；5：草；6：空气墙
    public GameObject[] m_Items;

    private List<Vector3> m_ItemsPositionList = new List<Vector3>();

    private void Awake() {
        // 实例化老家
        CreateItems(m_Items[0], new Vector3(0, -8, 0), Quaternion.identity);
        // 用墙把老家围起来
        CreateItems(m_Items[1], new Vector3(-1, -8, 0), Quaternion.identity);
        CreateItems(m_Items[1], new Vector3(1, -8, 0), Quaternion.identity);
        for (int i = -1; i < 2; ++i) {
            CreateItems(m_Items[1], new Vector3(i, -7, 0), Quaternion.identity);
        }
        // 实例化外围空气墙
        for (int i = -11; i < 12; ++i) {
            CreateItems(m_Items[6], new Vector3(i, 9, 0), Quaternion.identity);
        }
        for (int i = -11; i < 12; ++i) {
            CreateItems(m_Items[6], new Vector3(i, -9, 0), Quaternion.identity);
        }
        for (int i = -8; i < 9; ++i) {
            CreateItems(m_Items[6], new Vector3(-11, i, 0), Quaternion.identity);
        }
        for (int i = -8; i < 9; ++i) {
            CreateItems(m_Items[6], new Vector3(11, i, 0), Quaternion.identity);
        }
        // 随机实例化其他物体
        for (int i = 0; i < 60; ++i) {
            CreateItems(m_Items[1], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; ++i) {
            CreateItems(m_Items[2], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; ++i) {
            CreateItems(m_Items[4], CreateRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < 20; ++i) {
            CreateItems(m_Items[5], CreateRandomPosition(), Quaternion.identity);
        }
        // 初始化玩家
        GameObject go = Instantiate(m_Items[3], new Vector3(-2, -8, 0), Quaternion.identity);
        go.GetComponent<Born>().m_bCreatePlayer = true;
        // 初始化敌人
        CreateItems(m_Items[3], new Vector3(-10, 8, 0), Quaternion.identity);
        CreateItems(m_Items[3], new Vector3(0, 8, 0), Quaternion.identity);
        CreateItems(m_Items[3], new Vector3(10, 8, 0), Quaternion.identity);
        InvokeRepeating("CreateEnemy", 4, 5);
    }

    private void CreateItems(GameObject original, Vector3 position, Quaternion rotation) {
        GameObject go = Instantiate(original, position, rotation);
        go.transform.SetParent(gameObject.transform);
        m_ItemsPositionList.Add(position);
    }

    // 产生随机位置的方法
    private Vector3 CreateRandomPosition() {
        // 不生成最外围的位置（x=-10，10的两列，y=-8，8的两列）
        while (true) {
            Vector3 position = new Vector3(Random.Range(-9, 10), Random.Range(-7, 8), 0);
            if (!HasPosition(position)) {
                return position;
            }
        }
    }

    // 判断位置列表中是否有这个位置
    private bool HasPosition(Vector3 position) {
        for (int i = 0; i < m_ItemsPositionList.Count; ++i) {
            if (position == m_ItemsPositionList[i]) {
                return true;
            }
        }

        return false;
    }

    // 随机产生敌人
    private void CreateEnemy() {
        int iRandomNum = Random.Range(0, 3);
        Vector3 EnemyPos = new Vector3();
        if (iRandomNum == 0) {
            EnemyPos = new Vector3(-10, 8, 0);
        }
        else if (iRandomNum == 1) {
            EnemyPos = new Vector3(0, 8, 0);
        }
        else if (iRandomNum == 2) {
            EnemyPos = new Vector3(10, 8, 0);
        }
        CreateItems(m_Items[3], EnemyPos, Quaternion.identity);
    }
}