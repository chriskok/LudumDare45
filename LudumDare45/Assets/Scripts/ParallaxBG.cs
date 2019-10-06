using UnityEngine;
using System.Collections;

public class ParallaxBG : MonoBehaviour
{
    public Vector3 startpos;
	public GameObject cam;
	
    public float parallax;

    void Start(){
        startpos = transform.position;
    }

	void Update()
	{
		float dist_x = (cam.transform.position.x * parallax);
		float dist_y = (cam.transform.position.y * parallax);

        transform.position = new Vector3(startpos.x + dist_x, startpos.y + dist_y, startpos.z);

	}
}