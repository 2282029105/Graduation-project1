using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereFollow : MonoBehaviour
{
    public Transform target;//�������λ��
    public float smoothing;//ʹ����ƶ�����ƽ��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if(target != null)//�ж���ҵ�ǰ״̬
        {
            if (transform.position != target.position)//�ж�����������ǰλ��
            {
                Vector3 targetPos = target.position;
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
                //��ֵ��������һ��������λ�ã��ڶ����Ƕ���λ�ã��������Ǵ����������ʱ�� a+(b-a)*t
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
