using Godot;
using System;

public partial class MainLevel : Node2D, IStartable
{
	#region Properties
	#region EntitiesProperties
	private Ball Ball { get; set; }
	private Paddle Player { get; set; }
	private Paddle Opponent { get; set; }
	#endregion
	#region ZoneProperties
	private Area2D PlayerZone { get; set; }
	private Area2D OpponentZone { get; set;}
	#endregion
	#region SpawnsProperties
	private Marker2D PlayerSpawn { get; set; }
	private Marker2D OpponentSpawn { get; set; }
	private Marker2D BallSpawn { get; set; }
	#endregion
	#region ScoreProperties
	private int PlayerScore { get; set; } = 0;
	private int OpponentScore { get; set; } = 0;
	[Export] public int ScoreToWin = 5;
	#endregion
	#region SoundProperties
	private AudioStreamPlayer PaddleHit { get; set; }
	private AudioStreamPlayer GoalSound { get; set; }
	private AudioStreamPlayer StartGameSound { get; set; }
	#endregion
	#region MiscProperties
	private Timer StartTimer { get; set; }
	private Timer RestartTimer { get; set; }
	private HUD HUD { get; set; }
	
	#endregion
	#endregion

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;

		// Initializations
		PlayerSpawn = GetNode<Marker2D>("Spawns/PlayerSpawn");
		OpponentSpawn = GetNode<Marker2D>("Spawns/OpponentSpawn");
		Player = GetNode<Paddle>("Player");
		Opponent = GetNode<Paddle>("Opponent");
		Ball = GetNode<Ball>("Ball");
		HUD = GetNode<HUD>("HUD");
		PlayerZone = GetNode<Area2D>("Zones/PlayerZone");
		OpponentZone = GetNode<Area2D>("Zones/OpponentZone");
		BallSpawn = GetNode<Marker2D>("Spawns/BallSpawn");
		PaddleHit = GetNode<AudioStreamPlayer>("Sounds/PaddleHit");
		GoalSound = GetNode<AudioStreamPlayer>("Sounds/GoalSound");
		StartGameSound = GetNode<AudioStreamPlayer>("Sounds/StartGameSound");
		StartTimer = GetNode<Timer>("Misc/StartTimer");
		RestartTimer = GetNode<Timer>("Misc/RestartTimer");

		//Assignments
		Player.SetController(new HumanController(Player));
		Opponent.SetController(new AiController(Opponent, Ball));

		//Event handlers
		Ball.BallHitPaddle += OnBallHitPaddle;
		PlayerZone.BodyEntered += OnBodyEnteredPlayerZone;
		OpponentZone.BodyEntered += OnBodyEnteredOpponentZone;
		StartTimer.Timeout += OnStartTimerTimeout;
		RestartTimer.Timeout += OnRestartTimerTimeout;

		Player.Start();
		ResetState();
	}

	/// <summary>
	/// Executed when a Node2D enters the Player Zone.
	/// Updates the Opponent Score and calls the "Goal" method.
	/// </summary>
	/// <param name="node">Node that entered the Area2D</param>
	private void OnBodyEnteredPlayerZone(Node2D node)
	{
		if(!(node is Ball)){
			return;
		}
			
		OpponentScore += 1;
		HUD.UpdateOpponentScore(OpponentScore);

		Goal();
	}

	/// <summary>
	/// Executed when a Node2D enters the Opponent Zone.
	/// Updates the Player Score and calls the "Goal" method.
	/// </summary>
	/// <param name="node">Node that entered the Area2D</param>
	private void OnBodyEnteredOpponentZone(Node2D node)
	{
		if(!(node is Ball)){
			return;
		}

		PlayerScore += 1;
		HUD.UpdatePlayerScore(PlayerScore);
		
		Goal();
	}

	/// <summary>
	/// Executed when the ball enters either the <see cref="PlayerZone"/> or the <see cref="OpponentZone"/>.
	/// Plays <see cref="GoalSound"/>, stops the <see cref="Ball"/>, <see cref="Player"/> and <see cref="Opponent"/>.
	/// Restarts their position to the spawn.
	/// Shows the "Get Ready" message for the duration of the start timer.
	/// Starts the StartTimer.
	/// </summary>
	private void Goal()
	{
		GoalSound.Play();
		StopScripts();
		Ball.Hide();

		if(PlayerScore < ScoreToWin && OpponentScore < ScoreToWin)
		{
			ResetState();
			return;
		}

		if(PlayerScore == ScoreToWin)
		{
			HUD.ShowMessageFor("Player has won", (int)RestartTimer.WaitTime);
		}

		if(OpponentScore == ScoreToWin)
		{
			HUD.ShowMessageFor("Opponent has won", (int)RestartTimer.WaitTime);
		}

		RestartTimer.Start();
	}

	/// <summary>
	/// Stops the scripts.
	/// Resets positions.
	/// Shows "Get Ready" message.
	/// Starts the StartTimer.
	/// </summary>
	private void ResetState()
	{
		Ball.Show();
		ResetPositions();
		HUD.ShowMessageFor("Get Ready", (int)StartTimer.WaitTime);
		StartTimer.Start();
	}

	public void RestartGame()
	{
		ResetScores();
		ResetState();		
	}

	private void ResetScores()
	{
		PlayerScore = 0;
		OpponentScore = 0;
		HUD.UpdatePlayerScore(PlayerScore);
		HUD.UpdateOpponentScore(OpponentScore);
	}

	/// <summary>
	/// Stops the scripts of the Ball, the Player and the Opponent.
	/// </summary>
	private void StopScripts()
	{
		Ball.Stop();
		Opponent.Stop();
	}

	/// <summary>
	/// Resets the position of the Ball, the Player and the Opponent.
	/// </summary>
	private void ResetPositions()
	{
		Player.GlobalPosition = PlayerSpawn.GlobalPosition;
		Opponent.GlobalPosition = OpponentSpawn.GlobalPosition;
		Ball.GlobalPosition = BallSpawn.GlobalPosition;
	}

	/// <summary>
	/// Executed when the Ball hits either of the two paddles.
	/// Emits the "PaddleHit" sound.
	/// </summary>
	private void OnBallHitPaddle()
	{
		PaddleHit.Play();
	}

	/// <summary>
	/// Executed when the StartTimer timeouts.
	/// Starts the game.
	/// </summary>
	private void OnStartTimerTimeout()
	{
		Start();
	}

	private void OnRestartTimerTimeout()
	{
		RestartGame();
	}

	/// <summary>
	/// Starts the player, opponent and ball scripts. Plays the <see cref="StartGameSound"/> 
	/// </summary>
	public void Start()
	{
		Opponent.Start();
		Ball.Start();
		StartGameSound.Play();
	}
}
