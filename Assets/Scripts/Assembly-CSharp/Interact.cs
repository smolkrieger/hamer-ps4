using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Interact : MonoBehaviour
{
	[Header("Interact Settings")]
	[Tooltip("Distance of ray to interact")]
	public float rayDistance;

	[Tooltip("Layers to interact (default as obstacle)")]
	public LayerMask interactLayers;

	[Tooltip("Tags for interact")]
	public string interactTag;

	[Tooltip("Inventory script")]
	public Inventory inventory;

	[Header("UI Settings")]
	[Tooltip("UI interactButton for mobile only")]
	public Image interactButton;

	private PlayerController player;

	private void Awake()
	{
		player = inventory.gameObject.GetComponent<PlayerController>();
	}

	private void Update()
	{
		if (RayCastCheck() != null)
		{
			interactButton.enabled = true;
			if (!CrossPlatformInputManager.GetButtonDown("Interact"))
			{
				return;
			}
			if ((bool)RayCastCheck().GetComponent<InteractCallEvent>())
			{
				RayCastCheck().GetComponent<InteractCallEvent>().InteractCall();
			}
			else if ((bool)RayCastCheck().GetComponent<Item>())
			{
				AudioSource.PlayClipAtPoint(RayCastCheck().GetComponent<Item>().pickupSound, base.transform.position);
				inventory.AddItem(RayCastCheck().GetComponent<Item>().itemID, RayCastCheck());
			}
			else if ((bool)RayCastCheck().GetComponent<Lock>())
			{
				if (inventory.CurrentItemID == RayCastCheck().GetComponent<Lock>().needItem && !RayCastCheck().GetComponent<Lock>().isOpen)
				{
					if (RayCastCheck().GetComponent<Lock>().removeAfterOpen)
					{
						inventory.RemoveItem();
					}
					if (RayCastCheck().GetComponent<Lock>().playItemAnim)
					{
						player.inventory.PlayItemAnim(RayCastCheck().GetComponent<Lock>().itemAnimation);
					}
					RayCastCheck().GetComponent<Lock>().UnlockLock();
				}
			}
			else if ((bool)RayCastCheck().GetComponent<DoorSiders>())
			{
				if (!RayCastCheck().GetComponent<DoorSiders>().genDoor.locked)
				{
					RayCastCheck().GetComponent<DoorSiders>().InteractWithDoor();
				}
				else if (inventory.CurrentItemID == RayCastCheck().GetComponent<DoorSiders>().genDoor.keyID)
				{
					inventory.RemoveItem();
					RayCastCheck().GetComponent<DoorSiders>().genDoor.UnlockDoor();
				}
				else
				{
					RayCastCheck().GetComponent<DoorSiders>().InteractWithDoor();
				}
			}
		}
		else
		{
			interactButton.enabled = false;
		}
	}

	private GameObject RayCastCheck()
	{
		Vector3 direction = base.transform.TransformDirection(Vector3.forward);
		if (Physics.Raycast(base.transform.position, direction, out var hitInfo, rayDistance, interactLayers) && hitInfo.transform.gameObject.tag == interactTag)
		{
			return hitInfo.transform.gameObject;
		}
		return null;
	}
}
