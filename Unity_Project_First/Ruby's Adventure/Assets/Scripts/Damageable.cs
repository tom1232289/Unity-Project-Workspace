using UnityEngine;

public class Damageable : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            RubyController rubycontroller = other.GetComponent<RubyController>();
            rubycontroller.ChangeHp(-1);
        }
    }
}