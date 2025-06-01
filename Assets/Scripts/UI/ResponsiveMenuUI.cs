using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveMenuUI : MonoBehaviour
{
    public RectTransform titulo;
    public RectTransform botonPlay;
    public RectTransform botonContinue;
    public RectTransform botonDeleteData;
    public RectTransform botonExit;

    private bool isPortrait;

    private void Start()
    {
        UpdateLayout();
    }

    private void Update()
    {
        if (isPortrait != IsPortrait())
        {
            UpdateLayout();
        }
    }

    private bool IsPortrait()
    {
        return Screen.height >= Screen.width;
    }

    private void UpdateLayout()
    {
        isPortrait = IsPortrait();

        if (isPortrait)
        {
            // Distribución vertical
            SetVerticalLayout();
        }
        else
        {
            // Distribución horizontal
            SetHorizontalLayout();
        }
    }

    private void SetVerticalLayout()
    {
        float yOffset = 200f;
        float startY = 400f;

        titulo.anchoredPosition = new Vector2(0, startY);
        botonPlay.anchoredPosition = new Vector2(0, startY - yOffset);
        botonContinue.anchoredPosition = new Vector2(0, startY - yOffset * 2);
        botonDeleteData.anchoredPosition = new Vector2(0, startY - yOffset * 3);
        botonExit.anchoredPosition = new Vector2(0, startY - yOffset * 4);
    }

    private void SetHorizontalLayout()
    {
        float xOffset = 400f;
        float startX = -600f;

        titulo.anchoredPosition = new Vector2(0, 400f);
        botonPlay.anchoredPosition = new Vector2(startX + xOffset * 1, 100);
        botonContinue.anchoredPosition = new Vector2(startX + xOffset * 2, 100);
        botonDeleteData.anchoredPosition = new Vector2(startX + xOffset * 3, 100);
        botonExit.anchoredPosition = new Vector2(startX + xOffset * 4, 100);
    }
}
