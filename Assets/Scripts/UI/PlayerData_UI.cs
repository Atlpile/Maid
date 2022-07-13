using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData_UI : MonoBehaviour
{
    //TODO:显示玩家等级
    //TODO:显示玩家经验值

    private Image healthSlider;
    private Text healthNumber;

    private Image magicSlider;
    private Text magicNumber;

    private void Awake()
    {
        healthSlider = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        healthNumber = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();

        magicSlider = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        magicNumber = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>();
    }

    private void Update()
    {
        UpdateHealth();
        UpdateMagic();
    }

    public void UpdateHealth()
    {
        healthNumber.text = GameManager.Instance.maidStats.CurrentHealth.ToString("0") + "/" + GameManager.Instance.maidStats.MaxHealth.ToString("0");

        float healthSliderPercent = (float)GameManager.Instance.maidStats.CurrentHealth / GameManager.Instance.maidStats.MaxHealth;
        healthSlider.fillAmount = healthSliderPercent;
    }

    public void UpdateMagic()
    {
        magicNumber.text = GameManager.Instance.maidStats.CurrentMagic.ToString("0") + "/" + GameManager.Instance.maidStats.MaxMagic.ToString("0");

        float magicSliderPercent = (float)GameManager.Instance.maidStats.CurrentMagic / GameManager.Instance.maidStats.MaxMagic;
        magicSlider.fillAmount = magicSliderPercent;
    }
}
