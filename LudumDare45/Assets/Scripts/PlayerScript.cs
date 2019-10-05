using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public float speed = 2.0f;
    private Vector3 pos;
    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && tr.position == pos) {
            pos += Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && tr.position == pos) {
            pos += Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.W) && tr.position == pos) {
            pos += Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && tr.position == pos) {
            pos += Vector3.down;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
    }
}
