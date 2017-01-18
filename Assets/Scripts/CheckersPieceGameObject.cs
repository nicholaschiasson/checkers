using UnityEngine;

public class CheckersPieceGameObject : MonoBehaviour
{
	Renderer pieceRenderer;
	Color teamColor;

	bool mouseDown = false;

	public CheckersTeam Team;

	bool Selectable
	{
		get
		{
			return !CheckersGame.MovingPiece && CheckersGame.CurrentPlayer.Team == Team;
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

		Team = CheckersTeam.NONE;
	}

	public void SetTeam(CheckersTeam team)
	{
		if (team != CheckersTeam.NONE)
		{
			Team = team;
			teamColor = (Team == CheckersTeam.BLUE ? Color.blue : Color.red);
			pieceRenderer.material.color = teamColor;
		}
	}

	public void Deselect()
	{
		pieceRenderer.material.color = teamColor;
	}

	public void OnMouseEnter()
	{
		if (Selectable && !Selected)
		{
			pieceRenderer.material.color = Color.Lerp(teamColor, Color.green, 0.6f);
		}
	}

	public void OnMouseOver()
	{
		if (Selectable && !Selected)
		{
			pieceRenderer.material.color = Color.Lerp(teamColor, Color.green, 0.6f);
		}
	}

	public void OnMouseExit()
	{
		if (!Selected)
		{
			pieceRenderer.material.color = teamColor;
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
				pieceRenderer.material.color = Color.Lerp(teamColor, Color.yellow, 0.6f);
			}
			else
			{
				CheckersGame.CurrentPlayer.SelectedPiece = null;
			}
		}
	}
}
