using Godot;
using System;
using System.Collections.Generic;

public partial class Paddle : CharacterBody2D, IStartable, IStoppable
{

	[Export] public float MovementSpeedByKeyOrJoystick { get; set; } = 200.0f;
	private bool _canMove = false;
	private Node _controllerContainer;
	private PaddleController _paddleController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_controllerContainer = GetNode<Node>("ControllerContainer");
	}

	public void Start()
	{
		_canMove = true;
	}

	public void Stop()
	{
		_canMove = false;
	}

	public void SetController(PaddleController paddleController)
	{
		foreach(var child in _controllerContainer.GetChildren())
		{
			child.QueueFree();
		}

		_paddleController = paddleController;
		_controllerContainer.AddChild(_paddleController);
	}

	/// <summary>
	/// If input is greater than 0, the paddle moves up. If input is less than 0, the paddle moves down.
	/// </summary>
	/// <param name="input">The direction the paddle will move, 1 is up, -1 is down</param>
	public void Move(Vector2 input)
	{
		if(!_canMove)
			return;

		_ = MoveAndCollide(input);
	}

}
