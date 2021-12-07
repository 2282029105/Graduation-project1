
using UnityEngine;

public class Shadowsprit : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;

    [Header("ʱ����Ʋ���")]
    public float acriveTime;//��ʾʱ��
    public float activeStart;// ��ʼ��ʾ��ʱ��

    [Header("��͸���ȿ���")]
    private float alpha;
    public float alphaSet;//���ó�ʼֵ
    public float alphaMultiplier;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;//��ȡ���tag
        thisSprite = GetComponent<SpriteRenderer>();//�������
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
        //��ȡ���x�ᷭת����ֵ

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
        //alphaΪ���õ���ɫ��������ɫΪ(1,1,1)����ʾ�Ĳ��ǰ�ɫ��������ԭ������ɫ

        thisSprite.color = color;//���¸�ֵ��ɫ

        if(Time.time >= activeStart + acriveTime)//�жϷ��ض���ص�����
        {
            //���ض����
            ShadowPool.instance.ReturnPool(this.gameObject);
        }


    }
    //��ִ��ʱ����ʾʱ�䳬������ʱ��󷵻ض������
}
