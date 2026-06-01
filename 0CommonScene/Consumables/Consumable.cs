using System;
using Godot;

public partial class Consumable : Node2D
{
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("Sprite2D/AnimatedSprite2D");
		PlayAnimation();
	}

	public void SetAnimation(GlobalProperties.ConsumableType type)
	{
		string animationName = GlobalProperties.ConsumableTypeToAnimString[type];
		if (_animatedSprite != null && _animatedSprite.SpriteFrames.HasAnimation(animationName))
		{
			_animatedSprite.Animation = animationName;
		}
	}

	public void SetMissingAnimation()
	{
		SetAnimation(GlobalProperties.ConsumableType.Missing);
	}

	public void SetTrumpAnimation()
	{
		SetAnimation(GlobalProperties.ConsumableType.ToTrump);
	}

	public GlobalProperties.ConsumableType GetCurrentConsumableType()
	{
		string currentAnim = _animatedSprite?.Animation ?? string.Empty;
		return GlobalProperties.AnimStringToConsumableType.ContainsKey(currentAnim)
			? GlobalProperties.AnimStringToConsumableType[currentAnim]
			: GlobalProperties.ConsumableType.Missing;
	}

	public void PlayAnimation()
	{
		_animatedSprite?.Play();
	}

	public void StopAnimation()
	{
		_animatedSprite?.Stop();
	}
}
