using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class RollerAgent : Agent
{
    Rigidbody rB;
    void Start()
    {
        rB = GetComponent<Rigidbody>();
    }
    public Transform Target;
    public override void OnEpisodeBegin()
    {
        if(this.transform.localPosition.y <0)
        {
            this.rB.angularVelocity = Vector3.zero;
            this.rB.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rB.velocity.x);
        sensor.AddObservation(rB.velocity.y);
    }
    public float forceMultiplier = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rB.AddForce(controlSignal * forceMultiplier);

        float distancetotarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        if(distancetotarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        if(this.transform.localPosition.y <0)
        {
            EndEpisode();
        }
    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}
