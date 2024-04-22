using Godot;
using System;

public partial class MainMenu : CanvasLayer
{

	private Container _currentScreen;
	
	private Menu _menu;
	private Options _options;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_menu = GetNode<Menu>("Menu");
		_menu.OnStartButtonPressed += StartGame;
		_menu.OnOptionsButtonPressed += OptionsButtonPressed;
		_menu.OnExitButtonPressed += ExitGame;

		SetCurrentScreen(_menu);

		_options = GetNode<Options>("Options");
		_options.OnSaveButtonPressed += OnOptionsSaved;
		_options.OnCancelButtonPressed += OnOptionsClosed;
	}

	private void SetCurrentScreen(Container screen)
	{
		if(_currentScreen != null)
			_currentScreen.Visible = false;
		_currentScreen = screen;
		screen.Visible = true;
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToFile("res://Main_level.tscn");
	}

	private void OptionsButtonPressed()
	{
		SetCurrentScreen(_options);
	}

	private void ExitGame()
	{
		GetTree().Quit();
	}

	private void OnOptionsSaved()
	{
		CloseOptions();
	}

	private void OnOptionsClosed()
	{
		CloseOptions();
	}
	
	private void CloseOptions()
	{
		SetCurrentScreen(_menu);
	}

}
