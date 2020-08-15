using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // 摄像机要跟随的对象
    private Transform m_Target;

    // 摄像机 和 跟随的对象 之间的 偏移
    private Vector3 m_Offset;

    private void Update()
    {
        if (m_Target == null && GameObject.FindGameObjectWithTag("Character") != null)
        {
            // 设置 要跟随的对象
            m_Target = GameObject.FindGameObjectWithTag("Character").transform;
            // 设置 偏移
            m_Offset = m_Target.transform.position - transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (m_Target != null)
        {
            float fPosX = Mathf.Lerp(transform.position.x, m_Target.position.x - m_Offset.x, 0.05f);
            float fPosY = Mathf.Lerp(transform.position.y, m_Target.position.y - m_Offset.y, 0.05f);

            if (fPosY > transform.position.y)
            {
                transform.position = new Vector3(fPosX, fPosY, transform.position.z);
            }
        }
    }
}