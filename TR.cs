using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BattleCharacterProfile;
using LOR_DiceSystem;
using UnityEngine;
using HMI_FragOfficeRemake_MOD;
using System.Reflection;
using System.Diagnostics;

namespace HMI_ErrorTeam_MOD
{
	public static class ExceptionMaker
	{
		static readonly List<Exception> exs = new List<Exception> { new AccessViolationException(), new AggregateException(), new AppDomainUnloadedException(), new ApplicationException(), new ArgumentException(), new ArgumentNullException(), new ArgumentOutOfRangeException(), new ArithmeticException(), new ArrayTypeMismatchException(), new BadImageFormatException(), new CannotUnloadAppDomainException(), new ContextMarshalException(), new DataMisalignedException(), new DecoderFallbackException(), new DivideByZeroException(), new DllNotFoundException(), new DuplicateWaitObjectException(), new EncoderFallbackException(), new EntryPointNotFoundException(), new FieldAccessException(), new FormatException(), new IndexOutOfRangeException(), new InsufficientExecutionStackException(), new InsufficientMemoryException(), new InvalidCastException(), new InvalidOperationException(), new InvalidProgramException(), new InvalidTimeZoneException(), new KeyNotFoundException(), new MemberAccessException(), new MethodAccessException(), new MissingFieldException(), new MissingMemberException(), new MissingMethodException(), new MulticastNotSupportedException(), new NotFiniteNumberException(), new NotImplementedException(), new NotSupportedException(), new NullReferenceException(), new ObjectDisposedException("LibraryModel"), new OperationCanceledException(), new OutOfMemoryException(), new OverflowException(), new PlatformNotSupportedException(), new RankException(), new StackOverflowException(), new TaskCanceledException(), new TaskSchedulerException(), new TimeoutException(), new TimeZoneNotFoundException(), new TypeAccessException(), new TypeInitializationException("LibraryModel", new NullReferenceException()), new TypeLoadException(), new TypeUnloadedException(), new UnauthorizedAccessException(), new UriFormatException(), new ReflectionTypeLoadException(new Type[] { typeof(Add_On) }, new Exception[] { new NullReferenceException() }) };
		public static Exception Work()
		{
			return RandomUtil.SelectOne(exs);
		}
		public static string GetStackTrace()
		{
			string info = "";
			StackTrace st = new StackTrace();
			StackFrame[] sfs = st.GetFrames();
			for (int i = 0; i < sfs.Length; ++i)
			{
				string name = sfs[i].GetMethod().DeclaringType.FullName + "." + sfs[i].GetMethod().Name, para = "";
				foreach (var parameter in sfs[i].GetMethod().GetParameters())
				{
					if (para != "") para += ", ";
					para += parameter.ParameterType.FullName + " " + parameter.Name;
				}
				if (name.Contains("HMI_ErrorTeam_MOD")) continue;
				info = "  at " + name + " (" + para + ") [0x" + RandomUtil.Range(0, 1048575).ToString("X5").ToLower() + "] in <" + RandomUtil.Range(0, 2147483646).ToString("X8").ToLower() + RandomUtil.Range(0, 2147483646).ToString("X8").ToLower() + RandomUtil.Range(0, 2147483646).ToString("X8").ToLower() + RandomUtil.Range(0, 2147483646).ToString("X8").ToLower() + ">:0" + Environment.NewLine + info;
			}
			return info;
		}
		public static int GetErrorFileCount()
		{
			int count = 0;
			DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/BaseMods");
			foreach (FileInfo file in dir.GetFiles())
			{
				if (file.Name.Contains("error") && !file.Name.Contains("Add_Onerror1")) ++count;
			}
			return count;
		}
	}
	public class DiceCardSelfAbility_HMIwriteErr1 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			try
			{
				Exception e = ExceptionMaker.Work();
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIRandom" + (RandomUtil.Range(0, 2147483646) + 1).ToString("D10") + "error.txt", e.Message + Environment.NewLine + ExceptionMaker.GetStackTrace());
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIMakeErrorerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
	}
	public class DiceCardSelfAbility_HMIlightErr3 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { owner.cardSlotDetail.RecoverPlayPointByCard(Math.Max(1, Math.Min(3, ExceptionMaker.GetErrorFileCount()))); }
	}
	public class DiceCardSelfAbility_HMIlightErr3drawErr2 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { owner.cardSlotDetail.RecoverPlayPointByCard(Math.Max(1, Math.Min(3, ExceptionMaker.GetErrorFileCount()))); owner.allyCardDetail.DrawCards(Math.Max(1, Math.Min(2, ExceptionMaker.GetErrorFileCount()))); }
	}
	public class DiceCardSelfAbility_HMIpowerErr10 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { card.ignorePower = false; card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { power = Math.Max(1, Math.Min(10, ExceptionMaker.GetErrorFileCount())) }); }
	}
	public class DiceCardSelfAbility_HMIpowerErr4 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { card.ignorePower = false; card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { power = Math.Max(1, Math.Min(4, ExceptionMaker.GetErrorFileCount())) }); }
	}
	public class DiceCardSelfAbility_HMIpowernoErr10 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { card.ignorePower = false; card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { power = Math.Max(1, 10 - ExceptionMaker.GetErrorFileCount()) }); }
	}
	public class DiceCardSelfAbility_HMIeRRoRFinalcard : DiceCardSelfAbilityBase
	{
		public override void OnUseCard() { card.ignorePower = false; card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { power = Math.Max(1, 10 - ExceptionMaker.GetErrorFileCount()) }); card.card.exhaust = true; }
	}
	public class DiceCardSelfAbility_HMIwebErrNxtRound : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			card.card.exhaust = true;
			foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(owner.faction)) model.allyCardDetail.AddNewCard(card.card.GetID() + 1, true).AddBuf(new DiceCardSelfAbility_lowelonly.BattleDiceCardBuf_lowel());
		}
	}
	public class PassiveAbility_3502001 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3502001);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3502001);
		}
		public override int SpeedDiceNumAdder()
		{
			return 2 - owner.emotionDetail.SpeedDiceNumAdder();
		}
	}
	public class PassiveAbility_3502002 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3502002);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3502002);
		}
		public override void OnSucceedAttack(BattleDiceBehavior behavior)
		{
			BattleUnitBuf_entropy.AddBuf(behavior.card.target, 1);
		}
		public override int GetDamageReduction(BattleDiceBehavior behavior)
		{
			return (behavior.card.card.GetSpec().Ranged == CardRange.FarArea || behavior.card.card.GetSpec().Ranged == CardRange.FarAreaEach) ? 214748364 : 0;
		}
		public override bool IsImmuneDmg(DamageType type, KeywordBuf keyword = KeywordBuf.None)
		{
			return type == DamageType.Buf;
		}
	}
	public class PassiveAbility_3502003 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3502003);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3502003);
		}
		public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
		{
			if (owner.hp <= dmg && BattleObjectManager.instance.GetAliveList(owner.faction).Count == 1) { owner.bufListDetail.AddBuf(new HMItmpBuf()); owner.RecoverHP(owner.MaxHp); }
			return false;
		}
		private class HMItmpBuf : BattleUnitBuf
		{
			public HMItmpBuf() { stack = 2; }
			public override int GetDamageReductionAll() { return 19260817; }
			public override int SpeedDiceNumAdder() { return -2; }
			public override void OnRoundEnd() { --stack; _owner.allyCardDetail.ExhaustAllCards(); _owner.allyCardDetail.AddNewCard(3502007, true); _owner.ResetBreakGauge(); if (stack <= 0) { Destroy(); _owner.Die(); } }
		}
	}
	public class PassiveAbility_3502004 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3502004);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3502004);
		}
		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus { power = cnt + Math.Min(ExceptionMaker.GetErrorFileCount() / 5, 3) });
		}
		public override void OnDieOtherUnit(BattleUnitModel unit) { if (unit.faction == (Faction)((int)owner.faction ^ 1)) ++cnt; }
		public override void OnWaveStart()
		{
			cnt = 0;
		}
		int cnt = 0;
	}
}
