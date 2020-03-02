using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSelf : MonoBehaviour
{
    public IEnumerator Hide(float delay) {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
