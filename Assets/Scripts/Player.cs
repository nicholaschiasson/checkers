using UnityEngine;
public class Player
{
	public Transform SelectedPiece
	{
		get
		{
			return selectedPiece;
		}
		set
		{
			if (selectedPiece)
			{
				selectedPiece.SendMessage("Deselect");
			}
			selectedPiece = value;
		}
	}
	public CheckersTeam Team { get; private set; }
	public DirectionOfMovement PermittedDirectionOfMovement { get; private set; }

	Transform selectedPiece;

	public Player(CheckersTeam team, DirectionOfMovement permittedDirectionOfMovement)
	{
		SelectedPiece = null;
		Team = team;
		PermittedDirectionOfMovement = permittedDirectionOfMovement;
	}
}