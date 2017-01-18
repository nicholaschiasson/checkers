﻿using UnityEngine;

public class ChessBoardTile : MonoBehaviour
{
	private Renderer tileRenderer;

	bool mouseDown = false;

	void Awake()
	{
		tileRenderer = GetComponent<Renderer>();
	}

	public void OnMouseEnter()
	{
		Color col = tileRenderer.material.color;
		tileRenderer.material.color = new Color(col.r, col.g, col.b, 0.9f);
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
		if (CheckersGame.CurrentPlayer.SelectedPiece)
		{
			CheckersGame.MovePieceTo(transform.position);
		}
	}
}
