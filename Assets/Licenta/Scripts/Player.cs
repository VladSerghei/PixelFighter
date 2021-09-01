using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
	public float deathDelay = 1f;
	public HPdisplay healthDisplay;

	[Header("Effects")]
	private Animator animator;
	public AudioClip deathSFX;
	public AudioClip attackSFX;
							  
			
	protected override void Start ()
	{
		animator = GetComponent<Animator>();
		healthDisplay = FindObjectOfType<HPdisplay>();
		healthDisplay.gameObject.GetComponent<Slider>().enabled = true;
		healthDisplay.SetMaxHP(HP);
		healthDisplay.ShowHp(HP);
		base.Start ();
	}
		
		
		
	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) // Exit
		{
			StartCoroutine(GameOver(true));
			isMoving = true;
		}
		if(isMoving || !GameManager.managerInstance.playersTurn) // Not player turn
			return;
			
		int horizontal = 0;  	
		int vertical = 0;

		if (Input.GetKeyDown(KeyCode.UpArrow)) // Up
			vertical = 1;
		if (Input.GetKeyDown(KeyCode.DownArrow)) // Down
			vertical = -1;
		if (Input.GetKeyDown(KeyCode.LeftArrow)) // Left
			horizontal = -1;
		if (Input.GetKeyDown(KeyCode.RightArrow)) // Right
			horizontal = 1;
		if(Input.GetKeyDown(KeyCode.Space)) // Skip turn
		{
			GameManager.managerInstance.playersTurn = false;
			return;
		}
		
		if(horizontal != 0)
		{
			vertical = 0; // Do not attempt to move on 2 axis
		}
		
		
		if(horizontal != 0 || vertical != 0)
		{
			AttemptMove(horizontal, vertical);
		}
	}

	
		
	
	protected override bool AttemptMove(int xDir, int yDir, string tag = "Enemy")
	{
		return base.AttemptMove (xDir, yDir, tag);
	}

	protected override IEnumerator MoveTo(Vector2 end)
	{
		 yield return base.MoveTo(end);
		 GameManager.managerInstance.playersTurn = false;
	}


	protected override void Attack (GameObject collider)
	{
		// Start animation
		animator.SetTrigger("Attack");
		// Play SFX
		AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);
		// Damage enemy
		Enemy enemy = collider.GetComponent<Enemy>();
		enemy.TakeDamage(damage);
		// End turn
		GameManager.managerInstance.playersTurn = false;
	}

	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);
		healthDisplay.ShowHp(HP);
		// If died
		if (HP <= 0)
		{
			StartCoroutine(GameOver(false));
		}
	}

	private IEnumerator GameOver(bool intended)
	{
		if(!intended)
		{
			// Play animation
			animator.SetTrigger("Die");
			// Play SFX
			AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
			// Wait animation complete
			yield return new WaitForSeconds(deathDelay);
			
			
		}
		// End game session
		GameManager.managerInstance.GameOver(intended);
		Destroy(gameObject);
		
	}

	
}

