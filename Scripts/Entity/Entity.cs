// ! Этот скрипт работает лишь с родителем типа Charapter Body 3D!

using Godot;

public partial class Entity : ScriptWithResource<EntityR>
{
    [Signal] delegate void DamagedEventHandler();

    [Export] public new EntityR Data
    {
        get => base.Data;
        set => base.Data = value;
    }
    [Export] public float ProtectedTime = 0.5f;


	public CharacterBody3D This;
    float _time; float time
    {
        get => _time;
        set
        {
            if(value==ProtectedTime) return;
            value = _time;
        }
    }
    bool Protected = false;

	public override void _Ready()
    {
        base._Ready();
        
        This = this.GetParent<CharacterBody3D>();
        

        Data.Die += Die;
        Data.HealthIsLow += LowHealthReaction;
    }

    public void TakeDamage(int damage)
    {
        if (Protected == false)
        {
            Data.Damage(damage);
            this.EmitSignal(SignalName.Damaged);
        }
    }
    public void TakeDamage(int damage, Vector3 PushVelocity)
    {
        this.TakeDamage(damage);
        This.Velocity = PushVelocity;
    }
    public virtual void LowHealthReaction()
	{
		GD.Print($"{Data.Name} low health");
	}
	public virtual void Die()
	{
		GD.Print($"{Data.Name} is die");
        This.QueueFree();
	}

}
