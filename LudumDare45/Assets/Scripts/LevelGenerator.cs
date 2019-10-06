using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	public Texture2D map;

	public ColorToPrefab[] colorMappings;

	public GameObject WallPrefab;
	public GameObject GroundPrefab;

	public int[,] mapArr;
	public GameObject[,] mapObjArr;
	public int goal_x, goal_y;

	public List<GameObject> interactables  = new List<GameObject>();

    [Header("BG Variables")]
	public GameObject bgObj;
	public GameObject bgParent1;
	public GameObject bgParent2;
	public GameObject bgParent3;
	public float maxPos = 10.0f;
	public float minPos = -10.0f;
	public float scaleMin = 0.3f;
	public float scaleMax = 2f;
	public int max = 100;

	// Use this for initialization
	void Start () {
		mapArr = new int[map.width,map.height];
		mapObjArr = new GameObject[map.width,map.height];

		GenerateLevel();
		GenerateBG();
	}

	public void PrintMapArr(){
		int rowLength = mapArr.GetLength(0);
		int colLength = mapArr.GetLength(1);
		string arrayString = "";
		for (int i = 0; i < rowLength; i++)
		{
			for (int j = 0; j < colLength; j++)
			{
				arrayString += string.Format("{0} ", mapArr[i, j]);
			}
			arrayString += System.Environment.NewLine + System.Environment.NewLine;
		}

		Debug.Log(arrayString);
	}

	public void GenerateBG(){
        for (int i = 0; i < max; i++) {
			Vector3 position = new Vector3 (UnityEngine.Random.Range(minPos,maxPos),UnityEngine.Random.Range(minPos,maxPos),0);
			GameObject newbg = Instantiate(bgObj, position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0,360))));
			newbg.transform.parent = bgParent1.transform;
			float randomScale = UnityEngine.Random.Range(scaleMin,scaleMax);
        	newbg.transform.localScale = new Vector3(randomScale, randomScale, 1);
        }
        for (int i = 0; i < max; i++) {
			Vector3 position = new Vector3 (UnityEngine.Random.Range(minPos,maxPos),UnityEngine.Random.Range(minPos,maxPos),0);
			GameObject newbg = Instantiate(bgObj, position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0,360))));
			newbg.transform.parent = bgParent2.transform;
			float randomScale = UnityEngine.Random.Range(scaleMin,scaleMax);
        	newbg.transform.localScale = new Vector3(randomScale, randomScale, 1);
        }
        for (int i = 0; i < max; i++) {
			Vector3 position = new Vector3 (UnityEngine.Random.Range(minPos,maxPos),UnityEngine.Random.Range(minPos,maxPos),0);
			GameObject newbg = Instantiate(bgObj, position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0,360))));
			newbg.transform.parent = bgParent3.transform;
			float randomScale = UnityEngine.Random.Range(scaleMin,scaleMax);
        	newbg.transform.localScale = new Vector3(randomScale, randomScale, 1);
        }
	}

	void GenerateLevel ()
	{
		for (int x = 0; x < map.width; x++)
		{
			for (int y = 0; y < map.height; y++)
			{
				GenerateTile(x, y);

				Vector2 position = new Vector2(x, y);
				mapObjArr[x,y] = Instantiate(GroundPrefab, position, Quaternion.identity, transform);
			}
		}

		PrintMapArr();

		GenerateWalls();

		TurnGroundOff();
	}

	void GenerateWalls(){
		int rowLength = mapArr.GetLength(0);
		int colLength = mapArr.GetLength(1);
		for (int i = 0; i < rowLength; i++)
		{
			for (int j = 0; j < colLength; j++)
			{
				if (mapArr[i, (j)] >= 1){
					// add wall to the top if no other tile above
					if (j != (colLength - 1) && mapArr[i, (j+1)] == 0){
						Vector2 position = new Vector2(i, (j+1));
						GameObject newWall = Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 90)), transform);
						mapObjArr[i,j].GetComponent<Ground>().walls.Add(newWall);
					} if (j == (colLength - 1)){
						Vector2 position = new Vector2(i, (j+1));
						GameObject newWall = Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 90)), transform);
						mapObjArr[i,j].GetComponent<Ground>().walls.Add(newWall);
					}

					// add wall to the right if no other tile to the right
					if (i != (rowLength - 1) && mapArr[(i+1), j] == 0){
						Vector2 position = new Vector2((i+1), j);
						GameObject newWall = Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)), transform);
						mapObjArr[i,j].GetComponent<Ground>().walls.Add(newWall);
					} else if (i == (rowLength - 1)) {
						Vector2 position = new Vector2((i+1), j);
						GameObject newWall = Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)), transform);
						mapObjArr[i,j].GetComponent<Ground>().walls.Add(newWall);
					}

					// add wall to the bottom if no other tile below
					if (j != 0 && mapArr[i, (j-1)] == 0){
						Vector2 position = new Vector2(i, (j-1));
						GameObject newWall = Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 270)), transform);
						mapObjArr[i,j].GetComponent<Ground>().walls.Add(newWall);
					} else if (j == 0){
						Vector2 position = new Vector2(i, (j-1));
						GameObject newWall = Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 270)), transform);
						mapObjArr[i,j].GetComponent<Ground>().walls.Add(newWall);
					}

					// add wall to the left if no other tile to the left
					if (i != 0 && mapArr[(i-1), j] == 0){
						Vector2 position = new Vector2((i-1), j);
						GameObject newWall = Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 180)), transform);
						mapObjArr[i,j].GetComponent<Ground>().walls.Add(newWall);
					}else if (i == 0){
						Vector2 position = new Vector2((i-1), j);
						GameObject newWall = Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 180)), transform);
						mapObjArr[i,j].GetComponent<Ground>().walls.Add(newWall);
					}
				}
			}
		}

	}

	void TurnGroundOff(){
        foreach (GameObject curr_ground in mapObjArr)
        {
            curr_ground.SetActive(false);
        }

		mapObjArr[0,0].SetActive(true);
		mapObjArr[goal_x,goal_y].SetActive(true);
	}

	void GenerateTile (int x, int y)
	{
		Color pixelColor = map.GetPixel(x, y);

		if (pixelColor.a == 0)
		{
			// The pixel is transparent. Let's ignore it!
			mapArr[x,y] = 0;
			return;
		}

		int interactCount = 0;
		foreach (ColorToPrefab colorMapping in colorMappings)
		{
			if (colorMapping.color.Equals(pixelColor))
			{
				Vector2 position = new Vector2(x, y);
				mapArr[x,y] = 1;

				if(colorMapping.prefabID.Equals("Ground")){

				} else if (colorMapping.prefabID.Equals("Enemy")){
					mapArr[x,y] = 3;
					Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
					// interactables.Add( newInteractable );
				} else if (colorMapping.prefabID.Equals("Goal")){
					Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
					mapArr[x,y] = 2;
					goal_x = x;
					goal_y = y;
				} else if (colorMapping.prefabID.Equals("Block")){
					mapArr[x,y] = 5 + interactCount;
					GameObject newInteractable = Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
					interactables.Add( newInteractable );
					interactCount += 1;
				}

			}
		}
	}
	
}
