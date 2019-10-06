using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    [Header("Networking Variables")]
	Thread receiveThread; 
	UdpClient client; 
	public bool fadeout;
    public bool eyesopen;
	int port; 

    [Header("Movement Variables")]
    public float speed = 2.0f;
    private Vector3 pos;
    private Transform tr;
    public LevelGenerator lg;
    public int blockThreshold = 20;
    public int enemyThreshold = 3;
    public bool GameOverride = false;

    [Header("Effects Variables")]
    public CameraShake camshake;
    public GameObject fadechange;
    public Animator currentUI;
    private bool levelstart;
    
    [Header("Audio Variables")]
    public AudioClip closeEyes;
    public AudioClip hitWall;
    public AudioClip hitBox;
    public AudioClip hitEnemy;
    public AudioClip hitGoal;
    public AudioClip[] walkClips;
    private AudioSource audiosource;

    void Awake () {
        audiosource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        tr = transform;

		port = 5065; 
		fadeout = false; 
        eyesopen = true;
        levelstart = false;
		// InitUDP();
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
                    // ScreenFade(false);
				    eyesopen = true;
                } else{
				    eyesopen = false;             
                }

                fadeout = true;
                

			} catch(Exception e)
			{
				print (e.ToString()); 
			}
		}
	}

    void ShakeCamera(float duration){
        camshake.shakeDuration = duration;
    }

    void ScreenFade(bool eyesopen){
        if (eyesopen){
            fadechange.GetComponent<Animator>().SetBool("Darkness", false); 
        } else{
            fadechange.GetComponent<Animator>().SetBool("Darkness", true); 
        }
    }

    // Directions up down left right are represented by (1,2,3,4) respectively
    int MoveBlock(GameObject block, Vector3 pos, int direction, int collision_val){
		int rowLength = lg.mapArr.GetLength(0);
		int colLength = lg.mapArr.GetLength(1);
        int x_val = Mathf.RoundToInt(pos.x);
        int y_val = Mathf.RoundToInt(pos.y);
        ShakeCamera(0.2f);

        Vector3 blockpos = block.transform.position;
        
        if (direction == 1){
            if (y_val + 1 >= colLength){ return 0; }
            int new_col_val = lg.mapArr[x_val, y_val + 1];
            if (y_val + 1 >= colLength || new_col_val < 1 || new_col_val >= blockThreshold){
                return 0;
            } else {
                if (new_col_val >= enemyThreshold && new_col_val < blockThreshold){
                    // Enemy detected - remove enemy
                    lg.enemies[new_col_val - enemyThreshold].SetActive(false);
                } 
                Block blockscript = block.GetComponent<Block>();
                blockscript.MoveMe(new Vector3(x_val, y_val + 1, 0), speed);
                lg.mapArr[x_val,y_val + 1] = lg.mapArr[x_val,y_val];
                lg.mapArr[x_val,y_val] = 1;
                return 1;
            }
        } else if (direction == 3){
            if (y_val - 1 < 0){ return 0; }
            int new_col_val = lg.mapArr[x_val, y_val- 1];
            if (y_val - 1 < 0 || new_col_val < 1 || new_col_val >= blockThreshold){
                return 0;
            } else {
                if (new_col_val >= enemyThreshold && new_col_val < blockThreshold){
                    // Enemy detected - remove enemy
                    lg.enemies[new_col_val - enemyThreshold].SetActive(false);
                } 
                Block blockscript = block.GetComponent<Block>();
                blockscript.MoveMe(new Vector3(x_val, y_val - 1, 0), speed);
                lg.mapArr[x_val,y_val - 1] = lg.mapArr[x_val,y_val];
                lg.mapArr[x_val,y_val] = 1;
                return 1;
            }
        } else if (direction == 2){
            if (x_val + 1 >= rowLength){ return 0; }
            int new_col_val = lg.mapArr[x_val+ 1, y_val];
            if (x_val + 1 >= rowLength || new_col_val < 1 || new_col_val >= blockThreshold){
                return 0;
            } else {
                if (new_col_val >= enemyThreshold && new_col_val < blockThreshold){
                    // Enemy detected - remove enemy
                    lg.enemies[new_col_val - enemyThreshold].SetActive(false);
                } 
                Block blockscript = block.GetComponent<Block>();
                blockscript.MoveMe(new Vector3(x_val + 1, y_val, 0), speed);
                lg.mapArr[x_val + 1,y_val] = lg.mapArr[x_val,y_val];
                lg.mapArr[x_val,y_val] = 1;
                return 1;
            }
        } else if (direction == 4){
            if (x_val - 1 < 0){ return 0; }
            int new_col_val = lg.mapArr[x_val- 1, y_val];
            if (x_val - 1 < 0 || new_col_val < 1 || new_col_val >= blockThreshold){
                return 0;
            } else {
                if (new_col_val >= enemyThreshold && new_col_val < blockThreshold){
                    // Enemy detected - remove enemy
                    lg.enemies[new_col_val - enemyThreshold].SetActive(false);
                } 
                Block blockscript = block.GetComponent<Block>();
                blockscript.MoveMe(new Vector3(x_val - 1, y_val, 0), speed);
                lg.mapArr[x_val - 1,y_val] = lg.mapArr[x_val,y_val];
                lg.mapArr[x_val,y_val] = 1;
                return 1;
            }
        }

        return 0;
    }

    public IEnumerator playSoundAndLoadLevel(int soundIndex, int levelIndex){
        if (soundIndex == 1){
            audiosource.PlayOneShot(hitGoal, 1f);
        } else if (soundIndex == 2){
            audiosource.PlayOneShot(hitEnemy, 1f);
        }
        fadechange.GetComponent<Animator>().SetBool("Darkness", true); 
        currentUI.SetTrigger("FadeOut"); 
        yield return new WaitWhile (()=> audiosource.isPlaying);
        SceneManager.LoadScene(levelIndex);
    }

    // called when the cube hits the floor
    Vector3 CheckCollision(Vector3 pos, int direction)
    {
		int rowLength = lg.mapArr.GetLength(0);
		int colLength = lg.mapArr.GetLength(1);
        int x_val = Mathf.RoundToInt(pos.x);
        int y_val = Mathf.RoundToInt(pos.y);

        if (x_val < 0 || y_val < 0 || x_val >= rowLength || y_val >= colLength){
            audiosource.PlayOneShot(hitWall, 1f);
            ShakeCamera(0.5f);
            return tr.position;
        }

        int collision_val = lg.mapArr[x_val,y_val];
        if (collision_val >= 1){
            // hit a block
            if (eyesopen && lg.mapObjArr[x_val, y_val].activeSelf == false){
                    Debug.Log("Can't explore with your eyes open!");
                    return tr.position;
            }

            if (collision_val >= blockThreshold){
                int blockmoved = MoveBlock(lg.interactables[collision_val - blockThreshold], pos, direction, collision_val);
                // lg.PrintMapArr();
                if (blockmoved == 1){
                    lg.mapObjArr[x_val, y_val].SetActive(true);
                    audiosource.PlayOneShot(walkClips[UnityEngine.Random.Range(0, walkClips.Length)],1f);
                    audiosource.PlayOneShot(hitBox, 1f);
                    if (x_val == lg.goal_x && y_val == lg.goal_y){
                        StartCoroutine(playSoundAndLoadLevel(1, SceneManager.GetActiveScene().buildIndex + 1));
                    }
                    return pos;
                } else {
                    audiosource.PlayOneShot(hitWall, 1f);
                    return tr.position;
                }
            } 
            // hit an enemy
            else if (collision_val >= 3){ 
                StartCoroutine(playSoundAndLoadLevel(2, SceneManager.GetActiveScene().buildIndex));
            }
            
            if (x_val == lg.goal_x && y_val == lg.goal_y){
                StartCoroutine(playSoundAndLoadLevel(1, SceneManager.GetActiveScene().buildIndex + 1));
            }

            lg.mapObjArr[x_val, y_val].SetActive(true);
            audiosource.PlayOneShot(walkClips[UnityEngine.Random.Range(0, walkClips.Length)],1f);
            return pos;
        } else{
            audiosource.PlayOneShot(hitWall, 1f); 
            ShakeCamera(0.5f);
            return tr.position;
        }
    }

    public void ResetLevel(){
        StartCoroutine(playSoundAndLoadLevel(3, SceneManager.GetActiveScene().buildIndex));
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && levelstart == false){
            fadechange.GetComponent<Animator>().SetTrigger("FadeIn"); 
            currentUI.SetTrigger("Fade"); 
            levelstart=true;
        }

        if (Input.GetKeyDown(KeyCode.Z)){
            eyesopen = !eyesopen;
        }

        if (Input.GetKeyDown(KeyCode.R)){
            eyesopen = !eyesopen;
            fadeout = true;
        }

        if (levelstart){
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

            if(fadeout == true)
            {
                ScreenFade(eyesopen);
                fadeout = false;
            }

        }
    }
}
