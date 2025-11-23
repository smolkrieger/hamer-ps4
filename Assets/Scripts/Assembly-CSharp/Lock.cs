using UnityEngine;

public class Lock : MonoBehaviour
{
	[Tooltip("Door that the lock block")]
	public Door door;

	[Tooltip("Needed item - needed key or other item like hammer for open this lock")]
	public int needItem;

	[Tooltip("If this value = true, then the item is deleted after use (for example for keys must be true, for hammer this value must be false)")]
	public bool removeAfterOpen;

	[Tooltip("This value need for save lock state")]
	public bool isOpen;

	private Animation lockAnimation;

	[Tooltip("Animation for lock")]
	public string unlockAnim;

	[Tooltip("If true, player item will play animation (hammer hit for example)")]
	public bool playItemAnim;

	[Tooltip("Animation for item (for example, hammer hit)")]
	public string itemAnimation;

	[Tooltip("Unlock sound")]
	public AudioClip unlockSound;

	private void Awake()
	{
		if ((bool)GetComponent<Animation>())
		{
			lockAnimation = GetComponent<Animation>();
		}
	}

	public void UnlockLock()
	{
		if ((bool)door)
		{
			door.locksCount--;
			door.UpdateState();
		}
		isOpen = true;
		AudioSource.PlayClipAtPoint(unlockSound, base.transform.position);
		UnlockEvent();
	}

	private void UnlockEvent()
	{
		GetComponent<Rigidbody>().isKinematic = false;
		if ((bool)lockAnimation)
		{
			lockAnimation.Play(unlockAnim);
		}
	}

	public void LoadState()
	{
		if (isOpen)
		{
			UnlockEvent();
		}
	}
}
