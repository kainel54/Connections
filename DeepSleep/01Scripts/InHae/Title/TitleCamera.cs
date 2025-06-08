using System;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _addClampValue;

    private Vector3 _defaultRot;

    private void Awake()
    {
        _defaultRot = transform.eulerAngles;
    }

    private void Update()
    {
        Vector3 delta = Input.mousePositionDelta;
        
        Vector3 rot = transform.eulerAngles;
        rot.x += _rotSpeed * delta.y * -1;
        rot.y += _rotSpeed * delta.x;
        rot.x = Mathf.Clamp(rot.x, _defaultRot.x - _addClampValue, _defaultRot.x + _addClampValue);
        rot.y = Mathf.Clamp(rot.y, _defaultRot.y - _addClampValue, _defaultRot.y + _addClampValue);
        
        transform.eulerAngles = rot;
    }
}
