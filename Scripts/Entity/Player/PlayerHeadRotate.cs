using Godot;


public partial class PlayerHeadRotate : Node3D
{
	[Export] float Sens = 2;
	Node3D PlayerCameraRotateX;
	float RotationX = 0f;
    public bool IsCameraRotate = true;
	


	public override void _Ready()
    {
        PlayerCameraRotateX = this.GetChild<Node3D>(0);
		if (IsCameraRotate) {Input.MouseMode = Input.MouseModeEnum.Captured;}
    }
	public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Escape"))
        {
            IsCameraRotate = !IsCameraRotate;

            if (IsCameraRotate)
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
            else
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            
        }
    }


    public override void _UnhandledInput(InputEvent @ThisEvent)
    {
        if (@ThisEvent is InputEventMouseMotion MouseMotionNow && IsCameraRotate)
        {
            this.RotateY(-MouseMotionNow.Relative.X * (0.001f * Sens));

			RotationX += -MouseMotionNow.Relative.Y * (0.001f * Sens);
			RotationX = Mathf.Clamp(RotationX, Mathf.DegToRad(-80), Mathf.DegToRad(90));
			Vector3 newRotate = PlayerCameraRotateX.Rotation;
			newRotate.X = RotationX;
			PlayerCameraRotateX.Rotation = newRotate;
        }
    }

}
