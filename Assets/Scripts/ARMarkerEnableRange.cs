using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARMarkerEnableRange : MonoBehaviour
{
    [SerializeField] private Image rangeImage;
    
    private void Update()
    {
        var arMenuPanels = GameObject.FindGameObjectsWithTag("ARMenu");
        var isVisible = true;
        foreach (var arMenuPanel in arMenuPanels)
        {
            if (arMenuPanel.GetComponent<ARMenuPanel>().IsVisible) isVisible = false;
        }

        rangeImage.enabled = isVisible;
    }
}
