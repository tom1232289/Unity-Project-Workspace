using UnityEngine;

public class Heart : MonoBehaviour {
    public Sprite m_BrokenSprite;
    public GameObject m_ExplosionPrefab;
    public AudioClip m_AudioClipDie;

    private SpriteRenderer m_SpriteRenderer;

    // Start is called before the first frame update
    private void Awake() {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update() {
    }

    private void Die() {
        m_SpriteRenderer.sprite = m_BrokenSprite;
        Instantiate(m_ExplosionPrefab, transform.position, transform.rotation);
        PlayerManager.Instanse.m_bIsGameover = true;
        AudioSource.PlayClipAtPoint(m_AudioClipDie, transform.position);
    }
}