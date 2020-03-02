using UnityEngine;

public class Born : MonoBehaviour {
    public GameObject m_PlayerPrefab;
    public float m_fDuringTime = 1.5f;
    public bool m_bCreatePlayer;
    public GameObject[] m_EnemyPrefabList;

    // Start is called before the first frame update
    private void Start() {
        Invoke("BornTank", m_fDuringTime);
        Destroy(gameObject, m_fDuringTime);
    }

    // Update is called once per frame
    private void Update() {
    }

    private void BornTank() {
        if (m_bCreatePlayer) {
            Instantiate(m_PlayerPrefab, transform.position, Quaternion.identity);
        }
        else {
            int num = Random.Range(0, 2);
            Instantiate(m_EnemyPrefabList[num], transform.position, Quaternion.identity);
        }
    }
}