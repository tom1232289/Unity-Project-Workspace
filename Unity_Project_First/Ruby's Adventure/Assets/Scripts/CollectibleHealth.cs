using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    /// <summary>
    /// 公有引用
    /// </summary>
    // 吃草莓的音效
    public AudioClip m_acPickUp;

    // 吃草莓的特效
    public GameObject m_effectPickUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            RubyController rubyController = other.GetComponent<RubyController>();
            if (rubyController.GetCurHp() < rubyController.m_fMaxHp)
            {
                rubyController.ChangeHp(1);
                rubyController.PlaySound(m_acPickUp);
                Instantiate(m_effectPickUp, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}