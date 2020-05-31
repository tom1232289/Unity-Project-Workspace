using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // 单例
    private static GameManager m_instance;

    public static GameManager Instance {
        get { return m_instance; }
    }

    // 公有引用
    public GameObject m_goMenu;
    public Toggle m_toggleMusic;

    // 公有变量
    public bool m_bIsPaused;    // 是否是暂停状态

    // 私有引用
    private AudioSource m_as;
    private List<GameObject> m_Monsters = new List<GameObject>();

    // 私有变量
    private bool m_bIsMute;

    private void Awake() {
        m_instance = this;
        m_as = GetComponent<AudioSource>();
        Transform transMonsters = GameObject.Find("Monsters").transform;
        for (int i = 0; i < transMonsters.childCount; ++i) {
            m_Monsters.Add(transMonsters.GetChild(i).gameObject);
        }
    }

    private void Start() {
        m_bIsMute = !Convert.ToBoolean(PlayerPrefs.GetInt("HasMusic", 1));
        m_toggleMusic.isOn = !m_bIsMute;
        SwitchMusic();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    // 暂停游戏
    private void Pause() {
        m_bIsPaused = true;
        m_goMenu.SetActive(true);
        Time.timeScale = 0;
    }

    // 恢复游戏
    private void Resume() {
        m_bIsPaused = false;
        m_goMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // 新游戏按钮
    public void OnBtnNewGameDown() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    // 继续游戏按钮
    public void OnBtnContinueGameDown() {
        Resume();
    }

    // 保存游戏按钮
    public void OnBtnSaveGameDown() {
        //SaveByBin();
        //SaveByJson();
        SaveByXML();
    }

    // 加载游戏按钮
    public void OnBtnLoadGameDown() {
        //LoadByBin();
        //LoadByJson();
        LoadByXML();
    }

    // 退出游戏按钮
    public void OnBtnExitGameDown() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 音乐开关按钮
    public void OnToggleSwitchMusicDown(bool isOn) {
        m_bIsMute = !isOn;
        SwitchMusic();
    }

    // 音乐开关
    private void SwitchMusic() {
        if (m_bIsMute) {
            m_as.Pause();
            PlayerPrefs.SetInt("HasMusic", 0);
        }
        else {
            m_as.Play();
            PlayerPrefs.SetInt("HasMusic", 1);
        }
    }

    // 创建存档的实例
    private SaveInfo CreateSaveInfoGO() {
        SaveInfo saveInfo = new SaveInfo();
        for (int i = 0; i < m_Monsters.Count; ++i) {
            MonsterManager monsterManager = m_Monsters[i].GetComponent<MonsterManager>();
            if (monsterManager.m_goActiveMonster != null) {
                // 保存 怪物在哪几个九宫格里存活
                saveInfo.m_listLivingPos.Add(i);
                // 保存 怪物的类型
                saveInfo.m_listLivingMonsterType.Add(monsterManager.m_iMonsterType);
            }
        }

        // 保存 射击数
        saveInfo.m_iShootNum = UIManager.Instance.m_iShoot;
        // 保存 分数
        saveInfo.m_iScore = UIManager.Instance.m_iScore;

        return saveInfo;
    }

    private void LoadSaveInfoGO(SaveInfo saveInfo) {
        // 将九宫格中的怪物清空，并重置所有计数
        for (int i = 0; i < m_Monsters.Count; ++i) {
            MonsterManager monsterManager = m_Monsters[i].GetComponent<MonsterManager>();
            monsterManager.ClearMonsters();
        }

        UIManager.Instance.m_iShoot = 0;
        UIManager.Instance.m_iScore = 0;
        // 根据SaveInfo对象中的数据，在指定的九宫格中激活指定的怪物
        for (int i = 0; i < saveInfo.m_listLivingPos.Count; ++i) {
            int iPos = saveInfo.m_listLivingPos[i];
            int iType = saveInfo.m_listLivingMonsterType[i];
            m_Monsters[iPos].GetComponent<MonsterManager>().GenerateMonster(iType);
        }
        // 更新UI计数
        UIManager.Instance.m_iShoot = saveInfo.m_iShootNum;
        UIManager.Instance.m_iScore = saveInfo.m_iScore;
        // 取消暂停状态
        Resume();
    }

    // 二进制方法保存游戏
    private void SaveByBin() {
        /// 序列化过程（将SaveInfo对象转化为字节流）
        // 创建一个保存当前游戏状态的SaveInfo对象
        SaveInfo saveInfo = CreateSaveInfoGO();
        // 创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        // 创建一个文件流
        FileStream fileStream = File.Create(Application.dataPath + "/StreamingFile/saveByBin");
        // 序列化
        bf.Serialize(fileStream, saveInfo);
        // 关闭流
        fileStream.Close();
        // 提示信息
        if (File.Exists(Application.dataPath + "/StreamingFile/saveByBin")) {
            UIManager.Instance.ShowMessage("保存成功");
        }
    }

    // 二进制方法加载游戏
    private void LoadByBin() {
        if (!File.Exists(Application.dataPath + "/StreamingFile/saveByBin")) {
            UIManager.Instance.ShowMessage("加载失败");
            return;
        }

        /// 反序列化过程
        // 创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        // 打开一个文件流
        FileStream fileStream = File.Open(Application.dataPath + "/StreamingFile/saveByBin", FileMode.Open);
        // 反序列化 
        SaveInfo saveInfo = (SaveInfo)bf.Deserialize(fileStream);
        // 关闭文件流
        fileStream.Close();
        // 提示信息
        UIManager.Instance.ShowMessage("加载成功");
        // 重置游戏内容
        LoadSaveInfoGO(saveInfo);
    }

    // Json方法保存游戏
    private void SaveByJson() {
        SaveInfo saveInfo = CreateSaveInfoGO();
        string sFilePath = Application.dataPath + "/StreamingFile/SaveByJson.json";
        // 利用JsonMapper将SaveInfo对象转换为Json格式的字符串
        string sJson = JsonMapper.ToJson(saveInfo);
        // 将Json字符串写入文件中
        StreamWriter sw = new StreamWriter(sFilePath);
        sw.Write(sJson);
        // 关闭StreamWriter
        sw.Close();
        // 提示信息
        UIManager.Instance.ShowMessage("保存成功");
    }

    // Json方法加载游戏
    private void LoadByJson() {
        string sFilePath = Application.dataPath + "/StreamingFile/SaveByJson.json";
        if (!File.Exists(sFilePath)) {
            UIManager.Instance.ShowMessage("加载失败");
            return;
        }
        // 创建一个StreamReader对象，用来读取流
        StreamReader sr = new StreamReader(sFilePath);
        string sJson = sr.ReadToEnd();
        sr.Close();
        // 将Json字符串转化为SaveInfo对象
        SaveInfo saveInfo = JsonMapper.ToObject<SaveInfo>(sJson);
        // 提示信息
        UIManager.Instance.ShowMessage("加载成功");
        // 重置游戏内容
        LoadSaveInfoGO(saveInfo);
    }

    // XML方法保存游戏
    private void SaveByXML() {
        SaveInfo saveInfo = CreateSaveInfoGO();
        string sFilePath = Application.dataPath + "/StreamingFile/SaveByXML.xml";

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("SaveInfo");
        root.SetAttribute("name", "SaveFile1");

        XmlElement monster;
        XmlElement monsterPos;
        XmlElement monsterType;

        for (int i = 0; i < saveInfo.m_listLivingPos.Count; ++i) {
            monster = xmlDoc.CreateElement("monster");
            monsterPos = xmlDoc.CreateElement("monsterPos");
            monsterPos.InnerText = saveInfo.m_listLivingPos[i].ToString();
            monsterType = xmlDoc.CreateElement("monsterType");
            monsterType.InnerText = saveInfo.m_listLivingMonsterType[i].ToString();

            monster.AppendChild(monsterPos);
            monster.AppendChild(monsterType);
            root.AppendChild(monster);
        }

        XmlElement shootNum = xmlDoc.CreateElement("shootNum");
        shootNum.InnerText = saveInfo.m_iShootNum.ToString();
        root.AppendChild(shootNum);

        XmlElement score = xmlDoc.CreateElement("score");
        score.InnerText = saveInfo.m_iScore.ToString();
        root.AppendChild(score);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(sFilePath);

        // 提示信息
        if (File.Exists(sFilePath)) {
            UIManager.Instance.ShowMessage("保存成功");
        }
    }

    // XML方法读取游戏
    private void LoadByXML() {
        string sFilePath = Application.dataPath + "/StreamingFile/SaveByXML.xml";
        if (!File.Exists(sFilePath)) {
            UIManager.Instance.ShowMessage("加载失败");
            return;
        }

        SaveInfo saveInfo = new SaveInfo();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(sFilePath);

        XmlNodeList monsters = xmlDoc.GetElementsByTagName("monster");
        foreach (XmlNode monster in monsters) {
            XmlNode monsterPos = monster.ChildNodes[0];
            int iMonsterPos = int.Parse(monsterPos.InnerText);
            saveInfo.m_listLivingPos.Add(iMonsterPos);

            XmlNode monsterType = monster.ChildNodes[1];
            int iMonsterType = int.Parse(monsterType.InnerText);
            saveInfo.m_listLivingMonsterType.Add(iMonsterType);
        }

        XmlNodeList shootNum = xmlDoc.GetElementsByTagName("shootNum");
        if (shootNum.Count > 0) {
            int iShootNum = int.Parse(shootNum[0].InnerText);
            saveInfo.m_iShootNum = iShootNum;
        }

        XmlNodeList score = xmlDoc.GetElementsByTagName("score");
        if (score.Count > 0) {
            int iScore = int.Parse(score[0].InnerText);
            saveInfo.m_iScore = iScore;
        }

        // 提示信息
        UIManager.Instance.ShowMessage("加载成功");
        // 重置游戏内容
        LoadSaveInfoGO(saveInfo);
    }
}