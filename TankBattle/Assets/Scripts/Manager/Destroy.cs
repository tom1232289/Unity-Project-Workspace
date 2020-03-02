using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    public float m_fDuringTime = 0.167f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,m_fDuringTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
