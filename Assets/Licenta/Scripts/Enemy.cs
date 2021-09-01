using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class Enemy : MovingObject
{

	public GameObject dmgTextPrefab;	// Prefab of text for when taking damage
	private Transform target;			// Position of player

	private List<Vector2> directions;	// Movement directions
	private Animator animator;			// Animator controller			
	
	public AudioClip deathSFX;
	public AudioClip attackSFX;




	protected override void Start ()
	{
		// Add in game manager list
		GameManager.managerInstance.AddEnemy (this);
		// Init directions
		directions = new List<Vector2>();
		directions.Clear();
		directions.Add(Vector2.left);
		directions.Add(Vector2.right);
		directions.Add(Vector2.up);
		directions.Add(Vector2.down);
		// Set animator and target
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		// Call base
		base.Start ();
	}
		
	
	public void MoveEnemy ()
	{
		
		int xDir = 0;
		int yDir = 0;
		// Options of movement
		List<Vector2> dir = new List<Vector2>(directions);
		for (int i = 0; i < dir.Count; i++)
			dir[i] += (Vector2)transform.position;
		// Sort by closest to target
		dir.Sort(delegate (Vector2 a, Vector2 b)
		{
			if (((Vector2)target.position - a).sqrMagnitude > ((Vector2)target.position - b).sqrMagnitude)
				return 1;
			return -1;
		});

		// Transform back to directions
		for (int i = 0; i < dir.Count; i++)
		{
			dir[i] -= (Vector2)transform.position;
		}
		// Attempt to move until it moves or attacks
		for (int i = 0; i < dir.Count; i++)
		{
			xDir = (int)dir[i].x;
			yDir = (int)dir[i].y;
			// Break if action was taken
			if(AttemptMove(xDir, yDir, "Player"))
				break;
		}
	}


	public override void TakeDamage(int damage)
	{
		
		base.TakeDamage(damage);
		// Display damage text and destroy after 1 second
		var dmgText = dmgTextPrefab;
		dmgText.GetComponent<TextMeshProUGUI>().text = damage.ToString();
		Vector2 pos = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f);
		var obj = Instantiate(dmgText, pos, Quaternion.identity);
		obj.transform.SetParent(FindObjectOfType<Canvas>().transform);
		Destroy(obj, 1f);
		// Check death
		if (HP <= 0)
		{
			// Play animation
			animator.SetTrigger("Die");
			// Play SFX
			AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
			// Remove from enemy list and destroy
			GameManager.managerInstance.RemoveEnemy(this);
			Destroy(gameObject, 0.5f);
		}


	}

	protected override void Attack(GameObject collider)
	{
		// Damage player
		Player player = collider.GetComponent<Player>();
		player.TakeDamage (damage);
		// Determine direction of player
		Vector2 posDif = collider.transform.position - gameObject.transform.position;
		// Play SFX and corresponding animation
		AudioSource.PlayClipAtPoint(attackSFX, Camera.main.transform.position);
		if (posDif == Vector2.left)
			animator.SetTrigger("AttackLeft");
		else if (posDif == Vector2.right)
			animator.SetTrigger("AttackRight");
		else if (posDif == Vector2.up)
			animator.SetTrigger("AttackUp");
		else
			animator.SetTrigger("AttackDown");
	}
}

