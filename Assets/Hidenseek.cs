using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Hidenseek : Agent
{
    Rigidbody rb;
    public float speed = 10f;
    public float rotationspeed = 5f;
    public GameObject hider;
    RaycastHit hit;
    RaycastHit hit1;
    RaycastHit hit2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        this.rb.angularVelocity = Vector3.zero;
        this.rb.velocity = Vector3.zero;
        this.transform.localPosition = Vector3.zero;
        hider.transform.localPosition = new Vector3(12,0,-10);
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 controlSignal;
        Vector3 rotationSignal;
        controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rotationSignal.x = vectorAction[2];
        this.transform.Rotate(hider.transform.up,rotationSignal.x*rotationspeed);
        rb.AddForce(controlSignal * speed);
   
       
       
        if (Physics.Raycast(this.transform.localPosition, this.transform.forward + new Vector3(1,0,0), out hit))
        {
            if (hit.transform.tag == "hider")
            {
                AddReward(1.0f);
                Debug.Log(this.transform.forward);
                EndEpisode();
            }
        }
        if (Physics.Raycast(this.transform.localPosition, this.transform.forward + new Vector3(-1, 0, 0), out hit1))
        {
            if (hit1.transform.tag == "hider")
            {
                AddReward(1.0f);
                Debug.Log(this.transform.forward);
                EndEpisode();
            }
        }
        if (Physics.Raycast(this.transform.localPosition, this.transform.forward + new Vector3(0, 0, 0), out hit2))
        {
            if (hit2.transform.tag == "hider")
            {
                AddReward(1.0f);
                Debug.Log(this.transform.forward);
                EndEpisode();
            }
        }

    }
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        if(Input.GetKey("q"))
        {
            actionsOut[2] = 1;
        }
        if(Input.GetKey("e"))
        {
            actionsOut[2] = -1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
