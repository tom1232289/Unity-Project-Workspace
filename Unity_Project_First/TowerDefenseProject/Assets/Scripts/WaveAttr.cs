using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveAttr {
    public GameObject m_go;     // 敌人的预制体
    public int m_iCount;        // 生成敌人的数量
    public float m_fRate;  // 生成敌人的间隔时间
}
