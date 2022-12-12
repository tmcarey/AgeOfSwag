using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogisticsTransporter : MonoBehaviour
{
    public Storage storage;
    public NavMeshAgent agent;
    public Economy.LogisticsAssignment assignment;
    private Economy _economy;

    public void Initialize(Economy economy)
    {
        this._economy = economy;
    }

    public void SetAssignment(Economy.LogisticsAssignment assignment)
    {
        this.assignment = assignment;
        this.agent.SetDestination(assignment.Claims[0].Storage.transform.position);
    }
    
    void Update()
    {
        if (this.assignment == null)
        {
            return;
        }

        if (this.agent.remainingDistance < 1)
        {
            if (this.assignment.Claims.Count > 0)
            {
                this.assignment.Claims[0].Storage.TransferResource(storage, this.assignment.Resource, this.assignment.Claims[0].Amount);
                this.assignment.Claims.RemoveAt(0);
                if (this.assignment.Claims.Count == 0)
                {
                    this.agent.SetDestination(this.assignment.Target.transform.position);
                }
                else
                {
                    this.agent.SetDestination(this.assignment.Claims[0].Storage.transform.position);
                }
            }
            else
            {
                storage.TransferResource(this.assignment.Target, this.assignment.Resource, this.assignment.Total);
                _economy.OnTransporterFinished(this);
                this.assignment = null;
            }
        }
    }
    
    
}
