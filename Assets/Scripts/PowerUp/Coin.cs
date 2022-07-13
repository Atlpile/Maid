using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //播放拾取金币音效
            AudioManager.Instance.GetCoinAudio();
            //TODO:更新UI中获取金币的个数
            Item_UI.currentCoinNumber++;
            //销毁物体
            Destroy(gameObject);
        }
    }
}
