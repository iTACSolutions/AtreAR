using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ARで表示するメニュー
/// </summary>
public class ARMenuPanel : MonoBehaviour
{
    private Camera _camera;

    // 表示するメニューのプレファブ
    [SerializeField] private GameObject _starbucksMenu;
    [SerializeField] private GameObject _tullysMenu;
    [SerializeField] private GameObject _wiredMenu;

    // 現在表示中のメニューオブジェクト
    private GameObject _displayingPanel;
    // 表示中のメニューの店の種類
    private ARMenuManager.TrackingObjectType _displayingType;

    // メニューの表示状態(1店舗でも表示されて入ればtrue)
    public bool IsVisible => _displayingPanel != null && _displayingPanel.activeSelf;

    private void Start()
    {
        _camera = Camera.main;
        UpdatePanel();
    }

    private void Update()
    {
        //パネルをカメラの方に向ける（なんか微妙に角度がおかしかったため、今はなし）
//        transform.forward = _camera.transform.forward;

//        UpdatePanel();
        UpdateVisible();
    }

    /// <summary>
    /// トラッキング中の画像に応じて、メニューの表示を更新
    /// </summary>
    void UpdatePanel()
    {
        if (ARMenuManager.TrackingType != _displayingType)
        {
            ChangePanel(ARMenuManager.TrackingType);
        }

        _displayingType = ARMenuManager.TrackingType;
    }
    
    /// <summary>
    /// メニューの種類を更新
    /// </summary>
    /// <param name="setType"></param>
    void ChangePanel(ARMenuManager.TrackingObjectType setType)
    {
        if (_displayingPanel != null)
            _displayingPanel.SetActive(false);

        switch (setType)
        {
            case ARMenuManager.TrackingObjectType.None:
                break;
            case ARMenuManager.TrackingObjectType.Starbucks:
                _displayingPanel = _starbucksMenu;
                break;
            case ARMenuManager.TrackingObjectType.Tullys:
                _displayingPanel = _tullysMenu;
                break;
            case ARMenuManager.TrackingObjectType.Wired:
                _displayingPanel = _wiredMenu;
                break;
        }

        if (setType != ARMenuManager.TrackingObjectType.None && _displayingPanel != null)
            _displayingPanel.SetActive(true);
    }

    /// <summary>
    /// 表示・非表示の更新
    /// </summary>
    void UpdateVisible()
    {
        if (_displayingPanel == null) return;
        var viewportPos = _camera.WorldToViewportPoint(transform.position);
        if (0.3f < viewportPos.x && viewportPos.x < 0.7f && 0.3f < viewportPos.y && viewportPos.y < 0.7f)
            _displayingPanel.SetActive(true);
        else
            _displayingPanel.SetActive(false);
    }
}