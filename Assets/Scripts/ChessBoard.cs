using UnityEngine;
using UnityEngine.EventSystems;

public class ChessBoard : MonoBehaviour
{
	public GameObject[,] CheckersPieces;

	void Awake()
	{
		CheckersPieces = new GameObject[6, 6];
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;
		float rotx = transform.rotation.eulerAngles.x;
		float roty = transform.rotation.eulerAngles.y;
		float rotz = transform.rotation.eulerAngles.z;
		string chessBoardTilePrefabPath = Utils.Path.Combine("Prefabs", "ChessBoardTile");
		for (int i = -3; i < 3; i++)
		{
			for (int j = -3; j < 3; j++)
			{
				Instantiate(Resources.Load(chessBoardTilePrefabPath), new Vector3(x + j + 0.5f, y + 0.5125f, z + i + 0.5f), Quaternion.Euler(rotx + 90.0f, roty, rotz));
			}
		}
		string checkersPiecePrefabPath = Utils.Path.Combine("Prefabs", "CheckersPiece");
		for (int i = -3; i < 3; i += 4)
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = -3; k < 3; k += 2)
				{
					GameObject checkersPiece = Instantiate(Resources.Load(checkersPiecePrefabPath), new Vector3(x + k + j + 0.5f, y + 0.625f, z + i + j + 0.5f), Quaternion.Euler(rotx, roty, rotz)) as GameObject;
					checkersPiece.SendMessage("SetTeam", i == -3 ? CheckersTeam.RED : CheckersTeam.BLUE);
					CheckersPieces[k + j + 3, i + j + 3] = checkersPiece;
				}
			}
		}
	}
}
