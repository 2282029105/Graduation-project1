using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereFollow : MonoBehaviour
{
    public Transform target;//代表玩家位置
    public float smoothing;//使相机移动更加平滑

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if(target != null)//判断玩家当前状态
        {
            if (transform.position != target.position)//判断玩家与相机当前位置
            {
                Vector3 targetPos = target.position;
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
                //差值函数，第一个是自身位置，第二个是对象位置，第三个是从自身到对象的时间 a+(b-a)*t
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
