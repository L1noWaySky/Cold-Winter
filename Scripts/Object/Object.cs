using Godot;

public partial class Object : Area3D
{
	[Signal] delegate void UsedEventHandler();	

	public void Use()
	{
		GD.Print($"{this.Name}: был активирован метод Use");
		EmitSignal(SignalName.Used);
	}
}
