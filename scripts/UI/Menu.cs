using Godot;
using System;

public partial class Menu : CenterContainer
{
	private Button _startButton;
	private Button _optionsButton;
	private Button _exitButton;

	[Signal]
	public delegate void OnStartButtonPressedEventHandler();
	[Signal]
	public delegate void OnExitButtonPressedEventHandler();
	[Signal]
	public delegate void OnOptionsButtonPressedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startButton = GetNode<Button>("VBoxContainer/StartButton");
		_optionsButton = GetNode<Button>("VBoxContainer/OptionsButton");
		_exitButton = GetNode<Button>("VBoxContainer/ExitButton");

		_startButton.Pressed += () => EmitSignal(SignalName.OnStartButtonPressed);
		_optionsButton.Pressed += () => EmitSignal(SignalName.OnOptionsButtonPressed);
		_exitButton.Pressed += () => EmitSignal(SignalName.OnExitButtonPressed);
	}


}
