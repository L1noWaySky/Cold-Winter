using Godot;

public partial class Hud : Control
{
	[Export] Entity PlayerScript;
	[Export] Control Aim;
	[Export] public AnimatedSprite2D AimVisual;
	[Export] public Sprite2D Hand;
	[Export] public Label HP;
	

	bool isAim;
	[Export] public bool IsAim
	{
		get => isAim;
		set
		{
			if (isAim == value) return;
			isAim = value;

			if (value) { AimVisual.Play("active"); }
			else { AimVisual.Play("default"); }
		}
	}
	[Export] public bool IsCanTake
	{
		get => Hand.Visible;
		set
		{
			if (Hand.Visible == value) return;
			Hand.Visible = value;
		}
	}
	
	

    public override void _Ready()
    {
        base._Ready();
	
		PlayerScript.Data.HealthIsUpdate += Update;
    }


	void Update()
	{
		HP.Text = PlayerScript.Data.CurrentHealth.ToString();
	}

	
}
