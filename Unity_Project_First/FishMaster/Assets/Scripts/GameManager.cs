using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // 单例
    private static GameManager m_instance;

    public static GameManager Instance {
        get { return m_instance; }
    }

    // 公有引用
    public GameObject[] m_Guns;     // 各种枪
    public GameObject[] m_Bullets1; // 第一档的子弹
    public GameObject[] m_Bullets2;
    public GameObject[] m_Bullets3;
    public GameObject[] m_Bullets4;
    public GameObject[] m_Bullets5;
    public Color m_colorGoldSource; // 金币原来的颜色
    public GameObject m_efGold;     // 金币特效
    public GameObject m_efFire;
    public GameObject m_efChangeGun;
    public GameObject m_efLvUp;
    public Sprite[] m_spriteBgs;    // 背景图片
    public GameObject m_efSeaWave;

    // 公有变量
    public int m_iLevel = 0;
    public int m_iGold = 500;
    public int m_iExp = 0;
    public const int m_iBigCountdown = 240;
    public const int m_iSmallCountdown = 60;
    public float m_fBigTimer = m_iBigCountdown;
    public float m_fSmallTimer = m_iSmallCountdown;
    public const int m_iChangeBg = 20;

    // 私有引用
    private Text m_textCost;            // 每颗子弹的开销
    private Transform m_bulletHolder;
    private Text m_textCountdown;       // 大奖金倒计时
    private GameObject m_btnCountdown;  // 获取大奖金的按钮
    private Text m_textGold;
    private Text m_textLevel;
    private Text m_textLevelName;
    private Text m_textSmallCountdown;  // 小奖金倒计时
    private Slider m_sliderExp;         // 经验滑动条
    private GameObject m_LvUpTips;      // 升级提示
    private Image m_imgBg;              // 背景图片
    private Toggle m_toggleMute;

    // 私有变量
    private int[] m_costPerShoot = { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 300, 600, 700, 800, 1000, 1300, 1500, 1800, 2000 };  // 每一炮花费的金币数和造成的伤害值
    private int m_iCost = 0;    // 使用的是第几档的炮弹
    private string[] m_sLevelName = { "Glock", "USP", "Eagle", "Mp5", "Famas", "AK47", "M4A4", "Aug", "AWP", "Boom" };
    private int m_iBgIndex;     // 背景图片的下标

    private void Awake() {
        m_instance = this;
        m_textCost = GameObject.Find("BulletCost").transform.Find("Text").GetComponent<Text>();
        m_bulletHolder = GameObject.Find("bulletHolder").transform;
        m_textCountdown = GameObject.Find("textCountdown").GetComponent<Text>();
        m_btnCountdown = GameObject.Find("panelCountdown").transform.Find("btnCountdown").gameObject;
        m_textGold = GameObject.Find("textGold").GetComponent<Text>();
        m_textLevel = GameObject.Find("textLevel").GetComponent<Text>();
        m_textLevelName = GameObject.Find("textLevelName").GetComponent<Text>();
        m_textSmallCountdown = GameObject.Find("textSmallCountdown").GetComponent<Text>();
        m_sliderExp = GameObject.Find("SliderExp").GetComponent<Slider>();
        m_LvUpTips = GameObject.Find("UI").transform.Find("LvUpTips").gameObject;
        m_imgBg = GameObject.Find("Bg").GetComponent<Image>();
        m_toggleMute = GameObject.Find("Order180Canvas").transform.Find("panelSetting").Find("toggleMute").GetComponent<Toggle>();
    }

    private void Start() {
        LoadData();
        m_toggleMute.isOn = !AudioManager.Instance.m_bIsMute;
    }

    private void Update() {
        ChangeBulletCost();
        Fire();
        UpdateUI();
        ChangeBg();
    }

    private void ChangeBulletCost() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            OnBtnMinusClickedDown();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            OnBtnPlusClickedDown();
        }
    }

    public void OnBtnPlusClickedDown() {
        m_Guns[m_iCost / 4].SetActive(false);
        ++m_iCost;
        m_iCost = (m_iCost > m_costPerShoot.Length - 1) ? 0 : m_iCost;
        m_Guns[m_iCost / 4].SetActive(true);
        m_textCost.text = "$" + m_costPerShoot[m_iCost];
        // 换枪特效
        Instantiate(m_efChangeGun);
        // 换枪音效
        AudioManager.Instance.PlayAudio(AudioManager.Instance.m_acChangeGun);
    }

    public void OnBtnMinusClickedDown() {
        m_Guns[m_iCost / 4].SetActive(false);
        --m_iCost;
        m_iCost = (m_iCost < 0) ? (m_costPerShoot.Length - 1) : m_iCost;
        m_Guns[m_iCost / 4].SetActive(true);
        m_textCost.text = "$" + m_costPerShoot[m_iCost];
        // 换枪特效
        Instantiate(m_efChangeGun);
        // 换枪音效
        AudioManager.Instance.PlayAudio(AudioManager.Instance.m_acChangeGun);
    }

    private void Fire() {
        GameObject[] useBullets = m_Bullets5;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            // 钱不够就打不了
            if (m_iGold - m_costPerShoot[m_iCost] < 0) {
                StartCoroutine(GoldNotEnough());
                return;
            }

            if (m_iCost / 4 == 0) {
                useBullets = m_Bullets1;
            }
            else if (m_iCost / 4 == 1) {
                useBullets = m_Bullets2;
            }
            else if (m_iCost / 4 == 2) {
                useBullets = m_Bullets3;
            }
            else if (m_iCost / 4 == 3) {
                useBullets = m_Bullets4;
            }
            else if (m_iCost / 4 == 4) {
                useBullets = m_Bullets5;
            }

            int iBullet = m_iLevel % 10;
            iBullet = (iBullet > 9) ? 9 : iBullet;
            GameObject goBullet = Instantiate(useBullets[iBullet]);
            goBullet.transform.SetParent(m_bulletHolder, false);
            goBullet.transform.position = m_Guns[m_iCost / 4].transform.Find("posFire").transform.position;
            goBullet.transform.rotation = m_Guns[m_iCost / 4].transform.Find("posFire").transform.rotation;
            goBullet.AddComponent<Ef_AutoMove>().m_dir = Vector3.up;
            goBullet.GetComponent<Ef_AutoMove>().m_fSpeed = goBullet.GetComponent<BulletAttr>().m_iSpeed;
            goBullet.GetComponent<BulletAttr>().m_iDamage = m_costPerShoot[m_iCost];
            // 打炮扣钱
            m_iGold -= m_costPerShoot[m_iCost];
            // 开火特效
            Instantiate(m_efFire, m_Guns[m_iCost / 4].transform.Find("posFire").transform.position, Quaternion.identity);
            // 开火音效
            AudioManager.Instance.PlayAudio(AudioManager.Instance.m_acFire);
        }
    }

    private void UpdateUI() {
        // 计时器减1
        m_fBigTimer -= Time.deltaTime;
        m_fSmallTimer -= Time.deltaTime;
        // 发小奖金
        if (m_fSmallTimer <= 0) {
            m_fSmallTimer = m_iSmallCountdown;
            m_iGold += 50;
        }
        // 发大奖金
        if (m_fBigTimer <= 0 && !m_btnCountdown.activeSelf) {
            m_textCountdown.gameObject.SetActive(false);
            m_btnCountdown.SetActive(true);
        }
        // 升级
        // 经验等级换算公式：升级所需经验 = 1000 + 200 * 当前等级
        while (m_iExp >= 1000 + 200 * m_iLevel) {
            m_iExp -= (1000 + 200 * m_iLevel);
            ++m_iLevel;
            // 升级提示
            m_LvUpTips.SetActive(true);
            m_LvUpTips.transform.Find("Text").GetComponent<Text>().text = m_iLevel.ToString();
            StartCoroutine(m_LvUpTips.GetComponent<HideSelf>().Hide(0.5f));
            // 升级特效
            Instantiate(m_efLvUp);
            // 升级音效
            AudioManager.Instance.PlayAudio(AudioManager.Instance.m_acLvUp);
        }
        // 等级
        m_textLevel.text = m_iLevel.ToString();
        // 等级名
        if (m_iLevel / 10 <= 9) {
            // 等级在100内
            m_textLevelName.text = m_sLevelName[m_iLevel / 10];
        }
        else {
            // 等级超出100
            m_textLevelName.text = m_sLevelName[9];
        }
        // 经验条
        m_sliderExp.value = (float)m_iExp / (1000 + 200 * m_iLevel);
        // 金币
        m_textGold.text = "$" + m_iGold;
        // 倒计时
        m_textCountdown.text = (int)m_fBigTimer + "s";
        m_textSmallCountdown.text = (int)m_fSmallTimer / 10 + "  " + (int)m_fSmallTimer % 10;
    }

    public void OnBtnCountDownClicked() {
        m_iGold += 500;
        m_btnCountdown.SetActive(false);
        m_textCountdown.gameObject.SetActive(true);
        m_fBigTimer = m_iBigCountdown;
        // 播放特效
        Instantiate(m_efGold);
        AudioManager.Instance.PlayAudio(AudioManager.Instance.m_acReward);
    }

    private IEnumerator GoldNotEnough() {
        //m_textGold.color = m_colorGoldSource;
        m_textGold.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        m_textGold.color = m_colorGoldSource;
    }

    // 每20级更换一下背景
    private void ChangeBg() {
        if (m_iLevel / m_iChangeBg > m_iBgIndex) {
            ++m_iBgIndex;
            // 生成水波纹
            Instantiate(m_efSeaWave);
            if (m_iBgIndex >= m_spriteBgs.Length - 1) {
                return;
            }
            // 更换背景
            m_imgBg.sprite = m_spriteBgs[m_iBgIndex];
        }
    }

    public void SaveData() {
        PlayerPrefs.SetInt("gold", m_iGold);
        PlayerPrefs.SetInt("level", m_iLevel);
        PlayerPrefs.SetInt("exp", m_iExp);
        PlayerPrefs.SetFloat("bigTimer", m_fBigTimer);
        PlayerPrefs.SetFloat("smallTimer", m_fSmallTimer);
        PlayerPrefs.SetInt("mute", Convert.ToInt32(AudioManager.Instance.m_bIsMute));
    }

    private void LoadData() {
        m_iGold = PlayerPrefs.GetInt("gold", m_iGold);
        m_iLevel = PlayerPrefs.GetInt("level", m_iLevel);
        m_iExp = PlayerPrefs.GetInt("exp", m_iExp);
        m_fBigTimer = PlayerPrefs.GetFloat("bigTimer", m_fBigTimer);
        m_fSmallTimer = PlayerPrefs.GetFloat("smallTimer", m_fSmallTimer);
        AudioManager.Instance.m_bIsMute = Convert.ToBoolean(PlayerPrefs.GetInt("mute", Convert.ToInt32(AudioManager.Instance.m_bIsMute)));
    }
}