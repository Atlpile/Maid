using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;      //设置单例

    public GameObject shadowPrefab;         //获取残影预制体

    public int shadowCount;                 //用于设置游戏开始时残影生成的数量

    private Queue<GameObject> availableObjects = new Queue<GameObject>();   //定义队列（功能与列表类似），用于控制对象池的位置排序

    void Awake()
    {
        //实现单例
        instance = this;

        FillPool();                 //初始化对象池
    }

    public void FillPool()                                  //填充对象（池）
    {
        for (int i = 0; i < shadowCount; i++)               //循环生成预制体（循环次数由残影数量决定）
        {
            var newShadow = Instantiate(shadowPrefab);      //生成的预制体，定义为临时变量newShadow

            newShadow.transform.SetParent(transform);       //将生成的预制体，设为当前对象的子集

            ReturnPool(newShadow);                          //将生成的预制体，返回至对象池
        }
    }
    public void ReturnPool(GameObject gameObject)           //返回对象池
    {
        gameObject.SetActive(false);                        //关闭接收的预制体

        availableObjects.Enqueue(gameObject);               //将游戏对象放回队列的末端
    }
    public GameObject GetFromPool()                         //从对象池获取物体（有返回值的方法）
    {
        if (availableObjects.Count == 0)                    //若对象池中的数量为0（防止对象池中的物体消耗完）
        {
            FillPool();                                     //再次填充一倍数量对象（池）
        }

        var outShadow = availableObjects.Dequeue();         //从队列的开头获取预制体，定义为临时变量outShadow

        outShadow.SetActive(true);                          //启用接收的预制体

        return outShadow;                                   //将接收的预制体数值信息，返回给游戏对象
    }
}
