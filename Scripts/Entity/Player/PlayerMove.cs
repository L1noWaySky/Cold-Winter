using Godot;


public partial class PlayerMove : CharacterBody3D
{
	[Export] Node3D Head;
    [Export] RayCast3D CheckForSquat;
	//[Export] float Speed = 10f;
    [Export] float acceleration = 20f;
    [Export] float accelerationAir = 3f;
    [Export] float deceleration = 50f;
    [Export] float MaxSpeedOnFloor = 5f;
    [Export] float MaxSpeedUnFloor = 3;
    [Export] float MaxSpeedOnSit = 3f;
    [Export] float maxSpeedOnSprint = 8f;
    [Export] float Jump = 4.5f;
    public float SpeedRealInTimeX = 0;
    public float SpeedRealInTimeZ = 0;
    public bool IsSquat;
    Vector3 LastMove = Vector3.Zero;
    public Vector2 Direction = Vector2.Zero;

	
	public override void _Ready()
    {
        IsSquat = false;
    }


    public override void _PhysicsProcess(double delta)
    {
		Vector3 _Velocity = this.Velocity;
        float currentSpeedAcceleration = 
            this.IsOnFloor()
            ?
            acceleration
            :
            accelerationAir;
        float currentSpeed =
            this.IsOnFloor()
            ?
            (IsSquat ? MaxSpeedOnSit : (Input.IsActionPressed("ShiftL") ? maxSpeedOnSprint : MaxSpeedOnFloor))
            :
            MaxSpeedUnFloor;
        
        if (Input.IsActionJustPressed("CntrL") && this.IsOnFloor())
        {
            if (!CheckForSquat.IsColliding()) { 
                IsSquat = !IsSquat; 
                GD.Print($"Squat: {IsSquat}");
            }
            
        }

		if (!this.IsOnFloor())
        {
            _Velocity += this.GetGravity() *(float)delta;

            if (IsSquat != false) IsSquat = false;
        }

        if (Input.IsActionJustPressed("Space") && this.IsOnFloor() && IsSquat==false)
        {
            _Velocity.Y = Jump;

        }

        Direction = Input.GetVector("A", "D", "W", "S");
        Vector3 MoveDirection = (Head.Basis * new Vector3(Direction.X, 0, Direction.Y)).Normalized();
		

		if (Direction != Vector2.Zero)
        {
            //_Velocity = MoveDirection * Speed;
            SpeedRealInTimeX = Mathf.MoveToward(
                SpeedRealInTimeX,
                currentSpeed * Direction.X,
                currentSpeedAcceleration * (float)delta
            );
            SpeedRealInTimeZ = Mathf.MoveToward(
                SpeedRealInTimeZ,
                currentSpeed * Direction.Y,
                currentSpeedAcceleration * (float)delta
            );
            LastMove = new Vector3(MoveDirection.X, 0, MoveDirection.Y);
        }
        else
        {
            //_Velocity.X = Mathf.MoveToward(Velocity.X, 0, 2f);
			//_Velocity.Z = Mathf.MoveToward(Velocity.Z, 0, 2f);
            SpeedRealInTimeX = Mathf.MoveToward(
                SpeedRealInTimeX,
                0,
                currentSpeedAcceleration * (float)delta
            );
            SpeedRealInTimeZ = Mathf.MoveToward(
                SpeedRealInTimeZ,
                0,
                currentSpeedAcceleration * (float)delta
            );
        }
        void PrintDebug()
        {
            GD.Print($"X: {(int)_Velocity.X}");
            GD.Print($"Z: {(int)_Velocity.Z}");
            GD.Print($"LastMove:{LastMove}");
            GD.Print($"currentAcceleration:{currentSpeedAcceleration}");
        }
        //PrintDebug();
        
        //GD.Print($"{this.GetWallNormal()}");
        _Velocity = Head.Basis * new Vector3(SpeedRealInTimeX, _Velocity.Y, SpeedRealInTimeZ);
		this.Velocity = _Velocity;
		this.MoveAndSlide();
    }

}
