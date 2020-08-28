using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class FlappybirdAi : Agent
{
    Rigidbody rB;
    public float RightForce;
    public float JumpForce;
    private float prpos;
    public float offset;
    public Transform Upperfloor;
    public Transform Lowerfloor;

    void Start()
    {
        rB = GetComponent<Rigidbody>();
        rB.velocity = Vector3.right * 1f;
        prpos = this.transform.localPosition.x;
    }
    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rB.angularVelocity = Vector3.zero;
            this.rB.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0,4,0);
        }
        Upperfloor.localPosition = new Vector3(0,10,0);
        Lowerfloor.localPosition = new Vector3(0, -2, 0);
        prpos = 0;
     
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rB.velocity.x);
        sensor.AddObservation(rB.velocity.y);
    }
    public float forceMultiplier = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        float jump = 0f;
        Vector3 controlSignal = Vector3.zero;
        jump = vectorAction[0];
        rB.AddForce(Vector3.right * RightForce,ForceMode.VelocityChange);
        rB.AddForce(jump * JumpForce * Vector3.up, ForceMode.Impulse);

           if(this.transform.localPosition.x > prpos+offset)
        { 
            prpos = this.transform.localPosition.x;
            Upperfloor.localPosition = new Vector3(prpos, Upperfloor.localPosition.y, Upperfloor.localPosition.z);
            Lowerfloor.localPosition = new Vector3(prpos, Lowerfloor.localPosition.y, Lowerfloor.localPosition.z);
        }

        if (false)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        if (this.transform.localPosition.y < 0)
        {
            if (this.transform.localPosition.z < 0)
            {
                Debug.Log("negative reward");
                SetReward(-1.0f);
            }
            EndEpisode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //  if(other.gameObject.CompareTag("point"))
        //  {
        //     Debug.Log("point");
        //    SetReward(1f);
        //  EndEpisode();
        // }
    }
    public override void Heuristic(float[] actionsOut)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            actionsOut[0] = 1;
        }
        else
        {
            actionsOut[0] = 0;
        }

    }
}
