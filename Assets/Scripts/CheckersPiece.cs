using UnityEngine;
using UnityEngine.EventSystems;

public class CheckersPiece : MonoBehaviour
{
	Animator animator;
	CheckersPieceGameObject checkersPieceGameObject;
	Vector3 destination;

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
			float remainingAnimationTime = animatorState.length - animatorState.normalizedTime;
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
		checkersPieceGameObject.SetTeam(team);
	}

	public void Deselect()
	{
		checkersPieceGameObject.Deselect();
	}

	public void MoveTo(Vector3 position)
	{
		destination = position;
		CheckersGame.CurrentPlayer.SelectedPiece = null;
		animator.Play("CheckersPieceStepAnimation");
	}
}
