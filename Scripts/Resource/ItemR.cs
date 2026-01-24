using Godot;


public partial class ItemR : BasicResource
{
    [Export] public string value {get; set;}
    [Export] public int Height {get; set;}
    [Export] public Vector2 Scale
    {
        get
        {
            return scale;
        }
        set
        {
            if (value.X >= 1 && value.Y >= 1)
            {
                scale.X = value.X - 1;
                scale.Y = value.Y - 1;
            }
        }
    }
    [Export] public Vector2 Center
    {
        get
        {
            return center;
        }
        set
        {
            if (value.X > -1 && value.Y > -1)
            {
                center = value;
            }
        }
    }
    Vector2 scale;
    Vector2 center;
}
