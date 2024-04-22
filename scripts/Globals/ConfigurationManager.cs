using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

public partial class ConfigurationManager : Node
{
    public static readonly Vector2I[] AvailableResolutions = {
        new Vector2I(800, 600),
        new Vector2I(1024, 768),
        new Vector2I(1280, 768),
        new Vector2I(1920, 1080)
    };

    private static string _filePath;
    private static readonly string _fileName = "configuration.json";
    public static Configuration Configuration;

    public override async void _Ready()
    {
        _filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _fileName);
        await ReadFromFile();
        ChangeGameState();
    }

    public void SaveToFile(Configuration newConfiguration)
    {
        Configuration = newConfiguration;
        // Update the game's current state
        ChangeGameState();
        // Save the configuration to a file
        System.IO.File.WriteAllText(_filePath, JsonSerializer.Serialize(Configuration));
    }

    private async Task ReadFromFile()
    {
        // Read configuration from filePath
        await CreateFileIfItDoesntExist();
        // Parse its content & assign configuration from values read
        if(Configuration == null)
            Configuration = JsonSerializer.Deserialize<Configuration>(System.IO.File.ReadAllText(_filePath));
    }

    public void ChangeGameState()
    {
        // Change game's resolution
        DisplayServer.WindowSetSize(Configuration.Resolution);
        // Change game's main audio volume
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Configuration.MasterVolume);
        // Change game's window mode
        DisplayServer.WindowSetMode(Configuration.IsFullScreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);
    }

    private async Task CreateFileIfItDoesntExist()
    {
       if(!System.IO.File.Exists(_filePath))
        {
            try
            {
                System.IO.File.Create(_filePath).Close();
                Configuration = new Configuration();
                await System.IO.File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(Configuration));
            } catch(Exception ex)
            {
                GD.PrintErr($"Could not create configuration file at {_filePath}, Error description: {ex.Message}");
            }
        } 
    }
}