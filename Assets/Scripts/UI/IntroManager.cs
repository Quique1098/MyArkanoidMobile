using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour
{
    public Canvas introCanvas; 
    public Canvas mainMenuCanvas;
     public TextMeshProUGUI creatorText; 
    public float blinkSpeed = 1f; 

    private bool isFadingOut = true; 

    void Start()
    {
        
        if (mainMenuCanvas != null)
        {
            mainMenuCanvas.gameObject.SetActive(false);
        }

        
        if (creatorText != null)
        {
            StartCoroutine(BlinkText());
        }
    }

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            ShowMainMenu();
        }
    }

    IEnumerator BlinkText()
    {
        while (introCanvas.gameObject.activeSelf) 
        {
           
            Color color = creatorText.color;
            color.a += (isFadingOut ? -1 : 1) * blinkSpeed * Time.deltaTime;
            color.a = Mathf.Clamp01(color.a); 
            creatorText.color = color;

            
            if (color.a <= 0) isFadingOut = false;
            if (color.a >= 1) isFadingOut = true;

            yield return null; 
        }
    }

    void ShowMainMenu()
    {
        if (introCanvas != null) introCanvas.gameObject.SetActive(false);
        if (mainMenuCanvas != null) mainMenuCanvas.gameObject.SetActive(true);
    }
}
