using Godot;
using System;

public partial class MiddleDottedLine : Node2D
{
	private Color DrawColor { get; set; } = new Color(1.0f, 1.0f, 1.0f);
	Vector2 From { get; set; } = new Vector2(0, 0);
	Vector2 To { get; set; } = new Vector2(0, 600);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _Draw()
    {
		DrawDashedLine(From, To, DrawColor, 5, 10, false, true);
    }

	public void DrawDashedLine(Vector2 from, Vector2 to, Color color, float width, float dashLength = 5, bool capEnd = false, bool antialiased = false)
	{
		float length = (to - from).Length();
		Vector2 normal = (to - from).Normalized();
		Vector2 dashStep = normal * dashLength;
		
		if(length < dashLength)
		{
			DrawLine(from, to, color, width, antialiased);
			return;
		}
		else
		{
			bool drawFlag = true;
			Vector2 segmentStart = from;
			int steps = (int)(length / dashLength);
			for(int startLength = 0; startLength <= steps; startLength++)
			{
				var segmentEnd = segmentStart + dashStep;
				if(drawFlag)
					DrawLine(segmentStart, segmentEnd, color, width, antialiased);

				segmentStart = segmentEnd;
				drawFlag = !drawFlag;
			}
			
			if(capEnd)
				DrawLine(segmentStart, to, color, width, antialiased);
		}
			
	}


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
