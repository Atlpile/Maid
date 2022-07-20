using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            AudioManager.Instance.GetCoinAudio();
            Item_UI.currentCoinNumber++;
            Destroy(other.gameObject);
        }

        //若碰到金币袋子，则金币数量+10
        if (other.gameObject.CompareTag("CoinBag"))
        {
            AudioManager.Instance.GetCoinAudio();
            Item_UI.currentCoinNumber += 10;
            Destroy(other.gameObject);
        }
    }


}
