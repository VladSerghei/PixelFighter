using UnityEngine;
using System.Collections;



public abstract class MovingObject : MonoBehaviour
{
	public float speed = 20f;				//Moving animation speed
	public int damage = 10;					//Damage
	public int HP = 100;					//HitPoints

	private BoxCollider2D boxCollider; 		
	private Rigidbody2D rigidBody;					
	protected bool isMoving = false;					
		
	protected virtual void Start ()
	{
		boxCollider = GetComponent <BoxCollider2D> ();
		rigidBody = GetComponent <Rigidbody2D> ();
	}

	protected virtual bool AttemptMove(int xDir, int yDir, string tag)
	{

		// Check if can move
		bool canMove = this.CanMove(xDir, yDir, out RaycastHit2D hit);

		if (hit.transform == null) // Moved
			return true;

		GameObject collider = hit.transform.gameObject; // Hit object
		//Check for attack
		if (!canMove && collider != null && collider.tag == tag)
		{
			Attack(collider);
			return true; // Attacked
		}
		return false; // did not move
	}

	protected bool CanMove (int xDir, int yDir, out RaycastHit2D hit)
	{
		// Compute end position
		Vector2 end = (Vector2)transform.position + new Vector2 (xDir, yDir);
		// Disable collider
		boxCollider.enabled = false;
		// Linecast and check for collision
		hit = Physics2D.Linecast (transform.position, end);
		// Enable collider
		boxCollider.enabled = true;
		// If nothing is hit and obj isnt moving
		if(hit.transform == null && !isMoving)
		{
			//Start movement
			StartCoroutine (MoveTo (end));
			return true;
		}
		return false; //Cant move in specified direction 
	}
		
	protected virtual IEnumerator MoveTo (Vector2 end)
	{
		
		isMoving = true; // Mark start of movement
		// Compute remaining distance
		float remainingDistance = ((Vector2)transform.position - end).magnitude;
		// Move smoothlly each frame
		while (remainingDistance > float.Epsilon) 
		{
			// Compute movement each frame
			Vector2 newPostion = Vector2.MoveTowards(rigidBody.position, end, speed * Time.deltaTime);
			// Move to calculated position
			rigidBody.MovePosition (newPostion);
			// Recalculate remaining distance
			remainingDistance = ((Vector2)transform.position - end).magnitude;
			// Wait for new frame
			yield return null;
		}
			
		rigidBody.MovePosition (end); // Move to end
		isMoving = false; // Mark end of movement
	}

	public virtual void TakeDamage(int damage)
	{	
		HP -= damage;	
	}

	protected abstract void Attack (GameObject collider);
}

