using UnityEngine;

public class HidePlace : MonoBehaviour
{
	public enum HidePlaceType
	{
		bed = 0,
		chest = 1
	}

	[Tooltip("Type of hide palce (bed, chest)")]
	public HidePlaceType hidePlaceType = HidePlaceType.bed;

	[Tooltip("Player position after interact with hide place")]
	public Transform enterPlace;

	[Tooltip("The position the player moves to after leaving hide place")]
	public Transform outPlace;

	[Tooltip("The position the enemy moves to when he wants to get the player out of hide place")]
	public Transform enemyPosition;

	[Tooltip("Set enemy Animator int value")]
	public int enemyAnimationState;

	[Tooltip("The name of the animation that will be played when the enemy removes the player from hide place")]
	public string cameraAnimationName;

	[Tooltip("Player camera rotation X limit (-10, 10)")]
	public Vector2 clampCameraX;

	[Tooltip("Player camera rotation Y limit (-10, 10) ")]
	public Vector2 clampCameraY;

	[Tooltip("Player hiding sound")]
	public AudioClip hideSound;

	[Tooltip("Player leave hide place sound")]
	public AudioClip unhideSound;

	[Tooltip("Animation for hide place object (for example, chest cap open/close)")]
	public Animation hidePlaceAnimation;

	[Tooltip("Animation hide name")]
	public string hidePlaceHideAnimationName;

	[Tooltip("Animation unhide name")]
	public string hidePlaceUnhideAnimationName;

	private PlayerController player;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	public void Interact()
	{
		player.hidePlace = this;
		player.Hide(1);
		player.transform.position = enterPlace.position;
		player.transform.rotation = enterPlace.rotation;
		float x = enterPlace.eulerAngles.y + clampCameraX.x;
		float y = enterPlace.eulerAngles.y + clampCameraX.y;
		player.clampByY = true;
		player.clampYaxis.x = x;
		player.clampYaxis.y = y;
		player.clampXaxis.x = clampCameraY.x;
		player.clampXaxis.y = clampCameraY.y;
		if ((bool)hidePlaceAnimation)
		{
			hidePlaceAnimation.Play(hidePlaceHideAnimationName);
		}
		if ((bool)hideSound)
		{
			AudioSource.PlayClipAtPoint(hideSound, base.transform.position);
		}
	}

	public void ExitHidePlace()
	{
		player.transform.position = outPlace.position;
		player.transform.rotation = outPlace.rotation;
		float y = enterPlace.eulerAngles.y;
		float y2 = enterPlace.eulerAngles.y;
		player.clampYaxis.x = y;
		player.clampYaxis.y = y2;
		player.clampXaxis.x = 0f;
		player.clampXaxis.y = 0f;
		player.clampXaxis.x = -90f;
		player.clampXaxis.y = 90f;
		player.clampByY = false;
		player.hidePlace = null;
		player.Hide(0);
		if ((bool)hidePlaceAnimation)
		{
			hidePlaceAnimation.Play(hidePlaceUnhideAnimationName);
		}
		if ((bool)unhideSound)
		{
			AudioSource.PlayClipAtPoint(unhideSound, base.transform.position);
		}
	}
}
