using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private Animator saveAnim;
    public bool canSave;



    private void Start()
    {
        saveAnim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canSave)
        {
            saveAnim.Play("save");

            float maidPositionX = GameManager.Instance.maidStats.transform.position.x;
            float maidPositionY = GameManager.Instance.maidStats.transform.position.y;
            // Debug.Log(maidPositionX);
            // Debug.Log(maidPositionY);

            SaveManager.Instance.SavePlayerPositionData(maidPositionX, maidPositionY, "PlayerPoX", "PlayerPoY");
            // Debug.Log("保存成功");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canSave = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canSave = false;
        }
    }
}
