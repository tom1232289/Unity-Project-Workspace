using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour {

    // 公有引用
    public GameObject m_effectBuild;    // 建造炮台的特效

    // 私有引用
    private Renderer m_renderer;

    [HideInInspector]
    public GameObject m_goTurret;   // 当前cube身上的炮台
    [HideInInspector]
    public TurretAttr m_CurTurretAttr; // 当前cube身上炮台的参数

    // 私有变量
    [HideInInspector] public bool m_bIsUpgraded = false;

    private void Awake() {
        m_renderer = GetComponent<Renderer>();
    }

    private void OnMouseEnter() {
        if (m_goTurret == null && EventSystem.current.IsPointerOverGameObject() == false) {
            m_renderer.material.color = Color.red;
        }
    }

    private void OnMouseExit() {
        m_renderer.material.color = Color.white;
    }

    public void BuildTurret(TurretAttr turretAttr) {
        m_bIsUpgraded = false;
        // 生成炮台
        m_goTurret = GameObject.Instantiate(turretAttr.m_goTurretPrefab, transform.position, Quaternion.identity);
        // 播放特效
        GameObject.Instantiate(m_effectBuild, transform.position, Quaternion.identity);
        // 保存炮台参数
        m_CurTurretAttr = turretAttr;
    }

    public void UpgradeTurret() {
        if (m_bIsUpgraded == true)
            return;

        m_bIsUpgraded = true;
        Destroy(m_goTurret);
        // 生成炮台
        m_goTurret = GameObject.Instantiate(m_CurTurretAttr.m_goTurretUpgradePrefab, transform.position, Quaternion.identity);
        // 播放特效
        GameObject.Instantiate(m_effectBuild, transform.position, Quaternion.identity);
    }

    public void DestroyTurret() {
        Destroy(m_goTurret);
        // 播放特效
        GameObject.Instantiate(m_effectBuild, transform.position, Quaternion.identity);
        m_bIsUpgraded = false;
        m_goTurret = null;
        m_CurTurretAttr = null;
    }
}