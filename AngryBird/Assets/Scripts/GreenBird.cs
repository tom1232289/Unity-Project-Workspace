using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBird : Bird
{
    public override void ReleaseSkill() {
        base.ReleaseSkill();
        Vector3 speed = m_rb.velocity;
        speed.x *= -1;
        m_rb.velocity = speed;
    }
}
