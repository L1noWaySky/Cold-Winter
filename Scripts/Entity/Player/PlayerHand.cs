using Godot;


public partial class PlayerHand : Node3D
{
	[Export] CharacterBody3D Player;
    [Export] float BobAmplitudeX = 0.015f;
    [Export] float BobAmplitudeY = 0.01f;
    [Export] float BobSpeed = 10f;
    [Export] float BobReturnSpeed = 2f;    
    [Export] float HandRotateX = Mathf.DegToRad(9f);
    [Export] float HandSquatPositionY = -0.08f;
    float _time = 0f;
    Vector3 DefaultPosition;
    Vector3 DefaultRotation;
    float HandMoveZ = 0f;
    float HandMoveX = 0f;
    float HandMoveY = 0f;
    float UpdateHandPositionY = 0;
	public override void _Ready()
    {
        DefaultPosition = this.Position;
        DefaultRotation = this.Rotation;
    }

	public override void _Process(double delta)
    {
        bool PlayerIsSquat = (bool)Player.Get("IsSquat");

        UpdateHandPositionY = Mathf.Lerp(
            UpdateHandPositionY,
            (
                PlayerIsSquat
                ?
                DefaultPosition.Y + HandSquatPositionY
                :
                DefaultPosition.Y
            ),
            (float)delta * 3
        );

        Vector3 CurrentDefaultPosition = DefaultPosition;
            
        //GD.Print($"{(int)_Player.Get("SpeedRealInTimeX")}, {(int)_Player.Get("SpeedRealInTimeZ")}");

        float PlayerSpeed = (Player.Velocity with { Y = 0 }).Length();
        if(PlayerSpeed > 0.01f)
        {
            _time += (float)delta * BobSpeed;
        }
        else
        {
            _time = Mathf.Lerp(_time, 0f, (float)delta * BobReturnSpeed);
        }
        if (_time < 0.01f)
        {
            _time = Mathf.Floor(_time);
        }

        
            HandMoveX = Mathf.Sin(_time * 0.8f) * BobAmplitudeX * PlayerSpeed;
            HandMoveZ = Mathf.Abs(Mathf.Sin(_time * 0.8f) * BobAmplitudeY * PlayerSpeed);
        
        Vector3 Bobing = new Vector3(HandMoveX, HandMoveZ, this.Position.Z);
        
        this.Position = (CurrentDefaultPosition with { Y = UpdateHandPositionY}) + Bobing;

        
        this.Rotation = Vector3.Right * Mathf.Lerp(
            this.Rotation.X,
            PlayerSpeed > 1f && Player.IsOnFloor() ? (DefaultRotation.X - Mathf.DegToRad(PlayerSpeed*1.2f)) : DefaultRotation.X,
            (float)delta * 7f
        );
    }
}
