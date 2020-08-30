﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private void Update()
    {
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f), Space.World);
    }
}
