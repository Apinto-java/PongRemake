using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

public partial class Options : VBoxContainer
{

	private ConfigurationManager _configurationManager;

	private Button _confirmButton;
	private Button _cancelButton;
	private HSlider _masterVolumeSlider;
	private OptionButton _resolutionOptions;
	private CheckButton _fullscreenButton;

	[Signal]
	public delegate void OnSaveButtonPressedEventHandler();
	[Signal]
	public delegate void OnCancelButtonPressedEventHandler();

	public override void _Ready()
	{
		_configurationManager = GetNode<ConfigurationManager>("/root/ConfigurationManager");
		_confirmButton = GetNode<Button>("HBoxContainer/Save");
		_cancelButton = GetNode<Button>("HBoxContainer/Cancel");
		_masterVolumeSlider = GetNode<HSlider>("VolumeContainer/MasterVolume");
		SetMinAndMaxVolume();
		_resolutionOptions = GetNode<OptionButton>("ResolutionContainer/Resolution");
		LoadResolutions();
		_fullscreenButton = GetNode<CheckButton>("FullScreenButton");

		_confirmButton.Pressed += () => SaveButtonPressed();
		_cancelButton.Pressed += () => CancelButtonPressed();

		LoadCurrentSettings();
	}

	private void SetMinAndMaxVolume()
	{
		var range = typeof(Configuration)
    		.GetProperty(nameof(Configuration.MasterVolume))
    		.GetCustomAttribute<RangeAttribute>();	

		_masterVolumeSlider.MinValue = Convert.ToSingle(range.Minimum);
		_masterVolumeSlider.MaxValue = Convert.ToSingle(range.Maximum);
	}

	private void LoadResolutions()
	{
		for(int i = 0; i < ConfigurationManager.AvailableResolutions.Length; i++)
		{
			var resolution = ConfigurationManager.AvailableResolutions[i];
			var optionText = $"{resolution.X}x{resolution.Y}";
			_resolutionOptions.AddItem(optionText, i);
		}
	}

	private void LoadCurrentSettings()
	{
		_masterVolumeSlider.Value = ConfigurationManager.Configuration.MasterVolume;
		var resIdx = Array.IndexOf(ConfigurationManager.AvailableResolutions, ConfigurationManager.Configuration.Resolution);
		_resolutionOptions.Select(resIdx); 
		_fullscreenButton.ButtonPressed = ConfigurationManager.Configuration.IsFullScreen;
	}

	private void SaveButtonPressed()
	{
		Configuration configuration = new Configuration() {
			IsFullScreen = _fullscreenButton.ButtonPressed,
			MasterVolume = (float)_masterVolumeSlider.Value,
			Resolution = ConfigurationManager.AvailableResolutions[_resolutionOptions.Selected]
		};
		_configurationManager.SaveToFile(configuration);
		EmitSignal(SignalName.OnSaveButtonPressed);
	}

	private void CancelButtonPressed()
	{
		EmitSignal(SignalName.OnCancelButtonPressed);
	}

}
