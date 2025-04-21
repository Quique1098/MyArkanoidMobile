using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPaddleController : MonoBehaviour
{
    public static TopPaddleController Instance { get; private set; }

    public Collider PaddleCollider => GetComponent<Collider>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
