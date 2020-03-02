using UnityEngine;

public class TankMovement : MonoBehaviour {

    // 公有引用
    public AudioClip m_IdleClip;

    public AudioClip m_DrivingClip;

    // 公有变量
    public float m_fSpeed = 20f;

    // 私有变量
    private float m_fVertical;

    private float m_fHorizontal;

    // 私有引用
    private AudioSource m_MoveAudio;

    private void Awake() {
        m_MoveAudio = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        if (GameManager.Instance.m_bIsGameover)
            return;

        m_fVertical = Input.GetAxis("Vertical");
        if (m_fVertical > 0) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (m_fVertical < 0) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        transform.Translate(Vector3.up * m_fVertical * Time.fixedDeltaTime * m_fSpeed, Space.World);

        // 播放坦克运动的声音
        if (Mathf.Abs(m_fVertical) > 0.05f) {
            PlayAudio(false);
        }

        if (m_fVertical != 0)
            return;

        m_fHorizontal = Input.GetAxis("Horizontal");
        if (m_fHorizontal > 0) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else if (m_fHorizontal < 0) {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }

        transform.Translate(Vector3.right * m_fHorizontal * Time.fixedDeltaTime * m_fSpeed, Space.World);

        // 播放坦克运动或闲置的声音
        if (Mathf.Abs(m_fHorizontal) > 0.05f) {
            PlayAudio(false);
        }
        else {
            PlayAudio(true);
        }
    }

    private void PlayAudio(bool bIsIdle) {
        if (bIsIdle) {
            m_MoveAudio.clip = m_IdleClip;
            if (!m_MoveAudio.isPlaying) {
                m_MoveAudio.Play();
            }
        }
        else {
            m_MoveAudio.clip = m_DrivingClip;
            if (!m_MoveAudio.isPlaying) {
                m_MoveAudio.Play();
            }
        }
    }
}