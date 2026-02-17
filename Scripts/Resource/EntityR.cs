using Godot;

[GlobalClass]
public partial class EntityR : BasicResource
{
	[Signal] public delegate void DieEventHandler();
	[Signal] public delegate void HealthIsLowEventHandler();
	[Signal] public delegate void HealthIsUpdateEventHandler();
	[Export] public int MaxHealth {get; set;}
	[Export] public int LowHealthReactionIf {get; set;}
	
	int currentHealth;
	public int CurrentHealth
	{
		get
		{
			return currentHealth;
		}
		set
		{
			currentHealth = value;

			EmitSignal(SignalName.HealthIsUpdate);
		}
	}

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
