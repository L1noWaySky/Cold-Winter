using Godot;


public partial class PlayerCamera : Camera3D
{
	[Export] CharacterBody3D Player;
	[Export] float DefaultFov = 75f;
	[Export] float SprintFov = 80f;
	[Export] float SitFov = 70f;
	[Export] float FovAcceleration = 1f;
	[Export] float FovDeceleration = 0.3f;
    [Export] float RotateStrength = Mathf.DegToRad(2f);
    [Export] float RotateAcceleration = 30f;
    float DefaultRotateZ = 0;
    float UpdateRotationZ = 0;
    

    public override void _Ready()
    {
        DefaultRotateZ = this.Rotation.Z;
    }

	
	
	public override void _PhysicsProcess(double delta)
    {
        float PlayerSpeed = (Player.Velocity with {Y = 0f}).Length();
        Vector2 PlayerDirection = (Vector2)Player.Get("Direction");

        if (PlayerSpeed > 5.5f)
        {
            this.Fov = Mathf.MoveToward(this.Fov, SprintFov, FovAcceleration);
        }
        else
        {
            this.Fov = Mathf.MoveToward(this.Fov, DefaultFov, FovDeceleration);
        }

        UpdateRotationZ = Mathf.MoveToward(UpdateRotationZ, RotateStrength * PlayerDirection.X * PlayerSpeed * 0.5f, (float)delta);
        this.Rotation = Vector3.Forward * (float)Mathf.Lerp(
            this.Rotation.Z, 
            //RotateStrength * PlayerDirection.X * (PlayerSpeed),
            UpdateRotationZ,
            RotateAcceleration * (float)delta
        );
        
        //GD.Print(Mathf.RadToDeg(this.Rotation.Z));
        GD.Print(UpdateRotationZ);

    }
}
