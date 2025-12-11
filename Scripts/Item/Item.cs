using Godot;


public partial class Item : Node
{
	[Export] string? ItemName;
	[Export] int[] ItemWeight = {0, 0};
}
