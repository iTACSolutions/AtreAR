using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMenuPanel : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
    
    private void Update()
    {
        transform.forward = _camera.transform.forward;
    }
}
