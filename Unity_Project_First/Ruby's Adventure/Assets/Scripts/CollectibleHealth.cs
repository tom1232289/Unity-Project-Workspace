using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            RubyController rubyController = other.GetComponent<RubyController>();
            if (rubyController.GetCurHp() < rubyController.m_fMaxHp)
            {
                rubyController.ChangeHp(1);
                Destroy(gameObject);
            }
        }
    }
}