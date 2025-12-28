using Godot;


public partial class PlayerMove : CharacterBody3D
{
    #region Сигналы
        [Signal] delegate void PlayerJumpedEventHandler();
        [Signal] delegate void PlayerLandedEventHandler();
    #endregion
    #region Свойства
	[Export] Node3D Head;
    [Export] RayCast3D CheckForSquat;
    [Export] Timer _timer;
    [ExportGroup("Acceleration")]
        [Export] float acceleration = 5f;
        [Export] float accelerationAir = 2f;
    [ExportGroup("Deceleration")]
        [Export] float deceleration = 10f;
        [Export] float decelerationAir = 0.8f;
    [ExportGroup("Speed")]
        [Export] float MaxSpeedOnFloor = 5f;
        [Export] float MaxSpeedUnFloor = 3;
        [Export] float MaxSpeedOnSit = 3f;
        [Export] float maxSpeedOnSprint = 8f;
    [ExportGroup("Endurance")]
        [Export] float EnduranceMax = 200f;
        [ExportGroup("Endurance/Pay")]
            [Export] float JumpP = 10;
            [Export] float WalkP = 0.5f;
            [Export] float RunP = 5f;
        [ExportGroup("Endurance/Jump")]
            [Export] float Jump = 4.5f;
        [ExportGroup("Endurance/Recovery")]
            [Export] float WaitTime = 3f;
            [Export] float RecoveryPay = 10f;
    protected bool IsSquat { get; private set; }
    protected bool IsMoving { get; private set; }
    protected bool IsRuning { get; private set; }
    protected bool CanControl { get; set; }
    protected bool CanEnduranceRecovery { get; private set; }
    Vector3 LastMove = Vector3.Zero;
    protected Vector2 Direction {get; private set;}
    float currentEndurance;
    public float CurrentEndurance
    {
        get { return currentEndurance; }
        private set
        {
            if (Mathf.IsEqualApprox(currentEndurance, value)) return;

            bool decreased = value < currentEndurance;
            currentEndurance = Mathf.Clamp(value, 0f, EnduranceMax);

            if (decreased)
            {
                CanEnduranceRecovery = false;
                if (_timer != null)
                    _timer.Start(WaitTime);
            }
        }
    }

    #endregion
	
	public override void _Ready()
    {
        IsSquat = false;
        IsMoving = false;
        IsRuning = false;
        CanControl = true;
        CanEnduranceRecovery = true;
        CurrentEndurance = EnduranceMax;
    }

    public override void _PhysicsProcess(double delta)
    {
		#region Переменные и тд

        Vector3 _Velocity = this.Velocity;

        float RunEnduranceBorder = EnduranceMax/2f;

        if(this.IsOnFloor()) 
            //LastMove = this.Velocity with {Y = 0};
            LastMove = new Vector3(
                Mathf.Clamp(this.Velocity.X, 0, 1),
                0,
                Mathf.Clamp(this.Velocity.Z, 0, 1)
            );
        float currentSpeed =
            this.IsOnFloor()
            ?
            (IsSquat ? MaxSpeedOnSit : (Input.IsActionPressed("ShiftL")&&CurrentEndurance>RunEnduranceBorder ? maxSpeedOnSprint : MaxSpeedOnFloor))
            :
            MaxSpeedUnFloor;
        float currentVelocityAcceleration =
            Direction != Vector2.Zero
            ?
            (
                this.IsOnFloor()
                ?
                acceleration
                :
                accelerationAir
            )
            :
            (
                this.IsOnFloor()
                ?
                deceleration
                :
                decelerationAir
            );
        
        if (Input.IsActionJustPressed("CntrL") && this.IsOnFloor())
        {
            if (!CheckForSquat.IsColliding()) { 
                IsSquat = !IsSquat; 
                GD.Print($"Squat: {IsSquat}");
            }
            
        }

        IsMoving = 
            (this.Velocity with {Y=0}).Length() > 0.1f && Direction!=Vector2.Zero
            ?
            true
            :
            false;
        IsRuning = 
            (this.Velocity with {Y=0}).Length() > 1f && Direction!=Vector2.Zero && Input.IsActionPressed("ShiftL")
            ?
            true
            :
            false;
        #endregion
        #region Прыжок и гравитация
		if (!this.IsOnFloor())
        {
            _Velocity += this.GetGravity() *(float)delta;

            if (IsSquat != false) IsSquat = false;
        }

        if (Input.IsActionJustPressed("Space") && this.IsOnFloor() && IsSquat==false && CurrentEndurance>100)
        {
            _Velocity.Y = Jump;
            this.EmitSignal(SignalName.PlayerJumped);
        }
        #endregion
        #region Движение и направление 
        if (this.CanControl) Direction = Input.GetVector("A", "D", "W", "S").Normalized();
		_Velocity = Head.Basis * new Vector3(
            currentSpeed * Direction.X,
            _Velocity.Y,
            currentSpeed * Direction.Y
        );
        #endregion
        #region Выносливость
        if (this.IsMoving && this.IsOnFloor() && CurrentEndurance >= 0)
        {
            CurrentEndurance -= 
                this.IsRuning
                ?
                RunP * (float)delta
                :
                WalkP * (float)delta;
        }
        
        CurrentEndurance = Mathf.Clamp(
            CurrentEndurance,
            0f,
            EnduranceMax
        );

        if (CanEnduranceRecovery && CurrentEndurance < EnduranceMax)
        {
            CurrentEndurance += RecoveryPay * (float)delta;
            CurrentEndurance = Mathf.Clamp(CurrentEndurance, 0f, EnduranceMax);
        }

        this.PlayerJumped -= playerJumped;
        this.PlayerJumped += playerJumped;
        #endregion
        void PrintDebug()
        {
            //GD.Print($"X: {(int)_Velocity.X}");
            //GD.Print($"Z: {(int)_Velocity.Z}");
            //GD.Print($"LastMove:{LastMove}");
            //GD.Print($"currentAcceleration:{currentVelocityAcceleration}");
            GD.Print($"CurrentEndurance:{CurrentEndurance}");
            GD.Print($"Can Endurance Recovery: {CanEnduranceRecovery}");
        }
        PrintDebug();

        
		this.Velocity =  new Vector3(
            Mathf.Lerp(this.Velocity.X, _Velocity.X, (float)delta * currentVelocityAcceleration),
            _Velocity.Y,
            Mathf.Lerp(this.Velocity.Z, _Velocity.Z, (float)delta * currentVelocityAcceleration)
        );
		this.MoveAndSlide();
    }

    public void OnWaitTimerTimeout() => CanEnduranceRecovery = true;

    public void CutEndurance(float cutValue) => CurrentEndurance -= cutValue;

    void playerJumped() => CutEndurance(JumpP);

}
