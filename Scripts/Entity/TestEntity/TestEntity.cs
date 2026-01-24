using Godot;


public partial class TestEntity : CharacterBody3D
{
	[Export] public Node scr;
	[Export] public Node3D Selects;

    public override void _Process(double delta)
    {
        base._Process(delta);

		
    }


    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

		if (@event.IsPressed())
		{
			if (@event.IsActionPressed("F"))
			{
				scr.Call("TakeDamage", 1, new Vector3(0,5,10));
			}
		}
    }

}
