using Godot;
using System;

public partial class MovementCommand : PaddleCommand
{
    public override void Execute(Paddle paddle, GodotObject data = null)
    {
        if(!(data is MovementParams movementParams))
            return;

        paddle.Move(movementParams.Input);
    }

}

public partial class MovementParams : GodotObject
{
    public Vector2 Input { get; set; }

    public MovementParams (Vector2 input)
    {
        Input = input;
    }
}