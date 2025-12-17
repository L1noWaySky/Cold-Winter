using Godot;


public partial class PlayerMove : CharacterBody3D
{
    #region Свойства
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
    #endregion
	
	public override void _Ready()
    {
        IsSquat = false;
    }

    public override void _PhysicsProcess(double delta)
    {
		#region Переменные и тд

        Vector3 _Velocity = this.Velocity;

        if(this.IsOnFloor() /* && (float)(this.Velocity with {Y = 0}).Length() != 0f */ ) 
            LastMove = this.Velocity with {Y = 0};
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

        #endregion
        #region Прыжок и гравитация
		if (!this.IsOnFloor())
        {
            _Velocity += this.GetGravity() *(float)delta;

            if (IsSquat != false) IsSquat = false;
        }

        if (Input.IsActionJustPressed("Space") && this.IsOnFloor() && IsSquat==false)
        {
            _Velocity.Y = Jump;

        }
        #endregion
        #region Движение и направление 
        Direction = Input.GetVector("A", "D", "W", "S");
		

		if (Direction != Vector2.Zero)
        {
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
        }
        else
        {
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
        #endregion

        void PrintDebug()
        {
            GD.Print($"X: {(int)_Velocity.X}");
            GD.Print($"Z: {(int)_Velocity.Z}");
            GD.Print($"LastMove:{LastMove}");
            GD.Print($"currentAcceleration:{currentSpeedAcceleration}");
        }
        //PrintDebug();
        GD.Print($"LastMove:{LastMove}");

        _Velocity = Head.Basis * new Vector3(SpeedRealInTimeX, _Velocity.Y, SpeedRealInTimeZ);
		this.Velocity = _Velocity;
		this.MoveAndSlide();
    }

}
