using Godot;
using System;


using static GlobalProperties;

public partial class Chip : Node2D
{
	private Vector2[] LeadPositions = {
		new Vector2(1824, 888),
		new Vector2(1856, 320),
		new Vector2(720, 56),
		new Vector2(48, 760)
	};
	private float[] LeadRotations = {
		(float)Math.PI * 0,
		(float)Math.PI * 1.5f,
		(float)Math.PI * 1,
		(float)Math.PI * 0.5f
	};
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void SetAnimation(string Anim)
	{
		AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("ChipAnim");
		sprite.Animation = Anim;
	}
	public void SetAnimation(Suit suit)
	{
		AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("ChipAnim");
		GD.Print(SuitToString[(int)suit]);
		sprite.Animation = SuitToString[(int)suit];
	}

	public void SetLeadPosition(Player player) {
		GD.Print(PlayerToString[(int)player]);
		this.Position = LeadPositions[(int)player];
		this.Rotation = LeadRotations[(int)player];
	}
}
