using Godot;

[GlobalClass]
public partial class EntityR : BasicResource
{
	[Signal] public delegate void DieEventHandler();
	[Signal] public delegate void HealthIsLowEventHandler();
	[Export] public int MaxHealth {get; set;}
	[Export] public int LowHealthReactionIf {get; set;}
	int CurrentHealth;



	public override void Init() 
	{
		base.Init();
		CurrentHealth = MaxHealth;
	}
	public virtual void Damage(int damage)
	{
		this.CurrentHealth -= damage;

		if (this.CurrentHealth <= LowHealthReactionIf) { EmitSignal(SignalName.HealthIsLow); }
        if (this.CurrentHealth <= 0) { EmitSignal(SignalName.Die); }
	}
	
}
