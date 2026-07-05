using System;

public class AppliedConsumable
{
	public GlobalProperties.ConsumableType Type { get; set; }
	public string OriginalCard { get; set; }

	public AppliedConsumable(GlobalProperties.ConsumableType type, string originalCard)
	{
		Type = type;
		OriginalCard = originalCard;
	}
}
