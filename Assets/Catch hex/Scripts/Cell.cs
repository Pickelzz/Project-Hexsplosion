using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum State
{
	wall,
	empty,
	bot,
}

public class Cell : MonoBehaviour 
{
	public State state;

	public bool gameOver;
	public bool forSpawn;

	public bool checkedCell;

	public List<Cell> cells;

	public Color botColor;
	public Color wallColor;
	public Color emptyColor;

	public int weight;
	[HideInInspector]
	public SpriteRenderer sr;

	private AudioSource _openSound;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer> ();
		_openSound = GetComponent<AudioSource> ();
	}

	void OnMouseDown()
	{
		if (state == State.empty&&GameController.inGame) 
		{
			_openSound.Play ();
			SetState (true, State.wall);
			GameController.Instance.NextStep ();
			GameController.Instance.score++;
			UICotroller.Instance.UpdateScore (GameController.Instance.score);
		}
	}

	void OnDrawGizmosSelected() 
	{
		if (cells == null)
			return;
		Gizmos.color = Color.blue;
		for(var i=0;i<cells.Count;i++)
			Gizmos.DrawLine(transform.position, cells[i].transform.position);
		
	}


	public void SetState (bool activate, State newState)
	{
		StartCoroutine (_setState (activate, newState));
	}

	private IEnumerator _setState(bool activate, State newState)
	{
		float timer = 0f;

		Color stateColor = (newState == State.bot) ? botColor : wallColor;

		state = (activate) ? newState : State.empty;

		do
		{
			timer += 0.05f;
			if (activate)
				sr.color = Color.Lerp (emptyColor, stateColor, timer);
			else 
				sr.color = Color.Lerp (stateColor, emptyColor, timer);
			
			yield return new WaitForSeconds (0.001f);
		}while (timer <= 1);
	}


	public void OpenArea()
	{
		checkedCell = true;

		for (int i = 0; i < cells.Count; i++) 
		{
			if (cells [i].state == State.empty && !cells [i].checkedCell) {				
				cells [i].OpenArea ();
			}
		}
	}


}