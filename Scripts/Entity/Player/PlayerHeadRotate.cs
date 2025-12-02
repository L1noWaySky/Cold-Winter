using Godot;


public partial class PlayerHeadRotate : Node3D
{
	[Export] float Sens = 2;
	Node3D PlayerCameraRotateX;
	float RotationX = 0f;
	


	public override void _Ready()
    {
        PlayerCameraRotateX = this.GetChild<Node3D>(0);
		Input.MouseMode = Input.MouseModeEnum.Captured;
    }
	public override void _Process(double delta)
    {
        
    }


    public override void _UnhandledInput(InputEvent @ThisEvent)
    {
        if (@ThisEvent is InputEventMouseMotion MouseMotionNow)
        {
            this.RotateY(-MouseMotionNow.Relative.X * (0.001f * Sens));

			RotationX += -MouseMotionNow.Relative.Y * (0.001f * Sens);
			RotationX = Mathf.Clamp(RotationX, Mathf.DegToRad(-75), Mathf.DegToRad(90));
			Vector3 newRotate = PlayerCameraRotateX.Rotation;
			newRotate.X = RotationX;
			PlayerCameraRotateX.Rotation = newRotate;
        }
    }

}
