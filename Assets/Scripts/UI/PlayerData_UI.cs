using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData_UI : MonoBehaviour
{
    [Header("血量状态")]
    [SerializeField] private Image healthSlider;
    [SerializeField] private Text healthNumber;

    [Header("蓝量状态")]
    [SerializeField] private Image magicSlider;
    [SerializeField] private Text magicNumber;


    private void FixedUpdate()
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
