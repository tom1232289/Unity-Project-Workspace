using UnityEngine;

public class Pin : MonoBehaviour {
    private bool m_bIsFly = false;  // 开关。开：针朝小球运动；关：针朝开始位置运动

    private bool m_bIsReachStartPoint = false;  // 若针到了开始位置后，就不再运动了

    private Transform m_startPoint; // 针发射的开始点

    private Transform m_circle; // 小球的圆心

    private Vector3 m_targetCirclePos;

    public float m_fSpeed = 20;  // 针运动的速度

    // Start is called before the first frame update
    private void Start() {
        m_startPoint = GameObject.Find("StartPoint").transform;
        m_circle = GameObject.Find("Circle").transform;
        m_targetCirclePos = m_circle.position;
        m_targetCirclePos.y -= 2.28f;
    }

    // Update is called once per frame
    private void Update() {
        // 针朝开始位置运动
        if (m_bIsFly == false) {
            // 针还没到开始位置
            if (m_bIsReachStartPoint == false) {
                transform.position = Vector3.MoveTowards(transform.position, m_startPoint.transform.position, m_fSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, m_startPoint.transform.position) < 0.05f) {
                    m_bIsReachStartPoint = true; // 针到了开始位置后，就不再运动了
                }
            }
        }
        // 针朝小球运动
        else {
            transform.position = Vector3.MoveTowards(transform.position, m_targetCirclePos, m_fSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_targetCirclePos) < 0.05f) {
                transform.position = m_targetCirclePos;
                transform.parent = m_circle;
                m_bIsFly = false;
                m_bIsReachStartPoint = true;
            }
        }
    }

    // 针朝小球运动
    public void StartFly() {
        m_bIsFly = true;    // 将开关设置为true，使针朝小球运动
    }
}