using UnityEngine;
using UnityEngine.InputSystem;

public class Constructor : MonoBehaviour
{
    public Transform structurePointer;
    public GameObject constructionProjectPrefab;
    public Economy localEconomy;
    
    private bool _isConstructing;
    private StructureScriptableObject _activeStructure;
    private GameObject _structurePreview;
    private Plane _groundPlane;

    public void StartConstruction(StructureScriptableObject structure)
    {
        _isConstructing = true;
        _activeStructure = structure;
        _structurePreview = Instantiate(structure.structurePrefab, structurePointer);
        _structurePreview.transform.localPosition = _structurePreview.GetComponent<Structure>().GetCenterOffset();
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
        Structure project =
            Instantiate(constructionProjectPrefab, _structurePreview.transform.position, Quaternion.identity)
                .GetComponent<Structure>();
        project.structure = _activeStructure;
        project.Initialize(localEconomy);
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

            var structurePointerPosition = structurePointer.position;
            structurePointerPosition = new Vector3(
                Mathf.Floor(structurePointerPosition.x * 0.25f) / 0.25f,
                0,
                Mathf.Floor(structurePointerPosition.z * 0.25f) / 0.25f);
            structurePointer.position = structurePointerPosition;
        }
    }
}
