using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    // 公有变量
    public float m_fAccelerate = 1.5f;

    public override void ReleaseSkill() {
        base.ReleaseSkill();
        m_rd.velocity *= m_fAccelerate;
    }
}
