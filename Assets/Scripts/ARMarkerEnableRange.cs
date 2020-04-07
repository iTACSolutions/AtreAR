using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ARマーカーの読み取り範囲クラス
/// </summary>
public class ARMarkerEnableRange : MonoBehaviour
{
    [SerializeField] private Image rangeImage;
    
    private void Update()
    {
        var arMenuPanels = GameObject.FindGameObjectsWithTag("ARMenu");
        var isVisible = true;
        
        // ARでメニューを表示しているか判定
        foreach (var arMenuPanel in arMenuPanels)
        {
            if (arMenuPanel.GetComponent<ARMenuPanel>().IsVisible) isVisible = false;
        }

        // ARでメニューを表示中は読み取り範囲の表示を消す
        rangeImage.enabled = isVisible;
    }
}
