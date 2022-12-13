using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogisticsTransporter : MonoBehaviour
{
    public Storage storage;
    public NavMeshAgent agent;
    
    private Economy.LogisticsAssignment _assignment;
    private Economy _economy;
    private Coroutine transportRoutine;
    
    private Logistics _logistics;
    
    public void Initialize(Economy economy, Logistics logistics)
    {
        _economy = economy;
        _logistics = logistics;
        SetVisibility(false);
        transform.position = logistics.GetAccessPoint();
    }

    public void SetAssignment(Economy.LogisticsAssignment assignment)
    {
        if (this._assignment != null)
        {
            Debug.LogError("Already has assignment");
            return;
        }
        
        this._assignment = assignment;
        if(transportRoutine != null)
            StopCoroutine(transportRoutine);
        transportRoutine = StartCoroutine(TransportRoutine());
    }
    
    private void SetVisibility(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
        agent.enabled = active;
    }

    private IEnumerator TransportRoutine()
    {
        SetVisibility(true);
        this.agent.SetDestination(_assignment.Claims[0].Storage.transform.position);
        while (this._assignment != null)
        {
            yield return new WaitForSeconds(0.1f);
            if (!(this.agent.remainingDistance < 1)) continue;
            if (this._assignment.Claims.Count > 0)
            {
                SetVisibility(false);

                yield return new WaitForSeconds(1.0f);
                this._assignment.Claims[0].Storage.TransferResource(storage, this._assignment.Resource,
                    this._assignment.Claims[0].Amount);
                this._assignment.Claims.RemoveAt(0);

                SetVisibility(true);

                if (this._assignment.Claims.Count == 0)
                {
                    this.agent.SetDestination(this._assignment.Target.GetAccessPoint());
                }
                else
                {
                    this.agent.SetDestination(this._assignment.Claims[0].Storage.GetAccessPoint());
                }
            }
            else
            {
                SetVisibility(false);

                yield return new WaitForSeconds(1.0f);

                storage.TransferResource(this._assignment.Target, this._assignment.Resource, this._assignment.Total);
                _economy.OnTransporterFinished(this);
                this._assignment = null;

                SetVisibility(true);
            }
        }

        agent.SetDestination(_logistics.GetAccessPoint());
        do
        {
            yield return new WaitForSeconds(0.1f);
        } while (this.agent.remainingDistance > 1);
        SetVisibility(false);
    }
}
