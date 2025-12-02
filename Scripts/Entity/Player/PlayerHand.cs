using Godot;


public partial class PlayerHand : Node3D
{
	[Export] CharacterBody3D _Player;
    [Export] float BobAmplitudeX = 0.015f;
    [Export] float BobAmplitudeY = 0.01f;
    [Export] float BobSpeed = 10f;
    [Export] float BobReturnSpeed = 2f;
    [Export] float HandMoveY = 0f;
    [Export] float HandMoveX = 0f;
    [Export] float HandRotateX = Mathf.DegToRad(9f);
    float _time = 0f;
    Vector3 DefaultPosition;
    Vector3 DefaultRotation;
	public override void _Ready()
    {
        DefaultPosition = this.Position;
        DefaultRotation = this.Rotation;
    }

	public override void _Process(double delta)
    {
        float currentRotateX = DefaultRotation.X - HandRotateX;
        //GD.Print($"{(int)_Player.Get("SpeedRealInTimeX")}, {(int)_Player.Get("SpeedRealInTimeZ")}");
        float PlayerSpeed = (_Player.Velocity with { Y = 0 }).Length();
        if(PlayerSpeed > 0.5f)
        {
            _time += (float)delta * BobSpeed;

            
        }
        else
        {
            _time = Mathf.Lerp(_time, 0f, (float)delta * BobReturnSpeed);
            //_time = Mathf.MoveToward(_time, 0f, BobReturnSpeed * (float)delta);
        }
        if (_time < 0.01f)
        {
            _time = Mathf.Floor(_time);
        }

        HandMoveX = Mathf.Sin(_time * 0.8f) * BobAmplitudeX * PlayerSpeed;
        HandMoveY = Mathf.Abs(Mathf.Sin(_time * 0.8f) * BobAmplitudeY * PlayerSpeed);
        Vector3 Bobing = new Vector3(HandMoveX, HandMoveY, this.Position.Z);

        this.Position = DefaultPosition + Bobing;

        this.Rotation = Vector3.Right * Mathf.Lerp(
            this.Rotation.X, 
            PlayerSpeed > 1f && _Player.IsOnFloor() ? (DefaultRotation.X - HandRotateX) : DefaultRotation.X,
            (float)delta * 7f
        );
        
        //GD.Print(HandMoveX);
    }
}
