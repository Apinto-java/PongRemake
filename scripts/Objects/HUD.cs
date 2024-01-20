using Godot;
using System;

public partial class HUD : Control
{
	private Label PlayerScore { get; set; }
	private Label OpponentScore { get; set; }
	private PanelContainer MessageContainer { get; set; }
	private Label Message { get; set; }
	private Timer MessageTimer { get; set;}

	public override void _Ready()
	{
		PlayerScore = GetNode<Label>("ScoreContainer/ScorePlayer");
		OpponentScore = GetNode<Label>("ScoreContainer/ScoreOponent");
		MessageContainer = GetNode<PanelContainer>("MessageContainer");
		MessageContainer.Visible = false;
		Message = GetNode<Label>("MessageContainer/CenterContainer/Message");
		MessageTimer = GetNode<Timer>("MessageTimer");
		MessageTimer.Timeout += OnMessageTimerTimeout;
	}

	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Updates the Player Score (shown on the left side of the screen) to the specified value
	/// </summary>
	/// <param name="newScore">New Player Score</param>
	public void UpdatePlayerScore(int newScore)
	{
		PlayerScore.Text = FormatScore(newScore);
	}

	/// <summary>
	/// Updates the Opponent Score (shown on the right side of the screen) to the specified value
	/// </summary>
	/// <param name="newScore">New Opponent Score</param>
	public void UpdateOpponentScore(int newScore)
	{
		OpponentScore.Text = FormatScore(newScore);
	}

	/// <summary>
	/// Formats the given value adding one leading zero.
	/// </summary>
	/// <param name="score">Score to format</param>
	/// <returns>Score with one leading zero</returns>
	private string FormatScore(int score)
	{
		return string.Format("{0:00}", score);
	}

	/// <summary>
	/// Shows the message specified in <c>message</c> for the time indicated in the <c>timeInSeconds</c> parameter.
	/// If <c>timeInSeconds</c> is less than or equal to 0, the message will stay on the screen.
	/// </summary>
	/// <param name="message">The message to display</param>
	/// <param name="timeInSeconds">The amount of time the message will stay on the screen. If less than or equal
	/// to 0, the message will stay on the screen until <see cref="HideMessage"/> is called</param>
	public void ShowMessageFor(string message, int timeInSeconds = 0)
	{
		Message.Text = message;
		MessageContainer.Visible = true;
		if(timeInSeconds > 0)
		{
			MessageTimer.WaitTime = timeInSeconds;
			MessageTimer.Start();
		}

	}

	/// <summary>
	/// Resets the Message <c>string</c> and hides the Message Container. Intended to be used after calling
	/// <see cref="ShowMessageFor"/> with <c>timeInSeconds</c> less than or equal to 0.
	/// </summary>
	public void HideMessage()
	{
		Message.Text = string.Empty;
		MessageContainer.Visible = false;
	}

	/// <summary>
	/// Called when MessageTimer reaches 0. It hides the Message displayed on the Screen, reseting the Message
	/// variable to <c>string.Empty</c> and setting the Visible property of the MessageContainer to <c>false</c>
	/// </summary>
	private void OnMessageTimerTimeout()
	{
		HideMessage();
	}
}
