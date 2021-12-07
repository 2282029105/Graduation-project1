using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    public static ShadowPool instance;

    public GameObject shadowPrefab;

    public int shadowCount;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    //��������
    void Awake()
    {
        instance = this;

        //��ʼ�������
        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);

            //ȡ�����ã����ض����

            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObjects.Enqueue(gameObject);//����ط�������ʧ��Ԫ���ڴ˵ȴ�
    }

    public GameObject GetfromPool()
    {
        if(availableObjects.Count == 0)
        {
            FillPool();
        }//����������ʹ��ʱ�Ӷ�������ٴε���
        var outShadow = availableObjects.Dequeue();//�Ӷ��п�ͷ��ȡGameObject

        outShadow.SetActive(true);//��Ϊtrue���onenable��ʼִ��

        return outShadow;
    }
}
