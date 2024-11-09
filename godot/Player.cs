using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

public class Player : Spatial
{
	private Timer timerprocessor;
	private Tween tween;
	private RayCast forward;
	private RayCast back;
	private RayCast right;
	private RayCast left;

	public override void _Ready()
	{
		timerprocessor = GetNode<Timer>("Timer");
		tween = GetNode<Tween>("Tween");
		forward = GetNode<RayCast>("RayForward");
		back = GetNode<RayCast>("RayBack");
		right = GetNode<RayCast>("RayRight");
		left = GetNode<RayCast>("RayLeft");

		timerprocessor.Connect("timeout", this, nameof(OnTimerTimeout));
	}
	
	  public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && IsLeftClick(mouseEvent))
		{
			CheckForInteractableInFront();
		}
	}

	private bool IsLeftClick(InputEventMouseButton mouseEvent)
	{
		return mouseEvent.Pressed && mouseEvent.ButtonIndex == (int)ButtonList.Left;
	}

	private void CheckForInteractableInFront()
	{
		if (!CollisionCheck(forward)) return;

		var collider = forward.GetCollider();
		if (collider is Node interactableNode && interactableNode.HasMethod("OnInteract"))
		{
			interactableNode.Call("OnInteract");
		}
	}
	
	private bool CollisionCheck(RayCast direction)
	{
		if (direction != null)
		{
			return direction.IsColliding();
		}
		return false;
	}

	private async Task TweenTranslation(Vector3 change)
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Step");
		tween.InterpolateProperty(
			this, "translation", Translation, Translation + change,
			0.5f, Tween.TransitionType.Quad, Tween.EaseType.InOut
		);
		tween.Start();
		await ToSignal(tween, "tween_completed");
	}

	private async Task TweenRotation(float change)
	{
		tween.InterpolateProperty(
			this, "rotation", Rotation, Rotation + new Vector3(0, change, 0),
			0.5f, Tween.TransitionType.Quad, Tween.EaseType.InOut
		);
		tween.Start();
		await ToSignal(tween, "tween_completed");
	}

	private async void OnTimerTimeout()
	{
		bool goW = Input.IsActionPressed("forward");
		bool goS = Input.IsActionPressed("back");
		bool goA = Input.IsActionPressed("strafe_left");
		bool goD = Input.IsActionPressed("strafe_right");
		bool turnQ = Input.IsActionPressed("turn_left");
		bool turnE = Input.IsActionPressed("turn_right");

		Vector3 movementDirection = Vector3.Zero;
		int turnDir = (turnQ ? 1 : 0) - (turnE ? 1 : 0);
		//float tileSize = Globals.GRID_SIZE; unused

		if (goW) movementDirection += -GlobalTransform.basis.z * Globals.GRID_SIZE;
		else if (goS) movementDirection += GlobalTransform.basis.z * Globals.GRID_SIZE;
		else if (goA) movementDirection += -GlobalTransform.basis.x * Globals.GRID_SIZE;
		else if (goD) movementDirection += GlobalTransform.basis.x *Globals.GRID_SIZE;
		else if (Convert.ToBoolean(turnDir))
		{
			timerprocessor.Stop();
			await TweenRotation(Mathf.Pi / 2 * turnDir);
			timerprocessor.Start();
		}

		if (movementDirection != Vector3.Zero)
		{
			if(goW)//checks button to see if pressed and if so then does a collision check
			{
				if (CollisionCheck(forward))
				{
					timerprocessor.Stop();
					await TweenTranslation(movementDirection);
					timerprocessor.Start();
				}
			}
			else if(goS)
			{
				if (CollisionCheck(back))
				{
					timerprocessor.Stop();
					await TweenTranslation(movementDirection);
					timerprocessor.Start();
				}
			}
			else if(goA)
			{
				if (CollisionCheck(left))
				{
					timerprocessor.Stop();
					await TweenTranslation(movementDirection);
					timerprocessor.Start();
				}
			}
			else if (goD)
			{
				if (CollisionCheck(right))
				{
					timerprocessor.Stop();
					await TweenTranslation(movementDirection);
					timerprocessor.Start();
				}
			}
		}
	}
}
