using UnityEngine;

public class Player : MonoBehaviour {

    // 公有变量
    public float m_fSpeed = 10f;
    public float m_fRotateSpeed = 6f;
    public bool m_bHasKey;  // 是否持有开门的钥匙

    // 私有引用
    private Animator m_anim;
    private AudioSource m_as;

    private void Awake() {
        m_anim = GetComponent<Animator>();
        m_as = GetComponent<AudioSource>();
    }

    private void Update() {
        // 判断是否是潜行
        if (Input.GetKey(KeyCode.LeftShift)) {
            m_anim.SetBool("Sneak", true);
        }
        else {
            m_anim.SetBool("Sneak", false);
        }

        float fHorizontal = Input.GetAxis("Horizontal");
        float fVertical = Input.GetAxis("Vertical");
        // 移动
        if (Mathf.Abs(fHorizontal) > 0.1f || Mathf.Abs(fVertical) > 0.1f) {
            // 向前移动
            float fSpeed = Mathf.Lerp(m_anim.GetFloat("Speed"), 5.6f, m_fSpeed * Time.deltaTime);
            m_anim.SetFloat("Speed", fSpeed);


            // 获取目标方向
            Vector3 targetDir = transform.position - Camera.main.transform.position;

            if (fVertical > 0 && Mathf.Abs(fHorizontal) < 0.1f) {
                targetDir = transform.position - Camera.main.transform.position;
                targetDir.y = 0;
            }
            else if (fVertical < 0 && Mathf.Abs(fHorizontal) < 0.1f) {
                targetDir = Camera.main.transform.position - transform.position;
                targetDir.y = 0;
            }
            else if (fHorizontal > 0 && Mathf.Abs(fVertical) < 0.1f) {
                targetDir = transform.position - Camera.main.transform.position;
                targetDir.y = 0;
                targetDir = Quaternion.Euler(0, 90, 0) * targetDir;
            }
            else if (fHorizontal < 0 && Mathf.Abs(fVertical) < 0.1f) {
                targetDir = transform.position - Camera.main.transform.position;
                targetDir.y = 0;
                targetDir = Quaternion.Euler(0, -90, 0) * targetDir;
            }
            else if (fVertical > 0 && fHorizontal > 0) {
                targetDir = transform.position - Camera.main.transform.position;
                targetDir.y = 0;
                targetDir = Quaternion.Euler(0, 45, 0) * targetDir;
            }
            else if (fVertical > 0 && fHorizontal < 0) {
                targetDir = transform.position - Camera.main.transform.position;
                targetDir.y = 0;
                targetDir = Quaternion.Euler(0, -45, 0) * targetDir;
            }
            else if (fVertical < 0 && fHorizontal > 0) {
                targetDir = Camera.main.transform.position - transform.position;
                targetDir.y = 0;
                targetDir = Quaternion.Euler(0, -45, 0) * targetDir;
            }
            else if (fVertical < 0 && fHorizontal < 0) {
                targetDir = Camera.main.transform.position - transform.position;
                targetDir.y = 0;
                targetDir = Quaternion.Euler(0, 45, 0) * targetDir;
            }

            // 角色转向
            /*第一种转向*/
            //float fAngle = Vector3.SignedAngle(transform.forward, targetDir, Vector3.up);
            //float fNewAngle = Mathf.Lerp(0, fAngle, m_fRotateSpeed * Time.deltaTime);
            //transform.Rotate(Vector3.up, fNewAngle);
            ///*第二种转向*/
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_fRotateSpeed * Time.deltaTime);
        }
        // 停止
        else {
            m_anim.SetFloat("Speed", 0);
        }

        // 走或者跑 => 播放脚步声音
        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) {
            PlayWalkSound();
        }
        else {
            StopWalkSound();
        }
    }

    // 播放脚步声音
    private void PlayWalkSound() {
        m_as.enabled = true;
    }

    // 停止播放脚步声音
    private void StopWalkSound() {
        m_as.enabled = false;
    }
}