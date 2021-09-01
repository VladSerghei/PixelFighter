using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;		
using UnityEngine.UI;
using TMPro;
	
public class GameManager : MonoBehaviour
{
	public float levelStartDelay = 2f;						//Delay at level start
	public float turnDelay = 0.5f;							//Delay between player turns
	
	public static GameManager managerInstance = null;		//Static instance of GameManager
	[HideInInspector] public bool playersTurn = false;		//Check if player turn
		
		
	private TextMeshProUGUI levelText;						//Current level text
	private GameObject levelImage;							//Image to display between levels
	private BoardManager boardManager;						//Reference to boardManager
	private int level = 0;									//Current level number
	private List<Enemy> enemies;							//List of enemies
	private bool enemiesMoving = false;						//Check enemy moving
	private bool doingSetup = true;							//Check if setup is done
		
		
		
	
	void Awake()
	{
		//Singleton
		SetUpSingleton();
		level = PlayerPrefsController.getLevel();
		//Init enemies
		enemies = new List<Enemy>();
		//Get boardManager
		boardManager = GetComponent<BoardManager>();
		
	}

	private void SetUpSingleton()
	{
		if (managerInstance == null)
		{
			managerInstance = this;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		else if (managerInstance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	static private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		managerInstance.level++; //Raise level
		PlayerPrefsController.setLevel(managerInstance.level);
		managerInstance.InitGame(); // Init scene
	}

	void InitGame()
	{
		//Mark start of setup
		doingSetup = true;
		playersTurn = false;
		//Display levelImage
		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LvlText").GetComponent<TextMeshProUGUI>();
		levelText.text = "Level " + level;
		levelImage.SetActive(true);
		//Setup level
		enemies.Clear();
		StartCoroutine(SetupLevel());

	}

	IEnumerator SetupLevel()
	{
		// Call boardManager setup
		boardManager.SetupScene(level);
		// Wait for setup
		yield return new WaitForSeconds(levelStartDelay);
		// Disable level image
		levelImage.SetActive(false);
		// Mark end of setup
		doingSetup = false;
		playersTurn = true;
	}

	public void GameOver(bool intended)
	{
		doingSetup = true;
		if(intended)
		{
			PlayerPrefsController.setLevel(level - 1);
			StartCoroutine(EndGame(intended));
		}
		else if(enemies.Count > 0) //Player lost
		{
			//Display end of game message
			levelText.text = "You survived " + level + " levels.";
			levelImage.SetActive(true);
			//End game session
			PlayerPrefsController.setLevel(0);
			StartCoroutine(EndGame(intended));
		}
		else // Player won
		{
			// Load next level
			FindObjectOfType<SceneLoader>().LoadGame();
		}

	}

	// End of a game session
	private IEnumerator EndGame(bool intended)
	{
		
		//Destroy current game manager after delay
		Destroy(gameObject, levelStartDelay);
		//Set instance to null
		managerInstance = null;
		//Unsubscribe from sceneLoaded
		SceneManager.sceneLoaded -= OnSceneLoaded;
		//Wait and load Game Over scene
		yield return new WaitForSeconds(levelStartDelay / 2);
		if(intended)
			FindObjectOfType<SceneLoader>().LoadMainMenu();
		else
			FindObjectOfType<SceneLoader>().LoadGameOver();
	}

	
		
	public void AddEnemy(Enemy enemy)
	{
		enemies.Add(enemy);
	}

	public void RemoveEnemy(Enemy enemy)
	{
		enemies.Remove(enemy);
		if(enemies.Count == 0) //No enemies left
		{
			GameOver(false);
		}
	}

	void Update()
	{
		// Check if computer turn
		if (playersTurn || enemiesMoving || doingSetup)
			return;
		StartCoroutine(MoveEnemies());
	}

	IEnumerator MoveEnemies()
	{
		// Mark enemy movement
		enemiesMoving = true;
		// Move enemies
		for (int i = 0; i < enemies.Count; i++)
		{
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(turnDelay); 
		}
		// Mark end of movement and give control to player
		enemiesMoving = false;
		playersTurn = true;
		
	}
}


