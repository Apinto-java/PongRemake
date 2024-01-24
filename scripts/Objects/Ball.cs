using Godot;
using System;

public partial class Ball : CharacterBody2D, IStartable, IStoppable
{

	[Export] public float Radius { get; set; } = 8.0f;
	[Export] public Color DrawColor { get; set; }= new Color(1, 1, 1);
	[Export] public float Speed { get; set; } = 100.0f;
	private bool _canMove = false;
	[Signal] public delegate void BallHitPaddleEventHandler();

	/// <summary>
	/// Draws a circle using the DrawColor, Center and Radius parameters
	/// </summary>
	public override void _Draw()
	{
		Modulate = DrawColor;
		Vector2 center = new Vector2(0, 0);

		DrawCircle(center, Radius, DrawColor);
	}

	/// <summary>
	/// Starts the ball movement script, assigning at first a random Vector2 for the Ball Velocity.
	/// Intended to be called from the MainLevel script.
	/// </summary>
	public void Start()
	{
		Vector2 initialVector = GetRandomDirection();
		Velocity = initialVector;
		_canMove = true;
	}

	/// <summary>
	/// Assigns ZERO to the Ball's Velocity and sets _canMove to false
	/// </summary>
	public void Stop()
	{
		Velocity = Vector2.Zero;
		_canMove = false;
	}

	/// <summary>
	/// Executed every frame. It checks if the ball can move and if it can, the ball follows its previous trajectory
	/// unless it collided with something. It checks if the collider is a Paddle, if it is, then the PaddleHit signal is emmited.
	/// Once it collided with something, the Velocity is "bounced"
	/// </summary>
	/// <param name="delta">Ammount of miliseconds that passed since the last frame</param>
	public override void _PhysicsProcess(double delta)
	{
		if(!_canMove)
			return;

		var collision = MoveAndCollide((float)delta * Velocity);

		if(collision == null)
			return;

		// If the ball collided with something
		var collider = collision.GetCollider() as Node;

		if(collider != null && collider is Paddle paddle){
			EmitSignal(SignalName.BallHitPaddle);

			var collisionPosition = collision.GetPosition();
			var paddlePosition = paddle.GlobalPosition;
			// 	If hit == 1, it's a low hit, if hit == -1 it's a high hit, and if hit == 0 it's a center hit
			var hit = collisionPosition.Y <= paddle.Position.Y + Radius && collisionPosition.Y >= paddle.Position.Y - Radius ? 0 :
						collisionPosition.Y > paddle.Position.Y + Radius ? 1 : -1;
			
			Vector2 bouncedVelocity = new Vector2(-Velocity.X, 0);
			if(hit == 0)
				GD.Print("Center");

			if(hit == 1)
			{
				GD.Print("Low");
				bouncedVelocity.Y = Mathf.Abs(Velocity.Y);
			}

			if(hit == -1)
			{
				GD.Print("High");
				bouncedVelocity.Y = Math.Abs(Velocity.Y) * -1;
			}

			if(Velocity.Y == 0 && hit != 0)
			{
				GD.Print("Adding variation to the velocity");
				// If it's a high hit, it should rotate it upwards, if it's a low hit, rotate it downwards
				var rotation = hit == -1 ? 
								GD.RandRange(-25, -45) : 
								GD.RandRange(25, 45);
				var rotationRadians = Mathf.DegToRad(rotation);
				var originalBouncedVelocity = bouncedVelocity; //	For debugging purposes
				bouncedVelocity = bouncedVelocity.Normalized().Rotated(rotationRadians) * Speed;
				GD.Print("Rotation angle: ", rotation, 
						", Rotation radians: ", rotationRadians, 
						", Original velocity: ", Velocity, 
						", Bounced velocity: ", originalBouncedVelocity,
						", Bounced Velocity normalized and rotated times speed: ", bouncedVelocity);
				
			}

			Velocity = bouncedVelocity;
			return;
		}

		// Get the bounced direction (the normal vector of the impact)
		Vector2 bouncedDirection = Velocity.Bounce(collision.GetNormal());
		Velocity = bouncedDirection;
	}


	/// <summary>
	/// Returns a Vector2 containing the Velocity the Ball will have when the game starts/restarts
	/// </summary>
	/// <returns>Vector2 containing the new Ball Velocity</returns>
	private Vector2 GetRandomDirection()
	{
		Vector2 initialVector = new Vector2();
		double direction = GD.RandRange(-1.0, 1.0);
		direction = direction <= 0 ? -1 : 1;
		initialVector.X = (float)direction;
		
		double rotationAngle = 0;
		rotationAngle = direction == 1 ? GD.RandRange(-25, 25) : GD.RandRange(-155, -235);
		rotationAngle = Mathf.DegToRad(rotationAngle);
		GD.Print("Rotation angle: ", rotationAngle);
		initialVector = initialVector.Rotated((float)rotationAngle);
		initialVector = initialVector.Normalized();
		initialVector *= Speed;
		GD.Print("Resulting vector: ", initialVector);
		return initialVector;
	}
}
