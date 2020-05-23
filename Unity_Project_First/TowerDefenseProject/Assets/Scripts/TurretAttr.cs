using UnityEngine;

[System.Serializable]
public class TurretAttr {
    public GameObject m_goTurretPrefab;
    public int m_iCost;
    public GameObject m_goTurretUpgradePrefab;
    public int m_iUpgradeCost;
    public TurretType m_Type;
}

public enum TurretType {
    LaserBeamer,
    MissileLauncher,
    StandardTurret
}