using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    public float speed;
    public Vector3 pos;
    public Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        tr = transform;
        speed = 5.0f;
    }

    public void MoveMe(Vector3 p, float s){
        pos = p;
        speed = s;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
    }
}
