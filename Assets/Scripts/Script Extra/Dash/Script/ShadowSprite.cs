using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    public Transform player;                    //获取player的坐标

    private SpriteRenderer thisSprite;          //获取当前渲染图
    private SpriteRenderer playerSprite;        //获取player渲染图

    private Color color;                        //用于控制颜色

    [Header("时间控制参数")]
    public float activeTime;                    //控制残影的显示时间
    public float activeStart;                   //获取残影开始启动的时间点

    [Header("不透明度控制")]
    public float alphaSet;                      //控制残影刚开始生成时，透明度的初始值
    public float alphaMultiplier;               //控制残影的生成后，渐变淡化速度（数值越大，淡化越慢）
    private float alpha;                        //残影的不透明度

    private void OnEnable()                                                 //（预制体）物体被启用时执行的内容
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;      //获取标签为“Player”物体的坐标，并定义为player
        thisSprite = GetComponent<SpriteRenderer>();                        //当前的渲染图，获得（新）SpriteRenderer组件
        playerSprite = player.GetComponent<SpriteRenderer>();               //player的渲染图，获得player上的SpriteRenderer组件（及内容）

        alpha = alphaSet;                                                   //残影的不透明度，为残影透明度的初始值

        thisSprite.sprite = playerSprite.sprite;                            //当前的渲染图，为player的渲染图（残影为player的残影）

        //当前残影的transform属性，为player的transform属性
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;                                            //残影开始显示的时间，为游戏中的时间

    }

    void FixedUpdate()                                                      //残影每帧执行的内容
    {
        alpha *= alphaMultiplier;                                           //使残影的不透明度越乘越小（降低）

        color = new Color(0.5f, 0.5f, 1, alpha);                            //（控制残影的颜色）使残影偏蓝

        thisSprite.color = color;                                           //残影的颜色，为当前渲染图的颜色

        if (Time.time >= activeStart + activeTime)                          //若当前时间，超过残影启动的时间点+残影显示的时间（残影运行的时间）
        {
            ShadowPool.instance.ReturnPool(this.gameObject);                //将当前游戏对象（残影）返回至对象池
        }
    }
}
