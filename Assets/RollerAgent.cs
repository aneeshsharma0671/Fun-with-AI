using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.ComponentModel;

public class RollerAgent : Agent
{
    Rigidbody rB;
    bool isjumping = false;
    public float jumptime = 0.5f;
    private float time = 0.0f;
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
            this.transform.localPosition = new Vector3(0, 0.5f, -4.0f);
        }
        if (this.transform.localPosition.z < 0)
        {
            Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 3 + 1);
        }
        else
        {
            Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, -(Random.value * 3 + 1));

        }


    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rB.velocity.x);
        sensor.AddObservation(rB.velocity.y);
        sensor.AddObservation(rB.velocity.z);
    }
    public float forceMultiplier = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        float jump=0f;
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        jump = vectorAction[2];
        rB.AddForce(controlSignal * forceMultiplier);
        rB.AddForce(jump *1f * Vector3.up,ForceMode.Impulse);

        float distancetotarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        if(distancetotarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        if(this.transform.localPosition.y <0)
        {
            if(this.transform.localPosition.z < 0)
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
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        if(this.transform.localPosition.y < 1f)
        {
            isjumping = false;
        }
        else
        {
            isjumping = true;
        }
        if (isjumping)
        {
            actionsOut[2] = 0;
        }
        if(Input.GetKey(KeyCode.Space)&& !isjumping)
        {
            actionsOut[2] = 1;
        }
    }
}
