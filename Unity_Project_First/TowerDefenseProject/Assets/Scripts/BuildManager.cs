using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {

    // 公有引用
    public TurretAttr m_LaserBeamer;
    public TurretAttr m_MissileLauncher;
    public TurretAttr m_StandardTurret;
    public Animator m_animMoneyFlicker;
    public GameObject m_goCanvasUpgrade;

    // 私有引用
    private Button m_btnUpgrade;
    private Animator m_animator;

    // 私有变量
    private TurretAttr m_SelectedTurret;    // UI上选择的炮台
    private MapCube m_CurMapCube;   // 上次点击的MapCube

    private void Awake() {
        m_btnUpgrade = m_goCanvasUpgrade.transform.Find("btnUpgrade").GetComponent<Button>();
        m_animator = m_goCanvasUpgrade.GetComponent<Animator>();
    }

    private void Start() {
        m_SelectedTurret = m_LaserBeamer;
    }

    private void Update() {
        // 建炮台
        if (Input.GetMouseButtonDown(0)) {
            // 是否点击到了UI，false => 没有点击到UI
            if (EventSystem.current.IsPointerOverGameObject() == false) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool bIsCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
                if (bIsCollider) {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();
                    // 此mapCube上没有炮台 => 新建炮台
                    if (mapCube.m_goTurret == null) {
                        // 有钱
                        if (GameManager.Instance.m_iMoney >= m_SelectedTurret.m_iCost) {
                            // 花钱
                            GameManager.Instance.ChangeMoney(-m_SelectedTurret.m_iCost);
                            // 组装炮台
                            mapCube.BuildTurret(m_SelectedTurret);
                        }
                        // 穷人
                        else {
                            // 钱闪一下，提示你是个穷人
                            m_animMoneyFlicker.SetTrigger("Flicker");
                        }
                    }
                    // 此mapCube上有炮台 => 显示升级炮台面板
                    else {
                        // 点击的是同一个MapCube，隐藏升级面板
                        if (m_CurMapCube == mapCube && m_goCanvasUpgrade.activeInHierarchy) {
                            StartCoroutine(HideUpgradePanel());
                        }
                        else {
                            ShowUpgradePanel(mapCube.transform.position, mapCube.m_bIsUpgraded);
                        }
                        // 保存此次点击的炮台
                        m_CurMapCube = mapCube;
                    }
                }
            }
        }
    }

    public void OnLaserSelected(bool isOn) {
        if (isOn) {
            m_SelectedTurret = m_LaserBeamer;
        }
    }

    public void OnMissileSelected(bool isOn) {
        if (isOn) {
            m_SelectedTurret = m_MissileLauncher;
        }
    }

    public void OnStandardSelected(bool isOn) {
        if (isOn) {
            m_SelectedTurret = m_StandardTurret;
        }
    }

    private void ShowUpgradePanel(Vector3 pos, bool bIsUpgraded) {
        StopCoroutine("HideUpgradePanel");  // 防止禁用的时候协程还在执行中
        m_goCanvasUpgrade.SetActive(false);     // 重置动画状态，使其激活时自动播放Show动画
        m_goCanvasUpgrade.SetActive(true);
        m_goCanvasUpgrade.transform.position = pos;
        m_btnUpgrade.interactable = !bIsUpgraded;
    }

    private IEnumerator HideUpgradePanel() {
        m_animator.SetTrigger("Hide");
        yield return new WaitForSeconds(0.5f);
        m_goCanvasUpgrade.SetActive(false);
    }

    public void OnBtnUpgradeDown() {
        // 有钱人
        if (GameManager.Instance.m_iMoney >= m_CurMapCube.m_CurTurretAttr.m_iUpgradeCost) {
            // 花钱
            GameManager.Instance.ChangeMoney(-m_CurMapCube.m_CurTurretAttr.m_iUpgradeCost);
            // 然后升级
            m_CurMapCube.UpgradeTurret();
        }
        // 穷人
        else {
            // 钱闪一下，提示你是个穷人
            m_animMoneyFlicker.SetTrigger("Flicker");
        }
        
        StartCoroutine(HideUpgradePanel());
    }

    public void OnBtnDestroyDown() {
        m_CurMapCube.DestroyTurret();
        StartCoroutine(HideUpgradePanel());
    }
}