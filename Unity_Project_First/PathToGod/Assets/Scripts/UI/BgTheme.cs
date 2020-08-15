using UnityEngine;

public class BgTheme : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ManagerVars vars = ManagerVars.GetManagerVars();
        int iRandom = Random.Range(0, vars.m_listBgTheme.Count);
        m_spriteRenderer.sprite = vars.m_listBgTheme[iRandom];
    }
}