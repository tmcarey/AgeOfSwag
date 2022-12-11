using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Constructor : MonoBehaviour
{
    public Transform structurePointer;
    
    private bool _isConstructing;
    private StructureScriptableObject _activeStructure;
    private GameObject _structurePreview;
    private Plane _groundPlane;

    public void StartConstruction(StructureScriptableObject structure)
    {
        _isConstructing = true;
        _activeStructure = structure;
        _structurePreview = Instantiate(structure.structurePrefab, structurePointer);
    }

    public void OnEndConstruction(InputValue val)
    {
        if (!_isConstructing)
            return;
        _hudController.CloseConstruction();
        _isConstructing = false;
        Destroy(_structurePreview);
    }

    public void OnConfirmConstruction(InputValue val)
    {
        if (!_isConstructing)
            return;
        Instantiate(_activeStructure.structurePrefab, structurePointer.position, Quaternion.identity);
    }

    private HUDController  _hudController;

    private void Awake()
    {
        _hudController = GetComponent<HUDController>();
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (_isConstructing)
        {
            Ray mouseRay = _hudController.GetMouseRay();
            if (_groundPlane.Raycast(mouseRay, out float rayLength))
            {
                Vector3 pointToPlace = mouseRay.GetPoint(rayLength);
                structurePointer.position = pointToPlace;
            }
            structurePointer.position = new Vector3(
                Mathf.Floor(structurePointer.position.x * 0.25f) / 0.25f,
                0,
                Mathf.Floor(structurePointer.position.z * 0.25f) / 0.25f);
        }
    }
}
