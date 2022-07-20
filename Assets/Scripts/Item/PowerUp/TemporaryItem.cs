using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryItem : MonoBehaviour
{
    public E_TemporaryItemType temporaryItemType;
    public Sprite[] itemSprites;
    public float itemTime = 10f;
    private float currentItemTime;

    private SpriteRenderer itemSpriteRenderer;
    private BoxCollider2D itemColl;
    private Rigidbody2D rb;

    private void Awake()
    {
        itemSpriteRenderer = GetComponent<SpriteRenderer>();
        itemColl = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            StartCoroutine(IE_GetItem());
    }

    private void GetItemTypeSprite()
    {
        switch (temporaryItemType)
        {
            case E_TemporaryItemType.Speed:
                itemSpriteRenderer.sprite = itemSprites[0];
                break;
            case E_TemporaryItemType.JumpCount:
                itemSpriteRenderer.sprite = itemSprites[1];
                break;
            case E_TemporaryItemType.Defence:
                itemSpriteRenderer.sprite = itemSprites[2];
                break;
            case E_TemporaryItemType.Pet:
                itemSpriteRenderer.sprite = itemSprites[3];
                break;
            case E_TemporaryItemType.AddBullet:
                itemSpriteRenderer.sprite = itemSprites[4];
                break;
            case E_TemporaryItemType.Invincible:
                itemSpriteRenderer.sprite = itemSprites[5];
                break;
        }
    }

    //拾取后需要计时器
    IEnumerator IE_Timer(float oneSecond)
    {
        currentItemTime = itemTime;

        while (true)
        {
            Debug.Log("当前物品使用剩余时间为：" + currentItemTime);
            yield return new WaitForSeconds(oneSecond);
            currentItemTime -= oneSecond;

            if (currentItemTime < 0) break;
        }
    }

    private IEnumerator IE_GetItem()
    {
        //TODO:得到加成
        //TODO:物品消失

        itemColl.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;


        yield return IE_Timer(1f);

        Destroy(gameObject);
    }
}
