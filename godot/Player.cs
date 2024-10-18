using Godot;
using System.Collections;

public class YourScript : Spatial
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
	}

	private bool CollisionCheck(RayCast direction)
	{
		if (direction != null)
		{
			return direction.IsColliding();
		}
		return false;
	}

	private Vector3 GetDirection(RayCast direction)
	{
		if (!(direction is RayCast)) return Vector3.Zero;
		return direction.GetCollider().GlobalTransform.origin - GlobalTransform.origin;
	}

	private async void TweenTranslation(Vector3 change)
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Step");
		tween.InterpolateProperty(
			this, "translation", Translation, Translation + change,
			0.5f, Tween.TransitionType.Quad, Tween.EaseType.InOut
		);
		tween.Start();
		await ToSignal(tween, "tween_completed");
	}

	private async void TweenRotation(float change)
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

		RayCast rayDir = null;
		int turnDir = (turnQ ? 1 : 0) - (turnE ? 1 : 0);

		if (goW)
		{
			rayDir = forward;
		}
		else if (goS)
		{
			rayDir = back;
		}
		else if (goA)
		{
			rayDir = left;
		}
		else if (goD)
		{
			rayDir = right;
		}
		else if (turnDir != 0)
		{
			timerprocessor.Stop();
			await TweenRotation(Mathf.Pi / 2 * turnDir);
			timerprocessor.Start();
		}

		if (CollisionCheck(rayDir))
		{
			timerprocessor.Stop();
			await TweenTranslation(GetDirection(rayDir));
			timerprocessor.Start();
		}
	}
}
