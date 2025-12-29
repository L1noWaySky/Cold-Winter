using System.Linq;
using Godot;


public partial class PlayerRayCast : RayCast3D
{
    [Signal] delegate void ItemCountEventHandler(int Count);
    [Export] float TakeZoneRadius = 0.4f;
    [Export] Color TakeZoneDebugColor = new Color(1,0.8f,0.2f,0.9f);
    
    string TakeZoneName = "TakeZoneForItems";

    void InstanceTakeZone(float _Radius, Color _DebugColor)
    {
        Area3D TakeZone = new Area3D();
        TakeZone.Name = TakeZoneName;
        TakeZone.Monitorable = false;
        TakeZone.Position = new Vector3(0,0,-1);
        TakeZone.SetCollisionLayerValue(1, false);
        TakeZone.SetCollisionMaskValue(1, false);
        TakeZone.SetCollisionLayerValue(10, true);
        TakeZone.SetCollisionMaskValue(10, true);

        SphereShape3D ShapeForCollision = new SphereShape3D();
        ShapeForCollision.Radius = _Radius;
        CollisionShape3D TakeZoneCollision = new CollisionShape3D();
        TakeZoneCollision.Name = "ZoneCollision";
        TakeZoneCollision.Shape = ShapeForCollision;
        TakeZoneCollision.DebugColor = _DebugColor;
        

        TakeZone.AddChild(TakeZoneCollision);
        this.AddChild(TakeZone);

    }
	
    Godot.Collections.Array<Area3D> SortArrayToDistance(Godot.Collections.Array<Area3D> _selArray, int selLength, Vector3 _selPos)
    {
        Vector3 selPos = _selPos;
        Godot.Collections.Array<Area3D> selArray = _selArray;
        Area3D temp;
        for (int i = 0; i < selLength - 1; i++)
        {
            for (int j = i + 1; j < selLength; j++)
            {
                float a = selArray[i].GlobalPosition.DistanceTo(selPos);
                float b = selArray[j].GlobalPosition.DistanceTo(selPos);
                if (a > b)
                {
                    temp = selArray[i];
                    selArray[i] = selArray[j];
                    selArray[j] = temp;
                }
            }
        }
        return selArray;
        
    }
    public override void _Process(double delta)
    {
        if (this.IsColliding())
        {
            if (this.HasNode(TakeZoneName)) // Проверка существования узла
            {
                Area3D TakeZone = this.GetNode<Area3D>(TakeZoneName);
                Godot.Collections.Array<Area3D>? ItemsInTakeZone;

                if (TakeZone.GlobalPosition != this.GetCollisionPoint()) { TakeZone.GlobalPosition = this.GetCollisionPoint(); }
                // Проверка предметов внутри зоны
                if (TakeZone.HasOverlappingAreas())
                {
                    ItemsInTakeZone = SortArrayToDistance(
                        TakeZone.GetOverlappingAreas(),
                        (int)TakeZone.GetOverlappingAreas().LongCount(),
                        this.GetCollisionPoint()
                    );
                    Area3D selected = ItemsInTakeZone[0];
                    
                    if (Input.IsActionJustPressed("E")) TakeItem(selected);
                }
                else
                {
                    ItemsInTakeZone = null;
                }
            }
            else { InstanceTakeZone(TakeZoneRadius, TakeZoneDebugColor); GD.Print("Take Zone is Created!"); }
        }
        else
        {
            if (this.HasNode(TakeZoneName)) { this.GetChild<Node>(0).QueueFree(); GD.Print("Take Zone is Deleted"); }
        }
    }

    void TakeItem(Area3D _item)
    {
        _item.QueueFree();
    }

	
}
