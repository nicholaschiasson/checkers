using UnityEngine;

public class ChessBoardTile : MonoBehaviour
{
	private Renderer tileRenderer;

	bool mouseDown = false;

	bool AvailableMoveTile
	{
		get
		{
			Vector2 pos = CheckersGame.ChessBoard.ConvertGamePositionToBoardPosition(new Vector2(transform.position.x, transform.position.z));
			return CheckersGame.CurrentPlayer.SelectedPiece && CheckersGame.ChessBoard.ChessBoardTileStates[(int)pos.x, (int)pos.y] == ChessBoardTileState.AVAILABLE;
		}
	}

	void Awake()
	{
		tileRenderer = GetComponent<Renderer>();
	}

	public void OnMouseEnter()
	{
		if (AvailableMoveTile)
		{
			Color col = tileRenderer.material.color;
			tileRenderer.material.color = new Color(col.r, col.g, col.b, 1.0f);
		}
	}

	public void OnMouseExit()
	{
		Color col = tileRenderer.material.color;
		tileRenderer.material.color = new Color(col.r, col.g, col.b, 0.0f);
	}

	public void OnMouseDown()
	{
		mouseDown = true;
	}

	public void OnMouseUp()
	{
		if (mouseDown)
		{
			OnMouseClick();
		}
		mouseDown = true;
	}

	public void OnMouseClick()
	{
		if (AvailableMoveTile)
		{
			CheckersGame.MovePieceTo(transform.position);
		}
	}
}
