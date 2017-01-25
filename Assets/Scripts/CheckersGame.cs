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

	public static GameObject CheckersPieceToDie;

	public static Player CurrentPlayer
	{
		get
		{
			return players.Count > 0 ? players.Peek() : null;
		}
	}

	public static bool MovingPiece { get; private set; }

	public static GameObject ChessBoardGameObject { get; private set; }

	public static ChessBoard ChessBoard { get; private set; }

	void Start()
	{
		LoadGame();

		// UI
		string resetButtonPrefabPath = Utils.Path.Combine("Prefabs", "ResetGameButton");
		GameObject resetButton = Instantiate(Resources.Load(resetButtonPrefabPath)) as GameObject;
		resetButton.transform.position = new Vector3(-4.25f, 0.5f, -2.5f);
		resetButton.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
		resetButton.transform.localScale *= 1.5f;
	}

	public static void MovePieceTo(Vector3 position)
	{
		// Using this "newPos" because the position passed in has a lower Y value than the checkers pieces.
		Vector3 newPos = new Vector3(position.x, CurrentPlayer.SelectedPiece.transform.position.y, position.z);
		if (newPos != CurrentPlayer.SelectedPiece.transform.position)
		{
			MovingPiece = true;
			if (CheckersPieceToDie = ChessBoard.MovePieceTo(new Vector2(CurrentPlayer.SelectedPiece.position.x, CurrentPlayer.SelectedPiece.position.z), new Vector2(newPos.x, newPos.z)))
			{
				CurrentPlayer.SelectedPiece.SendMessage("MoveToAndTake", newPos);
			}
			    else
			{
				CurrentPlayer.SelectedPiece.SendMessage("MoveTo", newPos);
			}
		}
	}

	public static void MoveComplete()
	{
		MovingPiece = false;
		if (CheckersPieceToDie)
		{
			CheckersPieceToDie.SendMessage("Die");
			CurrentPlayer.SelectedPiece = CurrentPlayer.SelectedPiece;
			if (!ChessBoard.SelectedPieceCanMove)
			{
				PassPlayerTurn(true);
			}
		}
		else
		{
			PassPlayerTurn();
		}
	}

	public static void PassPlayerTurn(bool force = false)
	{
		if (force)
		{
			CurrentPlayer.SelectedPiece = null;
			CheckersPieceToDie = null;
				
		}
		players.Enqueue(players.Dequeue());
	}

	public static void ResetGame()
	{
		UnloadGame();
		LoadGame();
	}

	static void LoadGame()
	{
		string chessBoardPrefabPath = Utils.Path.Combine("Prefabs", "ChessBoard");
		ChessBoardGameObject = Instantiate(Resources.Load(chessBoardPrefabPath)) as GameObject;
		ChessBoard = ChessBoardGameObject.GetComponent<ChessBoard>();

		players = new Queue<Player>();
		players.Enqueue(new Player(CheckersTeam.RED, DirectionOfMovement.FORWARD));
		players.Enqueue(new Player(CheckersTeam.BLUE, DirectionOfMovement.BACKWARD));

		CheckersPieceToDie = null;

		MovingPiece = false;
	}

	static void UnloadGame()
	{
		ChessBoard.Cleanup();
		Destroy(ChessBoardGameObject);
		ChessBoardGameObject = null;
		ChessBoard = null;
		players.Clear();
		players = null;
		CheckersPieceToDie = null;
		MovingPiece = false;
	}
}
