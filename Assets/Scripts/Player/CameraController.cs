using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Vector2 _lateralMoveInput;
    private float _zoomInput;
    private float _rotateInput;
    private float _rotateAngle;
    private float _height;
    
    public float lateralMoveSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public float minHeight = 2.0f;
    public float zoomLerpSpeed = 2.0f;
    public float rotateSpeed = 1.0f;
    public Transform cameraTransform;
    
    public void OnMove(InputValue val)
    {
        _lateralMoveInput = val.Get<Vector2>();
    }
    
    public void OnZoom(InputValue val)
    {
        _zoomInput = val.Get<float>();
    }

    public void OnRotate(InputValue val)
    {
        _rotateInput = val.Get<float>();
    }

    private void Awake()
    {
        _height = cameraTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=
            transform.right * (_lateralMoveInput.x * lateralMoveSpeed * Time.unscaledDeltaTime) +
            transform.forward * (_lateralMoveInput.y * lateralMoveSpeed * Time.unscaledDeltaTime);

        cameraTransform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position, Vector3.up);

        _height = Mathf.Clamp(_height + _zoomInput * (Time.unscaledDeltaTime * zoomSpeed), minHeight, Mathf.Infinity);
        
        cameraTransform.position =
            Vector3.Lerp(cameraTransform.position, new Vector3(cameraTransform.position.x, _height, cameraTransform.position.z), Time.unscaledDeltaTime * zoomLerpSpeed);

        _rotateAngle += _rotateInput * Time.unscaledDeltaTime * rotateSpeed;
        transform.rotation = Quaternion.Euler(0, _rotateAngle, 0);
    }
}
