using Godot;

public partial class PlayerSquat : CollisionShape3D
{
	#region properties
	[Export] Node3D PlayerHead;
	[Export] CharacterBody3D Player;
	[Export] float CollisionSquatHeight = 1.3f;
	[Export] float CollisionSqautPositionY = 0.35f;
	float CollisionDefaultHeight;
	float CollisionDefaultPositionY;
	float HeadSquatPositionY = 0f;
	float HeadDefaultPositionY;

	float UpdateCollisionSquatHeight = 0f;
	float UpdateCollisionSqautPositionY = 0f;
	float UpdateHeadSquatPositionY = 0f;
	float UpdateMVNPositionY = 0f;

	float SquatAcceleration = 2f;
	float SquatDeceleration = 1f;
	#endregion
	public override void _Ready()
    {
		CollisionDefaultHeight = ((CapsuleShape3D)this.Shape).Height;
		CollisionDefaultPositionY = (float)this.Position.Y;
		HeadDefaultPositionY = PlayerHead.Position.Y;
		
		UpdateHeadSquatPositionY = HeadDefaultPositionY;
		UpdateCollisionSquatHeight = CollisionDefaultHeight;
		UpdateCollisionSqautPositionY = CollisionDefaultPositionY;
    }

	
	public override void _PhysicsProcess(double delta)
    {
		bool PlayerIsSquat = (bool)Player.Get("IsSquat");
		//Положение камеры по Y
		UpdateHeadSquatPositionY = 
			PlayerIsSquat
			?
				Mathf.MoveToward(
					UpdateHeadSquatPositionY,
					HeadSquatPositionY,
					(float)delta * SquatAcceleration
				)
			:
				Mathf.MoveToward(
					UpdateHeadSquatPositionY,
					HeadDefaultPositionY,
					(float)delta * SquatDeceleration
				);
		if (PlayerHead.Position.Y!=UpdateHeadSquatPositionY) {PlayerHead.Position = Vector3.Up * (UpdateHeadSquatPositionY);}

		#region Понижение колизии игрока
		UpdateCollisionSquatHeight = 
			PlayerIsSquat
			?
				Mathf.MoveToward(
					UpdateCollisionSquatHeight,
					CollisionSquatHeight,
					(float)delta * SquatAcceleration
				)
			:
				Mathf.MoveToward(
					UpdateCollisionSquatHeight,
					CollisionDefaultHeight,
					(float)delta * SquatDeceleration
				);
		#endregion
		if (((CapsuleShape3D)this.Shape).Height != UpdateCollisionSquatHeight) {((CapsuleShape3D)this.Shape).Height = UpdateCollisionSquatHeight;}
 
		#region Понижение позиции колизии игрока по Y
		UpdateCollisionSqautPositionY =
			PlayerIsSquat
			?
				Mathf.MoveToward(
					UpdateCollisionSqautPositionY,
					-CollisionSqautPositionY,
					(float)delta 
				)
			:
				Mathf.MoveToward(
					UpdateCollisionSqautPositionY,
					CollisionDefaultPositionY,
					(float)delta * 0.5f
				);
		#endregion
		if (this.Position.Y != UpdateCollisionSqautPositionY) { this.Position = Vector3.Up * UpdateCollisionSqautPositionY; }
	}

}
