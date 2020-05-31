using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

    // 私有引用
    private List<GameObject> m_listMonsters = new List<GameObject>();   // 所有的怪物

    // 私有变量
    [HideInInspector]
    public GameObject m_goActiveMonster;   // 当前激活的怪物

    [HideInInspector] public int m_iMonsterType;    // 当前怪物的类型

    private void Awake() {
        for (int i = 0; i < transform.childCount; ++i) {
            m_listMonsters.Add(transform.GetChild(i).gameObject);
        }
    }

    private void Start() {
        foreach (var monster in m_listMonsters) {
            monster.GetComponent<BoxCollider>().enabled = false;
            monster.SetActive(false);
        }

        StartCoroutine("GenerateMonster");
    }

    // 生成怪物
    private IEnumerator GenerateMonster() {
        yield return new WaitForSeconds(Random.Range(1, 5));
        m_iMonsterType = Random.Range(0, m_listMonsters.Count);
        if (m_goActiveMonster == null) {
            m_goActiveMonster = m_listMonsters[m_iMonsterType];
            m_goActiveMonster.SetActive(true);
            m_goActiveMonster.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine("MonsterDisappears");
        }
    }

    // 怪物消失
    private IEnumerator MonsterDisappears() {
        yield return new WaitForSeconds(Random.Range(3, 7));
        if (m_goActiveMonster != null) {
            m_goActiveMonster.GetComponent<BoxCollider>().enabled = false;
            m_goActiveMonster.SetActive(false);
            m_goActiveMonster = null;
        }
        StartCoroutine("GenerateMonster");
    }

    // 怪物死亡
    public void MonsterDie() {
        StopAllCoroutines();
        if (m_goActiveMonster != null) {
            m_goActiveMonster.GetComponent<BoxCollider>().enabled = false;
            m_goActiveMonster.SetActive(false);
            m_goActiveMonster = null;
        }
        StartCoroutine("GenerateMonster");
    }

    // 加载游戏时使用：根据存档数据生成怪物
    public void GenerateMonster(int iIndex) {
        m_iMonsterType = iIndex;
        m_goActiveMonster = m_listMonsters[iIndex];
        m_goActiveMonster.SetActive(true);
        m_goActiveMonster.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine("MonsterDisappears");
    }

    // 加载游戏时使用：清空九宫格内所有怪物，重新生成怪物
    public void ClearMonsters() {
        MonsterDie();
    }
}