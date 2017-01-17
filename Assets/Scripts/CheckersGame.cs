using UnityEngine;
using System.Collections.Generic;

public enum CheckersTeam
{
	NONE,
	BLUE,
	RED
}

public class CheckersGame : MonoBehaviour
{
	void Start()
	{
		string chessBoardPrefabPath = Utils.Path.Combine("Prefabs", "ChessBoard");
		Instantiate(Resources.Load(chessBoardPrefabPath));
	}
}
