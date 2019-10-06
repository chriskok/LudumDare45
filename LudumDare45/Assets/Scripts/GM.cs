using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    private static GM instance = null;

    [Header("Networking Variables")]
	Thread receiveThread; 
	UdpClient client; 
	bool fadeout;
    bool eyesopen;
	int port; 

    public static GM Instance {
        get { return instance; }
    }

    AudioSource bgMusic;

    public PlayerScript currentPlayer;

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
				    currentPlayer.eyesopen = true;
                } else{
				    currentPlayer.eyesopen = false;             
                }

                currentPlayer.fadeout = true;
                

			} catch(Exception e)
			{
				print (e.ToString()); 
			}
		}
	}

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        bgMusic = GetComponent<AudioSource>();
        bgMusic.Play(0);

		port = 5065; 
        InitUDP();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer == null){
            currentPlayer = GameObject.Find("Player").GetComponent<PlayerScript>();
        }
    }
}
