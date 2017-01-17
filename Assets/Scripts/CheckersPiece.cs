using UnityEngine;
using UnityEngine.EventSystems;

public class CheckersPiece : MonoBehaviour
{
	private Renderer pieceRenderer;
	private Color teamColor;

	public CheckersTeam Team;

	void Awake()
	{
		pieceRenderer = GetComponent<Renderer>();
		teamColor = Color.gray;

		Team = CheckersTeam.NONE;
	}

	void Update()
	{

	}

	public void SetTeam(CheckersTeam team)
	{
		if (team != CheckersTeam.NONE)
		{
			Team = team;
			teamColor = (Team == CheckersTeam.BLUE ? Color.blue : Color.red);
			GetComponent<Renderer>().material.color = teamColor;
		}
	}

	public void OnMouseEnter()
	{
		pieceRenderer.material.color = Color.Lerp(teamColor, Color.green, 0.75f);
	}

	public void OnMouseExit()
	{
		pieceRenderer.material.color = teamColor;
	}
}
