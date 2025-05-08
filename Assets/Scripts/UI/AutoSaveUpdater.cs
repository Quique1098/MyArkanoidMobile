using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveUpdater : MonoBehaviour
{
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            timer = 0f;
            GameManager.SaveGame();
        }
    }
}

