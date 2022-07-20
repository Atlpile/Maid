using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasKey : MonoBehaviour
{
    public bool hasRedKey = false;
    public bool hasGreenKey = false;
    public bool hasBlueKey = false;
    public bool hasYellowKey = false;

    private void Start()
    {
        GameManager.Instance.RegisterKey(this);
    }
}
