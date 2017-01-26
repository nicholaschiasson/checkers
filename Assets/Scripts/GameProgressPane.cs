using UnityEngine;

public class GameProgressPane : MonoBehaviour
{
	TextMesh textMesh;

	void Awake()
	{
		textMesh = transform.Find("Text").GetComponent<TextMesh>();
	}

	public void SetCurrentPlayer(CheckersTeam currentTeam)
	{
		switch (currentTeam)
		{
			case CheckersTeam.BLUE:
				textMesh.color = Color.blue;
				textMesh.text = "Blue turn";
				break;
			case CheckersTeam.RED:
				textMesh.color = Color.red;
				textMesh.text = "Red turn";
				break;
			default:
				break;
		}
	}

	public void SetWinner(CheckersTeam currentTeam)
	{
		switch (currentTeam)
		{
			case CheckersTeam.BLUE:
				textMesh.color = Color.blue;
				textMesh.text = "Blue wins!";
				break;
			case CheckersTeam.RED:
				textMesh.color = Color.red;
				textMesh.text = "Red wins!";
				break;
			default:
				break;
		}
	}
}
