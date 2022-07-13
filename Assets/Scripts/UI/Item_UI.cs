using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_UI : MonoBehaviour
{
    public static int currentCoinNumber = 0;    //当前金币数量
    public Text coinNumber;                     //UI中金币的数量

    private void Update()
    {
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        //UI中的金币数量 = 当前金币数量
        coinNumber.text = currentCoinNumber.ToString();
    }
}
