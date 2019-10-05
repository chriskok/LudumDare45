using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

	// Networking variables
	Thread receiveThread; 
	UdpClient client; 
	bool eyeschanged;
	int port; 

    public float speed = 2.0f;
    private Vector3 pos;
    private Transform tr;

    public LevelGenerator lg;
    public CameraShake camshake;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        tr = transform;

		port = 5065; 
		eyeschanged = false; 
		InitUDP();
    }

	private void InitUDP()
	{
		print ("UDP Initialized");

		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true; 
		receiveThread.Start ();

	}

	private void ReceiveData()
	{
		client = new UdpClient (port);
		while (true) 
		{
			try
			{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port); 
				byte[] data = client.Receive(ref anyIP); 

				string text = Encoding.UTF8.GetString(data); 
				print (">> " + text);

                if (text.Equals("OPEN")){
                    print("EYES WIDE OPEN YO");
                } else{
                    print("you shouldnt be seeing this");
                }
				eyeschanged = true;

			} catch(Exception e)
			{
				print (e.ToString()); 
			}
		}
	}

    void ShakeCamera(float duration){
        camshake.shakeDuration = duration;
    }

    // called when the cube hits the floor
    Vector3 CheckCollision(Vector3 pos)
    {
        int x_val = Mathf.RoundToInt(pos.x);
        int y_val = Mathf.RoundToInt(pos.y);

        if (x_val < 0 || y_val < 0){
            ShakeCamera(0.5f);
            return tr.position;
        }

        if (lg.mapArr[x_val,y_val] == 1){
            return pos;
        } else{
            ShakeCamera(0.5f);
            return tr.position;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && tr.position == pos) {
            pos += Vector3.right;
            pos = CheckCollision(pos);        
        }
        else if (Input.GetKeyDown(KeyCode.A) && tr.position == pos) {
            pos += Vector3.left;
            pos = CheckCollision(pos);        
        }
        else if (Input.GetKeyDown(KeyCode.W) && tr.position == pos) {
            pos += Vector3.up;
            pos = CheckCollision(pos);        
        }
        else if (Input.GetKeyDown(KeyCode.S) && tr.position == pos) {
            pos += Vector3.down;
            pos = CheckCollision(pos);        
        }

        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        
    }
}
