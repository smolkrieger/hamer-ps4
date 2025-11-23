using UnityEngine;

public class CollideEvent : MonoBehaviour
{
	[Tooltip("The sound that will be played when an object falls")]
	public AudioClip collideSound;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 2f)
		{
			AudioSource.PlayClipAtPoint(collideSound, base.transform.position);
			GetComponent<EnemyCall>().CallEnemy();
		}
	}
}
