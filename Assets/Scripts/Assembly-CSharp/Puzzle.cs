using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
	[Header("Puzzle Settings")]
	[Tooltip("Empty block gameobject")]
	public PuzzleBlock emptyPuzzle;

	[Tooltip("All puzzle blocks")]
	public PuzzleBlock[] blocks;

	[Tooltip("Event that will occur after solving the puzzle")]
	public UnityEvent SolvedEvent;

	[Tooltip("Sets all puzzles to a random position at the start of the game.")]
	public bool randomize;

	[HideInInspector]
	public bool activated;

	public void Start()
	{
		if (randomize && !CheckBlocks())
		{
			for (int i = 0; i < 500; i++)
			{
				int num = Random.Range(0, blocks.Length);
				if ((double)(blocks[num].transform.localPosition - emptyPuzzle.transform.localPosition).sqrMagnitude < 0.1)
				{
					Vector3 position = blocks[num].transform.position;
					int iD = blocks[num].ID;
					blocks[num].ID = emptyPuzzle.ID;
					emptyPuzzle.ID = iD;
					blocks[num].transform.position = emptyPuzzle.transform.position;
					emptyPuzzle.transform.position = position;
				}
			}
			if (CheckBlocks())
			{
				SolvedEvent.Invoke();
				activated = true;
			}
		}
		else if (CheckBlocks())
		{
			SolvedEvent.Invoke();
			activated = true;
		}
	}

	public void MovePuzzle(PuzzleBlock block)
	{
		if ((double)(block.transform.localPosition - emptyPuzzle.transform.localPosition).sqrMagnitude < 0.1 && !activated)
		{
			Vector3 position = block.transform.position;
			int iD = block.ID;
			block.ID = emptyPuzzle.ID;
			emptyPuzzle.ID = iD;
			block.transform.position = emptyPuzzle.transform.position;
			emptyPuzzle.transform.position = position;
		}
		if (CheckBlocks() && !activated)
		{
			SolvedEvent.Invoke();
			activated = true;
		}
	}

	private bool CheckBlocks()
	{
		for (int i = 0; i < blocks.Length; i++)
		{
			if (blocks[i].ID != i)
			{
				return false;
			}
		}
		return true;
	}

	public void LoadState()
	{
		if (activated)
		{
			SolvedEvent.Invoke();
		}
	}
}
