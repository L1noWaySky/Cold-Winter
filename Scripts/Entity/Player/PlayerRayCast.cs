using Godot;


public partial class PlayerRayCast : RayCast3D
{
    [Export] float TakeZoneRadius = 0.3f;
    [Export] Color TakeZoneDebugColor = new Color(1,0.8f,0.2f,0.9f);
    
    string TakeZoneName = "TakeZoneForItems";


	public override void _Ready()
    {
        
    }

    void InstanceTakeZone(float _Radius, Color _DebugColor)
    {
        Area3D TakeZone = new Area3D();
        TakeZone.Name = TakeZoneName;
        TakeZone.Monitorable = false;
        TakeZone.Position = new Vector3(0,0,-1);

        SphereShape3D ShapeForCollision = new SphereShape3D();
        ShapeForCollision.Radius = _Radius;
        CollisionShape3D TakeZoneCollision = new CollisionShape3D();
        TakeZoneCollision.Name = "ZoneCollision";
        TakeZoneCollision.Shape = ShapeForCollision;
        TakeZoneCollision.DebugColor = _DebugColor;

        TakeZone.AddChild(TakeZoneCollision);
        this.AddChild(TakeZone);

    }
	public override void _Process(double delta)
    {
        if (this.IsColliding())
        {
            if (this.HasNode(TakeZoneName))
            {
                //GD.Print(1);
                Area3D TakeZone = this.GetChild<Area3D>(0);
                if (TakeZone.GlobalPosition != this.GetCollisionPoint()) { TakeZone.GlobalPosition = this.GetCollisionPoint(); /* GD.Print($"Take Zone Position is updated: {TakeZone.GlobalPosition}"); */}
            }
            else { InstanceTakeZone(TakeZoneRadius, TakeZoneDebugColor); GD.Print("Take Zone is Created!"); }
        }
        else
        {
            if (this.HasNode(TakeZoneName)) { this.GetChild<Node>(0).QueueFree(); GD.Print("Take Zone is Deleted"); }
            //GD.Print(0);
        }
    }

	
}
