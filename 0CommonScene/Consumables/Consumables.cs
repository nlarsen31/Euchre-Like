using System;
using Godot;

public partial class Consumables : Node2D
{
	private Consumable[] _consumables;

	public override void _Ready()
	{
		_consumables = new Consumable[3];
		_consumables[0] = GetNode<Consumable>("Consumable");
		_consumables[1] = GetNode<Consumable>("Consumable2");
		_consumables[2] = GetNode<Consumable>("Consumable3");

		SetAllConsumablesAnimation(GlobalProperties.ConsumableType.Missing);
	}

	public void SetConsumableAnimation(int index, GlobalProperties.ConsumableType type)
	{
		if (index >= 0 && index < _consumables.Length && _consumables[index] != null)
		{
			_consumables[index].SetAnimation(type);
		}
	}

	public void SetConsumable1Animation(GlobalProperties.ConsumableType type)
	{
		SetConsumableAnimation(0, type);
	}

	public void SetConsumable2Animation(GlobalProperties.ConsumableType type)
	{
		SetConsumableAnimation(1, type);
	}

	public void SetConsumable3Animation(GlobalProperties.ConsumableType type)
	{
		SetConsumableAnimation(2, type);
	}

	public void SetAllConsumablesAnimation(GlobalProperties.ConsumableType type)
	{
		for (int i = 0; i < _consumables.Length; i++)
		{
			SetConsumableAnimation(i, type);
		}
	}

	public GlobalProperties.ConsumableType GetConsumableType(int index)
	{
		if (index >= 0 && index < _consumables.Length && _consumables[index] != null)
		{
			return _consumables[index].GetCurrentConsumableType();
		}
		return GlobalProperties.ConsumableType.Missing;
	}
}
