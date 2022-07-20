using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Box : MonoBehaviour
{
    public E_BoxType boxType;

    [Header("宝箱图片")]
    public Sprite[] boxSprites;
    public Sprite openSprite;

    [Header("褪色设置")]
    public float fadeDuration;
    public float willFade;

    [Header("宝箱状态")]
    public bool canOpen;   //宝箱能否开启
    public bool isOpen;    //宝箱是否为开启状态

    private SpriteRenderer boxSpriteRenderer;


    private void Awake()
    {
        boxSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GetBoxType();

        isOpen = false;
    }

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
                    StartCoroutine(IE_BoxDisappear());
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


    #region 宝箱的消失过程

    private IEnumerator IE_BoxDisappear()   //宝箱的消失
    {
        yield return IE_BoxOpen();
        yield return IE_BoxFade();
        yield return IE_HideBox();

        yield break;
    }

    private IEnumerator IE_BoxOpen()        //打开宝箱
    {
        isOpen = true;
        boxSpriteRenderer.sprite = openSprite;

        //TODO:掉出道具

        yield return new WaitForSeconds(willFade);
    }

    private IEnumerator IE_BoxFade()        //打开宝箱后褪色后消失
    {
        Color targetColor = new Color(1, 1, 1, 0);
        boxSpriteRenderer.DOColor(targetColor, fadeDuration);

        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator IE_HideBox()        //隐藏箱子
    {
        gameObject.SetActive(false);

        yield return null;
    }

    #endregion


    private void GetBoxType()               //按宝箱类型切换宝箱默认图片
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

    private void CanOpenBox()               //判断能否开启特定箱子
    {
        if (GameManager.Instance.hasRedKey && boxType == E_BoxType.Red)
            canOpen = true;
        else if (GameManager.Instance.hasGreenKey && boxType == E_BoxType.Green)
            canOpen = true;
        else if (GameManager.Instance.hasBlueKey && boxType == E_BoxType.Blue)
            canOpen = true;
        else if (GameManager.Instance.hasYellowKey && boxType == E_BoxType.Yellow)
            canOpen = true;
        else if (boxType == E_BoxType.Normal)
            canOpen = true;
    }


    // private bool CanOpenBox()
    // {
    //     if (GameManager.Instance.keys.hasRedKey && boxType == E_BoxType.Red)
    //         return true;
    //     else if (GameManager.Instance.keys.hasGreenKey && boxType == E_BoxType.Green)
    //         return true;
    //     else if (GameManager.Instance.keys.hasBlueKey && boxType == E_BoxType.Blue)
    //         return true;
    //     else if (GameManager.Instance.keys.hasYellowKey && boxType == E_BoxType.Yellow)
    //         return true;
    //     else if (boxType == E_BoxType.Normal)
    //         return true;
    //     else
    //         return false;
    // }

}

