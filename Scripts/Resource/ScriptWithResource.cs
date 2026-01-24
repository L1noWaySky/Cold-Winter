using Godot;


public partial class ScriptWithResource<ChildType> : Node where ChildType : BasicResource
{
    public ChildType Data {get; set;}
    [Export] bool AutoName = false;
    

    public override void _Ready()
    {
        if (AutoName) {Data.Name = this.GetParent<Node>().Name;}

        if (Data != null)
        {
            Data = (ChildType)Data.Duplicate();
            Data.Init();
        }
        else
        {
            GD.Print($"Ошибка: Ресурс Data не присвоен");
        }
    }
}
