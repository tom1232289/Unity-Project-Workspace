using SonicBloom.Koreo;
using UnityEngine;

public class LightTest : MonoBehaviour {

    // 公有引用
    public Light[] m_QuarterNoteGroup;
    public Light[] m_EighthNoteGroup;

    // 私有变量
    private int iLastQuarterNote = 0;   // 上一个四分音符
    private int iLastEighthNote = 0;    // 上一个八分音符

    private void Update() {
        /// 四分音符
        int iCurQuarterNote = Mathf.RoundToInt(Koreographer.GetBeatTime());
        // 当前的四分音符 != 上一个四分音符
        if (iCurQuarterNote != iLastQuarterNote) {
            // 开关灯
            SwitchGroup(m_QuarterNoteGroup, iLastQuarterNote % 2 != 0);
            // 记录此时的四分音符
            iLastQuarterNote = iCurQuarterNote;
        }

        /// 八分音符
        int iCurEighthNote = Mathf.RoundToInt(Koreographer.GetBeatTime(null, 2));
        // 当前的八分音符 != 上一个八分音符
        if (iCurEighthNote != iLastEighthNote) {
            // 开关灯
            SwitchGroup(m_EighthNoteGroup, iLastEighthNote % 2 != 0);
            // 记录此时的八分音符
            iLastEighthNote = iCurEighthNote;
        }
    }

    // 上一个四(八)分音符为偶数 => 开灯；奇数 => 关灯
    private void SwitchGroup(Light[] lights, bool bIsOpen) {
        foreach (var light in lights) {
            light.enabled = bIsOpen;
        }
    }
}