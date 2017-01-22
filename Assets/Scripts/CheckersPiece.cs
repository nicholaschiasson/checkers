using UnityEngine;
using UnityEngine.EventSystems;

public class CheckersPiece : MonoBehaviour
{
	Animator animator;
	CheckersPieceGameObject checkersPieceGameObject;
	Vector3 destination;

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

	void Awake()
	{
		animator = GetComponent<Animator>();
		checkersPieceGameObject = transform.Find("CheckersPieceGameObject").GetComponent<CheckersPieceGameObject>();
		destination = transform.position;
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
				transform.position = destination;
				CheckersGame.MoveComplete();
			}
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
	}

	public void MoveToAndTake(Vector3 position)
	{
		SetNewDestination(position);
		animator.Play("CheckersPieceTakeAnimation");
	}

	void SetNewDestination(Vector3 position)
	{
		destination = position;
		if (!CheckersGame.CheckersPieceToDie)
		{
			CheckersGame.CurrentPlayer.SelectedPiece = null;
		}
	}
}
