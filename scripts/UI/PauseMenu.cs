using Godot;
using System;

public partial class PauseMenu : Control
{
	private bool _isPaused = false;

	private Button OptionsButton { get; set; }
	private Button QuitButton { get; set; }

	private Options Options { get; set; }

	private CenterContainer ButtonsContainer { get; set; }

	public bool IsPaused { 
		get => _isPaused; 
		set {
			_isPaused = value;
			GetTree().Paused = _isPaused;
			Visible = _isPaused;
			if(_isPaused)
				Input.MouseMode = Input.MouseModeEnum.Visible;
			else
				Input.MouseMode = Input.MouseModeEnum.Hidden;
		}
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event.IsActionPressed("pause"))
			IsPaused = !IsPaused;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Options = GetNode<Options>("Options");
		Options.OnSaveButtonPressed += () => OnSaveOptionsButtonPressed();
		Options.OnCancelButtonPressed += () => OnCancelButtonPressed();
		ButtonsContainer = GetNode<CenterContainer>("CenterContainer");
		OptionsButton = GetNode<Button>("CenterContainer/ButtonsContainer/OptionsButton");
		OptionsButton.Pressed += () => OnOptionsButtonPressed();
		QuitButton = GetNode<Button>("CenterContainer/ButtonsContainer/QuitButton");
		QuitButton.Pressed += () => OnQuitButtonPressed();
	}

	private void OnOptionsButtonPressed()
	{
		Options.Visible = true;
		ButtonsContainer.Visible = false;
	}

	private void OnSaveOptionsButtonPressed()
	{
		HideOptions();
	}

	private void OnCancelButtonPressed()
	{
		HideOptions();
	}

	private void HideOptions()
	{
		Options.Visible = false;
		ShowButtons();
	}

	private void ShowButtons()
	{
		ButtonsContainer.Visible = true;
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
