using System.Linq;
using Godot;

public partial class Door : Node3D
{
	[Export] Node3D Point;
	[Export] Godot.Collections.Array<RayCast3D> OpenRays;
	[Export] Godot.Collections.Array<RayCast3D> CloseRays;

	[Export] public float OpenValue = 135f;
	[Export] public float OpenSpeed = 2f;
	[Export] public float CloseSpeed = 2f;
	[Export] public bool Open
	{
		get => open;
		set
		{
			if (open==value) return;
			open = value;
			SwitchMethod = true;
		}
	}
	
	bool open;
	bool SwitchMethod;

	public void SwitchProperty() => Open = !Open;

    public override void _PhysicsProcess(double delta) 
    {
		if (SwitchMethod) AnimateDoor(ref SwitchMethod, Open, (float)delta);
    }

	void AnimateDoor(ref bool _switch, bool openOrClose, float delta)
	{
		bool CanAnimate = true;
		var currentRayCasts = openOrClose ? OpenRays : CloseRays;
		float currentValue = openOrClose ? OpenValue : 0f;
		float currentSpeed = openOrClose ? OpenSpeed : CloseSpeed;

		if (currentRayCasts.Any(Rays => Rays.IsColliding())) CanAnimate = false;

		if (CanAnimate)
		{
			Point.RotationDegrees = Vector3.Up * Mathf.Lerp(
				Point.RotationDegrees.Y,
				currentValue, // Open ? OpenValue : 0f,
				currentSpeed * delta
			);
		}

		if (Mathf.IsEqualApprox(Point.RotationDegrees.Y, currentValue, 0.1f))
		{
			Point.RotationDegrees = Vector3.Up * currentValue;
			_switch = !_switch;
			GD.Print($"Done!: {_switch}, {Point.RotationDegrees.Y}");
			return;
		}
	}
	
}
