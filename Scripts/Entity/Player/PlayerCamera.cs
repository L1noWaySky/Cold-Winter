using Godot;


public partial class PlayerCamera : Camera3D
{
	[Export] CharacterBody3D Player;
	[Export] float DefaultFov = 75f;
	[Export] float SprintFov = 80f;
	[Export] float SitFov = 70f;
	[Export] float FovAcceleration = 1f;
	[Export] float FovDeceleration = 0.3f;
    [Export] float RotateStrengthZ = Mathf.DegToRad(0.5f);
    [Export] float RotateAccelerationZ = 15f;
    [Export] float RotateStrengthX = Mathf.DegToRad(1f);
    [Export] float RotateAccelerationX = 25f;
    float DefaultRotateZ = 0;
    float UpdateRotationZ = 0;
    float UpdateRotationX = 0;
    
    enum HitDirection
    {
        Forward,
        Back,
        left,
        Right
    }
    

    public override void _Ready()
    {
        DefaultRotateZ = this.Rotation.Z;
    }

	void HitCamera(HitDirection HitDir, float HitStrength) 
    {
        switch (HitDir)
        {
            case HitDirection.left:
                break;
            case HitDirection.Right:
                break;
            case HitDirection.Forward:
                break;
            case HitDirection.Back:
                break;
        }
    }
	
	public override void _PhysicsProcess(double delta)
    {
        float PlayerSpeed = (Player.Velocity with {Y = 0f}).Length();
        float PlayerSpeedOnlyY = (Player.Velocity with {X = 0f, Z = 0f}).Length();
        Vector2 PlayerDirection = (Vector2)Player.Get("Direction");

        if (PlayerSpeed > 5.5f)
        {
            this.Fov = Mathf.MoveToward(this.Fov, SprintFov, FovAcceleration);
        }
        else
        {
            this.Fov = Mathf.MoveToward(this.Fov, DefaultFov, FovDeceleration);
        }



        UpdateRotationZ = Mathf.MoveToward(UpdateRotationZ, RotateStrengthZ * -PlayerDirection.X * PlayerSpeed * 0.5f, (float)delta);
        UpdateRotationX = Mathf.MoveToward(UpdateRotationX, RotateStrengthX * PlayerSpeedOnlyY * 0.3f, (float)delta * 4);
        this.Rotation = new Vector3(
            
            (float)Mathf.Lerp(
                this.Rotation.X,
                UpdateRotationX,
                RotateAccelerationX * (float)delta
            ),

            (float)this.Rotation.Y,

            (float)Mathf.Lerp(
                this.Rotation.Z, 
                //RotateStrength * PlayerDirection.X * (PlayerSpeed),
                UpdateRotationZ,
                RotateAccelerationZ * (float)delta
            )

        );
        
        
        HitCamera(HitDirection.Right, 1f);
        
        //GD.Print(Mathf.RadToDeg(this.Rotation.Z));
        //GD.Print(UpdateRotationZ);
        //GD.Print(UpdateRotationX);

    }
}
