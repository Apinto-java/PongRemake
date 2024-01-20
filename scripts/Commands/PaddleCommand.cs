using Godot;
using System;

[GodotClassName("PaddleCommand")]
public abstract partial class PaddleCommand : GodotObject
{
    public abstract void Execute(Paddle paddle, GodotObject data = null);
}
