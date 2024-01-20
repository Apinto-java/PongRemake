using Godot;
using System;

[GodotClassName("PlayerController")]
public partial class PaddleController : Node
{
    public Paddle Paddle { get; set; }
    public MovementCommand MovementCommand { get; set; } = new MovementCommand();

    public PaddleController(Paddle paddle)
    {
        Paddle = paddle;
    }
}
