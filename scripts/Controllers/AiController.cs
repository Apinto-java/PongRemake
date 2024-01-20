using System;
using Godot;

public partial class AiController : PaddleController
{
    public Ball Ball { get; set; }
    public float AIPaddleSpeed = 600.0f;

    public AiController(Paddle paddle, Ball ball) : base(paddle)
    {
        Ball = ball;
    }

    public override void _PhysicsProcess(double delta)
    {
        // If the ball is moving left, do nothing
        if(Ball.Velocity.X < 0)
            return;

        var ballPosition = Ball.GlobalPosition;
        var direction = (ballPosition - Paddle.GlobalPosition).Normalized();
        direction.X = 0;
        float distance = ballPosition.DistanceTo(Paddle.GlobalPosition);

        var predictedBallPosition = ballPosition + Ball.Velocity * (distance / AIPaddleSpeed);

        //If the paddle already is where the ball is going to land
        if(predictedBallPosition == Paddle.GlobalPosition)
            return;

        var movement = direction * AIPaddleSpeed * (float)delta;

        MovementCommand.Execute(Paddle, new MovementParams(movement));
    }
}