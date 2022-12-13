using UnityEngine;

public class Logistics : MonoBehaviour
{
    private Employment _employment;
    private Structure _structure;

    public GameObject transporterPrefab;

    public Vector3 GetAccessPoint()
    {
        return _structure.GetAccessPoint();
    }

    private void Awake()
    {
        _employment = GetComponent<Employment>();
        _structure = GetComponent<Structure>();
        _employment.OnEmployeeAdded += OnEmployeeAdded;
        _employment.OnEmployeeRemoved += OnEmployeeRemoved;
    }
    
    private void OnEmployeeAdded(Citizen citizen)
    {
        Destroy(citizen.GetComponent<UnityEngine.AI.NavMeshAgent>());
        LogisticsTransporter transporterObject =
            Instantiate(transporterPrefab, citizen.transform.position, Quaternion.identity)
                .GetComponent<LogisticsTransporter>();
                
        Transform transform1;
        (transform1 = citizen.transform).SetParent(transporterObject.transform);
        transform1.localPosition = Vector3.zero;
        transform1.localRotation = Quaternion.identity;
        
        transporterObject.Initialize(_structure.Economy, this);
        _structure.Economy.AddLogisticsTransporter(transporterObject);
    }
    
    private void OnEmployeeRemoved(Citizen citizen)
    {
        //TODO
    }
}
