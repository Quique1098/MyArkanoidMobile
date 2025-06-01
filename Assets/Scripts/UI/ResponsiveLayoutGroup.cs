using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ResponsiveLayoutGroup : MonoBehaviour
{
    public VerticalLayoutGroup verticalLayout;
    public HorizontalLayoutGroup horizontalLayout;

    private bool isPortrait;

    void Awake()
    {
        verticalLayout = GetComponent<VerticalLayoutGroup>();
        horizontalLayout = GetComponent<HorizontalLayoutGroup>();

        if (verticalLayout == null)
            verticalLayout = gameObject.AddComponent<VerticalLayoutGroup>();

        if (horizontalLayout == null)
            horizontalLayout = gameObject.AddComponent<HorizontalLayoutGroup>();

        // Empezamos con uno solo activo
        verticalLayout.enabled = false;
        horizontalLayout.enabled = false;

        UpdateLayout();
    }

    void Update()
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

        verticalLayout.enabled = isPortrait;
        horizontalLayout.enabled = !isPortrait;
    }
}

