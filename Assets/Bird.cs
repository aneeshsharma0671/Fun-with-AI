using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Bird : Agent
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private PipeHandler pipeHandler = null;
    [SerializeField] private Transform bodyTransform = null;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxVelocityMagnitude = 5f;

    private Vector3 startingpos;

    // awake like for mlagents
    public override void Initialize()
    {
        startingpos = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startingpos;
        rb.velocity = Vector3.zero;
        pipeHandler.ResetPipes();
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        //survival point
        AddReward(0.1f);
        if(Mathf.FloorToInt(vectorAction[0])!= 1) { return; }
        Jump();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        if (!Input.GetKey(KeyCode.Space)) { return; }
        actionsOut[0] = 1;
    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocityMagnitude);
    }
    private void OnTriggerEnter(Collider other)
    {
        AddReward(-1.0f);
        EndEpisode();
    }
    private void Update()
    {
        bodyTransform.rotation = Quaternion.LookRotation(rb.velocity + new Vector3(10f, 0f, 0f), transform.up);
    }
}
