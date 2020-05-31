using UnityEngine;

public class Gun : MonoBehaviour {

    // 公有引用
    public Transform m_posFire;     // 开火的位置
    public GameObject m_goBullet;   // 子弹
    public AudioClip m_acFire;      // 开枪的音效

    // 私有引用
    private Animation m_anim;

    // 公有变量
    public float m_fFireCD = 1;     // 开火的CD
    public float m_fForce = 2000;   // 施加给子弹的力的大小

    // 私有变量
    private float m_fCurFireCD;

    private void Awake() {
        m_anim = GetComponent<Animation>();
    }

    private void Update() {
        if (GameManager.Instance.m_bIsPaused)
            return;

        GunRotate();
        Fire();
    }

    // 枪跟随鼠标旋转
    private void GunRotate() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // 通过MainCamera射出一道无限延伸的射线到Mouse的位置
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo)) {
            transform.LookAt(hitInfo.point);
        }
    }

    private void Fire() {
        m_fCurFireCD += Time.deltaTime;
        if (Input.GetMouseButtonDown(0)) {
            if (m_fCurFireCD >= m_fFireCD) {
                m_fCurFireCD = 0;
                // 播放开枪动画
                m_anim.Play();
                // 播放开枪音效
                AudioSource.PlayClipAtPoint(m_acFire, transform.position);
                // 生成子弹
                GameObject goBullet = Instantiate(m_goBullet, m_posFire.position, Quaternion.identity);
                // 给子弹一个力
                goBullet.GetComponent<Rigidbody>().AddForce(transform.forward * m_fForce);
                // 更新UI
                UIManager.Instance.AddShootNum();
            }
        }
    }
}