using Godot;

public partial class Entity : Node
{
    [Signal] public delegate void eveEventHandler();

 	[Export] int Health = 0;
    [Export] int LowHealthReactionIf = 0;

	CharacterBody3D This;

	public override void _Ready()
    {
        This = this.GetParent<CharacterBody3D>();
    }

	
	public override void _Process(double delta)
    {
        if (this.Health <= LowHealthReactionIf) {LowHealthReaction();}
        if (this.Health <= 0) { Die(); }


    }

	public void Damage(int Damage)
    {
        this.Health = this.Health - Damage;
    }

	public void Damage(int Damage, Vector3 PushRotate)
    {
        this.Health = this.Health - Damage;

		This.Velocity = PushRotate;
    }

    public void LowHealthReaction()
    {
        
    }

	public void Die()
    {
        GD.Print($"{This.Name} is Die");
    }
}
