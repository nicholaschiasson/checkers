using UnityEngine;

public class PassTurnButton : MonoBehaviour
{
	Renderer buttonRenderer;
	Color buttonUpColor;
	Color highlightColor
	{
		get
		{
			return Color.Lerp(ButtonUpColor, Color.yellow, 0.4f);
		}
	}
	Color buttonDownColor
	{
		get
		{
			return Color.Lerp(ButtonUpColor, Color.yellow, 0.8f);
		}
	}

	bool mouseDown = false;

	public Color ButtonUpColor
	{
		get
		{
			return buttonUpColor;
		}
		set
		{
			buttonRenderer.material.color = value;
			buttonUpColor = value;
		}
	}

	void Awake()
	{
		buttonRenderer = GetComponent<Renderer>();
		ButtonUpColor = Color.white;
	}

	public void OnMouseEnter()
	{
		buttonRenderer.material.color = highlightColor;
	}

	public void OnMouseOver()
	{
		if (mouseDown)
		{
			buttonRenderer.material.color = buttonDownColor;
		}
		else
		{
			buttonRenderer.material.color = highlightColor;
		}
	}

	public void OnMouseExit()
	{
		buttonRenderer.material.color = buttonUpColor;
		mouseDown = false;
	}

	public void OnMouseDown()
	{
		buttonRenderer.material.color = buttonDownColor;
		mouseDown = true;
	}

	public void OnMouseUp()
	{
		if (mouseDown)
		{
			buttonRenderer.material.color = highlightColor;
			OnMouseClick();
		}
		mouseDown = false;
	}

	void OnMouseClick()
	{
		CheckersGame.PassPlayerTurn(true);
	}
}
