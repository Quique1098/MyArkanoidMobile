using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadTester : MonoBehaviour
{
    public Button saveButton;
    public Button loadButton;

    void Start()
    {
        saveButton.onClick.AddListener(GameManager.SaveGame);
        loadButton.onClick.AddListener(GameManager.LoadGame);
    }
}

