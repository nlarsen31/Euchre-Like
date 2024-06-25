using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static GlobalProperties;

public partial class GlobalMethods : Node
{
	private static Random randy = new Random();
	private static Dictionary<string, Rank> StringToRankDict  =  new Dictionary<string, Rank>();
	private static Dictionary<string, Suit> StringToSuitDic  =  new Dictionary<string, Suit>();
	public static void RandomizeList<T>(List<T> list)
	{    
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = randy.Next(n + 1);
			(list[n], list[k]) = (list[k], list[n]);
		}
	}
	public static Tuple<Rank,Suit> GetSuitRankFromString(string s)
	{
		string[] sArr = s.Split("_");
		bool badData = sArr.Length != 2;
		if(!StringToSuitDic.ContainsKey(sArr[0])) badData = true;
		if(!StringToRankDict.ContainsKey(sArr[1])) badData = true;
		if (badData)
		{
			return new Tuple<Rank, Suit>(Rank.undefined, Suit.UNASSIGNED);
		}
		
		Suit suit = StringToSuitDic[sArr[0]];
		Rank rank = StringToRankDict[sArr[1]];
		return new Tuple<Rank, Suit>(rank, suit);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (StringToRankDict.Keys.Count == 0) {
			string[] ranksStr = {
				"a", "k", "q", "j", "10", "9", "8", "7", "6", "5", "4", "3", "2"
			};
			Rank[] ranksInt = {
				Rank.ace, Rank.king, Rank.queen, Rank.jack, Rank.ten, Rank.nine, Rank.eight, Rank.seven, Rank.six, Rank.five, Rank.four, Rank.three, Rank.two
			};
			for(int i = 0; i < ranksInt.Length; i++) {
				StringToRankDict.Add(ranksStr[i], ranksInt[i]);
			}
		}

		if (StringToSuitDic.Keys.Count == 0) {
			string[] suitsStr = {"spades", "hearts", "clubs", "diamonds"};
			Suit[] suitsInt = { Suit.SPADES, Suit.HEARTS, Suit.CLUBS, Suit.DIAMONDS };
			for (int i = 0; i < suitsStr.Length; i++) {
				StringToSuitDic.Add(suitsStr[i], suitsInt[i]);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
