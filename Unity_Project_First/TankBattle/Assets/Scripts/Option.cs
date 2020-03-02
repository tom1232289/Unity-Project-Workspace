using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Option : MonoBehaviour {
    public Transform m_Pos1;
    public Transform m_Pos2;

    private int m_iChoice = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            m_iChoice = 1;
            transform.position = m_Pos1.position;
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            m_iChoice = 2;
            transform.position = m_Pos2.position;
        }

        if (m_iChoice == 1 && Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene("main");
        }
    }
}
