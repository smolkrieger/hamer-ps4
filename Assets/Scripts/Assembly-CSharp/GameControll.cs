using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameControll : MonoBehaviour
{
	[Header("General parameters")]
	[Tooltip("count of player life. If life count = 0, game over")]
	public int lifeCount;

	[Tooltip("Player controller script here")]
	public PlayerController player;

	[Tooltip("Player inventory script")]
	public Inventory inventory;

	[Tooltip("Enemy gameobject")]
	public Enemy enemy;

	[Tooltip("Player spawn point")]
	public Transform playerSpawnPoint;

	[Tooltip("Enemy spawn point")]
	public Transform enemySpawnPoint;

	[Tooltip("Screen fade gameobject")]
	public AudioSource dangerAmbient;

	private bool pause;

	[Tooltip("Hide mouse cursor")]
	public bool hideCursor;

	[Header("UI Settings")]
	public Animation fadeScreen;

	public Animation bloodScreen;

	public Animation lifesCountScreen;

	public string bloodPulsingAnimName;

	public string bloodFadeAnimName;

	public string fadeOutAnimName;

	public string fadeInAnimName;

	public string fadeDieAnimName;

	public string fadeHideDieAnimName;

	public GameObject gameControllPanel;

	public GameObject mobileControllPanel;

	public GameObject pausePanel;

	public GameObject pauseCursor;

	public GameObject gameOverPanel;

	public GameObject gameWinPanel;

	public Slider volumeSlider;

	public Slider sensitivitySlider;

	public Image dropImage;

	public Image standImage;

	public Image crouchImage;

	public Image hidePlaceExitImage;

	public Image interactImage;

	private bool InOtherMenu;

	private void Start()
	{
		if (hideCursor)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		InOtherMenu = false;
		Time.timeScale = 1f;
		player.locked = true;
		enemy.gameObject.SetActive(value: false);
		pausePanel.SetActive(value: false);
		pauseCursor.SetActive(value: false);
		fadeScreen.Play("Fade");
		lifesCountScreen.gameObject.SetActive(value: true);
		lifesCountScreen.Play("LifesCount");
		lifesCountScreen.transform.GetChild(0).GetComponent<Text>().text = lifeCount.ToString();
		StartCoroutine(WaitRestart());
		if (PlayerPrefs.HasKey("Volume"))
		{
			AudioListener.volume = PlayerPrefs.GetFloat("Volume");
			volumeSlider.value = PlayerPrefs.GetFloat("Volume");
		}
		else
		{
			AudioListener.volume = 1f;
			volumeSlider.value = 1f;
		}
		if (PlayerPrefs.HasKey("Sensitivity"))
		{
			player.mouseSensetivity = PlayerPrefs.GetFloat("Sensitivity");
			sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
		}
		else
		{
			player.mouseSensetivity = 50f;
			sensitivitySlider.value = 50f;
		}
	}

	private void Update()
	{
		AmbientChange();
		ControllGame();
	}

	private void ControllGame()
	{
		if ((CrossPlatformInputManager.GetButtonDown("Pause") || Input.GetButtonDown("Options")) && !InOtherMenu)
		{
			PauseGame();
		}
	}

	public void PauseGame()
	{
		if (!pause)
		{
			pause = true;
			pausePanel.SetActive(pause);
			pauseCursor.SetActive(pause);
			Time.timeScale = 0f;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			pause = false;
			pausePanel.SetActive(pause);
			pauseCursor.SetActive(pause);
			Time.timeScale = 1f;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void GameOver()
	{
		PlayerPrefs.SetInt("HasSaveGame", 0);
		gameOverPanel.SetActive(value: true);
		InOtherMenu = true;
	}

	public void GameWin()
	{
		gameControllPanel.SetActive(value: false);
		gameWinPanel.SetActive(value: true);
		player.gameObject.SetActive(value: false);
		enemy.gameObject.SetActive(value: false);
		PlayerPrefs.SetInt("HasSaveGame", 0);
		InOtherMenu = true;
	}

	public void MainMenuExit(int saveGame)
	{
		if (saveGame == 0)
		{
			PlayerPrefs.SetInt("HasSaveGame", 0);
			PlayerPrefs.SetString("SceneName", "MainMenu");
			SceneManager.LoadScene("LoadScene");
		}
		if (saveGame == 1)
		{
			PlayerPrefs.SetInt("HasSaveGame", 1);
			PlayerPrefs.SetString("SceneName", "MainMenu");
			SceneManager.LoadScene("LoadScene");
		}
	}

	private void AmbientChange()
	{
		if (enemy.seePlayer)
		{
			if (dangerAmbient.volume < 1f)
			{
				dangerAmbient.volume += Time.deltaTime / 2f;
			}
		}
		else if (dangerAmbient.volume > 0f)
		{
			dangerAmbient.volume -= Time.deltaTime / 8f;
		}
	}

	public void Respawn()
	{
		enemy.gameObject.SetActive(value: true);
		player.clampXaxis.x = 0f;
		player.clampXaxis.y = 0f;
		player.clampXaxis.x = -90f;
		player.clampXaxis.y = 90f;
		player.clampByY = false;
		player.hidePlace = null;
		player.transform.position = playerSpawnPoint.position;
		player.transform.rotation = playerSpawnPoint.rotation;
		enemy.transform.position = enemySpawnPoint.position;
		enemy.transform.rotation = enemySpawnPoint.rotation;
		player.locked = false;
		player.lockedMovement = false;
		player.cameraTransform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		player.cameraAnimation.Play(player.cameraIdleAnimName);
		enemy.RestartEnemyStats();
		ScreenFade(0);
	}

	public void ScreenFade(int state)
	{
		if (state == 0)
		{
			fadeScreen.Play(fadeOutAnimName);
		}
		if (state == 1)
		{
			fadeScreen.Play(fadeInAnimName);
		}
		if (state == 2)
		{
			StartCoroutine(WaitKillAnim(3f));
		}
		if (state == 3)
		{
			StartCoroutine(WaitKillAnim(4f));
		}
	}

	public void ScreenBlood(int state)
	{
		if (state == 0)
		{
			bloodScreen.Play(bloodFadeAnimName);
		}
		if (state == 1)
		{
			bloodScreen.Play(bloodPulsingAnimName);
		}
	}

	private IEnumerator WaitKillAnim(float killTime)
	{
		yield return new WaitForSeconds(killTime);
		fadeScreen.Play(fadeInAnimName);
		StartCoroutine(WaitFadeAnim(fadeInAnimName));
	}

	private IEnumerator WaitFadeAnim(string name)
	{
		yield return new WaitForSeconds(fadeScreen[name].length);
		if (lifeCount > 1)
		{
			lifeCount--;
			lifesCountScreen.gameObject.SetActive(value: true);
			lifesCountScreen.Play("LifesCount");
			lifesCountScreen.transform.GetChild(0).GetComponent<Text>().text = lifeCount.ToString();
			StartCoroutine(WaitRestart());
		}
		else
		{
			GameOver();
		}
	}

	private IEnumerator WaitRestart()
	{
		yield return new WaitForSeconds(3f);
		lifesCountScreen.gameObject.SetActive(value: false);
		Respawn();
	}

	public void ConfigureApply()
	{
		PlayerPrefs.SetFloat("Volume", volumeSlider.value);
		PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
		AudioListener.volume = PlayerPrefs.GetFloat("Volume");
		player.mouseSensetivity = PlayerPrefs.GetFloat("Sensitivity");
	}
}
