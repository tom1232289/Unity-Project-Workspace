using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBird : Bird
{
    public override void ReleaseSkill() {
        base.ReleaseSkill();
        Vector3 speed = m_rd.velocity;
        speed.x *= -1;
        m_rd.velocity = speed;
    }
}
