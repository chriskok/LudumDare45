using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
	public List<GameObject> walls  = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        // gameObject.SetActive(false);
    }

    public void OnEnable(){
        Debug.Log(transform.position);
        Debug.Log(walls.Count);
        foreach (GameObject wall in walls)
        {
            wall.SetActive(true);
        }
    }

    public void ActivateWalls(){
        foreach (GameObject wall in walls)
        {
            wall.SetActive(true);
        }
    }
}
