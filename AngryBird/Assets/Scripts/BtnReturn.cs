using UnityEngine;

public class BtnReturn : MonoBehaviour {

    public void OnBtnReturnClicked() {
        transform.parent.gameObject.SetActive(false);
        GameObject.Find("ScorllPanel").transform.Find("Maps").gameObject.SetActive(true);
    }
}