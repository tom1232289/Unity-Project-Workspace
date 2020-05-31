using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    // 公有引用
    public AnimationClip m_acIdle;
    public AnimationClip m_acDie;
    public AudioClip m_acKick;      // 击中怪物的音效
    public List<Material> m_materials = new List<Material>(); // 怪物的各种材质
    public SkinnedMeshRenderer m_skinnedMeshRenderer;

    // 私有引用
    private Animation m_anim;
    private BoxCollider m_collider;

    private void Awake() {
        m_anim = GetComponent<Animation>();
        m_collider = GetComponent<BoxCollider>();
    }

    private void Start() {
        RandomMonsterMesh();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.tag == Tags.Bullet) {
            // 更新UI
            UIManager.Instance.AddScoreNum();
            // 播放击中音效
            AudioSource.PlayClipAtPoint(m_acKick, Camera.main.transform.position);
            // 销毁子弹 
            Destroy(other.gameObject);
            // 播放怪物死亡动画
            m_anim.clip = m_acDie;
            m_anim.Play();
            // 禁用Collider
            m_collider.enabled = false;
            // 怪物死亡
            StartCoroutine("Die");
        }
    }

    // 禁用的时候恢复默认动画
    private void OnDisable() {
        m_anim.clip = m_acIdle;
    }

    private IEnumerator Die() {
        yield return new WaitForSeconds(0.5f);
        GetComponentInParent<MonsterManager>().MonsterDie();
    }

    // 随机生成怪物的材质
    private void RandomMonsterMesh() {
        int iRandom = Random.Range(0, m_materials.Count);
        Material randomMaterial = m_materials[iRandom];
        m_skinnedMeshRenderer.material = randomMaterial;
    }
}