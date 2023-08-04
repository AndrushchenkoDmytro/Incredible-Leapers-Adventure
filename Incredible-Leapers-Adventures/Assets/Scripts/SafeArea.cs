using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();

        var safeArea = Screen.safeArea;
        Debug.Log(" safeArea " + safeArea);
        var anchorMin = safeArea.position;
        Debug.Log(" anchorMin  " + anchorMin);
        var anchorMax = anchorMin + safeArea.size;
        Debug.Log(" anchorMax " + anchorMax);
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        Debug.Log(" anchorMin1  " + anchorMin);
        Debug.Log(" anchorMax2 " + anchorMax);

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
