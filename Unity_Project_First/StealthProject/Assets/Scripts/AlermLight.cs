using UnityEngine;

public class AlermLight : MonoBehaviour {

    // 单例
    private static AlermLight m_instance;

    public static AlermLight Instance {
        get { return m_instance; }
    }

    // 公有变量
    public bool m_bIsOn = false;        // 是否打开警报灯
    public float m_fLowIntensity = 0;   // 光的最低强度
    public float m_fHighIntensity = 1;  // 光的最高强度
    public float m_fSpeed = 0.3f;       // 光变化的速度

    // 私有变量
    private Light m_light;
    private float m_fTargetIntensity;   // 光要变化的目标强度

    private void Awake() {
        m_instance = this;
        m_light = GetComponent<Light>();
        m_fTargetIntensity = m_fHighIntensity;  // 先变到最高强度
    }

    private void Update() {
        // 打开了警报灯
        if (m_bIsOn) {
            // 警报灯开始闪烁
            m_light.intensity = Mathf.Lerp(m_light.intensity, m_fTargetIntensity, m_fSpeed * Time.deltaTime);
            // 到了目标强度了 => 反过来
            if (Mathf.Abs(m_light.intensity - m_fTargetIntensity) < 0.05f) {
                if (m_fTargetIntensity == m_fLowIntensity) {
                    m_fTargetIntensity = m_fHighIntensity;
                }
                else if (m_fTargetIntensity == m_fHighIntensity) {
                    m_fTargetIntensity = m_fLowIntensity;
                }
            }
        }
        // 警报灯关掉了 或者 没开警报灯
        else {
            // 优化：由于lerp永远不会等于目标值，所以在此优化处理
            if (m_light.intensity < 0.05f) {
                m_light.intensity = 0;
                return;
            }

            // 强度渐变到0
            m_light.intensity = Mathf.Lerp(m_light.intensity, 0, m_fSpeed * Time.deltaTime);
        }
    }
}