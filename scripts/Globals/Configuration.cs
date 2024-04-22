using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Godot;

[Serializable]
public class Configuration
{
    [Range(-80.0, 6.0)]
    public float MasterVolume { get; set; } = 0.0f;

    [JsonConverter(typeof(Vector2IConverter))]
    public Vector2I Resolution { get; set; } = new Vector2I(800, 600);

    public bool IsFullScreen { get; set; } = false;
}