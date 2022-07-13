using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasKey : MonoBehaviour
{
    public bool hasRedKey;
    public bool hasGreenKey;
    public bool hasBlueKey;
    public bool hasYellowKey;

    private void Start()
    {
        GameManager.Instance.RegisterKey(this);
    }
}
