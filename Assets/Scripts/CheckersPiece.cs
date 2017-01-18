using UnityEngine;
using UnityEngine.EventSystems;

public class CheckersPiece : MonoBehaviour
{
	private Renderer pieceRenderer;
	private Color teamColor;

	public CheckersTeam Team;

	void Awake()
	{
		pieceRenderer = transform.Find("CheckersPieceGameObject").GetComponent<Renderer>();
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
			pieceRenderer.material.color = teamColor;
			transform.Find("CheckersPieceGameObject").SendMessage("SetTeam", teamColor);
		}
	}
}
