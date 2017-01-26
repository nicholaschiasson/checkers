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

public static class CheckersTeamExtension
{
	public static CheckersTeam Opponent(this CheckersTeam team)
	{
		switch (team)
		{
			case CheckersTeam.BLUE:
				return CheckersTeam.RED;
			case CheckersTeam.RED:
				return CheckersTeam.BLUE;
			default:
				break;
		}
		return CheckersTeam.NONE;
	}
}

public class CheckersGame : MonoBehaviour
{
	static Queue<Player> players;

	static GameObject gameProgressPane;

	static GameObject resetButton;

	public static GameObject CheckersPieceToDie;

	public static Player CurrentPlayer
	{
		get
		{
			return players.Count > 0 ? players.Peek() : null;
		}
	}

	public static bool MovingPiece { get; private set; }

	public static bool DoubleJump { get; private set; }

	public static GameObject ChessBoardGameObject { get; private set; }

	public static ChessBoard ChessBoard { get; private set; }

	void Start()
	{
		// UI
		string gameProgressPanePrefabPath = Utils.Path.Combine("Prefabs", "GameProgressPane");
		gameProgressPane = Instantiate(Resources.Load(gameProgressPanePrefabPath)) as GameObject;
		gameProgressPane.transform.position = new Vector3(-4.25f, 0.5f, 1.5f);
		gameProgressPane.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
		gameProgressPane.transform.localScale *= 1.5f;

		string resetButtonPrefabPath = Utils.Path.Combine("Prefabs", "ResetGameButton");
		resetButton = Instantiate(Resources.Load(resetButtonPrefabPath)) as GameObject;
		resetButton.transform.position = new Vector3(-4.25f, 0.5f, -2.5f);
		resetButton.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
		resetButton.transform.localScale *= 1.5f;

		// Game board and objects
		LoadGame();
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
				DoubleJump = true;
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
		DoubleJump = false;
		players.Enqueue(players.Dequeue());
		if (ChessBoard.GetCheckersPiecesOfTeam(CurrentPlayer.Team).Count > 0)
		{
			gameProgressPane.SendMessage("SetCurrentPlayer", CurrentPlayer.Team);
		}
		else
		{
			resetButton.SendMessage("SetButtonUpColor", Color.Lerp(Color.green, Color.white, 0.5f));
			gameProgressPane.SendMessage("SetWinner", CurrentPlayer.Team.Opponent());
		}
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
		gameProgressPane.SendMessage("SetCurrentPlayer", CurrentPlayer.Team);

		CheckersPieceToDie = null;

		MovingPiece = false;
		DoubleJump = false;

		resetButton.SendMessage("SetButtonUpColor", Color.white);
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
