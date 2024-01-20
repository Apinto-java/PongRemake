using System;
using Godot;

public partial class HumanController : PaddleController
{

    public HumanController(Paddle paddle) : base(paddle)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        var movementInput = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
        if(movementInput == 0)
            return;

        var velocity = new Vector2(0.0f, movementInput * Paddle.MovementSpeedByKeyOrJoystick * (float)delta);
        MovementCommand.Execute(Paddle, new MovementParams(velocity));
    }

    public override void _Input(InputEvent inputEvent)
    {
        if(!(inputEvent is InputEventMouseMotion mouseMotion))
            return;

        Vector2 newMousePosition = mouseMotion.Position;
        Vector2 targetPosition = new Vector2(Paddle.GlobalPosition.X, newMousePosition.Y);

        MovementCommand.Execute(Paddle, new MovementParams(targetPosition - Paddle.GlobalPosition));
    }
}