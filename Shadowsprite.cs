
using UnityEngine;

public class Shadowsprit : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;

    [Header("时间控制参数")]
    public float acriveTime;//显示时间
    public float activeStart;// 开始显示的时间

    [Header("不透明度控制")]
    private float alpha;
    public float alphaSet;//设置初始值
    public float alphaMultiplier;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;//获取玩家tag
        thisSprite = GetComponent<SpriteRenderer>();//物体组件
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
        //获取玩家x轴翻转等数值

        activeStart = Time.time;


    }

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(0.5f, 1, 1, alpha);
        //alpha为设置的颜色，当此颜色为(1,1,1)是显示的不是白色而是物体原本的颜色

        thisSprite.color = color;//重新赋值颜色

        if(Time.time >= activeStart + acriveTime)//判断返回对象池的条件
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }


    }
    //当执行时的显示时间超过设置时间后返回对象池中
}
