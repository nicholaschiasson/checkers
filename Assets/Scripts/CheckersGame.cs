using UnityEngine;
using System.Collections.Generic;

public enum CheckersTeam
{
	NONE,
	BLUE,
	RED
}

public enum DirectionOfMovement
{
	FORWARD,
	BACKWARD
}

public class CheckersGame : MonoBehaviour
{
	static Queue<Player> players;

	public static Player CurrentPlayer
	{
		get
		{
			return players.Count > 0 ? players.Peek() : null;
		}
	}

	public static bool MovingPiece { get; private set; }

	public static GameObject ChessBoard { get; private set; }

	void Start()
	{
		players = new Queue<Player>();
		players.Enqueue(new Player(CheckersTeam.RED, DirectionOfMovement.FORWARD));
		players.Enqueue(new Player(CheckersTeam.BLUE, DirectionOfMovement.BACKWARD));

		MovingPiece = false;

		string chessBoardPrefabPath = Utils.Path.Combine("Prefabs", "ChessBoard");
		ChessBoard = Instantiate(Resources.Load(chessBoardPrefabPath)) as GameObject;
	}

	public static void MovePieceTo(Vector3 position)
	{
		// Using this "newPos" because the position passed in has a lower Y value than the checkers pieces.
		Vector3 newPos = new Vector3(position.x, CurrentPlayer.SelectedPiece.transform.position.y, position.z);
		if (newPos != CurrentPlayer.SelectedPiece.transform.position)
		{
			MovingPiece = true;
			//ChessBoard.SendMessage("MovePieceTo", new Vector2(newPos.x, newPos.z));
			CurrentPlayer.SelectedPiece.SendMessage("MoveTo", newPos);
		}
	}

	public static void MoveComplete()
	{
		MovingPiece = false;
		players.Enqueue(players.Dequeue());
	}
}
