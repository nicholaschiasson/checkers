using UnityEngine;

public class CheckersPieceGameObject : MonoBehaviour
{
	Renderer pieceRenderer;
	Color teamColor;
	Color highlightColor;

	bool mouseDown = false;

	public CheckersTeam Team;

	bool Selectable
	{
		get
		{
			return !CheckersGame.DoubleJump && !CheckersGame.MovingPiece && CheckersGame.CurrentPlayer.Team == Team;
		}
	}

	bool Selected
	{
		get
		{
			return CheckersGame.CurrentPlayer.Team == Team && CheckersGame.CurrentPlayer.SelectedPiece == transform.parent;
		}
	}

	void Awake()
	{
		pieceRenderer = GetComponent<Renderer>();
		teamColor = Color.gray;
		highlightColor = Color.white;

		Team = CheckersTeam.NONE;
	}

	public void SetTeam(CheckersTeam team)
	{
		switch (team)
		{
			case CheckersTeam.BLUE:
				teamColor = Color.blue;
				break;
			case CheckersTeam.RED:
				teamColor = Color.red;
				break;
			default:
				break;
		}
		Team = team;
		pieceRenderer.material.color = teamColor;
	}

	public void Select()
	{
		pieceRenderer.material.color = Color.Lerp(teamColor, highlightColor, 0.5f);
	}

	public void Deselect()
	{
		pieceRenderer.material.color = teamColor;
	}

	public void OnMouseEnter()
	{
		Debug.Log("Selectable: " + Selectable + ", Selected: " + Selected);
		if (Selectable && !Selected)
		{
			pieceRenderer.material.color = Color.Lerp(teamColor, highlightColor, 0.5f);
		}
	}

	public void OnMouseOver()
	{
		if (Selectable && !Selected)
		{
			pieceRenderer.material.color = Color.Lerp(teamColor, highlightColor, 0.5f);
		}
	}

	public void OnMouseExit()
	{
		if (!Selected)
		{
			Deselect();
		}
		mouseDown = false;
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
		mouseDown = false;
	}

	public void OnMouseClick()
	{
		if (Selectable)
		{
			if (!Selected)
			{
				CheckersGame.CurrentPlayer.SelectedPiece = transform.parent;
			}
			else
			{
				CheckersGame.CurrentPlayer.SelectedPiece = null;
			}
		}
	}
}
