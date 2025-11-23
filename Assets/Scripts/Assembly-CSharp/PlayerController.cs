using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
	[Header("GeneralSettings")]
	[Tooltip("Object with GameControll.cs script")]
	public GameControll gameControll;

	[HideInInspector]
	public Inventory inventory;

	[Header("PlayerSettings")]
	[Tooltip("Player walk speed")]
	public float walkSpeed;

	[Tooltip("Player crouch speed")]
	public float crouchSpeed;

	[HideInInspector]
	public bool locked;

	[HideInInspector]
	public bool lockedMovement;

	[HideInInspector]
	public bool canBeCatchen;

	private CharacterController characterController;

	private float moveSpeed;

	[HideInInspector]
	public bool playerMoving;

	[Header("CameraSettings")]
	[Tooltip("Mouse Sensetivity value")]
	public float mouseSensetivity;

	[Tooltip("Main camera transform")]
	public Transform cameraTransform;

	private float clampX;

	private float clampY;

	[Header("Camera Animations")]
	[Tooltip("Camera animation gameobject")]
	public Animation cameraAnimation;

	[Tooltip("Camera hit animation name")]
	public string cameraHitAnimName;

	[Tooltip("Camera idle animation name")]
	public string cameraIdleAnimName;

	[Tooltip("Camera move animation name")]
	public string cameraMoveAnimName;

	[Header("CrouchSettings")]
	private float lerpSpeed = 10f;

	[Tooltip("Player character controller normal height")]
	public float normalHeight;

	[Tooltip("Player character controller crouch height")]
	public float crouchHeight;

	[Tooltip("Player camera normal offset")]
	public float cameraNormalOffset;

	[Tooltip("Player camera crouch offset")]
	public float cameraCrouchOffset;

	[Tooltip("Player obstacle layers")]
	public LayerMask obstacleLayers;

	[Tooltip("Clamp camera by Y axis")]
	public bool clampByY;

	public Vector2 clampXaxis;

	public Vector2 clampYaxis;

	[Tooltip("Time takes to repair broken legs")]
	public float legsFixTime;

	[HideInInspector]
	public bool crouch = false;

	[Header("UI Settigns")]
	[Tooltip("UI stand icon for mobile only")]
	public Image imageStand;

	[Tooltip("UI crouch icon for mobile only")]
	public Image imageCrouch;

	[Tooltip("UI crouch icon for mobile only")]
	public Image imageExitHidePlace;

	[Header("Hide Place Settings")]
	public HidePlace hidePlace;

	[Header("Sounds Settings")]
	private AudioSource AS;

	[Tooltip("Foot steps sounds")]
	public AudioClip[] footSteps;

	[Tooltip("Sound of breaking legs")]
	public AudioClip legBreakSound;

	private bool legBreak;

	private void Awake()
	{
		AS = GetComponent<AudioSource>();
		inventory = GetComponent<Inventory>();
		characterController = GetComponent<CharacterController>();
		clampX = 0f;
		moveSpeed = walkSpeed;
		imageStand.enabled = true;
		imageCrouch.enabled = false;
		imageExitHidePlace.enabled = false;
	}

	private void Update()
	{
		canBeCatchen = characterController.isGrounded;
		if (!locked)
		{
			CameraRotation();
			HidePlaceExit();
			if (!lockedMovement)
			{
				Movement();
				Controll();
			}
		}
	}

	private void Controll()
	{
		if (!characterController.isGrounded && characterController.velocity.y <= -7f && !legBreak)
		{
			legBreak = true;
		}
		else if (characterController.isGrounded && legBreak)
		{
			legBreak = false;
			PlayerLegsBreak();
		}
		float b = (crouch ? crouchHeight : normalHeight);
		characterController.height = Mathf.Lerp(characterController.height, b, Time.deltaTime * lerpSpeed);
		characterController.center = Vector3.down * (normalHeight - characterController.height) / 2f;
		float y = (crouch ? cameraCrouchOffset : cameraNormalOffset);
		Vector3 b2 = new Vector3(cameraTransform.localPosition.x, y, cameraTransform.localPosition.z);
		cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, b2, Time.deltaTime * lerpSpeed);
		if (characterController.isGrounded && (CrossPlatformInputManager.GetButtonDown("Crouch") || Input.GetButtonDown("Circle")))
		{
			SetCrouch();
		}
		if (CrossPlatformInputManager.GetButtonDown("Drop") || Input.GetButtonDown("Triangle"))
		{
			gameControll.inventory.DropItem();
		}
	}

	private void PlayerLegsBreak()
	{
		gameControll.ScreenBlood(1);
		lockedMovement = true;
		crouch = true;
		moveSpeed = crouchSpeed;
		imageStand.enabled = false;
		imageCrouch.enabled = true;
		characterController.height = crouchHeight;
		cameraTransform.localPosition = new Vector3(0f, cameraCrouchOffset, 0f);
		AS.PlayOneShot(legBreakSound);
		StartCoroutine(WaitLegsFix());
	}

	public void Hide(int state)
	{
		if (state == 1)
		{
			if (gameControll.enemy.seePlayer)
			{
				gameControll.enemy.SendHidePlace();
			}
			StopAllCoroutines();
			imageExitHidePlace.enabled = true;
			lockedMovement = true;
			crouch = true;
			characterController.height = crouchHeight;
			cameraTransform.localPosition = new Vector3(0f, cameraCrouchOffset, 0f);
		}
		else
		{
			imageExitHidePlace.enabled = false;
			lockedMovement = false;
			crouch = false;
			moveSpeed = walkSpeed;
			characterController.height = normalHeight;
			cameraTransform.localPosition = new Vector3(0f, cameraNormalOffset, 0f);
			imageStand.enabled = true;
			imageCrouch.enabled = false;
		}
	}

	public void CatchPlayer(int state, string camHitName)
	{
		if (state == 1)
		{
			StopAllCoroutines();
			gameControll.inventory.DropItem();
			locked = true;
			characterController.height = normalHeight;
			cameraTransform.localPosition = new Vector3(0f, cameraNormalOffset, 0f);
			crouch = false;
			moveSpeed = walkSpeed;
			imageStand.enabled = true;
			imageCrouch.enabled = false;
		}
		if (state == 2)
		{
			cameraAnimation.Play(cameraHitAnimName);
			gameControll.ScreenFade(2);
		}
		if (state == 3)
		{
			cameraAnimation.Play(camHitName);
			gameControll.inventory.DropItem();
			locked = true;
			crouch = false;
			moveSpeed = walkSpeed;
			imageStand.enabled = true;
			imageCrouch.enabled = false;
			gameControll.ScreenFade(3);
		}
		if (state == 4)
		{
			gameControll.ScreenBlood(0);
		}
	}

	private void Movement()
	{
		float num = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed;
		float num2 = CrossPlatformInputManager.GetAxis("Vertical") * moveSpeed;
		float num3 = Input.GetAxis("LeftStickY") * moveSpeed;
		float num4 = Input.GetAxis("LeftStickX") * moveSpeed;
		Vector3 vector = base.transform.forward * (num2 + num4);
		Vector3 vector2 = base.transform.right * (num + num3);
		characterController.SimpleMove(vector + vector2);
		if (characterController.velocity.magnitude > 0.5f)
		{
			playerMoving = true;
			cameraAnimation.Play(cameraMoveAnimName);
			cameraAnimation[cameraMoveAnimName].speed = moveSpeed / 4f;
		}
		else
		{
			playerMoving = false;
			cameraAnimation.Play(cameraIdleAnimName);
		}
	}

	private void CameraRotation()
	{
		float num = CrossPlatformInputManager.GetAxis("Mouse X") * (mouseSensetivity * 2f) * Time.deltaTime;
		float num2 = CrossPlatformInputManager.GetAxis("Mouse Y") * (mouseSensetivity * 2f) * Time.deltaTime;
		float num3 = Input.GetAxis("RightStickX") * (mouseSensetivity * 2f) * Time.deltaTime;
		float num4 = (Input.GetAxis("RightStickY") * -1) * (mouseSensetivity * 2f) * Time.deltaTime;
		clampX += (num2 + num4);
		clampY += (num + num3);
		if (clampX > clampXaxis.y)
		{
			clampX = clampXaxis.y;
			num2 = 0f;
			num4 = 0f;
			ClampXAxis(clampXaxis.x);
		}
		else if (clampX < clampXaxis.x)
		{
			clampX = clampXaxis.x;
			num2 = 0f;
			num4 = 0f;
			ClampXAxis(clampXaxis.y);
		}
		if (clampByY)
		{
			if (clampY > clampYaxis.y)
			{
				clampY = clampYaxis.y;
				num = 0f;
				num3 = 0f;
				ClampYAxis(clampYaxis.y);
			}
			else if (clampY < clampYaxis.x)
			{
				clampY = clampYaxis.x;
				num = 0f;
				num3 = 0f;
				ClampYAxis(clampYaxis.x);
			}
		}
		cameraTransform.Rotate(Vector3.left * (num2 + num4));
		base.transform.Rotate(Vector3.up * (num + num3));
	}

	private void ClampXAxis(float value)
	{
		Vector3 eulerAngles = cameraTransform.eulerAngles;
		eulerAngles.x = value;
		cameraTransform.eulerAngles = eulerAngles;
	}

	private void ClampYAxis(float value)
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.y = value;
		base.transform.eulerAngles = eulerAngles;
	}

	private void SetCrouch()
	{
		if (!crouch)
		{
			crouch = true;
			moveSpeed = crouchSpeed;
			imageStand.enabled = false;
			imageCrouch.enabled = true;
		}
		else if (CheckDistance() > normalHeight)
		{
			crouch = false;
			moveSpeed = walkSpeed;
			imageStand.enabled = true;
			imageCrouch.enabled = false;
		}
	}

	private float CheckDistance()
	{
		Vector3 origin = base.transform.position + characterController.center - new Vector3(0f, characterController.height / 2f, 0f);
		if (Physics.SphereCast(origin, characterController.radius, base.transform.up, out var hitInfo, 10f, obstacleLayers))
		{
			return hitInfo.distance;
		}
		return 3f;
	}

	private void HidePlaceExit()
	{
		if ((bool)hidePlace && (CrossPlatformInputManager.GetButtonDown("Unhide") || Input.GetButtonDown("Square")))
		{
			hidePlace.ExitHidePlace();
		}
	}

	public void FootStepPlay()
	{
		int num = Random.Range(1, footSteps.Length);
		AS.volume = moveSpeed / 6f;
		AS.PlayOneShot(footSteps[num]);
	}

	private IEnumerator WaitLegsFix()
	{
		yield return new WaitForSeconds(legsFixTime);
		lockedMovement = false;
		gameControll.ScreenBlood(0);
	}
}
