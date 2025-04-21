using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class CustomUpdateManager : MonoBehaviour
{
    public static CustomUpdateManager Instance { get; private set; }
    private List<ICustomUpdateable> customUpdateables = new List<ICustomUpdateable>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddObject(ICustomUpdateable obj) => customUpdateables.Add(obj);
    public void RemoveObject(ICustomUpdateable obj) => customUpdateables.Remove(obj);

    void Start() { foreach (var item in customUpdateables) item.CustomStart(); }



    void Update()
    {
        for (int i = customUpdateables.Count - 1; i >= 0; i--)
        {
            var item = customUpdateables[i];
            item.CustomUpdate();
        }


    }

    void FixedUpdate() { foreach (var item in customUpdateables) item.CustomFixedUpdate(); }
    void LateUpdate() { foreach (var item in customUpdateables) item.CustomLateUpdate(); }
    void OnApplicationPause(bool pause)
    {
        foreach (var item in customUpdateables) item.CustomOnPause();
    }
}
