// ! Этот скрипт работает лишь с родителем типа Charapter Body 3D!

using Godot;



public partial class PhysicsEntity : Entity
{
    [ExportGroup("Deceleration")]
        [Export] float deceleration = 10f;
        [Export] float decelerationAir = 0.8f;

    Vector3 mv; public Vector3 MainVelocity
    {
        get => This.Velocity;
        set
        {
            if (This.Velocity==value) return;
            This.Velocity = value;
        }
    }
    protected Vector2 Direction {get; set;}
    protected Vector3 ThisVelocity {get; set;}

    float currentVelocityY = 0;
    float UnFloorTime = 0;
    bool Activate = true;

    public override void _PhysicsProcess(double delta)
    {
        
        float currentVelocityAcceleration =
                This.IsOnFloor()
                ?
                deceleration
                :
                decelerationAir;


        




		if (!This.IsOnFloor())
        {
            MainVelocity += This.GetGravity() *(float)delta;
            currentVelocityY = MainVelocity.Y;
            UnFloorTime += 1;
            Activate = true;
        }
        if (This.IsOnFloor())
        {
            if (Activate)
            {
                HighDamage();
                Activate = !Activate;
            }
            
        }


	 	this.MainVelocity = new Vector3(
            this.MainVelocity.X < 0.001 ? 0 : Mathf.Lerp(this.MainVelocity.X, 0f, (float)delta * currentVelocityAcceleration),
            this.MainVelocity.Y,
            this.MainVelocity.Z < 0.001 ? 0 : Mathf.Lerp(this.MainVelocity.Z, 0f, (float)delta * currentVelocityAcceleration)
        );

        
		This.MoveAndSlide();
    }

    public void HighDamage()
    {
        GD.Print(currentVelocityY);
        GD.Print(UnFloorTime);

        if (currentVelocityY < -10f)
        {
            int damage = (int)(currentVelocityY * 0.7f);
            GD.Print($"Get damage: {currentVelocityY} to {damage}");
            TakeDamage(-damage);
        }

        currentVelocityY = 0;
        UnFloorTime = 0;
    }
}
