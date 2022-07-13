using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Box : MonoBehaviour
{
    public E_BoxType boxType;
    public Sprite[] boxSprites;
    public Sprite openSprite;
    public float fadeDuration;
    public float willFade;

    private SpriteRenderer boxSpriteRenderer;
    public bool canOpen;   //宝箱能否开启
    public bool isOpen;    //宝箱是否为开启状态


    private void Awake()
    {
        boxSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        BoxType();
        isOpen = false;
    }


    private void BoxType()      //按宝箱类型切换宝箱默认图片
    {
        switch (boxType)
        {
            case E_BoxType.Red:
                boxSpriteRenderer.sprite = boxSprites[0];
                break;
            case E_BoxType.Green:
                boxSpriteRenderer.sprite = boxSprites[1];
                break;
            case E_BoxType.Blue:
                boxSpriteRenderer.sprite = boxSprites[2];
                break;
            case E_BoxType.Yellow:
                boxSpriteRenderer.sprite = boxSprites[3];
                break;
            case E_BoxType.Normal:
                boxSpriteRenderer.sprite = boxSprites[4];
                break;
        }
    }

    private void BoxOpen()      //打开宝箱
    {
        isOpen = true;
        boxSpriteRenderer.sprite = openSprite;
        //TODO:掉出道具

    }

    private void BoxFade()      //打开宝箱后褪色后消失
    {
        Color targetColor = new Color(1, 1, 1, 0);
        boxSpriteRenderer.DOColor(targetColor, fadeDuration);
    }

    private void HideBox()      //隐藏箱子
    {
        gameObject.SetActive(false);
    }

    private void CanOpenBox()   //判断能否开启箱子
    {
        if (GameManager.Instance.keys.hasRedKey && boxType == E_BoxType.Red)
            canOpen = true;
        else if (GameManager.Instance.keys.hasGreenKey && boxType == E_BoxType.Green)
            canOpen = true;
        else if (GameManager.Instance.keys.hasBlueKey && boxType == E_BoxType.Blue)
            canOpen = true;
        else if (GameManager.Instance.keys.hasYellowKey && boxType == E_BoxType.Yellow)
            canOpen = true;
        else if (boxType == E_BoxType.Normal)
            canOpen = true;
    }

    //Player检测
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //判断能否开启宝箱
            CanOpenBox();

            //打开宝箱的条件
            if (Input.GetKey(KeyCode.E))
            {
                if (!isOpen && canOpen)
                {
                    BoxOpen();

                    Invoke("BoxFade", willFade);
                    Invoke("HideBox", willFade + fadeDuration);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canOpen = false;
        }
    }
}

