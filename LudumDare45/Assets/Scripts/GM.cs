using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    private static GM instance = null;

    public static GM Instance {
        get { return instance; }
    }

    AudioSource bgMusic;

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
        Debug.Log("started bg music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
