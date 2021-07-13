using System;
using Sound;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using BattleCharacterProfile;
using LOR_DiceSystem;
using System.Collections.Generic;
using HarmonyLib;
using UI;

namespace HMI_FragOfficeRemake_MOD
{
	public class BattleUnitBuf_HMILightUp : BattleUnitBuf
	{
		public BattleUnitBuf_HMILightUp(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMILightUp) is BattleUnitBuf_HMILightUp battleUnitBuf_HMILightUp)) result = 0;
			else result = battleUnitBuf_HMILightUp.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMILightUp) is BattleUnitBuf_HMILightUp battleUnitBuf_HMILightUp))
			{
				battleUnitBuf_HMILightUp = new BattleUnitBuf_HMILightUp(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMILightUp);
				return;
			}
			battleUnitBuf_HMILightUp.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
		}
		public override void OnRoundEnd()
		{
			Destroy();
		}
		public override BufPositiveType positiveType
		{
			get
			{
				return BufPositiveType.Positive;
			}
		}
	}
	public class DiceCardSelfAbility_HMIbattle3recoverlight : DiceCardSelfAbilityBase
	{
		public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
		{
			int stack = BattleUnitBuf_HMILightUp.GetStack(unit);
			unit.cardSlotDetail.RecoverPlayPointByCard(1);
			unit.TakeBreakDamage(2 + stack * 3, DamageType.Card_Ability);
			BattleUnitBuf_HMILightUp.Akari(unit, 1);
			unit.allyCardDetail.ExhaustCard(self.GetID());
			unit.allyCardDetail.AddNewCard(self.GetID(), true).temporary = true;
			BattleUnitModel t = BattleObjectManager.instance.GetList(1 - unit.faction)[0];
			if (stack >= 2 && BattleUnitBuf_HMIdsu.GetStack(t) > 0) BattleUnitBuf_HMIdsu.GetBuf(t).cancel();
		}
	}
	public class DiceCardSelfAbility_HMIuseMusicStone : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			AudioClip audioClip = Harmony_Patch.mp3toAudioClip(Harmony_Patch.path.FullName + "/Sound/MusicStone.mp3");
			SingletonBehavior<SoundEffectManager>.Instance.PlayClip(audioClip);
		}
	}
	public class DiceCardAbility_HMIbreakdamagearea13 : DiceCardAbilityBase
	{
		public override void OnSucceedAreaAttack(BattleUnitModel target)
		{
			target.TakeBreakDamage(13, DamageType.Card_Ability);
		}
	}
	public class DiceCardSelfAbility_HMIusePaintStone : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			ScreenCapture.CaptureScreenshot(Application.dataPath + "/existenz.jpg");
		}
	}
	public class DiceCardAbility_HMIdestr0yarea1dice : DiceCardAbilityBase
	{
		public override void OnSucceedAreaAttack(BattleUnitModel target)
		{
			target.bufListDetail.AddBuf(new BattleUnitBuf_destr0y1dice2());
		}
	}
	public class DiceCardSelfAbility_HMIrefGod : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			if (card.target.faction == owner.faction) card.target.bufListDetail.AddKeywordBufThisRoundByCard(new BattleUnitBuf_AllPowerUp().bufType, 7, owner);
		}
	}
	public class BattleUnitBuf_HMIselfDestr0y : BattleUnitBuf
	{
		public BattleUnitBuf_HMIselfDestr0y(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIselfDestr0y"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIselfDestr0y";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIselfDestr0y) is BattleUnitBuf_HMIselfDestr0y battleUnitBuf_HMIselfDestr0y)) result = 0;
			else result = battleUnitBuf_HMIselfDestr0y.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIselfDestr0y) is BattleUnitBuf_HMIselfDestr0y battleUnitBuf_HMIselfDestr0y))
			{
				battleUnitBuf_HMIselfDestr0y = new BattleUnitBuf_HMIselfDestr0y(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIselfDestr0y);
				return;
			}
			battleUnitBuf_HMIselfDestr0y.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
			if (stack > 2012) stack = 2012;
		}
	}
	public class BattleUnitBuf_HMIreasonLight : BattleUnitBuf
	{
		public BattleUnitBuf_HMIreasonLight(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIreasonLight"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIreasonLight";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIreasonLight) is BattleUnitBuf_HMIreasonLight battleUnitBuf_HMIreasonLight)) result = 0;
			else result = battleUnitBuf_HMIreasonLight.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIreasonLight) is BattleUnitBuf_HMIreasonLight battleUnitBuf_HMIreasonLight))
			{
				battleUnitBuf_HMIreasonLight = new BattleUnitBuf_HMIreasonLight(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIreasonLight);
				return;
			}
			battleUnitBuf_HMIreasonLight.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
		}
		public override void OnRoundEnd()
		{
			--stack; if (stack < 0) Destroy();
		}
	}
	public class BattleUnitBuf_HMIreason : BattleUnitBuf
	{
		private const int limit = 12;
		public BattleUnitBuf_HMIreason(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIreason"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIreason";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIreason) is BattleUnitBuf_HMIreason battleUnitBuf_HMIreason)) result = 0;
			else result = battleUnitBuf_HMIreason.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIreason) is BattleUnitBuf_HMIreason battleUnitBuf_HMIreason))
			{
				battleUnitBuf_HMIreason = new BattleUnitBuf_HMIreason(model)
				{
					stack = Math.Max(add % limit, 0)
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIreason);
				if (add >= limit) BattleUnitBuf_HMIreasonLight.Akari(model, add / limit);
				return;
			}
			battleUnitBuf_HMIreason.Edd(add);
		}
		public void Edd(int add)
		{
			stack = Math.Max(stack + add, 0);
			if (stack >= limit) BattleUnitBuf_HMIreasonLight.Akari(_owner, stack / limit);
			stack %= limit;
		}
	}
	public class BattleUnitBuf_HMItower3 : BattleUnitBuf
	{
		public BattleUnitBuf_HMItower3(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMItower3"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMItower3";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMItower3) is BattleUnitBuf_HMItower3 battleUnitBuf_HMItower3)) result = 0;
			else result = battleUnitBuf_HMItower3.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMItower3) is BattleUnitBuf_HMItower3 battleUnitBuf_HMItower3))
			{
				battleUnitBuf_HMItower3 = new BattleUnitBuf_HMItower3(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMItower3);
				return;
			}
			battleUnitBuf_HMItower3.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
		}
		public override void OnRoundStart() { _owner.allyCardDetail.AddNewCard(3500114).temporary = true; }
	}
	public class BattleUnitBuf_HMItower2 : BattleUnitBuf
	{
		int cnt;
		public BattleUnitBuf_HMItower2(BattleUnitModel model)
		{
			_owner = model;
			stack = cnt = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMItower2"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMItower2";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMItower2) is BattleUnitBuf_HMItower2 battleUnitBuf_HMItower2)) result = 0;
			else result = battleUnitBuf_HMItower2.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMItower2) is BattleUnitBuf_HMItower2 battleUnitBuf_HMItower2))
			{
				battleUnitBuf_HMItower2 = new BattleUnitBuf_HMItower2(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMItower2);
				return;
			}
			battleUnitBuf_HMItower2.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add; if (stack > 1) stack = 1;
		}
		public override void OnRoundStart()
		{
			_owner.allyCardDetail.ExhaustCard(3500107);
			_owner.allyCardDetail.AddNewCard(3500110).temporary = true;
			_owner.allyCardDetail.AddNewCard(3500113).temporary = true;
		}
		public override void OnLoseParrying(BattleDiceBehavior behavior)
		{
			if (behavior.card.card.GetID() == 3500110 && behavior.abilityList.FindAll((DiceCardAbilityBase x) => x is DiceCardAbility_HMISpecialDice).Count > 0) ++cnt;
			if (cnt >= 3) { BattleUnitBuf_HMIreason.Akari(_owner, 11); BattleUnitBuf_HMIselfDestr0y.Akari(_owner, 111); BattleUnitBuf_HMItower3.Akari(_owner, 1); Destroy(); }
		}
	}
	public class BattleUnitBuf_HMItower1 : BattleUnitBuf
	{
		private const int limit = 3;
		public BattleUnitBuf_HMItower1(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMItower1"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMItower1";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMItower1) is BattleUnitBuf_HMItower1 battleUnitBuf_HMItower1)) result = 0;
			else result = battleUnitBuf_HMItower1.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMItower1) is BattleUnitBuf_HMItower1 battleUnitBuf_HMItower1))
			{
				battleUnitBuf_HMItower1 = new BattleUnitBuf_HMItower1(model)
				{
					stack = add % limit
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMItower1);
				if (add >= limit) BattleUnitBuf_HMItower2.Akari(model, add / limit);
				return;
			}
			battleUnitBuf_HMItower1.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
			if (stack >= limit) { BattleUnitBuf_HMItower2.Akari(_owner, stack / limit); Destroy(); }
			stack %= limit;
		}
	}
	public class BattleUnitBuf_HMIwall3 : BattleUnitBuf
	{
		public BattleUnitBuf_HMIwall3(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIwall3"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIwall3";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIwall3) is BattleUnitBuf_HMIwall3 battleUnitBuf_HMIwall3)) result = 0;
			else result = battleUnitBuf_HMIwall3.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIwall3) is BattleUnitBuf_HMIwall3 battleUnitBuf_HMIwall3))
			{
				battleUnitBuf_HMIwall3 = new BattleUnitBuf_HMIwall3(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIwall3);
				return;
			}
			battleUnitBuf_HMIwall3.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
		}
		public override void OnRoundStart() { _owner.allyCardDetail.AddNewCard(3500114).temporary = true; }
	}
	public class BattleUnitBuf_HMIwall2 : BattleUnitBuf
	{
		int cnt;
		public BattleUnitBuf_HMIwall2(BattleUnitModel model)
		{
			_owner = model;
			stack = cnt = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIwall2"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIwall2";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIwall2) is BattleUnitBuf_HMIwall2 battleUnitBuf_HMIwall2)) result = 0;
			else result = battleUnitBuf_HMIwall2.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIwall2) is BattleUnitBuf_HMIwall2 battleUnitBuf_HMIwall2))
			{
				battleUnitBuf_HMIwall2 = new BattleUnitBuf_HMIwall2(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIwall2);
				return;
			}
			battleUnitBuf_HMIwall2.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add; if (stack > 1) stack = 1;
		}
		public override void OnRoundStart()
		{
			_owner.allyCardDetail.ExhaustCard(3500108);
			_owner.allyCardDetail.AddNewCard(3500111).temporary = true;
			_owner.allyCardDetail.AddNewCard(3500113).temporary = true;
		}
		public override void OnLoseParrying(BattleDiceBehavior behavior)
		{
			if (behavior.card.card.GetID() == 3500111 && behavior.abilityList.FindAll((DiceCardAbilityBase x) => x is DiceCardAbility_HMISpecialDice).Count > 0) ++cnt;
			if (cnt >= 3) { BattleUnitBuf_HMIreason.Akari(_owner, 11); BattleUnitBuf_HMIselfDestr0y.Akari(_owner, 111); BattleUnitBuf_HMIwall3.Akari(_owner, 1); Destroy(); }
		}
	}
	public class BattleUnitBuf_HMIwall1 : BattleUnitBuf
	{
		private const int limit = 3;
		public BattleUnitBuf_HMIwall1(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIwall1"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIwall1";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIwall1) is BattleUnitBuf_HMIwall1 battleUnitBuf_HMIwall1)) result = 0;
			else result = battleUnitBuf_HMIwall1.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIwall1) is BattleUnitBuf_HMIwall1 battleUnitBuf_HMIwall1))
			{
				battleUnitBuf_HMIwall1 = new BattleUnitBuf_HMIwall1(model)
				{
					stack = add % limit
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIwall1);
				if (add >= limit) BattleUnitBuf_HMIwall2.Akari(model, add / limit);
				return;
			}
			battleUnitBuf_HMIwall1.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
			if (stack >= limit) BattleUnitBuf_HMIwall2.Akari(_owner, stack / limit);
			stack %= limit;
		}
	}
	public class BattleUnitBuf_HMIforest3 : BattleUnitBuf
	{
		public BattleUnitBuf_HMIforest3(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIforest3"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIforest3";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIforest3) is BattleUnitBuf_HMIforest3 battleUnitBuf_HMIforest3)) result = 0;
			else result = battleUnitBuf_HMIforest3.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIforest3) is BattleUnitBuf_HMIforest3 battleUnitBuf_HMIforest3))
			{
				battleUnitBuf_HMIforest3 = new BattleUnitBuf_HMIforest3(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIforest3);
				return;
			}
			battleUnitBuf_HMIforest3.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
		}
		public override void OnRoundStart() { _owner.allyCardDetail.AddNewCard(3500114).temporary = true; }
	}
	public class BattleUnitBuf_HMIforest2 : BattleUnitBuf
	{
		public BattleUnitBuf_HMIforest2(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIforest2"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIforest2";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIforest2) is BattleUnitBuf_HMIforest2 battleUnitBuf_HMIforest2)) result = 0;
			else result = battleUnitBuf_HMIforest2.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIforest2) is BattleUnitBuf_HMIforest2 battleUnitBuf_HMIforest2))
			{
				battleUnitBuf_HMIforest2 = new BattleUnitBuf_HMIforest2(model)
				{
					stack = add
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIforest2);
				return;
			}
			battleUnitBuf_HMIforest2.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add; if (stack > 1) stack = 1;
		}
		public override void OnRoundStart()
		{
			_owner.allyCardDetail.ExhaustCard(3500109);
			_owner.allyCardDetail.AddNewCard(3500112).temporary = true;
			_owner.allyCardDetail.AddNewCard(3500113).temporary = true;
		}
	}
	public class BattleUnitBuf_HMIforest1 : BattleUnitBuf
	{
		private const int limit = 7;
		public BattleUnitBuf_HMIforest1(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIforest1"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		protected override string keywordId
		{
			get
			{
				return "HMIforest1";
			}
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIforest1) is BattleUnitBuf_HMIforest1 battleUnitBuf_HMIforest1)) result = 0;
			else result = battleUnitBuf_HMIforest1.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIforest1) is BattleUnitBuf_HMIforest1 battleUnitBuf_HMIforest1))
			{
				battleUnitBuf_HMIforest1 = new BattleUnitBuf_HMIforest1(model)
				{
					stack = add % limit
				};
				model.bufListDetail.AddBuf(battleUnitBuf_HMIforest1);
				if (add >= limit) BattleUnitBuf_HMIforest2.Akari(model, add / limit);
				return;
			}
			battleUnitBuf_HMIforest1.Edd(add);
		}
		public void Edd(int add)
		{
			stack += add;
			if (stack >= limit) BattleUnitBuf_HMIforest2.Akari(_owner, stack / limit);
			stack %= limit;
		}
	}
	public class HMINewBehaviourScript : MonoBehaviour
	{
		private void Start()
		{
			gameObject.AddComponent<Canvas>();
			gameObject.AddComponent<SpriteRenderer>();
			gameObject.AddComponent<CanvasScaler>();
			gameObject.AddComponent<Image>();
			Canvas canvas = gameObject.GetComponent<Canvas>();
			if (canvas != null)
			{
				canvas.renderMode = RenderMode.ScreenSpaceCamera;
				canvas.worldCamera = Camera.main;
				canvas.planeDistance = 5f;
				canvas.enabled = true;
			}
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			if (component != null)
			{
				component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				component.referenceResolution = new Vector2(1920f, 1080f);
				component.enabled = true;
			}
			Image mage = gameObject.GetComponent<Image>();
			if (canvas != null)
			{
				mage.sprite = BaseMod.Harmony_Patch.ArtWorks["HMIFatalerror"];
				mage.enabled = true;
			}
			gameObject.SetActive(true);
		}
		private void Update()
		{
		}
		public Sprite img;
		public Camera camera;
	}
	public class DiceCardSelfAbility_HMItest : DiceCardSelfAbilityBase
	{
		public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
		{
			try
			{
				Resolution[] resolutions = Screen.resolutions;
				Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
				Screen.fullScreen = true;
				SingletonBehavior<BattleManagerUI>.Instance.ui_unitListInfoSummary.EnableCanvas(false);
				SingletonBehavior<BattleManagerUI>.Instance.ui_unitInformation.EnableCanvas(false);
				SingletonBehavior<BattleManagerUI>.Instance.ui_unitInformationPlayer.EnableCanvas(false);
				SingletonBehavior<BattleManagerUI>.Instance.ui_unitCardsInHand.EnableCanvas(false);
				try { SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.GetComponent<Canvas>().enabled = false; } catch (Exception) { }
				try { SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.GetComponent<Canvas>().enabled = false; } catch (Exception) { }
				try { SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.GetComponent<Canvas>().enabled = false; } catch (Exception) { }
				try { SingletonBehavior<BattleManagerUI>.Instance.ui_battleEmotionCoinUI.GetComponent<Canvas>().enabled = false; } catch (Exception) { }
				SingletonBehavior<BattleMapEffectManager>.Instance.transform.gameObject.SetActive(false);
				SingletonBehavior<BattleSceneRoot>.Instance.transform.gameObject.SetActive(false);
				foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList())
				{
					try { model.view.costUI.EnableCanvas(false); } catch (Exception) { }
					try { model.view.speedDiceSetterUI.SetDisable(); } catch (Exception) { }
					try { model.view.StartDeadEffect(false); } catch (Exception) { }
					try { model.view.GetComponent<Canvas>().enabled = false; } catch (Exception) { }
				}
				SingletonBehavior<BattleSoundManager>.Instance.EndBgm();
				SingletonBehavior<BattleSoundManager>.Instance.SetOffBattleSound();
				try { CursorManager.Instance.HideCursor(); } catch (Exception) { }
				try { SingletonBehavior<CursorManager>.Instance.HideCursor(); } catch (Exception) { }
				try { SingletonBehavior<BattleManagerUI>.Instance.GetComponent<Canvas>().enabled = false; } catch (Exception) { }
				try { SingletonBehavior<BattleManagerUI>.Instance.gameObject.SetActive(false); } catch (Exception) { }
				GameObject gameObject = new GameObject("HMItest");
				gameObject.AddComponent(typeof(HMINewBehaviourScript));
				gameObject.SetActive(true);
			}
			catch(Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMItesterror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
	}
	public class DiceCardSelfAbility_HMItower1 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { dmgRate = -100 }); BattleUnitBuf_HMIreason.Akari(owner, -1); _win = _lose = _cnt = 0; }
		bool EndUsingCard
		{
			get
			{
				return card.GetRemainingAbilityCount() == 0;
			}
		}
		void judge1() { if (_win == _cnt && EndUsingCard) { BattleUnitBuf_HMItower1.Akari(owner, 1); _win = 0; } }
		void judge2() { if (_lose == 1 && EndUsingCard) { BattleUnitBuf_HMIreason.Akari(owner, 3); _lose = 0; } }
		public override void OnWinParryingAtk() { ++_win; ++_cnt; judge1(); judge2(); }
		public override void OnLoseParrying() { ++_lose; ++_cnt; judge1(); judge2(); }
		int _win, _lose, _cnt;
	}
	public class DiceCardSelfAbility_HMIwall1 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { dmgRate = -100 }); BattleUnitBuf_HMIreason.Akari(owner, -1); _win = _lose = _cnt = 0; }
		bool EndUsingCard
		{
			get
			{
				return card.GetRemainingAbilityCount() == 0;
			}
		}
		void judge1() { if (_lose == _cnt && EndUsingCard) { BattleUnitBuf_HMIwall1.Akari(owner, 1); _lose = 0; } }
		void judge2() { if (_win == 1 && EndUsingCard) { BattleUnitBuf_HMIreason.Akari(owner, 3); _win = 0; } }
		public override void OnLoseParrying() { ++_lose; ++_cnt; judge1(); judge2(); }
		public override void OnWinParryingDef() { ++_win; ++_cnt; judge1(); judge2(); }
		int _win, _lose, _cnt;
	}
	public class DiceCardSelfAbility_HMIforest1 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { dmgRate = -100 }); BattleUnitBuf_HMIreason.Akari(owner, -2); _win = _lose = _cnt = 0; }
		bool EndUsingCard
		{
			get
			{
				return card.GetRemainingAbilityCount() == 0;
			}
		}
		void judge1() { if (_win < _cnt && EndUsingCard) { BattleUnitBuf_HMIforest1.Akari(owner, 1); _cnt = 0; } }
		void judge2() { if (_win == _cnt && EndUsingCard) { BattleUnitBuf_HMIreason.Akari(owner, 7); _win = 0; } }
		public override void OnLoseParrying() { ++_lose; ++_cnt; judge1(); judge2(); }
		public override void OnSucceedAttack() { ++_win; ++_cnt; judge1(); judge2(); }
		int _win, _lose, _cnt;
	}
	public class DiceCardSelfAbility_HMItower2 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			BattleUnitBuf_HMIreason.Akari(owner, -1); owner.cardSlotDetail.RecoverPlayPointByCard(3); owner.allyCardDetail.DrawCards(2);
		}
	}
	public class DiceCardSelfAbility_HMIwall2 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			BattleUnitBuf_HMIreason.Akari(owner, -1); owner.cardSlotDetail.RecoverPlayPointByCard(3); owner.allyCardDetail.DrawCards(2);
		}
	}
	public class DiceCardSelfAbility_HMIforest2 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			card.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus { dmgRate = -75, breakRate = -75 });
			BattleUnitBuf_HMIreason.Akari(owner, -3); owner.cardSlotDetail.RecoverPlayPointByCard(7); owner.allyCardDetail.DrawCards(4);
		}
	}
	public class DiceCardAbility_HMISpecialDice : DiceCardAbilityBase
	{
	}
	public class DiceCardAbility_HMISpecialParry1 : DiceCardAbilityBase
	{
		public static double GetExpectation(BattleDiceCardModel model)
		{
			double res = 0;
			foreach (DiceBehaviour dice in model.GetBehaviourList()) res += (dice.Min + dice.Dice) / 2.0;
			return res;
		}
		public static int GetMaxSum(BattleDiceCardModel model)
		{
			int res = 0;
			foreach (DiceBehaviour dice in model.GetBehaviourList()) res += dice.Dice;
			return res;
		}
		public static int GetMinSum(BattleDiceCardModel model)
		{
			int res = 0;
			foreach (DiceBehaviour dice in model.GetBehaviourList()) res += dice.Min;
			return res;
		}
		public override void BeforRollDice()
		{
			if (behavior == null) return;
			behavior.ApplyDiceStatBonus(new DiceStatBonus { dmgRate = -100, breakRate = -100 });
			if (behavior.TargetDice == null || behavior.TargetDice.card == null) return;
			a = (int)GetExpectation(behavior.TargetDice.card.card) - behavior.TargetDice.behaviourInCard.Min; b = GetMaxSum(behavior.TargetDice.card.card) - behavior.TargetDice.behaviourInCard.Dice;
			behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus { min = a, max = b });
		}
		public override void OnLoseParrying()
		{
			behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus { min = -a, max = -b });
		}
		int a, b;
		public override void OnWinParrying()
		{
			if (behavior.TargetDice == null || behavior.TargetDice.card == null) return;
			behavior.TargetDice.card.DestroyDice(DiceMatch.AllDice);
			behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 5, owner);
		}
	}
	public class DiceCardAbility_HMISpecialParry2 : DiceCardAbilityBase
	{
		public override void BeforRollDice()
		{
			if (behavior == null) return; behavior.forbiddenBonusDice = true;
			if (behavior.TargetDice == null || behavior.TargetDice.card == null) return;
			a = (int)DiceCardAbility_HMISpecialParry1.GetExpectation(behavior.TargetDice.card.card) - behavior.TargetDice.behaviourInCard.Min; b = DiceCardAbility_HMISpecialParry1.GetMaxSum(behavior.TargetDice.card.card) - behavior.TargetDice.behaviourInCard.Dice;
			behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus { min = a, max = b });
		}
		public override void OnLoseParrying()
		{
			behavior.TargetDice.ApplyDiceStatBonus(new DiceStatBonus { min = -a, max = -b });
		}
		int a, b;
		public override void OnWinParrying()
		{
			if (behavior.TargetDice == null || behavior.TargetDice.card == null) return;
			behavior.TargetDice.card.DestroyDice(DiceMatch.AllDice);
			BattleUnitBuf_HMIselfDestr0y.Akari(owner, -1);
		}
	}
	public class DiceCardSelfAbility_HMIcomingsoon : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Player)) model.Die(null, false);
		}
	}
	public class BattleUnitBuf_HMIcaught : BattleUnitBuf
	{
		public BattleUnitBuf_HMIcaught(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIcaught) is BattleUnitBuf_HMIcaught buf)) result = 0;
			else result = buf.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIcaught) is BattleUnitBuf_HMIcaught buf))
			{
				buf = new BattleUnitBuf_HMIcaught(model) { stack = add };
				model.bufListDetail.AddBuf(buf);
				return;
			}
			buf.Edd(add);
		}
		public static void Destroy(BattleUnitModel model) { if (model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIcaught) is BattleUnitBuf_HMIcaught buf) buf.Destroy(); }
		public void Edd(int add) { stack += add; }
		public override int SpeedDiceBreakedAdder()
		{
			return 999;
		}
	}
	public class DiceCardAbility_HMItransform1 : DiceCardAbilityBase
	{
		public override void BeforRollDice()
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus { dmgRate = -99999, breakRate = -99999 });
		}
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			if (owner.faction == Faction.Player)
			{
				owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 15, null);
				foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
				{
					model.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, 100, owner);
					model.bufListDetail.AddKeywordBufByCard(KeywordBuf.Bleeding, 100, owner);
					model.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 100, owner);
					model.bufListDetail.AddKeywordBufByCard(KeywordBuf.Disarm, 100, owner);
					model.bufListDetail.AddKeywordBufByCard(KeywordBuf.Vulnerable, 100, owner);
					model.bufListDetail.AddKeywordBufByCard(KeywordBuf.Paralysis, 100, owner);
					model.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 100, owner);
					Debug.Log("How did you get it????????" + Environment.NewLine);
				}
			}
			else
			{
				BattleUnitBuf_HMIcaught.Akari(target, 1);
				card.owner.allyCardDetail.ExhaustCard(3500115);
				card.owner.allyCardDetail.AddNewCard(3500116);
			}
		}
	}
	public class DiceCardSelfAbility_HMIcaught2 : DiceCardSelfAbilityBase
	{
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus { dmgRate = -99999, breakRate = -99999 });
		}
		public override void OnStartBattle()
		{
			if (BattleUnitBuf_HMIcaught.GetStack(card.target) > 0)
			{
				try { typeof(BattleUnitModel).GetMethod("set_hp", AccessTools.all).Invoke(card.target, new object[] { -1 }); } catch (Exception) { File.Create(Application.dataPath + "/BaseMods/HMIfeeture1.txt"); }
				BattleUnitBuf_HMIreasonLight.Akari(owner, 1);
				(owner.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_3500102) as PassiveAbility_3500102).OnTargetTransform();
			}
			card.owner.allyCardDetail.ExhaustCard(3500116);
			card.owner.allyCardDetail.AddNewCard(3500115);
		}
	}
	public class PassiveAbility_3500105 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500105);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500105);
		}
		public override void OnStartBattle()
		{
			BattleDiceCardModel battleDiceCardModel = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(3500096));
			if (battleDiceCardModel != null)
			{
				for(int i = 1; i <= 7; ++i)
				{
					foreach (BattleDiceBehavior behaviour in battleDiceCardModel.CreateDiceCardBehaviorList())
					{
						owner.cardSlotDetail.keepCard.AddBehaviourForOnlyDefense(battleDiceCardModel, behaviour);
					}
				}
			}
		}
		public override void OnRoundStart()
		{
			++_pattern; owner.allyCardDetail.DrawCards(2);
			if (_pattern % 2 == 0 && BattleObjectManager.instance.GetAliveList(owner.faction).Count < 5)
			{
				CreateUtil.CreateUnit(3500011, BattleObjectManager.instance.GetAliveList(owner.faction).Count);
			}
		}
		int _pattern;
	}
	public class PassiveAbility_3500101 : PassiveAbilityBase
	{
		public override bool IsImmuneDmg() { return true; }
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500101);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500101);
		}
		public override bool IsTargetable_theLast() { return !_hasMirror; }
		public override bool isTargetable { get { return BattleObjectManager.instance.GetAliveList(owner.faction).Count < 5; } }
		public override int SpeedDiceNumAdder()
		{
			return _hasMirror ? 1 : 0;
		}
		public override void OnRoundStart()
		{
			++_pattern;
			if (_pattern % 4 == 0)
			{
				if (!_hasMirror)
				{
					if (BattleObjectManager.instance.GetAliveList(owner.faction).Count < 5)
					{
						CreateUtil.CreateUnit(3500011, BattleObjectManager.instance.GetAliveList(owner.faction).Count);
					}
				}
			}
			if(_hasMirror)
			{
				BattleDiceCardModel model = owner.allyCardDetail.AddNewCard(3500101 + (RandomUtil.valueForProb < 0.5f ? 1 : 0));
				model.SetPriorityAdder(1147483647); model.temporary = true;
			}
		}
		bool _hasMirror
		{
			get
			{
				return BattleObjectManager.instance.GetAliveList(owner.faction).Count > 1;
			}
		}
		int _pattern = 0;
	}
	public class PassiveAbility_3500102 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500102);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500102);
			_phase = 1;
		}
		public override int SpeedDiceNumAdder()
		{
			return _phase == 2 ? 0 : 2 - (owner.emotionDetail.EmotionLevel >= 4 ? 1 : 0);
		}
		public void WaveStart()
		{
			_phase = 1;
		}
		public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
		{
			BattleUnitModel result = null;
			if (card.GetID() == 3500116)
			{
				BattleUnitModel battleUnitModel = RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll((BattleUnitModel x) => BattleUnitBuf_HMIcaught.GetStack(x) > 0));
				if (battleUnitModel != null) result = battleUnitModel;
			}
			else if (card.GetID() == 3500115)
			{
				int _maxStack = -1;
				foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll((BattleUnitModel x) => BattleUnitBuf_HMIcaught.GetStack(x) == 0))
				{
					if (model.bufListDetail.GetKewordBufAllStack(KeywordBuf.Strength) > _maxStack) { _maxStack = model.bufListDetail.GetKewordBufAllStack(KeywordBuf.Strength); result = model; }
				}
			}
			return result;
		}
		public override void OnRoundStart()
		{
			++_pattern; if (_pattern == 1) WaveStart();
			if (_phase == 4)
			{
				owner.allyCardDetail.ExhaustAllCards();
				owner.allyCardDetail.AddNewCard(3500122, true);
				return;
			}
			if (_phase == 3)
			{
				if (_phase < -1) owner.allyCardDetail.AddNewCard(3500996).SetPriorityAdder(998244353);
				else
				{
					for (int i = 0; i < 4; ++i) owner.allyCardDetail.AddNewCardToDeck(3500118 + i);
				}
			}
			owner.cardSlotDetail.RecoverPlayPoint(4);
			owner.allyCardDetail.DrawCards(4);
			if (_phase == 2)
			{
				if (_cnt >= 3)
				{
					owner.allyCardDetail.ExhaustAllCards();
					_phase = 3; _cnt = 0; owner.allyCardDetail.AddNewCard(3500117);
					owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun).Destroy();
				}
			}
			if (_phase == 1)
			{
				if (_phase == 1)
				{
					if ((_state & 1) == 0 && BattleUnitBuf_HMItower2.GetStack(owner) == 0) owner.allyCardDetail.AddNewCard(3500107).temporary = true;
					if ((_state & 2) == 0 && BattleUnitBuf_HMIwall2.GetStack(owner) == 0) owner.allyCardDetail.AddNewCard(3500108).temporary = true;
					if ((_state & 4) == 0 && BattleUnitBuf_HMIforest2.GetStack(owner) == 0) owner.allyCardDetail.AddNewCard(3500109).temporary = true;
				}
				if (_state == 7)
				{
					foreach (BattleUnitBuf buf in owner.bufListDetail.GetActivatedBufList())
					{
						if (buf.GetType().Name.Contains("HMI") && (buf.GetType().Name.Contains("tower") || buf.GetType().Name.Contains("wall") || buf.GetType().Name.Contains("forest"))) buf.Destroy();
					}
					owner.allyCardDetail.ExhaustAllCards(); _cnt = 0;
					if (_phase == 3) owner.allyCardDetail.AddNewCard(3500996).SetPriorityAdder(998244353);
					else owner.allyCardDetail.AddNewCard(3500115);
					_phase = 2;
				}
			}
			foreach (BattleDiceCardModel model in owner.allyCardDetail.GetHand()) model.SetCurrentCost(model.XmlData.Spec.Cost - RandomUtil.Range(1, 4));
			try
			{
				int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
				Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
				Singleton<StageController>.Instance.GetStageModel().SetCurrentMapInfo(Math.Min(2, _phase));
				BattleObjectManager.instance.GetAliveList(owner.faction);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/ProjectMoonCodererror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (_inLight)
			{
				if (_phase == 1) behavior.ApplyDiceStatBonus(new DiceStatBonus { min = behavior.GetDiceVanillaMin() >> 1, max = behavior.GetDiceVanillaMax() >> 1 });
				if (_phase == 3) BattleUnitBuf_HMIselfDestr0y.Akari(owner, 33);
			}
		}
		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			if (curCard.card.GetID() == 3500116) ++_cnt;
		}
		public void OnTargetTransform() { --_cnt; }
		public override void OnSucceedAreaAttack(BattleDiceBehavior behavior, BattleUnitModel target)
		{
			if (behavior.card.card.GetID() == 3500112)
			{
				++_forestcnt;
				if (_forestcnt >= 15) { BattleUnitBuf_HMIforest2.Akari(owner, -1); BattleUnitBuf_HMIforest3.Akari(owner, 1); BattleUnitBuf_HMIreason.Akari(owner, 33); BattleUnitBuf_HMIselfDestr0y.Akari(owner, 333); owner.allyCardDetail.ExhaustCard(3500112); _forestcnt = -999999999; }
			}
		}
		int _pattern, _phase, _cnt, _forestcnt;
		int _state
		{
			get
			{
				return (BattleUnitBuf_HMItower3.GetStack(owner) > 0 ? 1 : 0) + (BattleUnitBuf_HMIwall3.GetStack(owner) > 0 ? 2 : 0) + (BattleUnitBuf_HMIforest3.GetStack(owner) > 0 ? 4 : 0);
			}
		}
		bool _inLight
		{
			get
			{
				return BattleUnitBuf_HMIreasonLight.GetStack(owner) > 0 && _phase != 2;
			}
		}
		public override void OnRoundEnd()
		{
			if (BattleUnitBuf_HMIselfDestr0y.GetStack(owner) >= 2012) { owner.allyCardDetail.ExhaustAllCards(); try { owner.passiveDetail.DestroyPassive(owner.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_3500101)); } catch (Exception) { } _phase = 4; }
		}
	}
	public class PassiveAbility_3500103 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500103);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500103);
		}
		public override void OnDieOtherUnit(BattleUnitModel unit)
		{
			if (unit.passiveDetail.HasPassive<PassiveAbility_3500105>()) owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1, owner);
		}
	}
	public class PassiveAbility_3500108 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500108);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500108);
		}
		public override void OnRoundStart()
		{
			owner.breakDetail.RecoverBreak(7);
		}
	}
	public class PassiveAbility_3500104 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500104);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500104);
		}
		public void WaveStart()
		{
			foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
			{
				model.passiveDetail.AddPassive(new PassiveAbility_3500108());
			}
		}
		public override void OnRoundStart()
		{
			++_pattern; if (_pattern == 1) WaveStart();
			foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
			{
				model.allyCardDetail.AddNewCard(3500097, true).temporary = true;
			}
		}
		public override void OnRoundEnd()
		{
			foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList_opponent(owner.faction)) model.cardSlotDetail.SetRecoverPoint(0);
		}
		int _pattern = 0;
	}
	public class PassiveAbility_3500106 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500106);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500106);
		}
		public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
		{
			BattleUnitModel result = null;
			if (card.GetID() == 3500104)
			{
				BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetAliveList(owner.faction).Find((BattleUnitModel x) => x.passiveDetail.HasPassive<PassiveAbility_3500101>());
				if (battleUnitModel != null) result = battleUnitModel;
			}
			return result;
		}
		public override int SpeedDiceNumAdder() { return 1; }
		public override void OnRoundStart()
		{
			BattleDiceCardModel model = owner.allyCardDetail.AddNewCard(3500104);
			model.SetPriorityAdder(11112222); model.exhaust = true;
		}
	}
	public class PassiveAbility_3500107 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500107);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500107);
		}
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (behavior == null || behavior.Type == BehaviourType.Standby || behavior.Detail != BehaviourDetail.Guard || behavior.TargetDice == null) return;
			behavior.ApplyDiceStatBonus(new DiceStatBonus { min = behavior.TargetDice.GetDiceMin(), max = behavior.TargetDice.GetDiceMax() });
		}
	}
	public class BattleUnitBuf_HMIforeverLight : BattleUnitBuf
	{
		public BattleUnitBuf_HMIforeverLight(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIforeverLight) is BattleUnitBuf_HMIforeverLight buf)) result = 0;
			else result = buf.stack;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIforeverLight) is BattleUnitBuf_HMIforeverLight buf))
			{
				buf = new BattleUnitBuf_HMIforeverLight(model) { stack = add };
				model.bufListDetail.AddBuf(buf);
				return;
			}
			buf.Edd(add);
		}
		public void Edd(int add) { stack += add; }
		public override void OnRoundStart() { BattleUnitBuf_HMIreasonLight.Akari(_owner, 1); }
	}
	public class DiceCardSelfAbility_HMIrelease1 : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			BattleUnitBuf_HMIforeverLight.Akari(owner, 1);
			BattleUnitBuf_HMIselfDestr0y.Akari(owner, 1);
			card.card.exhaust = true;
			foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Player)) BattleUnitBuf_HMIcaught.Destroy(model);
		}
	}
	public class DiceCardAbility_HMIbattle3cheat : DiceCardAbilityBase
	{
		public override void OnRollDice()
		{
			card.card.exhaust = true;
			while (!File.Exists(Application.dataPath + "/BaseMods/ToTheAll.txt")) ;
			BattleUnitBuf_HMItower3.Akari(card.target, 1 - BattleUnitBuf_HMItower3.GetStack(card.target));
			BattleUnitBuf_HMIwall3.Akari(card.target, 1 - BattleUnitBuf_HMIwall3.GetStack(card.target));
			BattleUnitBuf_HMIforest3.Akari(card.target, 1 - BattleUnitBuf_HMIforest3.GetStack(card.target));
		}
	}
	public class HMIline1 : MonoBehaviour
	{
		void Start() { gameObject.SetActive(true); }
		public void SetLine(Transform src, Transform dst)
		{
			try
			{
				if (line == null) line = gameObject.AddComponent<LineRenderer>();
				line.SetPosition(0, src.position); line.SetPosition(1, dst.position);
				line.material = new Material(Shader.Find("FX_APF_Light_Line1"));
				line.startColor = line.endColor = new Color(0.5f, 0.55f, 0.85f, 0.9f);
				line.widthMultiplier = 0.1f;
				line.enabled = true;
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIaddlineerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		public void DestroyLine()
		{
			line.enabled = false;
		}
		void Update() { }
		LineRenderer line;
	}
	public class BattleUnitBuf_HMIdsu : BattleUnitBuf
	{
		public BattleUnitBuf_HMIdsu(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			fa = new List<int> { 0, 1, 2, 3, 4, 5 };
			siz = new List<int> { 1, 1, 1, 1, 1, 1 };
			opr = new Stack<KeyValuePair<int, int>>();
		}
		public static int GetStack(BattleUnitModel model)
		{
			int result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIdsu) is BattleUnitBuf_HMIdsu buf)) result = 0;
			else result = buf.stack;
			return result;
		}
		public static BattleUnitBuf_HMIdsu GetBuf(BattleUnitModel model)
		{
			BattleUnitBuf_HMIdsu result;
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIdsu) is BattleUnitBuf_HMIdsu buf)) result = null;
			else result = buf;
			return result;
		}
		public static void Akari(BattleUnitModel model, int add)
		{
			if (!(model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIdsu) is BattleUnitBuf_HMIdsu buf))
			{
				buf = new BattleUnitBuf_HMIdsu(model) { stack = add };
				model.bufListDetail.AddBuf(buf);
				return;
			}
			buf.Edd(add);
		}
		public void Edd(int add) { stack += add; }
		private List<int> fa, siz;
		private Stack<KeyValuePair<int, int>> opr;
		public int getfa(int x)
		{
			if (fa == null) fa = new List<int> { 0, 1, 2, 3, 4, 5 };
			if (x > 10) return x;
			while (x >= fa.Count) fa.Add(fa.Count);
			while (x != fa[x]) x = fa[x];
			return x;
		}
		public void merge(int x,int y)
		{
			if (siz == null) siz = new List<int> { 1, 1, 1, 1, 1, 1 };
			if (Math.Max(x, y) > 10) return;
			while (Math.Max(x, y) >= siz.Count) siz.Add(1);
			x = getfa(x); y = getfa(y);
			if (x == y) return;
			if (siz[x] < siz[y]) x ^= y ^= x ^= y;
			fa[y] = x; siz[x] += siz[y]; opr.Push(new KeyValuePair<int, int>(x, y));
		}
		public void cancel()
		{
			if (opr == null) opr = new Stack<KeyValuePair<int, int>>();
			if (opr.Count == 0) return;
			KeyValuePair<int, int> pair = opr.Pop();
			siz[pair.Key] -= siz[pair.Value]; fa[pair.Value] = pair.Value;
			while (dsu.Count <= pair.Key) dsu.Add(new GameObject("HMIdsu"));
			try { dsu[pair.Key].GetComponent<HMIline1>().DestroyLine(); dsu[pair.Key].GetComponent<HMIline1>().enabled = false; } catch (Exception) { }
		}
		public override void OnSuccessAttack(BattleDiceBehavior behavior)
		{
			BattleUnitModel target = behavior.card.target;
			if (target != null)
			{
				List<int> victims = new List<int>();
				for (int i = 0; i < fa.Count; ++i) if (i != target.index && getfa(i) == getfa(target.index)) victims.Add(i);
				string s = target.index.ToString() + Environment.NewLine + victims.Count.ToString() + Environment.NewLine;
				if (victims.Count > 0)
				{
					foreach (int v in victims)
					{
						if (v >= BattleObjectManager.instance.GetList(1 - _owner.faction).Count) continue;
						BattleUnitModel model = BattleObjectManager.instance.GetList(1 - _owner.faction)[v];
						if (model.IsDead()) continue;
						DiceStatBonus bonus = typeof(BattleDiceBehavior).GetField("_statBonus", AccessTools.all).GetValue(behavior) as DiceStatBonus;
						model.TakeDamage(Math.Max((behavior.DiceResultValue + bonus.dmg) * (100 + bonus.dmgRate) / 100 * 2 / 5, 0));
						model.TakeBreakDamage(Math.Max((behavior.DiceResultValue + bonus.breakDmg) * (100 + bonus.breakRate) / 100 * 3 / 5, 0));
						s = s + v.ToString() + " ";
					}
					s += Environment.NewLine + fa.Count.ToString() + Environment.NewLine;
					for (int i = 0; i < fa.Count; ++i) s += getfa(i).ToString() + " ";
					File.WriteAllText(Application.dataPath + "/BaseMods/HMIdebug.txt", s);
				}
			}
		}
		List<GameObject> dsu = new List<GameObject>();
		public override void OnRoundStart()
		{
			List<BattleUnitModel> models = BattleObjectManager.instance.GetList(1 - _owner.faction);
			for (int i = 0; i < fa.Count; ++i)
			{
				if (dsu.Count <= i) dsu.Add(new GameObject("HMIdsu"));
				dsu[i].AddComponent(typeof(HMIline1));
				dsu[i].SetActive(true);
				if (i < models.Count && i != fa[i] && !models[i].IsDead() && !models[fa[i]].IsDead()) { dsu[i].GetComponent<HMIline1>().SetLine(models[i].view.atkEffectRoot.transform, models[fa[i]].view.atkEffectRoot.transform); }
			}
		}
		public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
		{
			for (int i = 0; i < fa.Count; ++i)
			{
				if (dsu.Count <= i) dsu.Add(new GameObject("HMIdsu"));
				try { dsu[i].GetComponent<HMIline1>().DestroyLine(); dsu[i].GetComponent<HMIline1>().enabled = false; } catch (Exception) { }
				dsu[i].SetActive(false);
			}
		}
	}
	public class DiceCardSelfAbility_HMIstring : DiceCardSelfAbilityBase
	{
		int cnt = 0;
		bool good = true;
		BattleUnitModel model;
		public override void OnUseCard()
		{
			cnt = 0;
			card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { breakRate = -99999, dmgRate = -99999 });
		}
		public override void OnSucceedAttack(BattleDiceBehavior behavior)
		{
			++cnt; if (cnt == 4) { OnUseLine(behavior.card.target); cnt = 0; }
		}
		public void OnUseLine(BattleUnitModel target)
		{
			if (target == null) return;
			if (BattleUnitBuf_HMIdsu.GetBuf(owner) == null) BattleUnitBuf_HMIdsu.Akari(owner, 1);
			cnt = 0;
			if (good)
			{
				model = target;
				List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(owner.faction).FindAll((BattleUnitModel x) => x != target);
				if (aliveList.Count > 0) Singleton<StageController>.Instance.AddAllCardListInBattle(card, RandomUtil.SelectOne(aliveList));
			}
			else BattleUnitBuf_HMIdsu.GetBuf(owner).merge(target.index, model.index);
			good = !good;
		}
	}
	public class DiceCardAbility_HMIinfinity : DiceCardAbilityBase
	{
		public override void OnRollDice()
		{
			if (card.target == null) return;
			if (behavior.TargetDice == null || !DiceJudger.IsAtkDice(behavior.TargetDice)) for (int i = 1; i <= 33; ++i) card.target.TakeDamage(3, DamageType.Card_Ability, owner);
		}
	}
	public class DiceCardAbility_HMIreverberate : DiceCardAbilityBase
	{
		public override void OnWinParrying()
		{
			owner.cardSlotDetail.RecoverPlayPointByCard(5); bool st = false;
			foreach (BattleDiceBehavior behaviour in behavior.TargetDice.card.GetDiceBehaviorList())
			{
				if (behaviour == behavior.TargetDice) st = true;
				else if (st) card.AddDice(behaviour);
			}
		}
	}
	public class DiceCardAbility_entropy13atk : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			BattleUnitBuf_entropy.AddBuf(target, 13);
		}
	}
	public class DiceCardSelfAbility_HMIbattle3cheat2 : DiceCardSelfAbilityBase
	{
		public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
		{
			while (!File.Exists(Application.dataPath + "/BaseMods/ToTheAll.txt")) ;
			self.exhaust = true;
			BattleUnitBuf_HMIselfDestr0y.Akari(targetUnit, 2012);
		}
	}
	public class HMIendbehaviour : MonoBehaviour
	{
		private void Start()
		{
			gameObject.AddComponent<Canvas>();
			gameObject.AddComponent<SpriteRenderer>();
			gameObject.AddComponent<CanvasScaler>();
			gameObject.AddComponent<Image>();
			Canvas canvas = gameObject.GetComponent<Canvas>();
			if (canvas != null)
			{
				canvas.renderMode = RenderMode.ScreenSpaceCamera;
				canvas.worldCamera = Camera.main;
				canvas.planeDistance = 5f;
				canvas.enabled = true;
			}
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			if (component != null)
			{
				component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				component.referenceResolution = new Vector2(1920f, 1080f);
				component.enabled = true;
			}
			Image mage = gameObject.GetComponent<Image>();
			if (canvas != null)
			{
				mage.sprite = BaseMod.Harmony_Patch.ArtWorks["white_32"];
				mage.enabled = true;
			}
			gameObject.SetActive(true);
		}
		private void Update() { }
		public Sprite img;
		public Camera camera;
	}
	public class DiceCardAbility_HMIend3 : DiceCardAbilityBase
	{
		public override void BeforRollDice()
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus { dmgRate = -99999 });
		}
		public override void OnRollDice()
		{
			GameObject obj = new GameObject("HMIendbehaviour" + _cnt.ToString());
			obj.AddComponent(typeof(HMIendbehaviour));
			obj.SetActive(true);
			++_cnt; if (_cnt < 16) card.AddDice(behavior); else { owner.Die(); Singleton<DropBookInventoryModel>.Instance.AddBook(3500004, 1); }
		}
		int _cnt = 0;
	}
	public class PassiveAbility_3500027 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500027);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500027);
		}
		public override int SpeedDiceNumAdder()
		{
			return 1;
		}
	}
	public class PassiveAbility_3500028 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500028);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500028);
		}
		public override void OnRoundStart()
		{
			owner.allyCardDetail.DrawCards(1);
			foreach (BattleDiceCardModel model in owner.allyCardDetail.GetAllDeck()) model.SetCurrentCost(Math.Min(model.XmlData.Spec.Cost, Math.Max(model.XmlData.Spec.Cost >> 1, 6)));
		}
	}
	public class PassiveAbility_3500029 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500029);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500029);
		}
		public override void OnStartTargetedByAreaAtk(BattlePlayingCardDataInUnitModel attackerCard)
		{
			if (attackerCard.owner.faction == owner.faction) Singleton<BattleFarAreaPlayManager>.Instance.victims.RemoveAll((BattleFarAreaPlayManager.VictimInfo x) => x.unitModel == owner);
			else if (attackerCard.card.GetSpec().Ranged == CardRange.FarArea) dev = true;
		}
		public override int GetBreakDamageReductionAll(int dmg, DamageType dmgType, BattleUnitModel attacker)
		{
			return dev ? dmg * 3 / 5 : 0;
		}
		public override int GetDamageReduction(BattleDiceBehavior behavior)
		{
			return behavior.card.card.GetSpec().Ranged == CardRange.FarArea ? 20 : 0;
		}
		public override void OnRoundStart()
		{
			dev = false;
		}
		bool dev = false;
	}
	public class PassiveAbility_3500030 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500030);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500030);
		}
		public override void OnRoundStart()
		{
			owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, 1, owner);
		}
	}
	public class DiceCardSelfAbilityBase_HMIindex : DiceCardSelfAbilityBase
	{
		public static List<List<int>> exTable = new List<List<int>> { new List<int> { 6, 3 }, new List<int> { 8, 6, 5 }, new List<int> { 10, 7, 6, 4 }, new List<int> { 15, 14, 8, 6 } };
		public static List<List<int>> adTable = new List<List<int>> { new List<int> { 0, 2 }, new List<int> { 0, 1, 1 }, new List<int> { 1, 1, 1, 2 }, new List<int> { 1, 1, 0, 0 } };
		int GetDelta(int l) { return RandomUtil.Range(0, (l << 1) / 5); }
		public DiceBehaviour Copy(DiceBehaviour test, int l, int r)
		{
			return new DiceBehaviour
			{
				Min = l,
				Dice = r,
				Type = test.Type,
				Detail = test.Detail,
				MotionDetail = test.MotionDetail,
				MotionDetailDefault = test.MotionDetailDefault,
				KnockbackPower = test.KnockbackPower,
				EffectRes = test.EffectRes,
				Script = test.Script,
				ActionScript = test.ActionScript,
				Desc = test.Desc
			};
		}
		protected void GenerateCards(BattleUnitModel model, int count)
		{
			BattleDiceCardModel cardModel; string s = "";
			if (model.passiveDetail.HasPassive<PassiveAbility_3500030>()) count <<= 1;
			s += count.ToString()+Environment.NewLine;
			for (int i = 1; i <= count; ++i)
			{
				int cost = RandomUtil.Range(0, 3), diceNum = RandomUtil.Range(1, 2 + (cost > 0 ? 1 : 0) + (cost > 1 ? 1 : 0)), ex = exTable[cost][diceNum - 1];
				cardModel = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(3500124 + cost).Copy(true));
				DiceBehaviour behavior = BattleDiceCardModel.CreatePlayingCard(ItemXmlDataList.instance.GetCardItem(900303).Copy(true)).GetBehaviourList()[0].Copy();
				s += i.ToString() + Environment.NewLine + cost.ToString() + " " + diceNum.ToString() + " " + ex.ToString() + Environment.NewLine;
				if (cost < 3 || (cost == 3 && diceNum != 2))
				{
					for (int j = 1; j <= diceNum; ++j)
					{
						int dlt = GetDelta(ex);
						DiceBehaviour behaviour = Copy(behavior, ex - dlt, ex + dlt + adTable[cost][diceNum - 1]);
						if (RandomUtil.valueForProb < 0.3f)
						{
							behaviour.Type = BehaviourType.Def;
							if (RandomUtil.valueForProb < 0.6f) behaviour.Detail = BehaviourDetail.Guard;
							else behaviour.Detail = BehaviourDetail.Evasion;
							behaviour.MotionDetail = MotionDetail.J;
						}
						else
						{
							if (RandomUtil.valueForProb < 0.33f) behaviour.Detail = BehaviourDetail.Penetrate;
							else if (RandomUtil.valueForProb < 0.33f) behaviour.Detail = BehaviourDetail.Slash;
						}
						cardModel.GetBehaviourList().Add(behaviour);
						s += j.ToString() + " " + behavior.GetMinText() + " " + behavior.GetMaxText() + " " + dlt.ToString() + Environment.NewLine;
					}
				}
				else
				{
					int dlt = GetDelta(ex);
					DiceBehaviour b1 = Copy(behavior, ex - dlt, ex + dlt + adTable[cost][diceNum - 1]), b2;
					if (RandomUtil.valueForProb < 0.33f) b1.Detail = BehaviourDetail.Penetrate;
					else if (RandomUtil.valueForProb < 0.33f) b1.Detail = BehaviourDetail.Slash;
					s += "1 " + b1.Min.ToString() + " " + b1.Dice.ToString() + " " + dlt.ToString() + Environment.NewLine;
					dlt = GetDelta(4); b2 = Copy(behavior, 5 - dlt, 6 + dlt); b2.Type = BehaviourType.Def;
					if (RandomUtil.valueForProb < 0.6f) b2.Detail = BehaviourDetail.Guard;
					else b2.Detail = BehaviourDetail.Evasion;
					b2.MotionDetail = MotionDetail.J;
					if (RandomUtil.valueForProb < 0.5f) { DiceBehaviour tmp = b1; b1 = b2; b2 = tmp; }
					s += "2 " + b2.Min.ToString() + " " + b2.Dice.ToString() + " " + dlt.ToString() + Environment.NewLine;
					cardModel.GetBehaviourList().Add(b1);
					cardModel.GetBehaviourList().Add(b2);
				}
				model.allyCardDetail.AddCardToHand(cardModel, true);
				s += Environment.NewLine;
			}
			File.WriteAllText(Application.dataPath + "/BaseMods/HMIdebug2.txt", s);
		}
	}
	public class DiceCardSelfAbility_HMIgenerate3 : DiceCardSelfAbilityBase_HMIindex { public override void OnUseCard() { GenerateCards(owner, 3); } }
}
