using Godot;

public partial class PlayerSquat : CollisionShape3D
{
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
		/* Первый казус
        if (PlayerIsSquat == true)
        {
			//while (PlayerHead.Position.Y != HeadSquatPositionY)
			{
				
			}
			UpdateHeadSquatPositionY = Mathf.Lerp(
					UpdateHeadSquatPositionY,
					HeadSquatPositionY,
					(float)delta * SquatAcceleration
			);

			PlayerHead.Position =  Vector3.Up * (PlayerHead.Position.Y!=HeadSquatPositionY ? UpdateCollisionSqautPositionY : PlayerHead.Position.Y);
				
        }
		else if (PlayerIsSquat == false)
        {
            //while (PlayerHead.Position.Y != HeadDefaultPositionY)
            {
				
            }

			UpdateHeadSquatPositionY = Mathf.MoveToward(
					UpdateHeadSquatPositionY,
					HeadDefaultPositionY,
					(float)delta * SquatDeceleration
			);

			PlayerHead.Position = Vector3.Up * (PlayerHead.Position.Y!=HeadDefaultPositionY ? UpdateCollisionSqautPositionY : PlayerHead.Position.Y);
        }
		*/

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
		if (PlayerHead.Position.Y!=UpdateHeadSquatPositionY) {PlayerHead.Position = Vector3.Up * (UpdateHeadSquatPositionY);} // Выглядит не очень, но ладно. -_-
		//GD.Print(UpdateHeadSquatPositionY);

		//Понижение колизии игрока
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
		if (((CapsuleShape3D)this.Shape).Height != UpdateCollisionSquatHeight) {((CapsuleShape3D)this.Shape).Height = UpdateCollisionSquatHeight;}
		//GD.Print(UpdateCollisionSquatHeight);

		//Понижение позиции колизии игрока по Y
		UpdateCollisionSqautPositionY =
			PlayerIsSquat
			?
				Mathf.MoveToward(
					UpdateCollisionSqautPositionY,
					-CollisionSqautPositionY,
					(float)delta * (SquatAcceleration * 0.85f)
				)
			:
				Mathf.MoveToward(
					UpdateCollisionSqautPositionY,
					CollisionDefaultPositionY,
					(float)delta * SquatDeceleration
				);
		if (this.Position.Y != UpdateCollisionSqautPositionY) { this.Position = Vector3.Up * UpdateCollisionSqautPositionY; }

		//Понижение визуальных узлов
		/*
		UpdateMVNPositionY =
			PlayerIsSquat
			?
				Mathf.MoveToward(
					UpdateMVNPositionY,
					UpdateMVNPositionY,
					(float)delta * SquatAcceleration
				)
			:
				Mathf.MoveToward(
					UpdateMVNPositionY,
					MainVisualNodesDefaultPositionY,
					(float)delta * SquatDeceleration
				);
		if (MainVisualNodes.Position.Y!=UpdateMVNPositionY) { MainVisualNodes.Position = Vector3.Up * UpdateMVNPositionY; }
		*/
	}

}
