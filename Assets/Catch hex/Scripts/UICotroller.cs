using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICotroller : MonoBehaviour 
{
	public static UICotroller Instance;

	public GameObject mainMenuPanel;
	public GameObject gamePanel;
	public GameObject gameOverPanel;

	public Text gameScore;

	public Text gameOverScore;
	public Text gameOverBestScore;

	private AudioSource _buttonSound;

	void Awake()
	{
		Instance = this;
		_buttonSound = GetComponent<AudioSource> ();
	}

	void Start()
	{
		ActivateMainMenuPanel ();
	}

	public void ActivateMainMenuPanel()
	{
		mainMenuPanel.SetActive (true);
		gamePanel.SetActive (false);
		gameOverPanel.SetActive(false);
	}

	public void ActivateGamePanel()
	{
		_buttonSound.Play ();
		mainMenuPanel.SetActive (false);
		gamePanel.SetActive (true);
		gameOverPanel.SetActive(false);
	}

	public void ActivateGameOverPanel(bool won, int score,int bestScore)
	{
		mainMenuPanel.SetActive (false);
		gamePanel.SetActive (true);
		gameOverPanel.SetActive(true);

		gameOverScore.gameObject.SetActive (won);

		gameOverScore.text = "Score: " + score;
		gameOverBestScore.text = "Best: " + bestScore;
	}

	public void UpdateScore(int score)
	{
		gameScore.text = "Score: " + score;
	}
}