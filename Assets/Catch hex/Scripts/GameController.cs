using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour 
{
	public static GameController Instance;
	public static bool inGame;

	public int score;

	public List<Cell> cells;

	void Awake()
	{
		Instance = this;
	}

	public void StartGame()
	{
		inGame = true;
		score = 0;
		UICotroller.Instance.UpdateScore (0);
		cells.FindAll (pred => pred.forSpawn) [Random.Range (0, cells.FindAll (pred => pred.forSpawn).Count)].SetState (true, State.bot);

		for (var i = 0; i < 8; i++) 
		{
			cells.FindAll (pred => pred.state == State.empty&& !pred.gameOver) [Random.Range (0, cells.FindAll (pred => pred.state == State.empty && !pred.gameOver).Count)].SetState (true, State.wall);
		}
	}

	public void Restart()
	{
		for (var i = 0; i < cells.Count; i++)
			if(!cells[i].gameOver)
				cells [i].SetState (false, State.wall);
		StartGame ();
	}

	public void NextStep()
	{
		var bot = cells.Find (pred => pred.state == State.bot);

		for (var i = 0; i < cells.Count; i++) 
		{			
			cells [i].checkedCell = false;
		}

		bot.OpenArea ();

		if (cells.FindAll (pred => pred.checkedCell && pred.gameOver).Count == 0) 
		{
			if (bot.cells.FindAll (pred => pred.state == State.empty).Count != 0) 
			{
				bot.SetState (false, State.bot);
				var path=bot.cells.FindAll (pred => pred.state == State.empty);
				path[Random.Range(0,path.Count)].SetState (true, State.bot);
			}
			else
				GameOver (true);
		} 
		else 
		{
			var paths=new List<List<Cell>>();
			var edges = cells.FindAll (pred => pred.checkedCell && pred.gameOver&&pred.state==State.empty);
			for(var i=0;i<edges.Count;i++)
				paths.Add(GetPath(cells,bot,edges[i]));
			
			var minimalPath = paths [0].Count;
			var minmalIndex = 0;

			for (var i = 1; i < paths.Count; i++) 
			{
				if (paths [i].Count < minimalPath) {
					minimalPath = paths [i].Count;
					minmalIndex = i;
				}

			}
			paths [minmalIndex] [paths [minmalIndex].Count - 1].SetState (false, State.bot);
			paths [minmalIndex].RemoveAt (paths [minmalIndex].Count - 1);

			if (paths [minmalIndex] [paths [minmalIndex].Count - 1].gameOver) 
			{
				score++;
				GameOver (false);
			}
			else
				paths [minmalIndex] [paths [minmalIndex].Count - 1].SetState (true, State.bot);
		}
	}

	void GameOver(bool won)
	{
		inGame = false;

		if (won && (score < PlayerPrefs.GetInt ("Best")||PlayerPrefs.GetInt ("Best")==0))
			PlayerPrefs.SetInt ("Best", score);

		UICotroller.Instance.ActivateGameOverPanel (won, score, PlayerPrefs.GetInt ("Best"));
	}

	public List<Cell> GetPath(List<Cell> field, Cell bot,Cell finish)
	{
		var tempList = new List<Cell> ();
		for (var i = 0; i < cells.Count; i++) {
			cells [i].weight = -1;
		}


		var maximalWeight = 0;
		bot.weight = maximalWeight;

		do {
			for (var i = 0; i < field.Count; i++)
				if (field [i].state != State.wall && field [i].weight == maximalWeight)
					for (var j = 0; j < field [i].cells.Count; j++)
						if (field [i].cells [j].weight == -1 && field [i].cells [j].state == State.empty)
							field [i].cells [j].weight = maximalWeight + 1;
			maximalWeight++;
		} while(finish.weight == -1);


		tempList.Add (finish);

		do {
			maximalWeight--;
			
			finish = finish.cells.Find (pred => pred.weight == maximalWeight); //System.Array.Find (finish.cells, pred => pred.weight == maximalWeight);
			tempList.Add (finish);
		} while(finish != bot);


		return tempList;
	}
}
