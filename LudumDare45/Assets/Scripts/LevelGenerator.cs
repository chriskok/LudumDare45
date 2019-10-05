using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public Texture2D map;

	public ColorToPrefab[] colorMappings;

	public GameObject WallPrefab;

	public int[,] mapArr;

	// Use this for initialization
	void Start () {
		mapArr = new int[map.width,map.height];
		GenerateLevel();
	}

	void PrintMapArr(){
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

	void GenerateLevel ()
	{
		for (int x = 0; x < map.width; x++)
		{
			for (int y = 0; y < map.height; y++)
			{
				GenerateTile(x, y);
			}
		}

		PrintMapArr();

		GenerateWalls();
	}

	void GenerateWalls(){
		int rowLength = mapArr.GetLength(0);
		int colLength = mapArr.GetLength(1);
		for (int i = 0; i < rowLength; i++)
		{
			for (int j = 0; j < colLength; j++)
			{
				if (mapArr[i, (j)] == 1){
					// add wall to the top if no other tile above
					if (j != (colLength - 1) && mapArr[i, (j+1)] == 0){
						Vector2 position = new Vector2(i, (j+1));
						Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 90)), transform);
					}
					// add wall to the right if no other tile to the right
					if (i != (rowLength - 1) && mapArr[(i+1), j] == 0){
						Vector2 position = new Vector2((i+1), j);
						Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 0)), transform);
					}

					// add wall to the bottom if no other tile below
					if (j != 0 && mapArr[i, (j-1)] == 0){
						Vector2 position = new Vector2(i, (j-1));
						Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 270)), transform);
					} else if (j == 0){
						Vector2 position = new Vector2(i, (j-1));
						Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 270)), transform);
					}

					// add wall to the left if no other tile to the left
					if (i != 0 && mapArr[(i-1), j] == 0){
						Vector2 position = new Vector2((i-1), j);
						Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 180)), transform);
					}else if (i == 0){
						Vector2 position = new Vector2((i-1), j);
						Instantiate(WallPrefab, position, Quaternion.Euler(new Vector3(0, 0, 180)), transform);
					}
				}
			}
		}

	}

	void GenerateTile (int x, int y)
	{
		Color pixelColor = map.GetPixel(x, y);

		if (pixelColor.a == 0)
		{
			// The pixel is transparrent. Let's ignore it!
			mapArr[x,y] = 0;
			return;
		}

		foreach (ColorToPrefab colorMapping in colorMappings)
		{
			if (colorMapping.color.Equals(pixelColor))
			{
				if(colorMapping.prefabID.Equals("Ground")){
					mapArr[x,y] = 1;
				}
				Vector2 position = new Vector2(x, y);
				Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
			}
		}
	}
	
}
