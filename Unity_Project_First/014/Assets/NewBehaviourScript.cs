using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(ieFunc());
        new WaitForSeconds(10);
        for (int i = 0; i < 100000; ++i) {
            Debug.Log("1");
        }
    }

    // Update is called once per frame
    void Update()
    {
//         Debug.Log("Update()");
    }

    void LateUpdate() {
//         Debug.Log("LateUpdate()");
    }

    IEnumerator ieFunc() {
        for (int i = 0; i < 1; ++i) {
            Debug.Log("3");
        }
        yield return new WaitForSeconds(1);
        Debug.Log("4");

        yield return null;
    }
}
