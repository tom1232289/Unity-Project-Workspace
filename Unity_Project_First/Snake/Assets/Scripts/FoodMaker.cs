using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour {

    // 单例
    private static FoodMaker m_instance;

    public static FoodMaker Instance {
        get { return m_instance; }
    }

    // 公有引用
    public Sprite[] m_FoodSprites;      // 食物的图片数组
    public GameObject m_prefabFood;     // 食物的预制体
    public GameObject m_prefabReward;   // 奖励的预制体

    // 私有引用
    private Transform m_FoodMaker; // 管理食物制造器的空物体

    // 私有变量
    private int m_iLeftXLimit = 12; // 向左能走多少格
    private int m_iRightXLimit = 21;
    private int m_iUpYLimit = 11;
    private int m_iDownYLimit = 11;

    private void Awake() {
        // 初始化单例
        m_instance = this;
        // 初始化引用
        m_FoodMaker = GameObject.Find("FoodMaker").transform;
    }

    private void Start() {
        MakeFood();
    }

    public void MakeFood() {
        // 随机创建一个食物的实例
        int iRandom = Random.Range(0, m_FoodSprites.Length);
        GameObject food = Instantiate(m_prefabFood);
        food.GetComponent<Image>().sprite = m_FoodSprites[iRandom];
        food.transform.SetParent(m_FoodMaker, false);

        // 将食物随机摆放到指定位置
        int x = Random.Range(-m_iLeftXLimit, m_iRightXLimit);
        int y = Random.Range(-m_iDownYLimit, m_iUpYLimit);
        food.transform.localPosition = new Vector3(x * 30, y * 30, 0);

        // 随机生成奖励
        int iRandom2 = Random.Range(0, 100);
        if (iRandom2 < 20) {
            GameObject reward = Instantiate(m_prefabReward);
            reward.transform.SetParent(m_FoodMaker, false);
            // 将奖励随机摆放到指定位置
            x = Random.Range(-m_iLeftXLimit, m_iRightXLimit);
            y = Random.Range(-m_iDownYLimit, m_iUpYLimit);
            reward.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        }
    }
}