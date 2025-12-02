using Godot;


public partial class PlayerRayCast : RayCast3D
{
	public override void _Ready()
    {
        
    }
	public override void _Process(double delta)
    {
        if (this.IsColliding())
        {
            if (this.GetCollider() is CollisionShape3D SelectedObject)
            {
                GD.Print(SelectedObject.GetParent().Name);
            }
        }
    }

	
}
