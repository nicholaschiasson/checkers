using UnityEngine;

public class CheckersPieceGameObject : MonoBehaviour
{
	private Renderer pieceRenderer;
	private Color teamColor;

	void Awake()
	{
		pieceRenderer = GetComponent<Renderer>();
		teamColor = Color.gray;
	}

	public void SetTeam(Color teamCol)
	{
		teamColor = teamCol;
	}

	public void OnMouseEnter()
	{
		pieceRenderer.material.color = Color.Lerp(teamColor, Color.green, 0.6f);
	}

	public void OnMouseExit()
	{
		pieceRenderer.material.color = teamColor;
	}
}
