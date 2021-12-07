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
    //创建队列
    void Awake()
    {
        instance = this;

        //初始化对象池
        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);

            //取消启用，返回对象池

            ReturnPool(newShadow);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        availableObjects.Enqueue(gameObject);//对象池方法，消失的元素在此等待
    }

    public GameObject GetfromPool()
    {
        if(availableObjects.Count == 0)
        {
            FillPool();
        }//当数量不够使用时从对象池中再次调用
        var outShadow = availableObjects.Dequeue();//从队列开头获取GameObject

        outShadow.SetActive(true);//若为true则从onenable开始执行

        return outShadow;
    }
}
