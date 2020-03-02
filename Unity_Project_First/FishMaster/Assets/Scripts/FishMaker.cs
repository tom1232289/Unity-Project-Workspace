using System.Collections;
using UnityEngine;

public class FishMaker : MonoBehaviour {

    // 公有引用
    public Transform[] m_posGenerators; // 鱼要生成的位置
    public GameObject[] m_prefabFishs;  // 各种鱼

    // 公有变量
    public float m_fGenTime = 0.1f;     // 生成各种鱼的间隔时间
    public float m_fEveryFishTime = 0.5f;   // 生成每条鱼之间的间隔时间

    // 私有引用
    private GameObject m_fishHolder;    // 存放鱼的空物体

    private void Awake() {
        m_fishHolder = GameObject.Find("fishHolder");
    }

    private void Start() {
        InvokeRepeating("MakeFish", 0, m_fGenTime);
    }

    private void MakeFish() {
        // 在哪生成
        int iPosGen = Random.Range(0, m_posGenerators.Length);
        // 生成什么鱼
        int iFish = Random.Range(0, m_prefabFishs.Length);
        // 生成多少个
        int iMaxNum = m_prefabFishs[iFish].GetComponent<FishAttr>().m_iMaxNum;
        int iGenNum = Random.Range((iMaxNum / 2) + 1, iMaxNum);
        // 鱼的速度是多少
        int iMaxSpeed = m_prefabFishs[iFish].GetComponent<FishAttr>().m_iMaxSpeed;
        int iGenSpeed = Random.Range(iMaxSpeed / 2, iMaxSpeed);
        // 鱼是 直走 还是 转弯
        int iMoveType = Random.Range(0, 2);  // 0：直走；1：转弯
        // 直走
        if (iMoveType == 0) {
            int iAngOffset = Random.Range(-22, 22);
            StartCoroutine(GenStraightFish(iPosGen, iFish, iGenNum, iGenSpeed, iAngOffset));
        }
        // 转弯
        else {
            int iAngSpeed = 0;
            // 不能让角速度太小
            if (Random.Range(0, 2) == 0) {
                iAngSpeed = Random.Range(-15, -9);
            }
            else {
                iAngSpeed = Random.Range(9, 15);
            }
            StartCoroutine(GenTurnFish(iPosGen, iFish, iGenNum, iGenSpeed, iAngSpeed));
        }
    }

    private IEnumerator GenStraightFish(int iPosGen, int iFish, int iGenNum, int iGenSpeed, int iAngOffset) {
        for (int i = 0; i < iGenNum; ++i) {
            GameObject goFish = Instantiate(m_prefabFishs[iFish]);
            goFish.transform.SetParent(m_fishHolder.transform, false);
            goFish.transform.localPosition = m_posGenerators[iPosGen].localPosition;
            goFish.transform.localRotation = m_posGenerators[iPosGen].localRotation;
            goFish.transform.Rotate(0, 0, iAngOffset);
            goFish.AddComponent<Ef_AutoMove>().m_fSpeed = iGenSpeed;
            goFish.GetComponent<SpriteRenderer>().sortingOrder += i;    // 解决sortingOrder相同时Untiy不知道先渲染谁 导致的 闪烁问题
            yield return new WaitForSeconds(m_fEveryFishTime);
        }
    }

    private IEnumerator GenTurnFish(int iPosGen, int iFish, int iGenNum, int iGenSpeed, int iAngSpeed) {
        for (int i = 0; i < iGenNum; ++i) {
            GameObject goFish = Instantiate(m_prefabFishs[iFish]);
            goFish.transform.SetParent(m_fishHolder.transform, false);
            goFish.transform.localPosition = m_posGenerators[iPosGen].localPosition;
            goFish.transform.localRotation = m_posGenerators[iPosGen].localRotation;
            goFish.AddComponent<Ef_AutoMove>().m_fSpeed = iGenSpeed;
            goFish.AddComponent<Ef_AutoRotate>().m_fSpeed = iAngSpeed;
            goFish.GetComponent<SpriteRenderer>().sortingOrder += i;    // 解决sortingOrder相同时Untiy不知道先渲染谁 导致的 闪烁问题
            yield return new WaitForSeconds(m_fEveryFishTime);
        }
    }
}