using UnityEngine;
using System.Collections.Generic;

public enum ChessBoardTileState
{
	UNAVAILABLE,
	AVAILABLE,
	SELECTED_PIECE
}

public class ChessBoard : MonoBehaviour
{
	Vector2 dimensions;
	float tileWidth;
	float tileHeight;

	GameObject[,] tileSurfaces;

	public GameObject[,] CheckersPieces { get; private set; }

	public ChessBoardTileState[,] ChessBoardTileStates { get; private set; }

	public bool SelectedPieceCanMove
	{
		get
		{
			foreach (ChessBoardTileState tileState in ChessBoardTileStates)
			{
				if (tileState == ChessBoardTileState.AVAILABLE)
				{
					return true;
				}
			}
			return false;
		}
	}

	void Awake()
	{
		dimensions = new Vector2(transform.localScale.x, transform.localScale.z);
		tileWidth = dimensions.x / 6.0f;
		tileHeight = dimensions.y / 6.0f;
		int halfDimensionX = (int)(dimensions.x / 2.0f);
		int halfDimensionZ = (int)(dimensions.y / 2.0f);
		float widthby2 = tileWidth / 2.0f;
		float heightby2 = tileHeight / 2.0f;
		float depthby2 = transform.localScale.y / 2.0f;
		tileSurfaces = new GameObject[6, 6];
		CheckersPieces = new GameObject[6, 6];
		ChessBoardTileStates = new ChessBoardTileState[6, 6];

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;
		float rotx = transform.rotation.eulerAngles.x;
		float roty = transform.rotation.eulerAngles.y;
		float rotz = transform.rotation.eulerAngles.z;
		string chessBoardTilePrefabPath = Utils.Path.Combine("Prefabs", "ChessBoardTile");
		for (int i = -halfDimensionZ; i < halfDimensionZ; i++)
		{
			for (int j = -halfDimensionX; j < halfDimensionX; j++)
			{
				tileSurfaces[j + halfDimensionX, i + halfDimensionZ] = Instantiate(Resources.Load(chessBoardTilePrefabPath), new Vector3(x + j + widthby2, y + depthby2, z + i + heightby2), Quaternion.Euler(rotx + 90.0f, roty, rotz)) as GameObject;
			}
		}
		string checkersPiecePrefabPath = Utils.Path.Combine("Prefabs", "CheckersPiece");
		for (int i = -halfDimensionZ; i < halfDimensionZ; i += 4)
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = -halfDimensionX; k < halfDimensionX; k += 2)
				{
					GameObject checkersPiece = Instantiate(Resources.Load(checkersPiecePrefabPath), new Vector3(x + k + j + widthby2, y + depthby2 + 0.125f, z + i + j + heightby2), Quaternion.Euler(rotx, roty, rotz)) as GameObject;
					checkersPiece.SendMessage("SetTeam", i == -3 ? CheckersTeam.RED : CheckersTeam.BLUE);
					CheckersPieces[k + j + 3, i + j + 3] = checkersPiece;
				}
			}
		}
	}

	public void SelectPiece(Vector2 position)
	{
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				ChessBoardTileStates[j, i] = ChessBoardTileState.UNAVAILABLE;
			}
		}
		if (float.IsNaN(position.x) || float.IsNaN(position.y))
		{
			return;
		}
		Vector2 piecePosition = ConvertGamePositionToBoardPosition(position);
		ChessBoardTileStates[(int)piecePosition.x, (int)piecePosition.y] = ChessBoardTileState.SELECTED_PIECE;
		List<Vector2> availableMoveTiles = new List<Vector2>();
		List<DirectionOfMovement> dirs = new List<DirectionOfMovement>();
		dirs.Add(CheckersGame.CurrentPlayer.PermittedDirectionOfMovement);
		if ((CheckersGame.CurrentPlayer.SelectedPiece.GetComponent<CheckersPiece>() as CheckersPiece).IsCrowned)
		{
			switch (CheckersGame.CurrentPlayer.PermittedDirectionOfMovement)
			{
				case DirectionOfMovement.FORWARD:
					dirs.Add(DirectionOfMovement.BACKWARD);
					break;
				case DirectionOfMovement.BACKWARD:
					dirs.Add(DirectionOfMovement.FORWARD);
					break;
				default:
					break;
			}
		}
		foreach (DirectionOfMovement dir in dirs)
		{
			int rowOffset = (int)(dir == DirectionOfMovement.FORWARD ? 1 : -1);
			int stepRowIndex = (int)piecePosition.y + rowOffset;
			int takeRowIndex = (int)piecePosition.y + (2 * rowOffset);
			int leftStepColumn = (int)piecePosition.x - 1;
			int leftTakeColumn = (int)piecePosition.x - 2;
			int rightStepColumn = (int)piecePosition.x + 1;
			int rightTakeColumn = (int)piecePosition.x + 2;
			bool takeRowIndexInRange = takeRowIndex >= 0 && takeRowIndex < 6;
			if (stepRowIndex >= 0 && stepRowIndex < 6)
			{
				CheckersPiece selectedPiece = CheckersGame.CurrentPlayer.SelectedPiece.GetComponent<CheckersPiece>();
				if (leftStepColumn >= 0)
				{
					if (CheckersPieces[leftStepColumn, stepRowIndex])
					{
						CheckersPiece piece = CheckersPieces[leftStepColumn, stepRowIndex].GetComponent<CheckersPiece>();
						if (selectedPiece.Team != piece.Team && leftTakeColumn >= 0 && takeRowIndexInRange && !CheckersPieces[leftTakeColumn, takeRowIndex])
						{
							availableMoveTiles.Add(new Vector2(leftTakeColumn, takeRowIndex));
						}
					}
					else
					{
						if (!CheckersGame.CheckersPieceToDie)
						{
							availableMoveTiles.Add(new Vector2(leftStepColumn, stepRowIndex));
						}
					}
				}
				if (rightStepColumn < 6)
				{
					if (CheckersPieces[rightStepColumn, stepRowIndex])
					{
						CheckersPiece piece = CheckersPieces[rightStepColumn, stepRowIndex].GetComponent<CheckersPiece>();
						if (selectedPiece.Team != piece.Team && rightTakeColumn < 6 && takeRowIndexInRange && !CheckersPieces[rightTakeColumn, takeRowIndex])
						{
							availableMoveTiles.Add(new Vector2(rightTakeColumn, takeRowIndex));
						}
					}
					else
					{
						if (!CheckersGame.CheckersPieceToDie)
						{
							availableMoveTiles.Add(new Vector2(rightStepColumn, stepRowIndex));
						}
					}
				}
			}
		}
		foreach (Vector2 tilePos in availableMoveTiles)
		{
			ChessBoardTileStates[(int)tilePos.x, (int)tilePos.y] = ChessBoardTileState.AVAILABLE;
		}
	}

	public GameObject MovePieceTo(Vector2 source, Vector2 destination)
	{
		GameObject deadPiece = null;
		Vector2 srcPos = ConvertGamePositionToBoardPosition(source);
		Vector2 destPos = ConvertGamePositionToBoardPosition(destination);
		CheckersPieces[(int)destPos.x, (int)destPos.y] = CheckersPieces[(int)srcPos.x, (int)srcPos.y];
		CheckersPieces[(int)srcPos.x, (int)srcPos.y] = null;
		Vector2 midPos = new Vector2((int)((destPos.x + srcPos.x) / 2.0f), (int)((destPos.y + srcPos.y) / 2.0f));
		if (midPos != srcPos && midPos != destPos && CheckersPieces[(int)midPos.x, (int)midPos.y])
		{
			deadPiece = CheckersPieces[(int)midPos.x, (int)midPos.y];
			CheckersPieces[(int)midPos.x, (int)midPos.y] = null;
		}
		return deadPiece;
	}

	public Vector2 ConvertGamePositionToBoardPosition(Vector2 position)
	{
		return new Vector2(position.x - transform.position.x + (dimensions.x / 2.0f) - (tileWidth / 2.0f), position.y - transform.position.z + (dimensions.y / 2.0f) - (tileHeight / 2.0f));
	}

	public void Cleanup()
	{
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				Destroy(tileSurfaces[j, i]);
				if (CheckersPieces[j, i])
				{
					Destroy(CheckersPieces[j, i]);
				}
			}
		}
	}
}
