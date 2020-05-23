using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    // 公有引用
    public WaveAttr[] m_Waves;
    public Transform m_transStart;    // 敌人生成的位置

    // 公有变量
    public float m_fWaveRate = 3f;
    public static int m_iAliveCount = 0;

    // 私有变量
    private Coroutine m_coroutine;

    private void Start() {
        m_coroutine = StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy() {
        foreach (var wave in m_Waves) {
            for (int i = 0; i < wave.m_iCount; ++i) {
                GameObject.Instantiate(wave.m_go, m_transStart.position, Quaternion.identity);
                ++m_iAliveCount;
                if (i != wave.m_iCount - 1)
                    yield return new WaitForSeconds(wave.m_fRate);
            }

            while (m_iAliveCount > 0) {
                yield return 0;
            }
            yield return new WaitForSeconds(m_fWaveRate);
        }

        while (m_iAliveCount > 0) {
            yield return 0;
        }
        GameManager.Instance.Win();
    }

    public void Stop() {
        StopCoroutine(m_coroutine);
    }
}