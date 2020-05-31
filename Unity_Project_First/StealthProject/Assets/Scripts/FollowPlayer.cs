using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    // 公有变量
    public float m_fZoomSpeed = 1;         // 镜头缩放的速度
    public float m_fMinDistance = 1.5f;    // Camera到主角最近的距离
    public float m_fMaxDistance = 10f;     // Camera到主角最远的距离
    public float m_fCameraMinHeight = 0.2f; // Camera最小的高度

    // 私有引用
    private Transform m_player;

    // 私有变量
    private Vector3 m_Offset;       // 摄像机和主角之间的偏移
    private float m_fDistance;      // 摄像机和主角之间的距离

    private void Awake() {
        m_player = GameObject.FindGameObjectWithTag("Player").transform.Find("posPlayer").transform;

        m_Offset = transform.position - m_player.position;
        m_fDistance = m_Offset.magnitude;
    }

    private void LateUpdate() {
        // 镜头缩放
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            m_fDistance = m_fDistance - m_fZoomSpeed;
            if (m_fDistance < m_fMinDistance) {
                m_fDistance = m_fMinDistance;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            m_fDistance = m_fDistance + m_fZoomSpeed;
            if (m_fDistance > m_fMaxDistance) {
                m_fDistance = m_fMaxDistance;
            }
        }

        // 按下了右键
        if (Input.GetMouseButton(1)) {
            // 获取鼠标的移动
            float fCamH = Input.GetAxis("Mouse X");
            float fCamV = Input.GetAxis("Mouse Y") * -1;
            // 摄像机水平旋转
            transform.RotateAround(m_player.position, Vector3.up, fCamH);
            // 摄像机垂直旋转
            transform.RotateAround(m_player.position, transform.right, fCamV);
            // 刷新摄像机与玩家之间的偏移
            m_Offset = transform.position - m_player.position;
        }
        m_Offset = m_Offset.normalized * m_fDistance;
        // 重置Camera的位置
        transform.position = m_player.position + m_Offset;
        // 其中Camera不能低于地面
        if (transform.position.y < m_fCameraMinHeight) {
            transform.position = new Vector3(transform.position.x, m_fCameraMinHeight, transform.position.z);
        }
        // 使Camera望向玩家（解决玩家移动的同时按住右键后Camera朝向问题）
        transform.LookAt(m_player.transform);
    }
}