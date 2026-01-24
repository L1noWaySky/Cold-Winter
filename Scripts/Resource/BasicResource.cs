using Godot;

public partial class BasicResource : Resource
{
    [Export] public string Name {get; set;}

    public virtual void Init()
    {
        GD.Print($"Создан ресурс узлу {this.Name}");
    }
}
