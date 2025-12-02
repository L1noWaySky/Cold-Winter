using Godot;


public partial class TestEntity : CharacterBody3D
{
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        Vector3 _Velocity = this.Velocity;

		if (!this.IsOnFloor())
        {
            _Velocity += this.GetGravity() *(float)delta;
        }

		this.Velocity = _Velocity;
		this.MoveAndSlide();
    }

}
