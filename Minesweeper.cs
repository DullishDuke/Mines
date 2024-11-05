using System.Threading;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI.Table;

public class Minesweeper : MonoBehaviour
{
    [SerializeField] private int COLUMNS = 10;
	[SerializeField] private int ROWS = 10;
	[SerializeField] private int MINES = 25;

	// I GameObjects sono classi che contengono Components
	// I Components sono a loro volta delle classi e fungono da moduli

	[SerializeField] private GameObject tile;
	[SerializeField] private GameObject mine;

	GameObject[,] grid;	// Array 2D di <gameObject>

	private void Start()
	{
		print("Columns: " + COLUMNS);
		print("Rows: " + ROWS);
		grid = new GameObject[ROWS, COLUMNS];
		centerCamera();
		fillGrid(grid);
		checkNeighbour(grid, 2, 2);
		spawnGrid(grid);
	}

	private void fillGrid(GameObject[,] grid)
	{
		for(int y = 0; y < ROWS; y++)
		{
			for (int x = 0; x < COLUMNS; x++)
			{
				grid[y, x] = tile;
			}
		}
		placeMines(grid);
	}

	private void placeMines(GameObject[,] grid)
	{
		int placedMines = 0;
		while(placedMines < MINES)
		{
			int row = Random.Range(0, ROWS);
			int col = Random.Range(0, COLUMNS);
			if (grid[row, col] != mine)
			{
				grid[row, col] = mine;
				placedMines++;
			}
		}
	}

	private void checkNeighbour(GameObject[,] grid, int y, int x)
	{
		for (int localY = -1; localY < 1; localY++)
		{
			for(int localX = -1; localX < 1; x++)
			{
				int row = y + localY;
				int col = x + localX;

				if (row < 0 || col < 0 || row >= ROWS || col >= COLUMNS) continue;
				if (grid[row, col] == tile)
				{
					GameObject textChild = grid[row, col].transform.GetChild(1).gameObject;
					TextMeshPro TMPComp	 = textChild.GetComponent<TextMeshPro>();

					int value = int.Parse(TMPComp.text);
					value++;
					TMPComp.SetText(value.ToString());
				}
			}
		}
	}

	private void spawnGrid(GameObject[,] grid)
	{
		for(int x = 0; x < ROWS; x++)
		{
			for (int z = 0; z < COLUMNS; z++)
			{
				GameObject instance = Instantiate(grid[x,z], grid[x,z].transform.position = new Vector3(z, 0, x), Quaternion.identity);
				// Quaternion.identity vale (x:0, y:0, z:0, w:1) è un oggetto non ruotato che punta verso l'asse 

				//if (grid[x,z] == Mine)
				//{
				//	instance.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
				//}
			}
		}
	}


	// Crea un Vector3 ( utilizzato da Transform.position -> (x, y, z) )
	// che nelle assi x e z contiene la metà della lunghezza dei lati della griglia
	// e sottrae 0,5 unità per mettersi al centro delle caselle (larghe 1x1 unità)
	private void centerCamera()
	{
		Vector3 camPos = new Vector3((COLUMNS / 2.0f) - 0.5f, 10f, (ROWS / 2.0f) - 0.5f);
		Camera.main.transform.position = camPos;

		//Debug.Log("Camera Position: " + camPos);

		// Calcola la radice quadrata della moltiplicazione dei lati della griglia
		// per ottenere un valore moltiplicativo applicabile allo zoom della telecamera
		// (se con 1 casella lo zoom corretto è di 0.5, per 100 caselle sarà di 5, perchè:
		// sqrt(10x10) = 10   quindi   10 * 0.5 = 5)

		float gridSqrt = Mathf.Sqrt(COLUMNS * ROWS);
		float size = (int)gridSqrt;

		//Debug.Log("sqrt: " + gridSqrt);
		//Debug.Log("size: " + size);
		if (COLUMNS > ROWS)
		{
			if (COLUMNS % 2 != 0)
			{
				size -= 0.5f;
			}
			if (gridSqrt % 2 == 0 && gridSqrt > 2) size = 0.5f * ((int)size) - 1;
			else size = 0.5f * size;
		}
		else
		{
			if (ROWS % 2 != 0)
			{
				size -= 0.5f;
			}
		}
		
		if (size < 1)
		{
			size = 0.5f;
		}
		//size = (gridSqrt % 2 == 0) ? 0.5f * gridSqrt : 0.5f * (int)gridSqrt;
		Camera.main.orthographicSize = size;
	}
}
