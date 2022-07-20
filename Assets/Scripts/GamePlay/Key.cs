using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Key : MonoBehaviour
{
    public E_KeyType keyType;
    public Sprite[] keySprites;

    private SpriteRenderer keySpriteRenderer;


    private void Awake()
    {
        keySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GetKeyTypeSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetKeyType();

            Destroy(gameObject);
        }
    }

    private void GetKeyTypeSprite()
    {
        switch (keyType)
        {
            case E_KeyType.RedKey:
                keySpriteRenderer.sprite = keySprites[0];
                break;
            case E_KeyType.GreenKey:
                keySpriteRenderer.sprite = keySprites[1];
                break;
            case E_KeyType.BlueKey:
                keySpriteRenderer.sprite = keySprites[2];
                break;
            case E_KeyType.YellowKey:
                keySpriteRenderer.sprite = keySprites[3];
                break;
        }
    }

    private void GetKeyType()
    {
        switch (keyType)
        {
            case E_KeyType.RedKey:
                GameManager.Instance.hasRedKey = true;
                break;
            case E_KeyType.GreenKey:
                GameManager.Instance.hasGreenKey = true;
                break;
            case E_KeyType.BlueKey:
                GameManager.Instance.hasBlueKey = true;
                break;
            case E_KeyType.YellowKey:
                GameManager.Instance.hasYellowKey = true;
                break;
        }
    }
}
