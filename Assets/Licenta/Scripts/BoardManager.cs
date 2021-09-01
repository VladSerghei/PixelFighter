using UnityEngine;
using System;
using System.Collections.Generic; 		
using Random = UnityEngine.Random; 		


	
public class BoardManager : MonoBehaviour
{
	[Header("Board settings")]
	public int columns = 16;                                        // Number of columns
	public int rows = 16;                                           // Number of rows
	public int minWalls = 50;										// Minimum walls in level
	public int maxWalls = 70;                                       // Maximum walls in level

	[Header("Prefabs")]
	public GameObject[] floors;										// Floor prefabs
	public GameObject[] walls;										// Wall prefabs
	public GameObject[] enemies;									// Enemy prefabs
	public GameObject[] outerWalls;                                 // Outer wall prefabs
	public GameObject player;										// Player prefab
		
	private Transform boardHolder;									// Board holder to use in hierarchy
	private List <Vector2> gameBoard = new List <Vector2> ();		// Play area
		
		
	// Initialise gameBoard with positions in the play area
	void InitialiseBoardPositions ()
	{
		gameBoard.Clear ();	
		for(int x = 1; x < columns-1; x++)
		{
			for(int y = 1; y < rows-1; y++)
			{
				gameBoard.Add (new Vector2(x, y));
			}
		}
		
		
	}
		
	// Set up floor and walls
	void BoardSetup ()
	{
		// Create boardHolder object to use in hierarchy
		boardHolder = new GameObject ("Board").transform;
			
		// Loop and fill screen
		for(int x = -1; x < columns + 1; x++)
		{
			for(int y = -1; y < rows + 1; y++)
			{
				GameObject toInstantiate = null; // Create empty GameObject

				if (x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outerWalls[Random.Range(0, outerWalls.Length)]; // If on edges pick random outer wall
				else
					toInstantiate = floors[Random.Range(0, floors.Length)]; // If in play area pick random floor
				
				// Instantiate new object and set boardHolder as parent so that hierarchy doesnt fill
				GameObject instance = Instantiate (toInstantiate, new Vector2 (x, y), Quaternion.identity) as GameObject; 
				instance.transform.SetParent (boardHolder); 
			}
		}
	}
		
		
	// Returns a random position in play area and deletes it
	Vector2 RandomPosition ()
	{
		// Get a random index to pick from gameBoard
		int randomIndex = Random.Range (0, gameBoard.Count);
		// Get the position from the list
		Vector2 randomPosition = gameBoard[randomIndex];
		// Delete item from list to not be returned again
		gameBoard.RemoveAt (randomIndex);
		// Return the position
		return randomPosition; 
	}
		
		
	// Spawn random items from array in random positions on board
	void SpawnRandomly (GameObject[] array, int min, int max) 
	{
		int nrToSpawn = Random.Range(min, max + 1);
			
		// Instantiate objects until the limit is reached
		for(int i = 0; i < nrToSpawn; i++)
		{
			Vector2 randomPosition = RandomPosition(); // Get a random position
				
			GameObject tile = array[Random.Range (0, array.Length)];
			Instantiate(tile, randomPosition, Quaternion.identity).transform.SetParent(boardHolder); 
		}
	}
		
		
	// Initialize level
	public void SetupScene (int level)
	{
		
		BoardSetup (); // Create floor and walls
			
		InitialiseBoardPositions (); // Reset play area
		
		SpawnRandomly(walls, minWalls, maxWalls); // Spawn walls

		int enemyCount = (int)Mathf.Log(level, 2f) + 1; // Choose nr of enemies using log
		SpawnRandomly (enemies, enemyCount, enemyCount); // Spawn enemies
		Instantiate(player, new Vector2(0, 0), Quaternion.identity);
	}
}
