using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float _deadzoneWidthFraction;
    
    [SerializeField] private Camera _camera;

    [SerializeField] private Transform _cameraTransform;
    
    [SerializeField] private Transform _targetTransform;
    
    void Update()
    {
        if (_targetTransform == null)
            return;
        
        var screenHalfWidth = _camera.orthographicSize * Screen.width / Screen.height;
        var targetX = _targetTransform.position.x;
        var min = targetX - screenHalfWidth * _deadzoneWidthFraction;
        var max = targetX + screenHalfWidth * _deadzoneWidthFraction;
        
        var pos = _cameraTransform.position;
        pos.x = Mathf.Clamp(pos.x, min, max);
        _cameraTransform.position = pos;
    }
}
