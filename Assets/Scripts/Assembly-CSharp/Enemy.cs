using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	[Header("General Settings")]
	[Tooltip("Player gameobject from scene")]
	public PlayerController player;

	[Tooltip("Transform of enemy head")]
	public Transform head;

	[Tooltip("Layers by which the enemy will search for the player (Must include player layer, as well as obstacle layers (Default,Interact еtc))")]
	public LayerMask searchLayers;

	private NavMeshAgent agent;

	private float magnit;

	private Animator animator;

	[Header("AI Settings")]
	[Tooltip("Layer of doors (Interact by default)")]
	public LayerMask doorsLayer;

	[Tooltip("Distance through which the enemy sees the player")]
	public float seeDistance;

	[Tooltip("Enemy Field of View (x - minimal, y - maximal)")]
	public Vector2 enemyFOV;

	[Tooltip("Radius at which the enemy will notice the player if the player is too close (if player crouch this value = / 2) ")]
	public float closeDistance;

	[Tooltip("The time after which the enemy loses the player(after the time has passed, the player’s coordinates are transferred to the enemy and he goes to these coordinates)")]
	public float lostTime;

	[Tooltip("The time that the enemy spends at the point where he last saw or heard the player")]
	public float patrolTime;

	[Tooltip("Controll enemy walk speed")]
	public float walkSpeed;

	[HideInInspector]
	public bool seePlayer;

	private bool chasePlayer;

	private bool searchPlayer;

	private int searchState;

	private HidePlace playerHidePlace;

	[Header("Sound Settings")]
	[Tooltip("Sound of enemy footsteps")]
	public AudioClip[] footSteps;

	[Tooltip("The sound that the enemy makes when he catch a player")]
	public AudioClip catchSound;

	[Tooltip("Player Hitting Sound")]
	public AudioClip hitSound;

	private AudioSource AS;

	[Header("Patrol Settings")]
	[Tooltip("Transforms of way points for enemy patrolling")]
	public Transform[] wayPoints;

	[Tooltip("The time that the enemy waits on points")]
	public float wayPointWaitTime;

	private int wpID;

	[Header("Catch Settings")]
	[Tooltip("Sets the speed of the player’s camera turning to face the enemy at the moment when the enemy catches player")]
	public float playerLookSpeed;

	[Tooltip("The distance at which the enemy can catch player")]
	public float catchDistance;

	private int catchPlayerState;

	private Vector3 lastSawPoint;

	private void Awake()
	{
		AS = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
		agent.speed = walkSpeed;
		animator = GetComponent<Animator>();
		lastSawPoint = Vector3.zero;
		wpID = -1;
	}

	private void Update()
	{
		SearchPlayer();
		CatchingPlayer();
		DoorCheck();
		CheckLastPoint();
		PatrolWayPoints();
		CheckHidePlace();
		if (agent.enabled)
		{
			magnit = agent.velocity.magnitude;
			animator.SetFloat("Magnitude", magnit);
		}
		else
		{
			animator.SetFloat("Magnitude", 0f);
		}
	}

	public void SendHidePlace()
	{
		lastSawPoint = Vector3.zero;
		playerHidePlace = player.hidePlace;
	}

	private void PatrolWayPoints()
	{
		if (!seePlayer && !searchPlayer && wpID == -1)
		{
			StopAllCoroutines();
			int num = Random.Range(1, wayPoints.Length);
			EnemySetDestination(wayPoints[num].position);
			wpID = num;
			wayPoints[num].SetSiblingIndex(0);
		}
		if (wpID != -1)
		{
			float num2 = Vector3.Distance(base.transform.position, wayPoints[wpID].position);
			if (num2 <= agent.stoppingDistance)
			{
				EnemySetDestination(base.transform.position);
				StartCoroutine(WaitPatrolTime(0));
			}
		}
	}

	private void CheckHidePlace()
	{
		if (!playerHidePlace || searchState == 2)
		{
			return;
		}
		if (player.hidePlace != null && !seePlayer && player.hidePlace == playerHidePlace)
		{
			EnemySetDestination(playerHidePlace.enemyPosition.position);
			float num = Vector3.Distance(base.transform.position, playerHidePlace.enemyPosition.position);
			if (num <= agent.stoppingDistance)
			{
				StartCoroutine(WaitOnHidePlace());
				searchState = 2;
				base.transform.rotation = playerHidePlace.enemyPosition.rotation;
				agent.enabled = false;
			}
		}
		else
		{
			playerHidePlace = null;
		}
	}

	private void CheckLastPoint()
	{
		if (!seePlayer && lastSawPoint != Vector3.zero && searchState == 0)
		{
			if (NavMesh.SamplePosition(lastSawPoint, out var hit, 1.5f, -1))
			{
				lastSawPoint = hit.position;
			}
			EnemySetDestination(lastSawPoint);
			float num = Vector3.Distance(base.transform.position, lastSawPoint);
			if (num <= 2f)
			{
				EnemySetDestination(base.transform.position);
				lastSawPoint = Vector3.zero;
				searchState = 1;
				animator.SetBool("Patrol", value: true);
				StartCoroutine(WaitPatrolTime(1));
			}
		}
	}

	public void CallEnemy(Vector3 pos)
	{
		if (!seePlayer)
		{
			lastSawPoint = pos;
			if (NavMesh.SamplePosition(lastSawPoint, out var hit, 1f, -1))
			{
				lastSawPoint = hit.position;
			}
			searchState = 0;
		}
	}

	private void SearchPlayer()
	{
		if (!seePlayer)
		{
			float num = Vector3.Distance(base.transform.position, player.transform.position);
			float num2 = closeDistance;
			if (player.crouch)
			{
				num2 /= 2f;
			}
			if (num <= num2 && player.hidePlace == null && player.playerMoving && PlayerRaycast())
			{
				lastSawPoint = player.transform.position;
				if (NavMesh.SamplePosition(lastSawPoint, out var hit, 1f, -1))
				{
					lastSawPoint = hit.position;
				}
			}
		}
		if (PlayerRaycast() && PlayerFOV() && player.hidePlace == null)
		{
			chasePlayer = true;
			seePlayer = true;
			ResetSearchStates();
			StopAllCoroutines();
		}
		else if (seePlayer)
		{
			if (!searchPlayer)
			{
				seePlayer = false;
				searchPlayer = true;
				StartCoroutine(WaitLostTime());
			}
		}
		else
		{
			seePlayer = false;
		}
		if (seePlayer || chasePlayer)
		{
			EnemySetDestination(player.transform.position);
		}
	}

	private bool PlayerRaycast()
	{
		if (Physics.Raycast(base.transform.position, player.transform.position - base.transform.position, out var hitInfo, seeDistance) && hitInfo.transform.gameObject == player.gameObject)
		{
			return true;
		}
		return false;
	}

	private bool PlayerFOV()
	{
		Vector3 vector = player.transform.position - head.transform.position;
		float num = Vector3.Angle(vector, head.forward);
		if (num >= enemyFOV.x && num <= enemyFOV.y)
		{
			return true;
		}
		return false;
	}

	private void CatchingPlayer()
	{
		if (catchPlayerState == 0 && seePlayer && player.canBeCatchen)
		{
			float num = Vector3.Distance(base.transform.position, player.transform.position);
			if (num <= catchDistance)
			{
				AudioSource.PlayClipAtPoint(catchSound, base.transform.position);
				player.CatchPlayer(1, null);
				catchPlayerState = 1;
				agent.enabled = false;
			}
		}
		if (catchPlayerState == 1)
		{
			Vector3 forward = head.position - player.cameraTransform.position;
			player.cameraTransform.rotation = Quaternion.RotateTowards(player.cameraTransform.rotation, Quaternion.LookRotation(forward), Time.deltaTime * playerLookSpeed);
			Vector3 vector = player.transform.position - base.transform.position;
			vector.y = 0f;
			Quaternion quaternion = Quaternion.LookRotation(vector);
			Quaternion quaternion2 = Quaternion.LookRotation(-vector);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * playerLookSpeed);
			player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, quaternion2, Time.deltaTime * playerLookSpeed);
			float num2 = Quaternion.Angle(player.cameraTransform.rotation, Quaternion.LookRotation(forward));
			if ((double)num2 <= 4.0)
			{
				base.transform.rotation = quaternion;
				player.transform.rotation = quaternion2;
				forward = head.position - player.cameraTransform.position;
				player.cameraTransform.rotation = Quaternion.LookRotation(forward);
				animator.SetInteger("HitID", 0);
				animator.SetTrigger("HitPlayer");
				catchPlayerState = 2;
				player.CatchPlayer(2, null);
			}
		}
		if (catchPlayerState != 3)
		{
			return;
		}
		StopAllCoroutines();
		if (player.hidePlace == playerHidePlace)
		{
			if (playerHidePlace.hidePlaceType == HidePlace.HidePlaceType.bed)
			{
				GetComponent<IKControl>().ikActive = true;
				base.transform.position = playerHidePlace.enemyPosition.position;
				base.transform.rotation = playerHidePlace.enemyPosition.rotation;
				Vector3 vector2 = player.transform.position - base.transform.position;
				vector2.y = 0f;
				Quaternion rotation = Quaternion.LookRotation(-vector2);
				player.transform.rotation = rotation;
				player.cameraTransform.rotation = rotation;
				animator.SetInteger("HitID", playerHidePlace.enemyAnimationState);
				animator.SetTrigger("HitPlayer");
				player.CatchPlayer(3, playerHidePlace.cameraAnimationName);
				catchPlayerState = 4;
			}
			if (playerHidePlace.hidePlaceType == HidePlace.HidePlaceType.chest)
			{
				AudioSource.PlayClipAtPoint(catchSound, base.transform.position);
				player.CatchPlayer(1, null);
				catchPlayerState = 1;
				agent.enabled = false;
				if ((bool)playerHidePlace.hidePlaceAnimation)
				{
					playerHidePlace.hidePlaceAnimation.Play(playerHidePlace.hidePlaceUnhideAnimationName);
				}
			}
		}
		else
		{
			agent.enabled = true;
			playerHidePlace = null;
			catchPlayerState = 0;
		}
	}

	private void DoorCheck()
	{
		if (Physics.Raycast(head.transform.position, head.transform.forward, out var hitInfo, 1f, doorsLayer) && (bool)hitInfo.transform.gameObject.GetComponent<DoorSiders>() && hitInfo.transform.gameObject.GetComponent<DoorSiders>().genDoor.state == 0)
		{
			if (hitInfo.transform.gameObject.GetComponent<DoorSiders>().genDoor.locked)
			{
				hitInfo.transform.gameObject.GetComponent<DoorSiders>().genDoor.UnlockDoor();
			}
			hitInfo.transform.gameObject.GetComponent<DoorSiders>().InteractWithDoor();
		}
	}

	private void ResetSearchStates()
	{
		wpID = -1;
		searchState = 0;
		searchPlayer = false;
		animator.SetBool("Patrol", value: false);
	}

	private void EnemySetDestination(Vector3 pos)
	{
		if (agent.enabled)
		{
			animator.SetBool("Patrol", value: false);
			agent.SetDestination(pos);
		}
	}

	public void RestartEnemyStats()
	{
		StopAllCoroutines();
		GetComponent<IKControl>().ikActive = false;
		agent.enabled = true;
		catchPlayerState = 0;
	}

	public void FootStepPlay()
	{
		int num = Random.Range(1, footSteps.Length);
		AS.PlayOneShot(footSteps[num]);
	}

	public void PlayAnimationSound()
	{
		AudioSource.PlayClipAtPoint(hitSound, base.transform.position);
		player.CatchPlayer(4, null);
	}

	private IEnumerator WaitLostTime()
	{
		yield return new WaitForSeconds(lostTime);
		lastSawPoint = player.transform.position;
		if (NavMesh.SamplePosition(lastSawPoint, out var hit, 1f, -1))
		{
			lastSawPoint = hit.position;
		}
		chasePlayer = false;
	}

	private IEnumerator WaitPatrolTime(int state)
	{
		yield return new WaitForSeconds(patrolTime);
		if (state == 1)
		{
			lastSawPoint = Vector3.zero;
		}
		ResetSearchStates();
	}

	private IEnumerator WaitOnHidePlace()
	{
		yield return new WaitForSeconds(wayPointWaitTime);
		if (player.hidePlace != null && player.hidePlace == playerHidePlace)
		{
			catchPlayerState = 3;
			yield break;
		}
		agent.enabled = true;
		playerHidePlace = null;
	}
}
