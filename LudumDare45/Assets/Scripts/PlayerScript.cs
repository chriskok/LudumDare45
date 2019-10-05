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

    // Directions up down left right are represented by (1,2,3,4) respectively
    int MoveBlock(GameObject block, Vector3 pos, int direction){
		int rowLength = lg.mapArr.GetLength(0);
		int colLength = lg.mapArr.GetLength(1);
        int x_val = Mathf.RoundToInt(pos.x);
        int y_val = Mathf.RoundToInt(pos.y);
        ShakeCamera(0.2f);

        Vector3 blockpos = block.transform.position;
        
        if (direction == 1){
            if (y_val + 1 >= colLength || lg.mapArr[x_val, y_val + 1] > 2 || lg.mapArr[x_val, y_val + 1] < 1){
                return 0;
            } else {
                Block blockscript = block.GetComponent<Block>();
                blockscript.MoveMe(new Vector3(x_val, y_val + 1, 0), speed);
                lg.mapArr[x_val,y_val + 1] = lg.mapArr[x_val,y_val];
                lg.mapArr[x_val,y_val] = 1;
                return 1;
            }
        } else if (direction == 3){
            if (y_val - 1 < 0 || lg.mapArr[x_val, y_val - 1] > 2 || lg.mapArr[x_val, y_val- 1] < 1){
                return 0;
            } else {
                Block blockscript = block.GetComponent<Block>();
                blockscript.MoveMe(new Vector3(x_val, y_val - 1, 0), speed);
                lg.mapArr[x_val,y_val - 1] = lg.mapArr[x_val,y_val];
                lg.mapArr[x_val,y_val] = 1;
                return 1;
            }
        } else if (direction == 2){
            if (x_val + 1 >= rowLength || lg.mapArr[x_val+ 1, y_val] > 2 || lg.mapArr[x_val+ 1, y_val] < 1){
                return 0;
            } else {
                Block blockscript = block.GetComponent<Block>();
                blockscript.MoveMe(new Vector3(x_val + 1, y_val, 0), speed);
                lg.mapArr[x_val + 1,y_val] = lg.mapArr[x_val,y_val];
                lg.mapArr[x_val,y_val] = 1;
                return 1;
            }
        } else if (direction == 4){
            if (x_val - 1 < 0 || lg.mapArr[x_val - 1, y_val] > 2 || lg.mapArr[x_val- 1, y_val] < 1){
                return 0;
            } else {
                Block blockscript = block.GetComponent<Block>();
                blockscript.MoveMe(new Vector3(x_val - 1, y_val, 0), speed);
                lg.mapArr[x_val - 1,y_val] = lg.mapArr[x_val,y_val];
                lg.mapArr[x_val,y_val] = 1;
                return 1;
            }
        }

        return 0;
    }

    // called when the cube hits the floor
    Vector3 CheckCollision(Vector3 pos, int direction)
    {
		int rowLength = lg.mapArr.GetLength(0);
		int colLength = lg.mapArr.GetLength(1);
        int x_val = Mathf.RoundToInt(pos.x);
        int y_val = Mathf.RoundToInt(pos.y);

        if (x_val < 0 || y_val < 0 || x_val >= rowLength || y_val >= colLength){
            ShakeCamera(0.5f);
            return tr.position;
        }

        int collision_val = lg.mapArr[x_val,y_val];
        if (collision_val >= 1){
            // hit a block
            if (collision_val >= 5){
                int blockmoved = MoveBlock(lg.interactables[collision_val - 5], pos, direction);
                if (blockmoved == 1){
                    return pos;
                } else {
                    return tr.position;
                }
            }
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
            pos = CheckCollision(pos, 2);        
        }
        else if (Input.GetKeyDown(KeyCode.A) && tr.position == pos) {
            pos += Vector3.left;
            pos = CheckCollision(pos, 4);        
        }
        else if (Input.GetKeyDown(KeyCode.W) && tr.position == pos) {
            pos += Vector3.up;
            pos = CheckCollision(pos, 1);        
        }
        else if (Input.GetKeyDown(KeyCode.S) && tr.position == pos) {
            pos += Vector3.down;
            pos = CheckCollision(pos, 3);        
        }

        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
    }
}
