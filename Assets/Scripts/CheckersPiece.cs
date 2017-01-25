using UnityEngine;
using UnityEngine.EventSystems;

public class CheckersPiece : MonoBehaviour
{
	Animator animator;
	CheckersPieceGameObject checkersPieceGameObject;
	Vector3 destination;
	GameObject crown;
	Animator crownAnimator;
	bool dying = false;

	public bool Dead = false;

	public CheckersTeam Team
	{
		get
		{
			return checkersPieceGameObject.Team;
		}
		set
		{
			checkersPieceGameObject.SetTeam(value);
		}
	}

	public bool IsCrowned
	{
		get
		{
			return crown != null;
		}
	}

	void Awake()
	{
		animator = GetComponent<Animator>();
		checkersPieceGameObject = transform.Find("CheckersPieceGameObject").GetComponent<CheckersPieceGameObject>();
		destination = transform.position;
		crown = null;
		crownAnimator = null;
	}

	void Update()
	{
		if (transform.position != destination)
		{
			AnimatorStateInfo animatorState = animator.GetCurrentAnimatorStateInfo(0);
			float remainingAnimationTime = animatorState.length - (animatorState.length * animatorState.normalizedTime);
			float speed = Vector3.Distance(transform.position, destination) / (remainingAnimationTime / Time.deltaTime);
			if (Vector3.Distance(transform.position, destination) > speed)
			{
				transform.position = Vector3.MoveTowards(transform.position, destination, speed);
			}
			else
			{
				if (!dying)
				{
					transform.position = destination;
					if (CanBeCrowned(CheckersGame.CurrentPlayer.PermittedDirectionOfMovement, new Vector2(destination.x, destination.z)))
					{
						Crown();
					}
					CheckersGame.MoveComplete();
				}
			}
		}

		if (crown && crown.transform.position != transform.position)
		{
			crown.transform.position = Vector3.MoveTowards(crown.transform.position, transform.position, 10.0f * Time.deltaTime);
		}

		if (Dead)
		{
			Destroy(gameObject);
		}
	}

	public void SetTeam(CheckersTeam team)
	{
		Team = team;
	}

	public void Select()
	{
		checkersPieceGameObject.Select();
	}

	public void Deselect()
	{
		checkersPieceGameObject.Deselect();
	}

	public void MoveTo(Vector3 position)
	{
		SetNewDestination(position);
		animator.Play("CheckersPieceStepAnimation");
		if (crownAnimator)
		{
			crownAnimator.Play("CrownStepAnimation");
		}
	}

	public void MoveToAndTake(Vector3 position)
	{
		SetNewDestination(position);
		animator.Play("CheckersPieceTakeAnimation");
		if (crownAnimator)
		{
			crownAnimator.Play("CrownTakeAnimation");
		}
	}

	void SetNewDestination(Vector3 position)
	{
		destination = position;
		if (!CheckersGame.CheckersPieceToDie)
		{
			CheckersGame.CurrentPlayer.SelectedPiece = null;
		}
	}

	bool CanBeCrowned(DirectionOfMovement pieceDirection, Vector2 piecePosition)
	{
		Vector2 pos = CheckersGame.ChessBoard.ConvertGamePositionToBoardPosition(piecePosition);
		switch (pieceDirection)
		{
			case DirectionOfMovement.FORWARD:
				if (pos.y >= 5.0f) return true;
				break;
			case DirectionOfMovement.BACKWARD:
				if (pos.y <= 0.0f) return true;
				break;
			default:
				break;
		}
		return false;
	}

	public void Crown()
	{
		if (!crown)
		{
			string crownPrefabPath = Utils.Path.Combine("Prefabs", "Crown");
			crown = Instantiate(Resources.Load(crownPrefabPath), new Vector3(transform.position.x, Camera.main.transform.position.y + 1.0f, transform.position.z), Quaternion.identity, transform) as GameObject;
			crownAnimator = crown.GetComponent<Animator>();
		}
	}

	public void Die()
	{
		animator.Play("CheckersPieceDeathAnimation");
		if (crownAnimator)
		{
			crownAnimator.Play("CrownDeathAnimation");
		}
		dying = true;
		destination = new Vector3(transform.position.x, transform.position.y - 0.26f, transform.position.z);
		Team = CheckersTeam.NONE;
	}
}
