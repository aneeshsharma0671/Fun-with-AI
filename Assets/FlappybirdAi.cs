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
    public float obspos;
    public float offset;
    public Transform Upperfloor;
    public Transform Lowerfloor;
    public GameObject obstacleG;
    Rigidbody obstacle;
    public Transform TrainingArea;
    private Animator anim;

    void Start()
    {
        rB = GetComponent<Rigidbody>();
        rB.velocity = Vector3.right * 1f;
        prpos = this.transform.localPosition.x;
        obstacle = obstacleG.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    public override void OnEpisodeBegin()
    {
        this.rB.angularVelocity = Vector3.zero;
        this.rB.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0,4,0);
        Upperfloor.localPosition = new Vector3(0,10,0);
        Lowerfloor.localPosition = new Vector3(0, -2, 0);
        obstacleG.transform.localPosition = new Vector3(8, Random.value*2.5f, 0);
        obstacle.velocity = Vector3.left * RightForce;
        prpos = 0;
     
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Floor")
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("obstacle"))
        {
            AddReward(0.01f);
            Debug.Log("point");
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Upperfloor.position);
        sensor.AddObservation(Lowerfloor.position);
        sensor.AddObservation(obstacle.transform.position);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(obstacle.velocity.x);
        sensor.AddObservation(rB.velocity.x);
        sensor.AddObservation(rB.velocity.y);
    }
    public float forceMultiplier = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        float jump = 0f;
        jump = vectorAction[0];
        Jump(jump);
       
        if(obstacleG.transform.localPosition.x < -16)
        {
            obstacleG.transform.localPosition = new Vector3(16, 0, 0);
        }
           if(this.transform.localPosition.x > prpos+offset)
        { 
            prpos = this.transform.localPosition.x;
            Upperfloor.localPosition = new Vector3(prpos, Upperfloor.localPosition.y, Upperfloor.localPosition.z);
            Lowerfloor.localPosition = new Vector3(prpos, Lowerfloor.localPosition.y, Lowerfloor.localPosition.z);
        }
    }
    void Jump(float jump)
    {
      
        AddReward(0.1f);
        rB.AddForce(jump * JumpForce * Vector3.up, ForceMode.Impulse);
    }
    public override void Heuristic(float[] actionsOut)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetTrigger("Up");
            actionsOut[0] = 1;
        }
        else
        {
            actionsOut[0] = 0;
        }

    }
}
