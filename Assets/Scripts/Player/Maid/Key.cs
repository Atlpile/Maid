using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        KeyType();
    }

    private void KeyType()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (keyType)
            {
                case E_KeyType.RedKey:
                    GameManager.Instance.keys.hasRedKey = true;
                    break;
                case E_KeyType.GreenKey:
                    GameManager.Instance.keys.hasGreenKey = true;
                    break;
                case E_KeyType.BlueKey:
                    GameManager.Instance.keys.hasBlueKey = true;
                    break;
                case E_KeyType.YellowKey:
                    GameManager.Instance.keys.hasYellowKey = true;
                    break;
            }

            Destroy(gameObject);
        }
    }

    //传keyType参数
}
