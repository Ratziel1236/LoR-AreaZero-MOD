using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using BaseMod;
using HarmonyLib;
using UnityEngine;
using LOR_DiceSystem;
using System.Numerics;
using System.Security.Cryptography;
using NAudio.Wave;
using UI;
using HMI_Core;
using System.Threading;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
using System.Linq;

namespace HMI_FragOfficeRemake_MOD
{
	public class Harmony_Patch
	{
		public Harmony_Patch()
		{
			try
			{
				IEnumerator e = Enum.GetValues(typeof(ActionDetail)).GetEnumerator();
				while (e.MoveNext()) { }
				bases1.Clear(); bases2.Clear();
				for (int i = 0; i < lis.Count; i++)
				{
					Type type = Type.GetType("DiceCardAbility_" + lis[i].Trim());
					if (type != null)
					{
						DiceCardAbilityBase diceCardAbilityBase = Activator.CreateInstance(type) as DiceCardAbilityBase;
						if (diceCardAbilityBase != null)
						{
							bases1.Add(diceCardAbilityBase);
						}
					}
					type = Type.GetType("DiceCardSelfAbility_" + lis[i].Trim());
					if (type != null)
					{
						DiceCardSelfAbilityBase diceCardSelfAbilityBase = Activator.CreateInstance(type) as DiceCardSelfAbilityBase;
						if (diceCardSelfAbilityBase != null)
						{
							bases2.Add(diceCardSelfAbilityBase);
						}
					}
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/GOODTEKerror.txt", string.Concat(new string[]
				{
					"It's a feature,so you don't need to care about it.",
					Environment.NewLine,
					ex.Message,
					Environment.NewLine,
					ex.StackTrace
				}));
			}
			try
			{
				Harmony harmony = new Harmony("HMI_FragOffice_MOD");
				harmony.Patch(typeof(LibraryModel).GetMethod("OnClearStage", AccessTools.all), null, new HarmonyMethod(typeof(Harmony_Patch).GetMethod("LibraryModel_OnClearStage_Post")), null, null);
				harmony.Patch(typeof(BattleDiceCardUI).GetMethod("SetCard", AccessTools.all), null, new HarmonyMethod(typeof(Harmony_Patch).GetMethod("BattleDiceCardUI_SetCard_Post")), null, null);
				//harmony.Patch(typeof(BattleUnitInformationUI_Passive).GetMethod("SetPassives", AccessTools.all), null, new HarmonyMethod(typeof(Harmony_Patch).GetMethod("BattleUnitInformationUI_Passive_SetPassives_Post")), null, null);
				harmony.Patch(typeof(BattleDiceCard_BehaviourDescUI).GetMethod("SetBehaviourInfo", AccessTools.all), null, new HarmonyMethod(typeof(Harmony_Patch).GetMethod("BattleDiceCard_BehaviourDescUI_SetBehaviourInfo_Post")), null, null);
				harmony.Patch(typeof(UIBufOverlay).GetMethod("GetDescription", AccessTools.all/*, null, new Type[]{typeof(string)}, null*/), null, new HarmonyMethod(typeof(Harmony_Patch).GetMethod("UIBufOverlay_GetDescription_Post")), null, null);
				harmony.Patch(typeof(BattleAllyCardDetail).GetMethod("DrawCards", AccessTools.all), new HarmonyMethod(typeof(Harmony_Patch).GetMethod("BattleAllyCardDetail_DrawCards_Pre")), null, null, null);
				strMap = new Dictionary<string, string>();
			}
			catch (Exception ex2)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIHPerror.txt", ex2.Message + Environment.NewLine + ex2.StackTrace);
			}
			try
			{
				string str = "";
				foreach (DirectoryInfo directoryInfo in Add_On.instance.DirList)
				{
					if (Directory.Exists(directoryInfo.FullName + "/HMIBGM3"))
					{
						path = directoryInfo;
						str = directoryInfo.FullName;
					}
				}
				HMI3_11BGM = mp3toAudioClip(str + "/HMIBGM3/Branch1.mp3");
				HMI3_21BGM = mp3toAudioClip(str + "/HMIBGM3/Branch2.mp3");
			}
			catch (Exception ex3)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIImportingBGMerror.txt", ex3.Message + Environment.NewLine + ex3.StackTrace);
			}
			try
			{
				Harmony harmony = new Harmony("LOR.HMIBG3003");
				harmony.Patch(typeof(StageController).GetMethod("EndBattlePhase", AccessTools.all), null, new HarmonyMethod(typeof(Harmony_Patch).GetMethod("StageController_EndBattlePhase")), null, null);
				harmony.Patch(typeof(UIStoryProgressPanel).GetMethod("SetStoryLine", AccessTools.all), new HarmonyMethod(typeof(Harmony_Patch).GetMethod("UIStoryProgressPanel_SetStoryLine")), null, null, null);
				harmony.Patch(typeof(UISpriteDataManager).GetMethod("GetStoryIcon", AccessTools.all), new HarmonyMethod(typeof(Harmony_Patch).GetMethod("UISpriteDataManager_GetStoryIcon")), null, null, null);
				CreateUtil.DefFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
				EffectSprites = new Dictionary<string, Sprite>();
				Inited = false;
				DirectoryInfo dir = null;
				foreach (DirectoryInfo directoryInfo in Add_On.instance.DirList)
				{
					if (Directory.Exists(directoryInfo.FullName + "/HMIBack3"))
					{
						path = directoryInfo;
						dir = new DirectoryInfo(directoryInfo.FullName + "/HMIBack3");
					}
				}
				textures = new Dictionary<string, Texture2D>();
				textureInit(dir);
				battleBGM = new Dictionary<string, AudioClip>();
				AudioClip value = mp3toAudioClip(path.FullName + "/HMIBGM3/Branch3Emotion1.mp3");
				battleBGM.Add("331", value);
				value = mp3toAudioClip(path.FullName + "/HMIBGM3/Branch3Emotion2.mp3");
				battleBGM.Add("332", value);
				value = mp3toAudioClip(path.FullName + "/HMIBGM3/Branch3Emotion3.mp3");
				battleBGM.Add("333", value);
				value = mp3toAudioClip(path.FullName + "/HMIBGM3/Branch4Phase1.mp3");
				battleBGM.Add("401", value);
				value = mp3toAudioClip(path.FullName + "/HMIBGM3/Branch4Phase2.mp3");
				battleBGM.Add("402", value);
				Init = false;
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIBGerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		public static void LibraryModel_OnClearStage_Post(LibraryModel __instance, int stageId)
		{
			try
			{
				if (stageId == 3500001 && Singleton<DropBookInventoryModel>.Instance.GetBookCount(3500001) == 0)
				{
					int num = 3500001;
					int num2 = 7;
					List<string> list = new List<string>();
					Singleton<DropBookInventoryModel>.Instance.AddBook(num, num2);
					list.Add("获得" + Singleton<DropBookXmlList>.Instance.GetData(num, false).Name);
					string alarmText = string.Join("\n", list);
					UIAlarmPopup.instance.SetAlarmText(alarmText);
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIDropbookerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		public static bool BattleAllyCardDetail_DrawCards_Pre(BattleAllyCardDetail __instance, int count)
		{
			try
			{
				List<BattleDiceCardModel> list = new List<BattleDiceCardModel>();
				foreach (BattleDiceCardModel battleDiceCardModel in __instance.GetAllDeck())
				{
					if (battleDiceCardModel.GetID() == 3500097)
					{
						list.Add(battleDiceCardModel);
					}
				}
				if (list.Count > 0)
				{
					foreach (BattleDiceCardModel item in list)
					{
						((List<BattleDiceCardModel>)__instance.GetType().GetField("_cardInReserved", AccessTools.all).GetValue(__instance)).Remove(item);
						((List<BattleDiceCardModel>)__instance.GetType().GetField("_cardInUse", AccessTools.all).GetValue(__instance)).Remove(item);
						((List<BattleDiceCardModel>)__instance.GetType().GetField("_cardInDiscarded", AccessTools.all).GetValue(__instance)).Remove(item);
						((List<BattleDiceCardModel>)__instance.GetType().GetField("_cardInDeck", AccessTools.all).GetValue(__instance)).Remove(item);
						((List<BattleDiceCardModel>)__instance.GetType().GetField("_cardInHand", AccessTools.all).GetValue(__instance)).Remove(item);
						((List<BattleDiceCardModel>)__instance.GetType().GetField("_cardInHand", AccessTools.all).GetValue(__instance)).Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMISpecial.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
			return true;
		}

		public static void BattleCardDescXmlList_GetAbilityDesc_Post(int cardID, ref string __result)
		{
			try
			{
				if (__result != null && cardID != 3500021 && gotLightCardIDs.FindAll((int x) => x == cardID).Count != 0)
				{
					byte[] bytes = Convert.FromBase64String(__result);
					__result = Encoding.UTF8.GetString(bytes);
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIPatchCardXmlerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		
		/*private static string RandomShuffle_string(string s,int d = 2,int t = 20)
		{
			char[] res = s.ToCharArray();
			RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
			byte[] array = new byte[8];
			for (int i = 1; i <= Math.Min(t, 1000000); ++i)
			{
				randomNumberGenerator.GetBytes(array);
				BigInteger big = new BigInteger(array);
				int a = ((int)(big % new BigInteger(s.Length)) - d) / d * d;
				randomNumberGenerator.GetBytes(array);
				big = new BigInteger(array);
				int b = ((int)(big % new BigInteger(s.Length)) - d) / d * d;
				if (a < 0) a += d; if (b < 0) b += d;
				if (Math.Max(a, b) + d < s.Length) for (int j = 0; j < d; ++j) { char tmp = res[a + j]; res[a + j] = res[b + j]; res[b + j] = tmp; }
			}
			return res.ToString();
			if (s == null || s.Length == 0) return "";
			int[] res = new int[s.Length / d + 1];
			RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
			byte[] array = new byte[8];
			string ret = "";
			for(int i = 0; i < s.Length; ++i) { res[i / d] = (res[i / d] << 8) + s[i]; }
			for (int i = 1; i <= Math.Min(t, 1000000); ++i)
			{
				randomNumberGenerator.GetBytes(array);
				BigInteger big = new BigInteger(array);
				int a = ((int)(BigInteger.Abs(big) % new BigInteger(s.Length)) - d) / d * d;
				randomNumberGenerator.GetBytes(array);
				big = new BigInteger(array);
				int b = ((int)(BigInteger.Abs(big) % new BigInteger(s.Length)) - d) / d * d;
				a = (a % s.Length + s.Length) % s.Length;b = (b % s.Length + s.Length) % s.Length;
				int tmp = res[a]; res[a] = res[b]; res[b] = tmp;
			}
			for (int i = 0; i < s.Length; ++i) { ret += (char)((res[i] >> 8) & (1 << 8)); ret += (char)(res[i] & (1 << 8)); }
			return ret;
		}*/

		private static string WTF(List<string> lis)
		{
			lis = lis.OrderBy(p => Guid.NewGuid().ToString()).ToList();
			string res = "";for (int i = 0; i < lis.Count; ++i) res += lis[i];
			return res;
		}

		public static void UIBufOverlay_GetDescription_Post(UIBufOverlay __instance, ref string __result)
		{
			try
			{
				List<string> lis = new List<string>{ "机", "体", "随", "机", "性", "破", "损", "，", "受", "到", "的", "伤", "害", "可", "能", "增", "加" };
				if (__instance.GetName().Trim().Contains("序无")) __result = /*RandomShuffle_string("机体随机性破损，受到的伤害可能增加", 2, 20)*/WTF(lis);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIPatchBufUIerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		static bool IsGood(char fir, int len) { return (fir >= '0' && fir <= '9' || fir >= 'A' && fir <= 'Z' || fir == '+' || fir == '/') && (len & 3) == 0; }
		public static void BattleDiceCardUI_SetCard_Post(BattleDiceCardUI __instance, BattleDiceCardModel cardModel, params BattleDiceCardUI.Option[] options)
		{
			if (cardModel == null || __instance == null)
			{
				return;
			}
			int id = cardModel.GetID();
			if (gotLightCardIDs.Contains(id))
			{
				if (cardModel.GetRarity() == Rarity.Special)
				{
					try
					{
						Coloring(__instance, new Color(0.3f, 0.3f, 0.3f), new Color(0.4f, 0.4f, 0.4f));
						goto IL_157;
					}
					catch (Exception ex)
					{
						File.WriteAllText(Application.dataPath + "/BaseMods/HMIColoringCardSpecial.txt", ex.Message + Environment.NewLine + ex.StackTrace);
						goto IL_157;
					}
				}
				if (cardModel.GetRarity() == Rarity.Unique)
				{
					try
					{
						Coloring(__instance, new Color(0f, 0f, 0f), new Color(0.2f, 0.2f, 0.2f));
						goto IL_157;
					}
					catch (Exception ex2)
					{
						File.WriteAllText(Application.dataPath + "/BaseMods/HMIColoringCardUnique.txt", ex2.Message + Environment.NewLine + ex2.StackTrace);
						goto IL_157;
					}
				}
				try
				{
					Coloring(__instance, new Color(0.5f, 0.5f, 0.5f), new Color(0.6f, 0.6f, 0.6f));
				}
				catch (Exception ex3)
				{
					File.WriteAllText(Application.dataPath + "/BaseMods/HMIColoringCardCommon.txt", ex3.Message + Environment.NewLine + ex3.StackTrace);
				}
			IL_157:
				if (cardModel.owner != null && cardModel.owner.passiveDetail.HasPassive<PassiveAbility_3500011>())
				{
					if (!cardModel.owner.bufListDetail.GetActivatedBufList().Exists((BattleUnitBuf x) => x is BattleUnitBuf_GotLightLabel))
					{
						try
						{
							if (__instance.txt_selfAbility.text != null && __instance.txt_selfAbility.text.Trim() != "" && !IsGood(__instance.txt_selfAbility.text.Trim()[0], __instance.txt_selfAbility.text.Trim().Length)/* && !IsBase64Formatted(__instance.txt_selfAbility.text.Trim())*/)
							{
								if (!strMap.ContainsKey(__instance.txt_selfAbility.text.Trim())) strMap.Add(__instance.txt_selfAbility.text.Trim(), Convert.ToBase64String(Encoding.UTF8.GetBytes(__instance.txt_selfAbility.text.Trim())));
								__instance.txt_selfAbility.text = /*""*/strMap[__instance.txt_selfAbility.text.Trim()];
								/*byte[] bytes = Convert.FromBase64String(__instance.txt_selfAbility.text);
								__instance.txt_selfAbility.text = Encoding.UTF8.GetString(bytes);*/
								//byte[] bytes = new byte[Encoding.UTF8.GetBytes(__instance.txt_selfAbility.text).ToString().Length];
								//bytes = ;
								/*Convert.ToBase64String(Encoding.UTF8.GetBytes(__instance.txt_selfAbility.text))*/
							}//啥b月亮的UI是每次重新生成的，然后原代码会MLE。但是之前的版本表现良好，原因未知
						}
						catch (Exception ex4)
						{
							File.WriteAllText(Application.dataPath + "/BaseMods/HMIDecodingCardSelfScript.txt", ex4.Message + Environment.NewLine + ex4.StackTrace);
						}
						try
						{
							for (int i = 0; i < __instance.ui_behaviourDescList.Count; i++)
							{
								if (__instance.ui_behaviourDescList[i].txt_ability.text != null && __instance.ui_behaviourDescList[i].txt_ability.text.Trim() != "" && !IsGood(__instance.ui_behaviourDescList[i].txt_ability.text.Trim()[0], __instance.ui_behaviourDescList[i].txt_ability.text.Trim().Length)/* && !IsBase64Formatted(__instance.ui_behaviourDescList[i].txt_ability.text.Trim())*/)
								{
									if (!strMap.ContainsKey(__instance.ui_behaviourDescList[i].txt_ability.text.Trim())) strMap.Add(__instance.ui_behaviourDescList[i].txt_ability.text, Convert.ToBase64String(Encoding.UTF8.GetBytes(__instance.ui_behaviourDescList[i].txt_ability.text.Trim())));
									__instance.ui_behaviourDescList[i].txt_ability.text = /*""*/strMap[__instance.ui_behaviourDescList[i].txt_ability.text.Trim()];
									/*byte[] bytes2 = Convert.FromBase64String(__instance.ui_behaviourDescList[i].txt_ability.text);
									__instance.ui_behaviourDescList[i].txt_ability.text = Encoding.UTF8.GetString(bytes2);*/
									//byte[] bytes = new byte[Encoding.UTF8.GetBytes(__instance.ui_behaviourDescList[i].txt_ability.text).ToString().Length];
									//bytes = ;
									/*Convert.ToBase64String(Encoding.UTF8.GetBytes(__instance.ui_behaviourDescList[i].txt_ability.text))*/
								}
							}
						}
						catch (Exception ex5)
						{
							File.WriteAllText(Application.dataPath + "/BaseMods/HMIDecodingCardDiceBehaviourScript.txt", ex5.Message + Environment.NewLine + ex5.StackTrace);
						}
						return;
					}
				}
			}
			else if (id >= 3500100 && id <= 3500600 && id % 100 == 0)
			{
				try
				{
					Coloring(__instance, new Color(0.3f, 0.3f, 0.3f), new Color(0.4f, 0.4f, 0.4f));
				}
				catch (Exception)
				{
				}
			}
		}

		public static bool IsBase64Formatted(string input) { try { Convert.FromBase64String(input); return true; } catch { return false; } }

		/*public static void BattleUnitInformationUI_Passive_SetPassives_Post(BattleUnitInformationUI_Passive __instance, List<BattleUnitInformationUI.Desc> passives)
		{
			if (passives == null || passives.Count == 0 || __instance == null) return;
			foreach (BattleUnitInformationUI.Desc desc in passives)
			{
				if (IsBase64Formatted(desc.name))
			}
		}*/

		public static void BattleDiceCard_BehaviourDescUI_SetBehaviourInfo_Post(BattleDiceCard_BehaviourDescUI __instance, DiceBehaviour behaviour, int cardId, List<DiceBehaviour> behaviourList, bool isHide = false)
		{
			try
			{
				if (gotLightCardIDs.Contains(cardId))
				{
					Color color = new Color(0.75f, 0.75f, 0.75f);
					//Color color2 = new Color();color2 = color * 0.6f;
					//color2.a = 1f;
					//__instance.img_detail.color = color2;
					//__instance.img_diceFrame.color = color2;
					//__instance.img_diceLinearDodge.color = color;
					//__instance.img_diceLinearDodge.enabled = false;
					//__instance.img_baseLine.color = color;
					__instance.txt_ability.color = color;
					//__instance.img_dicefaces[0].color = color;
					//__instance.img_dicefaces[1].color = color;
					__instance.txt_range.color = color;
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIDiceColoringError.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		public static void Coloring(BattleDiceCardUI __instance, Color FrameColor, Color LinearDodgeColor)
		{
			try
			{
				typeof(BattleDiceCardUI).GetMethod("SetFrameColor", AccessTools.all).Invoke(__instance, new object[]
				{
					FrameColor
				});
				typeof(BattleDiceCardUI).GetMethod("SetLinearDodgeColor", AccessTools.all, null, new Type[]
				{
					typeof(Color)
				}, null).Invoke(__instance, new object[]
				{
					LinearDodgeColor
				});
				typeof(BattleDiceCardUI).GetField("colorFrame", AccessTools.all).SetValue(__instance, FrameColor);
				typeof(BattleDiceCardUI).GetField("colorLineardodge", AccessTools.all).SetValue(__instance, LinearDodgeColor);
				typeof(BattleDiceCardUI).GetField("colorLineardodge_deactive", AccessTools.all).SetValue(__instance, new Color(LinearDodgeColor.r, LinearDodgeColor.g, LinearDodgeColor.b, 0.1f));
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIColoringFunction.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		public static AudioClip mp3toAudioClip(string path)
		{
			Mp3FileReader sourceProvider = new Mp3FileReader(path);
			WaveFileWriter.CreateWaveFile(path + ".wav", sourceProvider);
			WAV wav = new WAV(File.ReadAllBytes(path + ".wav"));
			AudioClip audioClip = AudioClip.Create("HMI3BGM", wav.SampleCount, 1, wav.Frequency, false);
			audioClip.SetData(wav.LeftChannel, 0);
			File.Delete(path + ".wav");
			return audioClip;
		}

		public static AudioClip ChangeAtkSound(string path, BattleUnitModel model, MotionDetail changeDetail)
		{
			WAV wav = new WAV(File.ReadAllBytes(Harmony_Patch.path.FullName + "/CustomEffect/" + path + ".wav"));
			AudioClip audioClip = AudioClip.Create(path, wav.SampleCount, 1, wav.Frequency, false);
			audioClip.SetData(wav.LeftChannel, 0);
			List<CharacterSound.Sound> list = (List<CharacterSound.Sound>)model.view.charAppearance.soundInfo.GetType().GetField("_motionSounds", AccessTools.all).GetValue(model.view.charAppearance.soundInfo);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].motion == changeDetail)
				{
					CharacterSound.Sound value;
					value.motion = changeDetail;
					value.winSound = audioClip;
					value.loseSound = list[i].loseSound;
					list[i] = value;
					((Dictionary<MotionDetail, CharacterSound.Sound>)model.view.charAppearance.soundInfo.GetType().GetField("_dic", AccessTools.all).GetValue(model.view.charAppearance.soundInfo))[changeDetail] = value;
					break;
				}
			}
			return audioClip;
		}

		public static void AddEffect(string name, string path)
		{
			byte[] data = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			EffectSprites.Add(name, Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new UnityEngine.Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect));
		}

		public static void textureInit(DirectoryInfo dir)
		{
			if (dir.GetDirectories().Length != 0)
			{
				DirectoryInfo[] directories = dir.GetDirectories();
				for (int i = 0; i < directories.Length; i++)
				{
					textureInit(directories[i]);
				}
			}
			foreach (FileInfo fileInfo in dir.GetFiles())
			{
				if (fileInfo.Extension.Contains("png"))
				{
					byte[] data = File.ReadAllBytes(fileInfo.FullName);
					Texture2D texture2D = new Texture2D(2, 2);
					texture2D.LoadImage(data);
					textures.Add(fileInfo.Name, texture2D);
				}
			}
		}
		public static bool UISpriteDataManager_GetStoryIcon(ref UIIconManager.IconSet __result, string story)
		{
			bool flag = HMIIcons != null;
			if (flag)
			{
				bool flag2 = HMIIcons.ContainsKey(story);
				if (flag2)
				{
					__result = HMIIcons[story];
					return false;
				}
			}
			return true;
		}

		public static void StageController_EndBattlePhase() { }

		public static Sprite DuplicateSprite(Sprite sprite, Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new UnityEngine.Vector2(0.5f, 0.5f), sprite.pixelsPerUnit, 0U, SpriteMeshType.FullRect);
		}

		public static void Retexture_keter(GameObject obj, string name = "HMI3", string name2 = "HMI3")
		{
			GameObject gameObject = obj.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
			Texture2D texture = textures[name2 + "_background.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			for (int i = 1; i < 6; i++)
			{
				gameObject = obj.transform.GetChild(1).GetChild(0).GetChild(i).gameObject;
				texture = textures[name + "_whitetower" + i.ToString() + ".png"];
				gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			}
			gameObject = obj.transform.GetChild(2).GetChild(0).gameObject;
			texture = textures[name + "_road.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(4).GetChild(0).gameObject;
			texture = textures[name + "_road.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(4).GetChild(1).gameObject;
			texture = textures[name + "_road.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			texture = textures[name + "_bookgrave.png"];
			for (int j = 0; j < 4; j++)
			{
				gameObject = obj.transform.GetChild(3).GetChild(j).gameObject;
				gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			}
			texture = textures[name + "_bookgrave2.png"];
			for (int k = 4; k < 8; k++)
			{
				gameObject = obj.transform.GetChild(3).GetChild(k).gameObject;
				gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			}
			texture = textures[name + "_bookgrave.png"];
			for (int l = 0; l < 11; l++)
			{
				gameObject = obj.transform.GetChild(4).GetChild(2).GetChild(l).gameObject;
				gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
				gameObject = obj.transform.GetChild(4).GetChild(3).GetChild(l).gameObject;
				gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			}
			gameObject = obj.transform.GetChild(2).GetChild(1).gameObject;
			texture = textures[name + "_border.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
		}

		public static void Retexture_tomerry(GameObject obj)
		{
			GameObject gameObject = obj.transform.GetChild(0).GetChild(0).gameObject;
			Texture2D texture = textures["HMI3_background.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(2).GetChild(0).gameObject;
			texture = textures["HMI3_road.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(2).GetChild(1).gameObject;
			texture = textures["HMI3_road.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(2).GetChild(2).gameObject;
			texture = textures["HMI3_road.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(3).GetChild(0).gameObject;
			texture = textures["HMI3_obj1.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(3).GetChild(1).gameObject;
			texture = textures["HMI3_obj2.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(3).GetChild(2).gameObject;
			texture = textures["HMI3_obj3.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
			gameObject = obj.transform.GetChild(2).GetChild(3).gameObject;
			texture = textures["HMI3_obj0.png"];
			gameObject.GetComponent<SpriteRenderer>().sprite = DuplicateSprite(gameObject.GetComponent<SpriteRenderer>().sprite, texture);
		}

		public static void outputTexture(GameObject obj, string prepath)
		{
			SpriteRenderer[] components = obj.GetComponents<SpriteRenderer>();
			bool flag = components != null;
			if (flag)
			{
				foreach (SpriteRenderer spriteRenderer in components)
				{
					Sprite sprite = spriteRenderer.sprite;
					Texture2D tex = Add_On.duplicateTexture(sprite.texture);
					byte[] bytes = tex.EncodeToPNG();
					File.WriteAllBytes(string.Concat(new string[]
					{
						Application.dataPath,
						"/BaseMods/",
						prepath,
						"_",
						sprite.name,
						".png"
					}), bytes);
				}
			}
			int childCount = obj.transform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				Transform child = obj.transform.GetChild(j);
				outputTexture(child.gameObject, prepath + j.ToString() + "_");
			}
		}

		public static void SlotCopying(UIStoryProgressPanel __instance, UIStoryProgressIconSlot slot, UIStoryProgressIconSlot newslot)
		{
			Assembly assembly = Assembly.LoadFile(Application.dataPath + "/Managed/Assembly-CSharp.dll");
			newslot.currentStory = UIStoryLine.Rats;
			GameObject gameObject = newslot.transform.GetChild(1).gameObject;
			newslot.GetType().GetField("closeRect", AccessTools.all).SetValue(newslot, gameObject);
			Animator component = newslot.transform.GetChild(1).gameObject.GetComponent<Animator>();
			newslot.GetType().GetField("anim_closeRect", AccessTools.all).SetValue(newslot, component);
			CanvasGroup component2 = newslot.transform.GetChild(1).gameObject.GetComponent<CanvasGroup>();
			newslot.GetType().GetField("cg_closeRect", AccessTools.all).SetValue(newslot, component2);
			Image component3 = newslot.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
			newslot.GetType().GetField("img_highlighted", AccessTools.all).SetValue(newslot, component3);
			object obj = Activator.CreateInstance(assembly.GetType("UI.StoryIconSet"));
			GameObject gameObject2 = gameObject.transform.GetChild(1).gameObject;
			obj.GetType().GetField("root").SetValue(obj, gameObject2);
			Image component4 = gameObject2.transform.GetChild(0).gameObject.GetComponent<Image>();
			obj.GetType().GetField("img_iconbg").SetValue(obj, component4);
			Image component5 = gameObject2.transform.GetChild(1).gameObject.GetComponent<Image>();
			obj.GetType().GetField("img_iconFrame").SetValue(obj, component5);
			Image component6 = gameObject2.transform.GetChild(2).gameObject.GetComponent<Image>();
			obj.GetType().GetField("img_iconContent").SetValue(obj, component6);
			newslot.GetType().GetField("closeIconset", AccessTools.all).SetValue(newslot, obj);
			GameObject gameObject3 = newslot.transform.GetChild(2).gameObject;
			newslot.GetType().GetField("openRect", AccessTools.all).SetValue(newslot, gameObject3);
			GameObject gameObject4 = gameObject3.transform.GetChild(0).GetChild(0).gameObject;
			newslot.GetType().GetField("openFrameTarget", AccessTools.all).SetValue(newslot, gameObject4);
			object obj2 = Activator.CreateInstance(assembly.GetType("UI.StoryIconSet"));
			gameObject2 = gameObject3.transform.GetChild(1).gameObject;
			obj2.GetType().GetField("root").SetValue(obj2, gameObject2);
			component4 = gameObject2.transform.GetChild(0).gameObject.GetComponent<Image>();
			obj2.GetType().GetField("img_iconbg").SetValue(obj2, component4);
			obj2.GetType().GetField("img_iconFrame").SetValue(obj2, null);
			component6 = gameObject2.transform.GetChild(1).gameObject.GetComponent<Image>();
			obj2.GetType().GetField("img_iconContent").SetValue(obj2, component6);
			newslot.GetType().GetField("openIconset", AccessTools.all).SetValue(newslot, obj2);
			Array array = Array.CreateInstance(assembly.GetType("UI.storyIconLevel"), 3);
			object obj3 = Activator.CreateInstance(assembly.GetType("UI.storyIconLevel"));
			gameObject2 = gameObject3.transform.GetChild(3).GetChild(0).gameObject;
			obj3.GetType().GetField("root").SetValue(obj3, gameObject2);
			component4 = gameObject2.transform.GetChild(0).gameObject.GetComponent<Image>();
			obj3.GetType().GetField("img_iconbg").SetValue(obj3, component4);
			component5 = gameObject2.transform.GetChild(1).gameObject.GetComponent<Image>();
			obj3.GetType().GetField("img_iconFrame").SetValue(obj3, component5);
			TextMeshProUGUI component7 = gameObject2.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
			obj3.GetType().GetField("txt_iconContent").SetValue(obj3, component7);
			UICustomSelectable component8 = gameObject2.transform.GetChild(3).gameObject.GetComponent<UICustomSelectable>();
			obj3.GetType().GetField("selectable").SetValue(obj3, component8);
			object obj4 = Activator.CreateInstance(assembly.GetType("UI.storyIconLevel"));
			gameObject2 = gameObject3.transform.GetChild(3).GetChild(1).gameObject;
			obj4.GetType().GetField("root").SetValue(obj4, gameObject2);
			component4 = gameObject2.transform.GetChild(0).gameObject.GetComponent<Image>();
			obj4.GetType().GetField("img_iconbg").SetValue(obj4, component4);
			component5 = gameObject2.transform.GetChild(1).gameObject.GetComponent<Image>();
			obj4.GetType().GetField("img_iconFrame").SetValue(obj4, component5);
			component7 = gameObject2.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
			obj4.GetType().GetField("txt_iconContent").SetValue(obj4, component7);
			component8 = gameObject2.transform.GetChild(3).gameObject.GetComponent<UICustomSelectable>();
			obj4.GetType().GetField("selectable").SetValue(obj3, component8);
			object obj5 = Activator.CreateInstance(assembly.GetType("UI.storyIconLevel"));
			gameObject2 = gameObject3.transform.GetChild(3).GetChild(2).gameObject;
			obj5.GetType().GetField("root").SetValue(obj5, gameObject2);
			component4 = gameObject2.transform.GetChild(0).gameObject.GetComponent<Image>();
			obj5.GetType().GetField("img_iconbg").SetValue(obj5, component4);
			component5 = gameObject2.transform.GetChild(1).gameObject.GetComponent<Image>();
			obj5.GetType().GetField("img_iconFrame").SetValue(obj5, component5);
			component7 = gameObject2.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
			obj5.GetType().GetField("txt_iconContent").SetValue(obj5, component7);
			component8 = gameObject2.transform.GetChild(3).gameObject.GetComponent<UICustomSelectable>();
			obj5.GetType().GetField("selectable").SetValue(obj3, component8);
			array.SetValue(obj3, 0);
			array.SetValue(obj4, 1);
			array.SetValue(obj5, 2);
			newslot.GetType().GetField("iconset_Level", AccessTools.all).SetValue(newslot, array);
			List<GameObject> list = new List<GameObject>();
			GameObject gameObject5 = ((List<GameObject>)slot.GetType().GetField("connectLineList", AccessTools.all).GetValue(slot))[0];
			GameObject item = UnityEngine.Object.Instantiate(gameObject5, gameObject5.transform.parent);
			list.Add(item);
			newslot.GetType().GetField("connectLineList", AccessTools.all).SetValue(newslot, list);
			newslot.GetType().GetField("isChapterIcon", AccessTools.all).SetValue(newslot, false);
			newslot.GetType().GetField("currentGrade", AccessTools.all).SetValue(newslot, Grade.grade1);
			GameObject gameObject6 = null;
			for (int i = 0; i < gameObject3.transform.childCount; i++)
			{
				bool flag = gameObject3.transform.GetChild(i).gameObject.name.Contains("[text]chaptername");
				if (flag)
				{
					gameObject6 = gameObject3.transform.GetChild(i).gameObject;
					break;
				}
			}
			TextMeshProUGUI value = null;
			bool flag2 = gameObject6 != null;
			if (flag2)
			{
				value = gameObject6.GetComponent<TextMeshProUGUI>();
			}
			newslot.GetType().GetField("txt_openChapterName", AccessTools.all).SetValue(newslot, value);
			TextMeshProUGUI component9 = newslot.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
			newslot.GetType().GetField("txt_chaptergrade", AccessTools.all).SetValue(newslot, component9);
		}

		public static void CreateFragOffice(UIStoryProgressPanel __instance)
		{
			try
			{
				List<UIStoryProgressIconSlot> list = (List<UIStoryProgressIconSlot>)__instance.GetType().GetField("iconList", AccessTools.all).GetValue(__instance);
				UIStoryProgressIconSlot uistoryProgressIconSlot = null;
				foreach (UIStoryProgressIconSlot uistoryProgressIconSlot2 in list)
				{
					if (uistoryProgressIconSlot2.currentStory == UIStoryLine.Sweepers)
					{
						uistoryProgressIconSlot = uistoryProgressIconSlot2;
						break;
					}
				}
				UIStoryProgressIconSlot uistoryProgressIconSlot3 = UnityEngine.Object.Instantiate(uistoryProgressIconSlot, uistoryProgressIconSlot.transform.parent);
				SlotCopying(__instance, uistoryProgressIconSlot, uistoryProgressIconSlot3);
				uistoryProgressIconSlot3.Initialized(__instance);
				uistoryProgressIconSlot3.transform.localPosition += new UnityEngine.Vector3(-400f, 0f);
				List<GameObject> list2 = (List<GameObject>)uistoryProgressIconSlot3.GetType().GetField("connectLineList", AccessTools.all).GetValue(uistoryProgressIconSlot3);
				list2[0].transform.localPosition += new UnityEngine.Vector3(-400f, 0f);
				StageClassInfo data = Singleton<StageClassInfoList>.Instance.GetData(3500001);
				StageClassInfo data2 = Singleton<StageClassInfoList>.Instance.GetData(3500002);
				List<StageClassInfo> list3 = new List<StageClassInfo>();
				list3.Add(data);
				list3.Add(data2);
				uistoryProgressIconSlot3.SetSlotData(list3);
				Storyslots[list3] = uistoryProgressIconSlot3;
				UIIconManager.IconSet iconSet = new UIIconManager.IconSet();
				iconSet.icon = BaseMod.Harmony_Patch.ArtWorks["T"];
				iconSet.iconGlow = BaseMod.Harmony_Patch.ArtWorks["T"];
				HMIIcons["T"] = iconSet;
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIUSPPOUP0error.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		public static void UIStoryProgressPanel_SetStoryLine(UIStoryProgressPanel __instance)
		{
			((ScrollRect)__instance.GetType().GetField("scroll_viewPort", AccessTools.all).GetValue(__instance)).movementType = ScrollRect.MovementType.Unrestricted;
			try
			{
				if (!Init)
				{
					Storyslots = new Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot>();
					HMIIcons = new Dictionary<string, UIIconManager.IconSet>();
					CreateFragOffice(__instance);
					Init = true;
				}
				foreach (List<StageClassInfo> list in Storyslots.Keys)
				{
					Storyslots[list].SetSlotData(list);
					if (list[0].currentState != StoryState.Close)
					{
						Storyslots[list].SetActiveStory(true);
					}
					else
					{
						Storyslots[list].SetActiveStory(false);
					}
				}
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/HMIUSPPOUPerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
		public static AudioClip HMI3_11BGM;

		public static AudioClip HMI3_21BGM;

		internal static List<string> lis = new List<string>
		{
			"damage11and7pw",
			"entropy9atkeach",
			"fragnolimit",
			"lastpower6pl",
			"entropy5atk",
			"fraglevelup"
		};

		public static bool Init;

		public static bool Inited;

		internal static List<DiceCardAbilityBase> bases1 = new List<DiceCardAbilityBase>();

		internal static List<DiceCardSelfAbilityBase> bases2 = new List<DiceCardSelfAbilityBase>();

		internal static List<int> gotLightCardIDs = new List<int>
		{
			3500019,
			3500020,
			3500021,
			3500022,
			3500023,
			3500099
		};

		public static Dictionary<string, Sprite> EffectSprites;

		public static Dictionary<string, Texture2D> textures;

		public static DirectoryInfo path;

		public static Dictionary<string, AudioClip> battleBGM;

		public static Dictionary<List<StageClassInfo>, UIStoryProgressIconSlot> Storyslots;

		public static UIStoryProgressIconSlot slot;

		public static Dictionary<string, UIIconManager.IconSet> HMIIcons;

		static Dictionary<string, string> strMap;
	}
	public class AutoDestroyer : MonoBehaviour
	{
		public void SetDestroy(float time)
		{
			this.time = time;
			IsActive = true;
		}

		public void Update()
		{
			bool isActive = IsActive;
			if (isActive)
			{
			}
		}

		public float time;

		public bool IsActive;
	}
	public class CreateUtil
	{
		public static AudioClip CreateAudio(byte[] bytes)
		{
			WAV wav = new WAV(bytes);
			AudioClip audioClip = AudioClip.Create("sound", wav.SampleCount, 1, wav.Frequency, false);
			audioClip.SetData(wav.LeftChannel, 0);
			return audioClip;
		}

		public static AudioClip CreateAudio(string path)
		{
			byte[] bytes = File.ReadAllBytes(path);
			return CreateAudio(bytes);
		}

		public static BattleUnitModel CreateUnit(int id, int index, Faction faction = Faction.Enemy)
		{
			BattleUnitModel result = Singleton<StageController>.Instance.AddNewUnit(faction, id, index, -1);
			if (result != null) result.SetDeadSceneBlock(false);
			int num = 0;
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetList())
			{
				SingletonBehavior<UICharacterRenderer>.Instance.SetCharacter(battleUnitModel.UnitData.unitData, num++, false);
			}
			BattleObjectManager.instance.InitUI();
			return result;
		}

		public static Text CreateText(Transform target, UnityEngine.Vector2 position, int fsize, UnityEngine.Vector2 anchormin, UnityEngine.Vector2 anchormax, UnityEngine.Vector2 anchorposition, TextAnchor anchor, Color tcolor, Font font)
		{
			GameObject gameObject = new GameObject("Text");
			Text text = gameObject.AddComponent<Text>();
			gameObject.transform.SetParent(target);
			text.rectTransform.sizeDelta = UnityEngine.Vector2.zero;
			text.rectTransform.anchorMin = anchormin;
			text.rectTransform.anchorMax = anchormax;
			text.rectTransform.anchoredPosition = anchorposition;
			text.text = " ";
			text.font = font;
			text.fontSize = fsize;
			text.color = tcolor;
			text.alignment = anchor;
			gameObject.transform.localScale = new UnityEngine.Vector3(1f, 1f);
			gameObject.transform.localPosition = position;
			gameObject.SetActive(true);
			return text;
		}

		public static Font DefFont;
	}
	public class DiceJudger
	{
		public static bool IsAtkDice(BattleDiceBehavior behavior)
		{
			return behavior.Type == BehaviourType.Atk || (behavior.Type == BehaviourType.Standby && (behavior.Detail == BehaviourDetail.Hit || behavior.Detail == BehaviourDetail.Penetrate || behavior.Detail == BehaviourDetail.Slash));
		}

		public static bool IsDefDice(BattleDiceBehavior behavior)
		{
			return behavior.Type == BehaviourType.Def || (behavior.Type == BehaviourType.Standby && (behavior.Detail == BehaviourDetail.Evasion || behavior.Detail == BehaviourDetail.Guard));
		}
	}
	public static class PrimeJudger
	{
		public static bool MillerRabin(int s)
		{
			return MillerRabin(new BigInteger(s));
		}

		public static bool MillerRabin(BigInteger source)
		{
			int num = 2;
			if (source == 2L || source == 3L || source == 5L || source == 7L || source == 11L || source == 13L || source == 17L || source == 19L)
			{
				return true;
			}
			if (source < 2L || source % 2 == 0L)
			{
				return false;
			}
			BigInteger bigInteger = source - 1;
			int num2 = 0;
			while (bigInteger % 2 == 0L)
			{
				bigInteger /= 2;
				num2++;
			}
			RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
			byte[] array = new byte[(long)source.ToByteArray().Length];
			for (int i = 0; i < num; i++)
			{
				BigInteger bigInteger2;
				do
				{
					randomNumberGenerator.GetBytes(array);
					bigInteger2 = new BigInteger(array);
				}
				while (bigInteger2 < 2L || bigInteger2 >= source - 2);
				BigInteger bigInteger3 = BigInteger.ModPow(bigInteger2, bigInteger, source);
				if (!(bigInteger3 == 1L) && !(bigInteger3 == source - 1))
				{
					for (int j = 1; j < num2; j++)
					{
						bigInteger3 = BigInteger.ModPow(bigInteger3, 2, source);
						if (bigInteger3 == 1L)
						{
							return false;
						}
						if (bigInteger3 == source - 1)
						{
							break;
						}
					}
					if (bigInteger3 != source - 1)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
	public class WAV
	{
		private static float bytesToFloat(byte firstByte, byte secondByte)
		{
			short num = (short)((int)secondByte << 8 | (int)firstByte);
			return (float)num / 32768f;
		}

		private static int bytesToInt(byte[] bytes, int offset = 0)
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				num |= (int)bytes[offset + i] << i * 8;
			}
			return num;
		}

		private static byte[] GetBytes(string filename)
		{
			return File.ReadAllBytes(filename);
		}

		public float[] LeftChannel { get; internal set; }

		public float[] RightChannel { get; internal set; }

		public int ChannelCount { get; internal set; }

		public int SampleCount { get; internal set; }

		public int Frequency { get; internal set; }

		public WAV(string filename) : this(GetBytes(filename))
		{
		}

		public WAV(byte[] wav)
		{
			ChannelCount = (int)wav[22];
			Frequency = bytesToInt(wav, 24);
			int i = 12;
			while (wav[i] != 100 || wav[i + 1] != 97 || wav[i + 2] != 116 || wav[i + 3] != 97)
			{
				i += 4;
				int num = (int)wav[i] + (int)wav[i + 1] * 256 + (int)wav[i + 2] * 65536 + (int)wav[i + 3] * 16777216;
				i += 4 + num;
			}
			i += 8;
			SampleCount = (wav.Length - i) / 2;
			bool flag = ChannelCount == 2;
			if (flag)
			{
				SampleCount /= 2;
			}
			LeftChannel = new float[SampleCount];
			bool flag2 = ChannelCount == 2;
			if (flag2)
			{
				RightChannel = new float[SampleCount];
			}
			else
			{
				RightChannel = null;
			}
			int num2 = 0;
			while (i < wav.Length)
			{
				LeftChannel[num2] = bytesToFloat(wav[i], wav[i + 1]);
				i += 2;
				bool flag3 = ChannelCount == 2;
				if (flag3)
				{
					RightChannel[num2] = bytesToFloat(wav[i], wav[i + 1]);
					i += 2;
				}
				num2++;
			}
		}

		public override string ToString()
		{
			return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", new object[]
			{
				LeftChannel,
				RightChannel,
				ChannelCount,
				SampleCount,
				Frequency
			});
		}
	}
	public class ValidCardJudger
	{
		public static bool IsValid(int id)
		{
			return id >= 3500001 && id <= 3500011;
		}
	}
	public class BattleUnitBuf_allDiceMaxUp : BattleUnitBuf
	{
		public BattleUnitBuf_allDiceMaxUp(int x)
		{
			stack = x;
		}

		public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
		{
			card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				max = stack
			});
		}

		public override void OnRoundEnd()
		{
			Destroy();
			base.OnRoundEnd();
		}
	}
	public class BattleUnitBuf_destr0y1dice2 : BattleUnitBuf
	{
		public override int SpeedDiceBreakedAdder()
		{
			return 1;
		}

		public BattleUnitBuf_destr0y1dice2()
		{
			stack = 2;
		}

		public override void OnRoundEnd()
		{
			stack--;
			if (stack == 0) Destroy();
			base.OnRoundEnd();
		}
	}
	public class BattleUnitBuf_entropy : BattleUnitBuf
	{
		public BattleUnitBuf_entropy(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMIentropy"]);
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
				return "HMIentropy";
			}
		}

		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_entropy battleUnitBuf_entropy = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_entropy) as BattleUnitBuf_entropy;
			int result;
			if (battleUnitBuf_entropy == null)
			{
				result = 0;
			}
			else
			{
				result = battleUnitBuf_entropy.stack;
			}
			return result;
		}

		public static void AddBuf(BattleUnitModel model, int add)
		{
			if (add > 0 && (model.passiveDetail.HasPassive<PassiveAbility_3500101>() || model.passiveDetail.HasPassive<PassiveAbility_3500105>())) return;
			BattleUnitBuf_entropy battleUnitBuf_entropy = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_entropy) as BattleUnitBuf_entropy;
			if (battleUnitBuf_entropy == null)
			{
				battleUnitBuf_entropy = new BattleUnitBuf_entropy(model);
				battleUnitBuf_entropy.stack = add;
				model.bufListDetail.AddBuf(battleUnitBuf_entropy);
				return;
			}
			if (add <= 0 && battleUnitBuf_entropy.stack <= -add)
			{
				battleUnitBuf_entropy.Destroy();
				return;
			}
			battleUnitBuf_entropy.Add(add);
		}

		public void Add(int add)
		{
			stack += add;
		}

		public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
		{
			_owner.SetHp((int)_owner.hp - dmg * stack * RandomUtil.Range(3, 8) / 100);
			base.BeforeTakeDamage(attacker, dmg);
		}
	}
	public class BattleUnitBuf_GotLightLabel : BattleUnitBuf
	{
		public override void OnRoundEnd()
		{
			Destroy();
		}
	}
	public class BattleUnitBuf_HMIsign : BattleUnitBuf
	{
		public BattleUnitBuf_HMIsign(BattleUnitModel model)
		{
			_owner = model;
			stack = 0;
			try
			{
				typeof(BattleUnitBuf).GetField("_bufIcon", AccessTools.all).SetValue(this, BaseMod.Harmony_Patch.ArtWorks["HMISign"]);
				typeof(BattleUnitBuf).GetField("_iconInit", AccessTools.all).SetValue(this, true);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/Mantraerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_HMIsign battleUnitBuf_HMIsign = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIsign) as BattleUnitBuf_HMIsign;
			int result;
			if (battleUnitBuf_HMIsign == null)
			{
				result = 0;
			}
			else
			{
				result = battleUnitBuf_HMIsign.stack;
			}
			return result;
		}

		public static void AddBuf(BattleUnitModel model, int add)
		{
			BattleUnitBuf_HMIsign battleUnitBuf_HMIsign = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIsign) as BattleUnitBuf_HMIsign;
			if (battleUnitBuf_HMIsign == null)
			{
				battleUnitBuf_HMIsign = new BattleUnitBuf_HMIsign(model);
				battleUnitBuf_HMIsign.stack = add;
				model.bufListDetail.AddBuf(battleUnitBuf_HMIsign);
				return;
			}
			if (add <= 0 && battleUnitBuf_HMIsign.stack <= -add)
			{
				battleUnitBuf_HMIsign.Destroy();
				return;
			}
			battleUnitBuf_HMIsign.Add(add);
		}

		public void Add(int add)
		{
			stack += add;
		}

		protected override string keywordId
		{
			get
			{
				return "HMISign";
			}
		}
	}
	public class BattleUnitBuf_HMITheBlood : BattleUnitBuf
	{
		public BattleUnitBuf_HMITheBlood(BattleUnitModel model)
		{
			_owner = model;
		}

		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_HMITheBlood battleUnitBuf_HMITheBlood = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMITheBlood) as BattleUnitBuf_HMITheBlood;
			int result;
			if (battleUnitBuf_HMITheBlood == null)
			{
				result = 0;
			}
			else
			{
				result = battleUnitBuf_HMITheBlood.stack;
			}
			return result;
		}

		public static void AddBuf(BattleUnitModel model, int add)
		{
			BattleUnitBuf_HMITheBlood battleUnitBuf_HMITheBlood = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMITheBlood) as BattleUnitBuf_HMITheBlood;
			if (battleUnitBuf_HMITheBlood == null)
			{
				battleUnitBuf_HMITheBlood = new BattleUnitBuf_HMITheBlood(model);
				battleUnitBuf_HMITheBlood.stack = add;
				model.bufListDetail.AddBuf(battleUnitBuf_HMITheBlood);
				return;
			}
			if (add <= 0 && battleUnitBuf_HMITheBlood.stack <= -add)
			{
				battleUnitBuf_HMITheBlood.Destroy();
				return;
			}
			battleUnitBuf_HMITheBlood.Add(add);
		}

		public void Add(int add)
		{
			stack += add;
		}
	}
	public class BattleUnitBuf_passivedmgincreaser : BattleUnitBuf
	{
		public BattleUnitBuf_passivedmgincreaser()
		{
			stack = 1;
		}

		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (behavior == null)
			{
				return;
			}
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				power = (DiceJudger.IsDefDice(behavior) ? -1 : 1)
			});
		}

		public override void BeforeGiveDamage(BattleDiceBehavior behavior)
		{
			if (stack > 0)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					dmgRate = 25
				});
			}
		}

		public override void OnRollDice(BattleDiceBehavior behavior)
		{
			if (behavior == null)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					power = 1
				});
			}
			base.OnRollDice(behavior);
		}

		public override int GetDamageIncreaseRate()
		{
			if (_owner == null && stack > 0)
			{
				return 25;
			}
			return base.GetDamageIncreaseRate();
		}
	}
	public class BattleUnitBuf_passivelock : BattleUnitBuf
	{
		public BattleUnitBuf_passivelock(BattleUnitModel model)
		{
			_owner = model;
		}

		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_passivelock battleUnitBuf_passivelock = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_passivelock) as BattleUnitBuf_passivelock;
			int result;
			if (battleUnitBuf_passivelock == null) result = 0;
			else result = battleUnitBuf_passivelock.stack;
			return result;
		}

		public static void AddBuf(BattleUnitModel model, int add)
		{
			BattleUnitBuf_passivelock battleUnitBuf_passivelock = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_passivelock) as BattleUnitBuf_passivelock;
			if (battleUnitBuf_passivelock == null)
			{
				battleUnitBuf_passivelock = new BattleUnitBuf_passivelock(model);
				battleUnitBuf_passivelock.stack = add;
				model.bufListDetail.AddBuf(battleUnitBuf_passivelock);
				int index = RandomUtil.Range(0, model.passiveDetail.PassiveList.Count - 1);
				passiveModel = model.passiveDetail.PassiveList[index];
				return;
			}
			if (add <= 0 && battleUnitBuf_passivelock.stack <= -add)
			{
				battleUnitBuf_passivelock.Destroy();
				return;
			}
			if (info == "deserted buff") battleUnitBuf_passivelock.Add(add);
		}

		public void Add(int add)
		{
			stack += add;
		}

		public static PassiveAbilityBase passiveModel = new PassiveAbilityBase();

		private const string info = "deserted buff";
	}
	public class BattleUnitBuf_passivereviver : BattleUnitBuf
	{
		public BattleUnitBuf_passivereviver()
		{
			stack = 1;
		}

		public override void OnHpZero()
		{
			if (_owner.GetResistHP(BehaviourDetail.Hit) == AtkResist.Weak)
			{
				if (Singleton<DropBookInventoryModel>.Instance.GetBookCount(3500001) == 0)
				{
					Singleton<DropBookInventoryModel>.Instance.AddBook(3500001, 7);
				}
				return;
			}
			_owner.SetHp(_owner.MaxHp);
			switch (_owner.GetResistHP(BehaviourDetail.Hit))
			{
				case AtkResist.Vulnerable:
					_owner.Book.SetResistHP(BehaviourDetail.Slash, AtkResist.Weak);
					_owner.Book.SetResistHP(BehaviourDetail.Penetrate, AtkResist.Weak);
					_owner.Book.SetResistHP(BehaviourDetail.Hit, AtkResist.Weak);
					return;
				case AtkResist.Normal:
					_owner.Book.SetResistHP(BehaviourDetail.Slash, AtkResist.Vulnerable);
					_owner.Book.SetResistHP(BehaviourDetail.Penetrate, AtkResist.Vulnerable);
					_owner.Book.SetResistHP(BehaviourDetail.Hit, AtkResist.Vulnerable);
					return;
				case AtkResist.Endure:
					_owner.Book.SetResistHP(BehaviourDetail.Slash, AtkResist.Normal);
					_owner.Book.SetResistHP(BehaviourDetail.Penetrate, AtkResist.Normal);
					_owner.Book.SetResistHP(BehaviourDetail.Hit, AtkResist.Normal);
					return;
				default:
					return;
			}
		}
	}
	public class BattleUnitBuf_passivestate1 : BattleUnitBuf
	{
		public BattleUnitBuf_passivestate1()
		{
			stack = 1;
		}

		public override void OnRoundEnd()
		{
			stack--;
			if (stack == 0)
			{
				Destroy();
			}
			base.OnRoundEnd();
		}

		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (behavior == null)
			{
				return;
			}
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				power = (DiceJudger.IsDefDice(behavior) ? -1 : 1)
			});
		}

		public override void BeforeGiveDamage(BattleDiceBehavior behavior)
		{
			if (stack > 0)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					dmgRate = 30
				});
			}
		}

		public override int GetDamageIncreaseRate()
		{
			if (stack > 0)
			{
				return 30;
			}
			return base.GetDamageIncreaseRate();
		}
	}
	public class BattleUnitBuf_passivestate2 : BattleUnitBuf
	{
		public BattleUnitBuf_passivestate2()
		{
			stack = 1;
		}

		public override void OnRoundEnd()
		{
			_owner.TakeDamage(5, DamageType.Attack, null, KeywordBuf.None);
			stack--;
			if (stack == 0)
			{
				Destroy();
			}
			base.OnRoundEnd();
		}

		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (behavior != null)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					power = (DiceJudger.IsAtkDice(behavior) ? 2 : 0)
				});
			}
			base.BeforeRollDice(behavior);
		}
	}
	public class BattleUnitBuf_passivestate3 : BattleUnitBuf
	{
		public BattleUnitBuf_passivestate3()
		{
			stack = 1;
		}

		public override void OnRoundEnd()
		{
			stack--;
			if (stack == 0)
			{
				Destroy();
			}
			base.OnRoundEnd();
		}
	}
	public class BattleUnitBuf_passivestate4 : BattleUnitBuf
	{
		public BattleUnitBuf_passivestate4()
		{
			stack = 1;
		}

		public override void OnRoundEnd()
		{
			stack--;
			if (stack == 0)
			{
				Destroy();
			}
			base.OnRoundEnd();
		}

		public void SwapRandomPageByBug(BattleUnitModel ownermodel, BattleUnitModel targetmodel)
		{
			List<BattleDiceCardModel> allDeck = ownermodel.allyCardDetail.GetAllDeck();
			List<BattleDiceCardModel> allDeck2 = targetmodel.allyCardDetail.GetAllDeck();
			if (allDeck2 != null && allDeck2.Count > 0)
			{
				int count = allDeck2.Count;
				if (allDeck != null && allDeck.Count > 0)
				{
					int count2 = allDeck.Count;
					int num = RandomUtil.Range(0, count2 - 1);
					int num2 = RandomUtil.Range(0, count - 1);
					BattleDiceCardModel battleDiceCardModel = allDeck[num];
					BattleDiceCardModel battleDiceCardModel2 = allDeck2[num2];
					int id = battleDiceCardModel.GetID();
					int id2 = battleDiceCardModel2.GetID();
					try
					{
						ownermodel.allyCardDetail.ExhaustACardAnywhere(battleDiceCardModel);
						targetmodel.allyCardDetail.ExhaustACardAnywhere(battleDiceCardModel2);
					}
					catch (Exception)
					{
					}
					ownermodel.allyCardDetail.AddNewCard(id2, false).exhaust = true;
					targetmodel.allyCardDetail.AddNewCard(id, false).exhaust = true;
					if (count2 > 9999999)
					{
						List<int> list = new List<int>();
						List<int> list2 = new List<int>();
						for (int i = 0; i < count2; i++)
						{
							if (i != num)
							{
								list.Add(allDeck[i].GetID());
							}
						}
						for (int j = 0; j < count; j++)
						{
							if (j != num2)
							{
								list2.Add(allDeck2[j].GetID());
							}
						}
						ownermodel.allyCardDetail.ExhaustAllCards();
						targetmodel.allyCardDetail.ExhaustAllCards();
						Console.WriteLine("Old Code");
					}
				}
			}
		}

		public override void OnSuccessAttack(BattleDiceBehavior behavior)
		{
			SwapRandomPageByBug(_owner, behavior.card.target);
			base.OnSuccessAttack(behavior);
		}
	}
	public class BattleUnitBuf_passivestate5 : BattleUnitBuf
	{
		public BattleUnitBuf_passivestate5()
		{
			stack = 1;
		}

		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				dmgRate = -100
			});
			base.BeforeRollDice(behavior);
		}

		public override void OnRoundEnd()
		{
			_owner.RecoverHP(16);
			stack--;
			if (stack == 0)
			{
				Destroy();
			}
			base.OnRoundEnd();
		}
	}
	public class DiceCardAbility_bleeding1foreverarea : DiceCardAbilityBase
	{
		public override void OnSucceedAreaAttack(BattleUnitModel target)
		{
			BattleUnitBuf_bleedingforever.AddBuf(target, 1);
			base.OnSucceedAreaAttack(target);
		}

		public class BattleUnitBuf_bleedingforever : BattleUnitBuf
		{
			public BattleUnitBuf_bleedingforever(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_bleedingforever battleUnitBuf_bleedingforever = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_bleedingforever) as BattleUnitBuf_bleedingforever;
				int result;
				if (battleUnitBuf_bleedingforever == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_bleedingforever.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_bleedingforever battleUnitBuf_bleedingforever = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_bleedingforever) as BattleUnitBuf_bleedingforever;
				if (battleUnitBuf_bleedingforever == null)
				{
					battleUnitBuf_bleedingforever = new BattleUnitBuf_bleedingforever(model);
					battleUnitBuf_bleedingforever.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_bleedingforever);
					return;
				}
				if (add <= 0 && battleUnitBuf_bleedingforever.stack <= -add)
				{
					battleUnitBuf_bleedingforever.Destroy();
					return;
				}
				battleUnitBuf_bleedingforever.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_bleeding().bufType, stack, null);
				base.OnRoundStart();
			}
		}
	}
	public class DiceCardAbility_damage11and7pw : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			target.TakeDamage(11, DamageType.Attack, null, KeywordBuf.None);
			target.TakeBreakDamage(7, DamageType.Attack, null, AtkResist.Normal, KeywordBuf.None);
			base.OnSucceedAttack(target);
		}
	}
	public class DiceCardAbility_doorDice : DiceCardAbilityBase
	{
		public override void BeforRollDice()
		{
			int num = BattleUnitBuf_doorOpening.GetStack(owner) % 3;
			if (num == 1)
			{
				behavior.behaviourInCard = behavior.behaviourInCard.Copy();
				behavior.behaviourInCard.Detail = BehaviourDetail.Slash;
			}
			else if (num == 2)
			{
				behavior.behaviourInCard = behavior.behaviourInCard.Copy();
				behavior.behaviourInCard.Detail = BehaviourDetail.Hit;
			}
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				min = num * num * (num + 1),
				max = num * num * (num + 1)
			});
		}

		public override void OnSucceedAttack(BattleUnitModel target)
		{
			if (BattleUnitBuf_doorOpening.GetStack(owner) % 3 == 2) { owner.cardSlotDetail.RecoverPlayPoint(6); BattleUnitBuf_entropy.AddBuf(target, 3); }
			BattleUnitBuf_doorOpening.AddBuf(owner, 1);
		}

		public class BattleUnitBuf_doorOpening : BattleUnitBuf
		{
			public BattleUnitBuf_doorOpening(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_doorOpening battleUnitBuf_doorOpening = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_doorOpening) as BattleUnitBuf_doorOpening;
				int result;
				if (battleUnitBuf_doorOpening == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_doorOpening.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_doorOpening battleUnitBuf_doorOpening = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_doorOpening) as BattleUnitBuf_doorOpening;
				if (battleUnitBuf_doorOpening == null)
				{
					battleUnitBuf_doorOpening = new BattleUnitBuf_doorOpening(model);
					battleUnitBuf_doorOpening.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_doorOpening);
					return;
				}
				if (add <= 0 && battleUnitBuf_doorOpening.stack <= -add)
				{
					battleUnitBuf_doorOpening.Destroy();
					return;
				}
				battleUnitBuf_doorOpening.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}
		}
	}
	public class DiceCardAbility_EI : DiceCardAbilityBase
	{
		public override void BeforeRollDice_Target(BattleDiceBehavior targetDice)
		{
			if (targetDice == null || card.speedDiceResultValue > targetDice.card.speedDiceResultValue)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					power = 2
				});
			}
			if (BattleUnitBuf_entropy.GetStack(card.target) > 0)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					power = BattleUnitBuf_entropy.GetStack(card.target)
				});
				BattleUnitBuf_entropy.AddBuf(card.target, -BattleUnitBuf_entropy.GetStack(card.target));
			}
		}
	}
	public class DiceCardAbility_EIarea : DiceCardAbilityBase
	{
		public override void BeforRollDice()
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				power = 2
			});
			int num = 0;
			foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
			{
				num += BattleUnitBuf_entropy.GetStack(model);
			}
			num = (int)(num / 1.7);
			if (num > 0)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					power = num
				});
				foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList_opponent(owner.faction)) BattleUnitBuf_entropy.AddBuf(model, -BattleUnitBuf_entropy.GetStack(model));
			}
		}
	}
	public class DiceCardAbility_entropy1atk : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			BattleUnitBuf_entropy.AddBuf(target, 1);
		}
	}
	public class DiceCardAbility_entropy1atkeach : DiceCardAbilityBase
	{
		public override void OnSucceedAreaAttack(BattleUnitModel target)
		{
			BattleUnitBuf_entropy.AddBuf(target, 1);
		}
	}
	public class DiceCardAbility_entropy1def : DiceCardAbilityBase
	{
		public override void OnWinParryingDefense()
		{
			BattleUnitBuf_entropy.AddBuf(card.target, 1);
		}
	}
	public class DiceCardAbility_entropy3atkeach : DiceCardAbilityBase
	{
		public override void OnSucceedAreaAttack(BattleUnitModel target)
		{
			BattleUnitBuf_entropy.AddBuf(target, 3);
		}
	}
	public class DiceCardAbility_entropy5atk : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			BattleUnitBuf_entropy.AddBuf(target, 5);
		}
	}
	public class DiceCardAbility_entropy9atkeach : DiceCardAbilityBase
	{
		public override void OnSucceedAreaAttack(BattleUnitModel target)
		{
			BattleUnitBuf_entropy.AddBuf(target, 9);
		}
	}
	public class DiceCardAbility_fraglevelup : DiceCardAbilityBase
	{
		public override void BeforRollDice()
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				dmgRate = -100,
				breakRate = -100,
				min = BattleUnitBuf_HMIsign.GetStack(owner) * 100,
				max = BattleUnitBuf_HMIsign.GetStack(owner) * 100
			});
			base.BeforRollDice();
		}

		public override void OnWinParrying()
		{
			if (BattleUnitBuf_HMIsign.GetStack(owner) < 12)
			{
				BattleUnitBuf_HMIsign.AddBuf(owner, 1);
			}
			card.target.TakeDamage(12, DamageType.Attack, null, KeywordBuf.None);
		}
	}
	public class DiceCardAbility_HMIPoison1002 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			int num = RandomUtil.Range(1, 100);
			if (num <= 10)
			{
				BattleUnitBuf_HMIPoison1001.AddBuf(target, 1);
				return;
			}
			if (num <= 20)
			{
				BattleUnitBuf_HMIPoison1002.AddBuf(target, 1);
				return;
			}
			if (num <= 60)
			{
				BattleUnitBuf_HMIPoison1003.AddBuf(target, 1);
				return;
			}
			BattleUnitBuf_HMIPoison1004.AddBuf(target, 1);
		}

		public class BattleUnitBuf_HMIPoison1001 : BattleUnitBuf
		{
			public BattleUnitBuf_HMIPoison1001(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_HMIPoison1001 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison1001) as BattleUnitBuf_HMIPoison1001;
				int result;
				if (battleUnitBuf_HMIPoison == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_HMIPoison.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMIPoison1001 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison1001) as BattleUnitBuf_HMIPoison1001;
				if (battleUnitBuf_HMIPoison == null)
				{
					battleUnitBuf_HMIPoison = new BattleUnitBuf_HMIPoison1001(model)
					{
						stack = add
					};
					model.bufListDetail.AddBuf(battleUnitBuf_HMIPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMIPoison.stack <= -add)
				{
					battleUnitBuf_HMIPoison.Destroy();
					return;
				}
				battleUnitBuf_HMIPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_bleeding().bufType, 2, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_weak().bufType, 1, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_disarm().bufType, 1, null);
			}
		}

		public class BattleUnitBuf_HMIPoison1002 : BattleUnitBuf
		{
			public BattleUnitBuf_HMIPoison1002(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_HMIPoison1002 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison1002) as BattleUnitBuf_HMIPoison1002;
				int result;
				if (battleUnitBuf_HMIPoison == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_HMIPoison.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMIPoison1002 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison1002) as BattleUnitBuf_HMIPoison1002;
				if (battleUnitBuf_HMIPoison == null)
				{
					battleUnitBuf_HMIPoison = new BattleUnitBuf_HMIPoison1002(model);
					battleUnitBuf_HMIPoison.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_HMIPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMIPoison.stack <= -add)
				{
					battleUnitBuf_HMIPoison.Destroy();
					return;
				}
				battleUnitBuf_HMIPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_binding().bufType, 3, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_weak().bufType, 1, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_disarm().bufType, 1, null);
			}

			public override void OnSuccessAttack(BattleDiceBehavior behavior)
			{
				_owner.breakDetail.LoseBreakGauge(RandomUtil.Range(1, 2));
			}
		}

		public class BattleUnitBuf_HMIPoison1003 : BattleUnitBuf
		{
			public BattleUnitBuf_HMIPoison1003(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_HMIPoison1003 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison1003) as BattleUnitBuf_HMIPoison1003;
				int result;
				if (battleUnitBuf_HMIPoison == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_HMIPoison.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMIPoison1003 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison1003) as BattleUnitBuf_HMIPoison1003;
				if (battleUnitBuf_HMIPoison == null)
				{
					battleUnitBuf_HMIPoison = new BattleUnitBuf_HMIPoison1003(model);
					battleUnitBuf_HMIPoison.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_HMIPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMIPoison.stack <= -add)
				{
					battleUnitBuf_HMIPoison.Destroy();
					return;
				}
				battleUnitBuf_HMIPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_bleeding().bufType, 1, null);
			}

			public override void BeforeRollDice(BattleDiceBehavior behavior)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					min = -3
				});
				base.BeforeRollDice(behavior);
			}
		}

		public class BattleUnitBuf_HMIPoison1004 : BattleUnitBuf
		{
			public BattleUnitBuf_HMIPoison1004(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_HMIPoison1004 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison1004) as BattleUnitBuf_HMIPoison1004;
				int result;
				if (battleUnitBuf_HMIPoison == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_HMIPoison.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMIPoison1004 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison1004) as BattleUnitBuf_HMIPoison1004;
				if (battleUnitBuf_HMIPoison == null)
				{
					battleUnitBuf_HMIPoison = new BattleUnitBuf_HMIPoison1004(model);
					battleUnitBuf_HMIPoison.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_HMIPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMIPoison.stack <= -add)
				{
					battleUnitBuf_HMIPoison.Destroy();
					return;
				}
				battleUnitBuf_HMIPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_binding().bufType, 1, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_bleeding().bufType, 1, null);
			}
		}
	}
	public class DiceCardAbility_HMIPoison2002 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			int num = RandomUtil.Range(1, 100);
			if (num <= 10)
			{
				BattleUnitBuf_HMIPoison2001.AddBuf(target, 1);
				return;
			}
			if (num <= 20)
			{
				BattleUnitBuf_HMIPoison2002.AddBuf(target, 1);
				return;
			}
			if (num <= 60)
			{
				BattleUnitBuf_HMIPoison2003.AddBuf(target, 1);
				return;
			}
			BattleUnitBuf_HMIPoison2004.AddBuf(target, 1);
		}

		public class BattleUnitBuf_HMIPoison2001 : BattleUnitBuf
		{
			public BattleUnitBuf_HMIPoison2001(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_HMIPoison2001 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison2001) as BattleUnitBuf_HMIPoison2001;
				int result;
				if (battleUnitBuf_HMIPoison == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_HMIPoison.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMIPoison2001 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison2001) as BattleUnitBuf_HMIPoison2001;
				if (battleUnitBuf_HMIPoison == null)
				{
					battleUnitBuf_HMIPoison = new BattleUnitBuf_HMIPoison2001(model);
					battleUnitBuf_HMIPoison.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_HMIPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMIPoison.stack <= -add)
				{
					battleUnitBuf_HMIPoison.Destroy();
					return;
				}
				battleUnitBuf_HMIPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_bleeding().bufType, 7, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_weak().bufType, 3, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_disarm().bufType, 3, null);
			}
		}

		public class BattleUnitBuf_HMIPoison2002 : BattleUnitBuf
		{
			public BattleUnitBuf_HMIPoison2002(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_HMIPoison2002 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison2002) as BattleUnitBuf_HMIPoison2002;
				int result;
				if (battleUnitBuf_HMIPoison == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_HMIPoison.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMIPoison2002 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison2002) as BattleUnitBuf_HMIPoison2002;
				if (battleUnitBuf_HMIPoison == null)
				{
					battleUnitBuf_HMIPoison = new BattleUnitBuf_HMIPoison2002(model);
					battleUnitBuf_HMIPoison.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_HMIPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMIPoison.stack <= -add)
				{
					battleUnitBuf_HMIPoison.Destroy();
					return;
				}
				battleUnitBuf_HMIPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_binding().bufType, 3, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_paralysis().bufType, 3, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_weak().bufType, 1, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_disarm().bufType, 1, null);
			}

			public override void OnSuccessAttack(BattleDiceBehavior behavior)
			{
				_owner.breakDetail.LoseBreakGauge(RandomUtil.Range(3, 7));
			}
		}

		public class BattleUnitBuf_HMIPoison2003 : BattleUnitBuf
		{
			public BattleUnitBuf_HMIPoison2003(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_HMIPoison2003 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison2003) as BattleUnitBuf_HMIPoison2003;
				int result;
				if (battleUnitBuf_HMIPoison == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_HMIPoison.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMIPoison2003 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison2003) as BattleUnitBuf_HMIPoison2003;
				if (battleUnitBuf_HMIPoison == null)
				{
					battleUnitBuf_HMIPoison = new BattleUnitBuf_HMIPoison2003(model);
					battleUnitBuf_HMIPoison.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_HMIPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMIPoison.stack <= -add)
				{
					battleUnitBuf_HMIPoison.Destroy();
					return;
				}
				battleUnitBuf_HMIPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_bleeding().bufType, 2, null);
			}

			public override void BeforeRollDice(BattleDiceBehavior behavior)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					min = -32767
				});
				base.BeforeRollDice(behavior);
			}
		}

		public class BattleUnitBuf_HMIPoison2004 : BattleUnitBuf
		{
			public BattleUnitBuf_HMIPoison2004(BattleUnitModel model)
			{
				_owner = model;
			}

			public static int GetStack(BattleUnitModel model)
			{
				BattleUnitBuf_HMIPoison2004 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison2004) as BattleUnitBuf_HMIPoison2004;
				int result;
				if (battleUnitBuf_HMIPoison == null)
				{
					result = 0;
				}
				else
				{
					result = battleUnitBuf_HMIPoison.stack;
				}
				return result;
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMIPoison2004 battleUnitBuf_HMIPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMIPoison2004) as BattleUnitBuf_HMIPoison2004;
				if (battleUnitBuf_HMIPoison == null)
				{
					battleUnitBuf_HMIPoison = new BattleUnitBuf_HMIPoison2004(model);
					battleUnitBuf_HMIPoison.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_HMIPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMIPoison.stack <= -add)
				{
					battleUnitBuf_HMIPoison.Destroy();
					return;
				}
				battleUnitBuf_HMIPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_binding().bufType, 32767, null);
				_owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_bleeding().bufType, 2, null);
			}
		}
	}
	public class DiceCardAbility_HMIsign12 : DiceCardAbilityBase
	{
		public override void BeforRollDice()
		{
			BattleUnitBuf_HMIsign.AddBuf(card.target, 12);
		}
	}
	public class DiceCardAbility_HMIsimulatorPhillip : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			owner.emotionDetail.CreateEmotionCoin(EmotionCoinType.Negative, 3);
			SingletonBehavior<BattleManagerUI>.Instance.ui_battleEmotionCoinUI.OnAcquireCoin(owner, EmotionCoinType.Negative, 3);
		}
	}
	public class DiceCardAbility_HMIsimulatorShyao : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			target.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_burn().bufType, 5, card.owner);
		}
	}
	public class DiceCardAbility_HMIsimulatorYan : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			if (!owner.allyCardDetail.IsHighlander())
			{
				owner.allyCardDetail.DrawCards(3);
			}
		}
	}
	public class DiceCardAbility_lastpower6pl : DiceCardAbilityBase
	{
		public override void OnLoseParrying()
		{
			card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus
			{
				power = 6
			});
			base.OnLoseParrying();
		}
	}
	public class DiceCardAbility_shutdown1atk : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			while (true) ;
		}
	}
	public class DiceCardAbility_state4001 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			target.bufListDetail.AddBuf(new BattleUnitBuf_destr0y1dice2());
		}
	}
	public class DiceCardAbility_TheLight2 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			target.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_strength().bufType, target.PlayPoint >> 1, null);
			target.bufListDetail.AddBuf(new BattleUnitBuf_MeltByLight(target.PlayPoint));
			base.OnSucceedAttack(target);
		}

		public override void BeforRollDice()
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				min = card.target.PlayPoint << 1,
				max = (card.target.PlayPoint << 1) + card.target.PlayPoint
			});
			base.BeforRollDice();
		}

		public class BattleUnitBuf_MeltByLight : BattleUnitBuf
		{
			public BattleUnitBuf_MeltByLight(int s)
			{
				stack = s;
			}

			public override void OnRoundStart()
			{
				_owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_bleeding().bufType, stack, null);
				base.OnRoundStart();
			}
		}
	}
	public class DiceCardAbility_ZZYDOG : DiceCardAbilityBase
	{
		public override void OnRollDice()
		{
			if (behavior.DiceVanillaValue >= 2 && behavior.DiceVanillaValue >= behavior.GetDiceVanillaMax())
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					power = 15,
					breakDmg = 20
				});
				SystemUtil.LockWorkStation();
				return;
			}
			owner.TakeBreakDamage(20, DamageType.Attack, null, AtkResist.Normal, KeywordBuf.None);
			owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_weak().bufType, 4, null);
		}
	}
	public class DiceCardAbility_ZZYSuper : DiceCardAbilityBase
	{
		public override void OnRollDice()
		{
			if (cnt < 8)
			{
				cnt++;
				BattleDiceBehavior behavior = this.behavior;
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					power = -cnt + 1
				});
				card.AddDice(behavior);
			}
			else
			{
				foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList((owner.faction == Faction.Player) ? Faction.Enemy : Faction.Player))
				{
					battleUnitModel.bufListDetail.AddKeywordBufByEtc(new BattleUnitBuf_bleeding().bufType, 3, owner);
					battleUnitModel.TakeDamage(6, DamageType.Attack, null, KeywordBuf.None);
					battleUnitModel.TakeBreakDamage(3, DamageType.Attack, null, AtkResist.Normal, KeywordBuf.None);
				}
			}
			owner.RecoverHP(card.target.bufListDetail.GetActivatedBuf(KeywordBuf.Bleeding).stack << 1);
		}

		private int cnt;
	}
	public class DiceCardSelfAbility_allMaxUp4ThisRound : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			owner.bufListDetail.AddBuf(new BattleUnitBuf_allDiceMaxUp(4));
		}
	}
	public class DiceCardSelfAbility_fragnolimit : DiceCardSelfAbilityBase
	{
	}
	public class DiceCardSelfAbility_HMIintro1 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			int num = 0;
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(owner.faction))
			{
				if (battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250001>() && battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250020>() && battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250036>())
				{
					num |= 1;
				}
				if (battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250051>() && battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250151>())
				{
					num |= 2;
				}
				if (battleUnitModel.passiveDetail.HasPassive<PassiveAbility_240027>() && battleUnitModel.passiveDetail.HasPassive<PassiveAbility_240127>() && battleUnitModel.passiveDetail.HasPassive<PassiveAbility_240227>())
				{
					num |= 4;
				}
			}
			if ((num & 1) == 0)
			{
				card.GetDiceBehaviorList()[0].ApplyDiceStatBonus(new DiceStatBonus
				{
					power = -6
				});
			}
			if ((num & 2) == 0)
			{
				card.GetDiceBehaviorList()[1].ApplyDiceStatBonus(new DiceStatBonus
				{
					power = -6
				});
			}
			if ((num & 4) == 0)
			{
				card.GetDiceBehaviorList()[2].ApplyDiceStatBonus(new DiceStatBonus
				{
					power = -6
				});
			}
			if (num == 7)
			{
				foreach (BattleUnitModel battleUnitModel2 in BattleObjectManager.instance.GetAliveList(owner.faction))
				{
					battleUnitModel2.Die(null, true);
				}
			}
		}
	}
	public class DiceCardSelfAbility_HMIintro2 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			int cnt = 0;
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(owner.faction))
			{
				cnt += battleUnitModel.allyCardDetail.GetAllDeck().FindAll((BattleDiceCardModel x) => x.GetID() == 3500000).Count;
			}
			if (PrimeJudger.MillerRabin(cnt))
			{
				using (List<BattleUnitModel>.Enumerator enumerator2 = BattleObjectManager.instance.GetAliveList(owner.faction).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.Die(null, true);
					}
				}
			}
		}
	}
	public class DiceCardSelfAbility_HMIPoison1001 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			owner.allyCardDetail.ExhaustCard(card.card.GetID());
		}
	}
	public class DiceCardSelfAbility_HMIPoison2001 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			owner.allyCardDetail.ExhaustCard(card.card.GetID());
			owner.TakeDamage(31, DamageType.Attack, null, KeywordBuf.None);
			owner.TakeBreakDamage(31, DamageType.Attack, null, AtkResist.Normal, KeywordBuf.None);
			owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_weak().bufType, 10, null);
			owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_binding().bufType, 10, null);
			owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_disarm().bufType, 10, null);
			owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_bleeding().bufType, 4, null);
			BattleUnitBuf_HMISelfPoison.AddBuf(owner, RandomUtil.Range(2, 3));
		}

		public class BattleUnitBuf_HMISelfPoison : BattleUnitBuf
		{
			public override void OnRoundStart()
			{
				if (_owner.PlayPoint > _owner.MaxPlayPoint - stack)
				{
					_owner.cardSlotDetail.LosePlayPoint(_owner.PlayPoint - _owner.MaxPlayPoint + stack);
				}
				base.OnRoundStart();
			}

			public static void AddBuf(BattleUnitModel model, int add)
			{
				BattleUnitBuf_HMISelfPoison battleUnitBuf_HMISelfPoison = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_HMISelfPoison) as BattleUnitBuf_HMISelfPoison;
				if (battleUnitBuf_HMISelfPoison == null)
				{
					battleUnitBuf_HMISelfPoison = new BattleUnitBuf_HMISelfPoison(model);
					battleUnitBuf_HMISelfPoison.stack = add;
					model.bufListDetail.AddBuf(battleUnitBuf_HMISelfPoison);
					return;
				}
				if (add <= 0 && battleUnitBuf_HMISelfPoison.stack <= -add)
				{
					battleUnitBuf_HMISelfPoison.Destroy();
					return;
				}
				battleUnitBuf_HMISelfPoison.Add(add);
			}

			public void Add(int add)
			{
				stack += add;
			}

			public BattleUnitBuf_HMISelfPoison(BattleUnitModel model)
			{
				_owner = model;
			}
		}
	}
	public class DiceCardSelfAbility_negatePowerenemy : DiceCardSelfAbilityBase
	{
		public override void OnStartParrying()
		{
			BattleUnitModel target = card.target;
			if (target == null || target.currentDiceAction == null)
			{
				return;
			}
			target.currentDiceAction.ignorePower = true;
		}
	}
	public class DiceCardSelfAbility_TheLevelUpLight : DiceCardSelfAbilityBase
	{
		public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
		{
			targetUnit.bufListDetail.AddKeywordBufThisRoundByCard(new BattleUnitBuf_nullifyPower().bufType, 1, null);
			try
			{
				/*foreach (PassiveAbilityBase passiveAbilityBase in targetUnit.passiveDetail.PassiveList)
				{
					byte[] bytes = Convert.FromBase64String(passiveAbilityBase.desc);
					passiveAbilityBase.desc = Encoding.UTF8.GetString(bytes);
				}*/
				foreach (PassiveAbilityBase passiveAbilityBase in targetUnit.passiveDetail.PassiveList)
				{
					string[] array = passiveAbilityBase.GetType().ToString().Split(new char[] { '_' });
					if (array != null && array.Length != 0)
					{
						string s = array[array.Length - 1];
						int id = -1;
						if (int.TryParse(s, out id)) passiveAbilityBase.desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(id);
					}
				}
			}
			catch (Exception)
			{
			}
			targetUnit.bufListDetail.AddBuf(new BattleUnitBuf_GotLightLabel());
			BattleObjectManager.instance.InitUI();
		}
	}
	public class DiceCardSelfAbility_TheLight1 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			owner.bufListDetail.AddBuf(new BattleUnitBuf_GotLight());
			owner.allyCardDetail.ExhaustCard(card.card.GetID());
			BattleUnitBuf_entropy.AddBuf(card.target, 3);
		}

		public class BattleUnitBuf_GotLight : BattleUnitBuf
		{
			public override void OnRoundStart()
			{
				base.OnRoundStart();
				_owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_strength().bufType, 1, null);
				_owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_protection().bufType, 2, null);
				_owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_breakProtection().bufType, 2, null);
				_owner.bufListDetail.AddKeywordBufByCard(new BattleUnitBuf_quickness().bufType, 2, null);
				_owner.cardSlotDetail.RecoverPlayPoint(1);
			}
		}
	}
	public class DiceCardSelfAbility_ZZYClear : DiceCardSelfAbilityBase
	{
		public override void OnStartBattle()
		{
			owner.TakeDamage(10, DamageType.Attack, null, KeywordBuf.None);
			foreach (BattleUnitBuf battleUnitBuf in owner.bufListDetail.GetActivatedBufList())
			{
				if (battleUnitBuf.positiveType == BufPositiveType.Negative)
				{
					battleUnitBuf.Destroy();
				}
			}
			owner.bufListDetail.AddBuf(new PowerUp2thisRoundBuf());
		}

		protected class PowerUp2thisRoundBuf : BattleUnitBuf
		{
			public override void BeforeRollDice(BattleDiceBehavior behavior)
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					power = 2
				});
			}

			public override void OnRoundEnd()
			{
				Destroy();
			}
		}
	}
	public class DiceCardSelfAbility_ZZYSEP1 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			owner.bufListDetail.AddKeywordBufThisRoundByCard(new BattleUnitBuf_strength().bufType, 3, null);
			card.target.bufListDetail.AddKeywordBufThisRoundByCard(new BattleUnitBuf_weak().bufType, 3, null);
			if (!card.card.exhaust)
			{
				card.card.exhaust = true;
				owner.allyCardDetail.AddNewCardToDeck(card.card.GetID(), false).exhaust = true;
			}
			base.OnUseCard();
		}
	}
	public class PassiveAbility_3500001 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			foreach (BattleUnitBuf battleUnitBuf in owner.bufListDetail.GetActivatedBufList())
			{
				if (battleUnitBuf is BattleUnitBuf_passivestate1 || battleUnitBuf is BattleUnitBuf_passivestate2 || battleUnitBuf is BattleUnitBuf_passivestate3 || battleUnitBuf is BattleUnitBuf_passivestate4 || battleUnitBuf is BattleUnitBuf_passivestate5)
				{
					battleUnitBuf.Destroy();
				}
			}
			switch (RandomUtil.Range(0, 4))
			{
				case 1:
					owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate1());
					owner.allyCardDetail.AddNewCard(3500005, false).temporary = true;
					return;
				case 2:
					owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate2());
					owner.allyCardDetail.AddNewCard(3500006, false).temporary = true;
					return;
				case 3:
					owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate3());
					owner.allyCardDetail.AddNewCard(3500007, false).temporary = true;
					return;
				case 4:
					owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate4());
					owner.allyCardDetail.AddNewCard(3500008, false).temporary = true;
					return;
				default:
					return;
			}
		}
	}
	public class PassiveAbility_3500002 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			foreach (BattleUnitBuf battleUnitBuf in owner.bufListDetail.GetActivatedBufList())
			{
				if (battleUnitBuf is BattleUnitBuf_passivestate1 || battleUnitBuf is BattleUnitBuf_passivestate2 || battleUnitBuf is BattleUnitBuf_passivestate3 || battleUnitBuf is BattleUnitBuf_passivestate4 || battleUnitBuf is BattleUnitBuf_passivestate5)
				{
					battleUnitBuf.Destroy();
				}
			}
			switch (RandomUtil.Range(0, 5))
			{
				case 1:
					{
						owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate1());
						BattleDiceCardModel battleDiceCardModel = owner.allyCardDetail.AddNewCard(3500005, false);
						battleDiceCardModel.temporary = true;
						battleDiceCardModel.AddCost(-2);
						return;
					}
				case 2:
					{
						owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate2());
						BattleDiceCardModel battleDiceCardModel2 = owner.allyCardDetail.AddNewCard(3500006, false);
						battleDiceCardModel2.temporary = true;
						battleDiceCardModel2.AddCost(-2);
						return;
					}
				case 3:
					{
						owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate3());
						BattleDiceCardModel battleDiceCardModel3 = owner.allyCardDetail.AddNewCard(3500007, false);
						battleDiceCardModel3.temporary = true;
						battleDiceCardModel3.AddCost(-2);
						return;
					}
				case 4:
					{
						owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate4());
						BattleDiceCardModel battleDiceCardModel4 = owner.allyCardDetail.AddNewCard(3500008, false);
						battleDiceCardModel4.temporary = true;
						battleDiceCardModel4.AddCost(-2);
						return;
					}
				case 5:
					{
						owner.bufListDetail.AddBuf(new BattleUnitBuf_passivestate5());
						BattleDiceCardModel battleDiceCardModel5 = owner.allyCardDetail.AddNewCard(3500009, false);
						battleDiceCardModel5.temporary = true;
						battleDiceCardModel5.AddCost(-2);
						return;
					}
				default:
					return;
			}
		}
	}
	public class PassiveAbility_3500003 : PassiveAbilityBase
	{
		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			if (ValidCardJudger.IsValid(curCard.card.GetID()))
			{
				curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
				{
					min = 3,
					max = 6
				});
			}
			base.OnUseCard(curCard);
		}
	}
	public class PassiveAbility_3510003 : PassiveAbilityBase
	{
		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			if (ValidCardJudger.IsValid(curCard.card.GetID()))
			{
				curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
				{
					min = 2,
					max = 4
				});
			}
			base.OnUseCard(curCard);
		}
	}
	public class PassiveAbility_3500004 : PassiveAbilityBase
	{
		public override float GetStartHp(float hp)
		{
			return hp - (float)((int)((double)hp * 0.25));
		}

		public override void OnRoundStart()
		{
			owner.allyCardDetail.DrawCards(2);
			base.OnRoundStart();
		}

		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				power = -1
			});
			base.OnUseCard(curCard);
		}
	}
	public class PassiveAbility_3500005 : PassiveAbilityBase
	{
		public override float GetStartHp(float hp)
		{
			return hp - (float)((int)((double)hp * 0.5));
		}

		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				power = -1
			});
			base.BeforeRollDice(behavior);
		}

		public override void OnRoundStart()
		{
			owner.allyCardDetail.DrawCards(4);
			base.OnRoundStart();
		}
	}
	public class PassiveAbility_3500006 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			if (owner.GetResistHP(BehaviourDetail.Hit) == AtkResist.Weak)
			{
				_pattern ^= 1;
				if (_pattern == 0)
				{
					owner.allyCardDetail.AddNewCard(3500010, false).AddCost(-9);
				}
				if (_usecrystal < 3 && (double)RandomUtil.valueForProb < 0.8 - (double)_usecrystal * 0.2)
				{
					owner.allyCardDetail.AddNewCard(3500011, false);
					_usecrystal++;
				}
			}
			using (List<BattleUnitBuf>.Enumerator enumerator = owner.bufListDetail.GetActivatedBufList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is BattleUnitBuf_passivedmgincreaser)
					{
						return;
					}
				}
			}
			owner.bufListDetail.AddBuf(new BattleUnitBuf_passivedmgincreaser());
		}

		private int _pattern;

		private int _usecrystal;
	}
	public class PassiveAbility_3500007 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			owner.cardSlotDetail.RecoverPlayPoint(3);
			using (List<BattleUnitBuf>.Enumerator enumerator = owner.bufListDetail.GetActivatedBufList().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is BattleUnitBuf_passivereviver)
					{
						return;
					}
				}
			}
			owner.bufListDetail.AddBuf(new BattleUnitBuf_passivereviver());
		}
	}
	public class PassiveAbility_3500011 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			base.OnRoundStart();
			_patternCount++;
			for (int i = 0; i < 3; i++)
			{
				RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(owner.faction)).allyCardDetail.AddTempCard(3500098);
			}
			owner.allyCardDetail.DrawCards(5);
			if (!PassiveList.Contains(this))
			{
				PassiveList.Add(this);
			}
			try
			{
				int emotionTotalCoinNumber = Singleton<StageController>.Instance.GetCurrentStageFloorModel().team.emotionTotalCoinNumber;
				Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionTotalBonus = emotionTotalCoinNumber + 1;
				Singleton<StageController>.Instance.GetStageModel().SetCurrentMapInfo(0);
				BattleObjectManager.instance.GetAliveList(owner.faction);
			}
			catch (Exception ex)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/ProjectMoonCodererror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			}
			owner.allyCardDetail.AddTempCard(3500099);
			if (_patternCount % 4 == 0)
			{
				owner.allyCardDetail.AddTempCard(3500020).AddCost(-5);
				owner.allyCardDetail.AddTempCard(3500021).AddCost(-3);
			}
			try
			{
				if (!owner.bufListDetail.HasBuf<BattleUnitBuf_GotLightLabel>())
				{
					foreach (PassiveAbilityBase passiveAbilityBase in owner.passiveDetail.PassiveList)
					{
						string[] array = passiveAbilityBase.GetType().ToString().Split(new char[] { '_' });
						if (array != null && array.Length != 0)
						{
							string s = array[array.Length - 1];
							int id = -1;
							if (int.TryParse(s, out id))
							{
								string str = Singleton<PassiveDescXmlList>.Instance.GetDesc(id);
								byte[] bytes = Encoding.UTF8.GetBytes(str);
								passiveAbilityBase.desc = Convert.ToBase64String(bytes);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public override void OnWaveStart()
		{
			foreach (BattleDiceCardModel battleDiceCardModel in owner.allyCardDetail.GetHand())
			{
				battleDiceCardModel.ResetToOriginalData();
				battleDiceCardModel.CopySelf();
			}
			AudioClip[] array = new AudioClip[3];
			if (Harmony_Patch.battleBGM["331"] != null && Harmony_Patch.battleBGM["332"] != null && Harmony_Patch.battleBGM["333"] != null)
			{
				for (int i = 0; i < 1; i++)
				{
					array[i] = Harmony_Patch.battleBGM["331"];
				}
				for (int j = 1; j < 2; j++)
				{
					array[j] = Harmony_Patch.battleBGM["332"];
				}
				for (int k = 2; k < array.Length; k++)
				{
					array[k] = Harmony_Patch.battleBGM["333"];
				}
				_oldEnemytheme = SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(array);
				if (_oldEnemytheme[0] != null) _oldEnemytheme[0] = null;
				SingletonBehavior<BattleSoundManager>.Instance.ChangeEnemyTheme(0);
			}
		}

		public static List<PassiveAbility_3500011> PassiveList = new List<PassiveAbility_3500011>();

		private int _patternCount;

		private AudioClip[] _oldEnemytheme;
	}
	public class PassiveAbility_3500012 : PassiveAbilityBase
	{
		public override bool isImmortal
		{
			get
			{
				return true;
			}
		}

		public override int SpeedDiceBreakedAdder()
		{
			return 1;
		}

		public override void OnRoundEndTheLast_ignoreDead()
		{
			if (_isdead)
			{
				return;
			}
			if (owner.breakDetail.breakGauge == owner.breakDetail.GetDefaultBreakGauge())
			{
				_branch = 1;
			}
			else
			{
				if (owner.breakDetail.IsBreakLifeZero())
				{
					int num = 0, cnt = 0;
					foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
					{
						if ((battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250001>() && battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250020>()) || battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250036>())
						{
							num |= 1;
						}
						if (battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250051>() && battleUnitModel.passiveDetail.HasPassive<PassiveAbility_250151>())
						{
							num |= 2;
						}
						if ((battleUnitModel.passiveDetail.HasPassive<PassiveAbility_240027>() && battleUnitModel.passiveDetail.HasPassive<PassiveAbility_240127>()) || battleUnitModel.passiveDetail.HasPassive<PassiveAbility_240227>())
						{
							num |= 4;
						}
						cnt += battleUnitModel.allyCardDetail.GetAllDeck().FindAll((BattleDiceCardModel x) => x.GetID() == 3500000).Count;
					}
					if (File.Exists(Application.dataPath + "/BaseMods/A letter.txt"))
					{
						num = 0;
						if (PrimeJudger.MillerRabin(cnt))
						{
							_branch = 4;
							goto IL_17D;
						}
					}
					if (num == 7)
					{
						_branch = 3;
						goto IL_17D;
					}
				}
				_branch = 2;
			}
		IL_17D:
			owner.Die(null, true);
			_isdead = true;
			BattleObjectManager.instance.InitUI();
			switch (_branch)
			{
				case 1:
					Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 3500005, 0, -1).SetDeadSceneBlock(false);
					if (Singleton<InventoryModel>.Instance.GetCardCount(3500151) == 0)
					{
						Singleton<InventoryModel>.Instance.AddCard(3500151, 1);
					}
					break;
				case 2:
					Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 3500006, 0, -1).SetDeadSceneBlock(false);
					if (Singleton<InventoryModel>.Instance.GetCardCount(3500151) == 0)
					{
						Singleton<InventoryModel>.Instance.AddCard(3500151, 1);
					}
					break;
				case 3:
					Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 3500007, 0, -1).SetDeadSceneBlock(false);
					break;
				case 4:
					Singleton<StageController>.Instance.AddNewUnit(Faction.Enemy, 3500010, 0, -1).SetDeadSceneBlock(false);
					break;
			}
			BattleObjectManager.instance.InitUI();
		}

		private int _branch;

		private bool _isdead;
	}
	public class PassiveAbility_3500013 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			owner.allyCardDetail.ExhaustAllCards();
			if (!is1st)
			{
				is1st = true;
				for (int i = 0; i < 4; i++)
				{
					owner.allyCardDetail.AddTempCard(3500012).AddCost(-2);
				}
				owner.allyCardDetail.AddTempCard(3500013).AddCost(-10);
			}
			else
			{
				for (int j = 0; j < 5; j++)
				{
					owner.allyCardDetail.AddTempCard(3500014).AddCost(-10);
				}
			}
			Console.Out.WriteLine("How do you f**king see this???ProjBananalock don't have a heart!!!");
		}

		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (DiceJudger.IsAtkDice(behavior))
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					min = 1236,
					max = 1236
				});
			}
			base.BeforeRollDice(behavior);
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500013);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500013);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		public override int SpeedDiceNumAdder()
		{
			return 4;
		}

		public override void OnRoundStartAfter()
		{
			AudioClip[] array = new AudioClip[3];
			if (Harmony_Patch.HMI3_11BGM != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = Harmony_Patch.HMI3_11BGM;
				}
				_oldEnemytheme = SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(array);
				SingletonBehavior<BattleSoundManager>.Instance.ChangeEnemyTheme(0);
			}
		}

		private bool is1st;

		private AudioClip[] _oldEnemytheme;
	}
	public class PassiveAbility_3500014 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500014);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500014);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}
	}
	public class PassiveAbility_3500015 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500015);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500015);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}
	}
	public class PassiveAbility_3500016 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			_pattern++;
			owner.allyCardDetail.ExhaustAllCards();
			for (int i = 0; i < 5; i++)
			{
				owner.allyCardDetail.AddTempCard(lis[RandomUtil.Range(0, lis.Count - 1)]).AddCost(-2);
			}
			if (_pattern % 3 == 0)
			{
				owner.allyCardDetail.AddTempCard(3500018).AddCost(-5);
			}
			Console.Out.WriteLine("How do you f**king see this???ProjBananalock don't have a heart!!!");
		}

		public override void OnRoundEndTheLast()
		{
			owner.TakeBreakDamage(10, DamageType.Attack, null, AtkResist.Normal, KeywordBuf.None);
			base.OnRoundEndTheLast();
		}

		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (DiceJudger.IsAtkDice(behavior))
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					min = -2,
					max = -2
				});
			}
			base.BeforeRollDice(behavior);
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500016);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500016);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		public override int SpeedDiceNumAdder()
		{
			return 2 + ((owner.emotionDetail.EmotionLevel >= 3) ? 1 : 0);
		}

		public override void OnRoundStartAfter()
		{
			AudioClip[] array = new AudioClip[3];
			if (Harmony_Patch.HMI3_21BGM != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = Harmony_Patch.HMI3_21BGM;
				}
				_oldEnemytheme = SingletonBehavior<BattleSoundManager>.Instance.SetEnemyTheme(array);
				SingletonBehavior<BattleSoundManager>.Instance.ChangeEnemyTheme(0);
			}
		}

		private List<int> lis = new List<int>
		{
			3500015,
			3500016,
			3500017
		};

		private int _pattern;

		private AudioClip[] _oldEnemytheme;
	}
	public class PassiveAbility_3500017 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500017);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500017);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}
	}
	public class PassiveAbility_3500018 : PassiveAbilityBase
	{
		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500018);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500018);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}
	}
	public class PassiveAbility_3500019 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			_pattern++;
			Console.Out.WriteLine("How do you f**king see this???ProjBananalock don't have a heart!!!");
		}

		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (DiceJudger.IsAtkDice(behavior) && _pattern % 4 != 0 && !owner.bufListDetail.HasBuf<BattleUnitBuf_nullifyPower>())
			{
				behavior.ApplyDiceStatBonus(new DiceStatBonus
				{
					min = -1236,
					max = -1236
				});
			}
			base.BeforeRollDice(behavior);
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500019);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500019);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		public override int SpeedDiceNumAdder()
		{
			return 4;
		}

		private int _pattern;
	}
	public class PassiveAbility_3500020 : PassiveAbilityBase
	{
		public override bool isImmortal
		{
			get
			{
				return BattleUnitBuf_HMIsign.GetStack(owner) < 12;
			}
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500020);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500020);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		public override void OnRoundStart()
		{
			owner.TakeDamage((int)(owner.hp / 15f), DamageType.Attack, null, KeywordBuf.None);
		}

		public override void OnDie()
		{
			if (BattleUnitBuf_HMIsign.GetStack(owner) < 12)
			{
				string text = "";
				for (int i = 0; i < BattleUnitBuf_HMIsign.GetStack(owner); i++)
				{
					text += "?";
				}
				for (int j = 0; j < 2333; j++)
				{
					Debug.Log(text + " FATAL ERROR!!! " + text + Environment.NewLine);
					Console.Out.Write(text + " FATAL ERROR!!! " + text + Environment.NewLine);
				}
				int num = BattleUnitBuf_HMIsign.GetStack(owner) / 0;
				return;
			}
			if (Singleton<DropBookInventoryModel>.Instance.GetBookCount(3500002) == 0 && Singleton<BookInventoryModel>.Instance.GetBookCount(3500008) == 0 && Singleton<InventoryModel>.Instance.GetCardCount(3500200) == 0)
			{
				Singleton<DropBookInventoryModel>.Instance.AddBook(3500002, 1);
				return;
			}
			FileInfo fileInfo = new FileInfo(Application.dataPath + "/BaseMods/A letter.txt");
			while (!fileInfo.Exists)
			{
				File.WriteAllText(Application.dataPath + "/BaseMods/A letter.txt", content);
				fileInfo = new FileInfo(Application.dataPath + "/BaseMods/A letter.txt");
			}
			fileInfo.Attributes = (FileAttributes.ReadOnly | FileAttributes.Hidden);
		}

		private const string content = "Actually,nothing.";
	}
	public class PassiveAbility_3500021 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			_pattern++;
			if (_pattern % 12 == 0 && BattleUnitBuf_HMIsign.GetStack(owner) <= 9)
			{
				owner.RecoverHP(owner.MaxHp - (int)owner.hp);
				BattleUnitBuf_HMIsign.AddBuf(owner, -BattleUnitBuf_HMIsign.GetStack(owner));
			}
			Console.Out.WriteLine("How do you f**king see this???ProjBananalock don't have a heart!!!");
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500021);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500021);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		private int _pattern;
	}
	public class PassiveAbility_3500022 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			_pattern++;
			if (BattleUnitBuf_HMIsign.GetStack(owner) >= 12)
			{
				owner.breakDetail.TakeBreakDamage(owner.breakDetail.GetDefaultBreakGauge(), DamageType.Attack, null, AtkResist.Normal, KeywordBuf.None);
			}
			Console.Out.WriteLine("How do you f**king see this???ProjBananalock don't have a heart!!!");
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500022);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500022);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		public bool CanAddBuf_tmp(BattleUnitBuf buf)
		{
			foreach (string value in lis)
			{
				if (buf.bufKeywordText.Contains(value))
				{
					return false;
				}
			}
			return true;
		}

		public virtual bool IsImmuneDmg(DamageType type)
		{
			return type == DamageType.Buf || base.IsImmuneDmg(type, KeywordBuf.None);
		}

		private int _pattern;

		private List<string> lis = new List<string>
		{
			"OldLady_Lonely",
			"TheDreamingCurrent_Sea",
			"Bloodbath_Hands",
			"HappyTeddyBear_Hug",
			"ChildofGalaxy_Token",
			"Burn",
			"Bleeding",
			"Vulnerable",
			"Queenbee_Spore",
			"Ability/BloodBath_Hand",
			"Queenbee_Punish",
			"RedHood_Hunt",
			"BurningGirl_Ember",
			"HodFinal_CopiousBleeding",
			"Latitia_Heart",
			"Latitia_Gift",
			"Orchestra_Affect",
			"TakeBreakDamage2",
			"Nosferatu_Blight",
			"WhiteNight_Awe",
			"Emotion_BlueStar_MartyrBuf"
		};
	}
	public class PassiveAbility_3500023 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			_pattern++;
			if (owner.allyCardDetail.GetAllDeck().Find((BattleDiceCardModel x) => x.GetID() == 3500200) != null)
			{
				owner.allyCardDetail.ExhaustCard(3500200);
				owner.allyCardDetail.AddNewCardToDeck(3500100, false);
				BattleUnitBuf_HMITheBlood.AddBuf(owner, 1);
			}
			Console.Out.WriteLine("How do you f**king see this???ProjBananalock don't have a heart!!!");
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500023);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500023);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		private int _pattern;
	}
	public class PassiveAbility_3500024 : PassiveAbilityBase
	{
		public override int SpeedDiceNumAdder()
		{
			int num = 0;
			foreach (PassiveAbilityBase passiveAbilityBase in owner.passiveDetail.PassiveList)
			{
				if (!(passiveAbilityBase is PassiveAbility_3500024))
				{
					num += passiveAbilityBase.SpeedDiceNumAdder();
				}
			}
			return ((owner.emotionDetail.EmotionLevel >= 4) ? -1 : 0) - num;
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500024);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500024);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			if (BattleUnitBuf_HMITheBlood.GetStack(owner) > 0)
			{
				if (curCard.GetOriginalDiceBehaviorList().FindAll((DiceBehaviour x) => x.Type != BehaviourType.Standby).Count == 1)
				{
					BattleCardTotalResult battleCardResultLog = owner.battleCardResultLog;
					if (battleCardResultLog != null)
					{
						battleCardResultLog.SetPassiveAbility(this);
					}
					curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
					{
						min = 3,
						max = 6,
						breakRate = 100,
						dmgRate = 150
					});
				}
			}
		}
	}
	public class PassiveAbility_3500025 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			_pattern++;
			_realPattern++;
			Console.Out.WriteLine("How do you f**king see this???ProjBananalock don't have a heart!!!");
		}

		public override void OnRoundEndTheLast()
		{
			if (_pattern == 7)
			{
				owner.Die(null, true);
			}
			if (_realPattern % 7 == 0)
			{
				owner.RecoverHP(owner.MaxHp);
			}
		}

		public override void OnDieOtherUnit(BattleUnitModel unit)
		{
			if (unit == owner)
			{
				return;
			}
			_pattern %= 7;
			_pattern -= 7;
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500025);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500025);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		private int _pattern;

		private int _realPattern;
	}
	public class PassiveAbility_3500026 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			if (RandomUtil.valueForProb < 0.9f)
			{
				owner.bufListDetail.AddKeywordBufThisRoundByEtc(positiveBufs[RandomUtil.Range(0, positiveBufs.Count - 1)], 1, null);
			}
			else
			{
				owner.RecoverHP(3);
			}
			if (RandomUtil.valueForProb < 0.9f)
			{
				owner.bufListDetail.AddKeywordBufThisRoundByEtc(negativeBufs[RandomUtil.Range(0, negativeBufs.Count - 1)], 1, null);
			}
			else
			{
				owner.TakeDamage(5, DamageType.Attack, null, KeywordBuf.None);
			}
			Console.Out.WriteLine("How do you f**king see this???ProjBananalock don't have a heart!!!");
		}

		public override void OnCreated()
		{
			name = Singleton<PassiveDescXmlList>.Instance.GetName(3500026);
			desc = Singleton<PassiveDescXmlList>.Instance.GetDesc(3500026);
			Console.Out.WriteLine("Project Bananalock don't have a heart!!!");
		}

		private List<KeywordBuf> positiveBufs = new List<KeywordBuf>
		{
			KeywordBuf.Protection,
			KeywordBuf.BreakProtection,
			KeywordBuf.Strength,
			KeywordBuf.Endurance,
			KeywordBuf.Quickness
		};

		private List<KeywordBuf> negativeBufs = new List<KeywordBuf>
		{
			KeywordBuf.Burn,
			KeywordBuf.Paralysis,
			KeywordBuf.Bleeding,
			KeywordBuf.Vulnerable,
			KeywordBuf.Vulnerable_break,
			KeywordBuf.Weak,
			KeywordBuf.Disarm,
			KeywordBuf.Binding
		};
	}
	public class PassiveAbility_3501001 : PassiveAbilityBase
	{
		public override void OnDie()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Player))
			{
				battleUnitModel.RecoverHP((battleUnitModel.MaxHp / 10 <= 8) ? (battleUnitModel.MaxHp / 10) : 8);
				battleUnitModel.breakDetail.RecoverBreak((battleUnitModel.breakDetail.GetDefaultBreakGauge() / 10 <= 5) ? (battleUnitModel.breakDetail.GetDefaultBreakGauge() / 10) : 5);
			}
			base.OnDie();
		}
	}
	public class PassiveAbility_3501002 : PassiveAbilityBase
	{
		public override void AfterGiveDamage(int damage)
		{
			owner.RecoverHP(1);
			owner.breakDetail.RecoverBreak(1);
			base.AfterGiveDamage(damage);
		}
	}
	public class PassiveAbility_3501003 : PassiveAbilityBase
	{
		public override void OnRoundStart()
		{
			_pattern++;
			if (_pattern == 1)
			{
				owner.allyCardDetail.AddNewCard(3501001, false);
				owner.bufListDetail.AddKeywordBufThisRoundByEtc(new BattleUnitBuf_protection().bufType, 5, null);
				SystemUtil.keybd_event(32, 0, 0, 0);
				Thread.Sleep(100);
				SystemUtil.keybd_event(32, 0, 2, 0);
			}
			base.OnRoundStart();
		}

		public override void OnRoundStartAfter()
		{
			if (_pattern == 1)
			{
				Thread.Sleep(500);
				SystemUtil.keybd_event(32, 0, 0, 0);
				Thread.Sleep(100);
				SystemUtil.keybd_event(32, 0, 2, 0);
			}
		}

		private int _pattern;
	}
	public class PassiveAbility_3501004 : PassiveAbilityBase
	{
		public override int SpeedDiceNumAdder()
		{
			if (owner.emotionDetail.EmotionLevel >= 3)
			{
				return 2;
			}
			return 0;
		}

		public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
		{
			if (owner.emotionDetail.EmotionLevel >= 3)
			{
				curCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
				{
					power = 2
				});
			}
			base.OnUseCard(curCard);
		}
	}
	public class HMI3MapManager : CustomMapManager
	{
		protected override void LateUpdate()
		{
		}

		public override void CustomInit()
		{
			mapBgm = new AudioClip[3];
			mapBgm[0] = Harmony_Patch.battleBGM["331"];
			mapBgm[1] = Harmony_Patch.battleBGM["332"];
			mapBgm[2] = Harmony_Patch.battleBGM["333"];
			Harmony_Patch.Retexture_keter(gameObject, "HMI3");
			mapSize = MapSize.L;
		}

		public override void InitializeMap()
		{
			_bMapInitialized = true;
			_dlgIdList = new List<string>();
			switch (Singleton<LibraryFloorModel>.Instance.Sephirah)
			{
				case SephirahType.Malkuth:
					_dlgIdList.Add("462 613 126 361 462 305 976 45 146 402 673 114 187 856 150 881 36 242 1101 35 30 627 358 182 190 721 282 79 553 536 1048 79 40 374 167 656 613 325 225 573 526 30 1089 1 96 71 160 928 827 301 48 727 1 451 161 1012 4 61 201 920 265 901 24 532 115 548");
					break;
				case SephirahType.Yesod:
					_dlgIdList.Add("923 145 131 1044 108 3 256 835 96 237 541 402 981 187 30 899 227 36 843 235 94 974 121 16 315 1 840 713 121 336 453 571 147 1167 5 8 1149 11 10 499 115 534 77 397 670 721 51 350 873 241 80 514 514 144 669 77 399 241 1 900 846 241 90 563 593 24 865 118 162 661 17 464 96 893 184 936 21 184 486 241 468 501 409 221 391 71 741 627 25 458 720 65 408 55 745 384 607 313 271 405 535 255 561 333 282 61 798 320");
					break;
				case SephirahType.Hod:
					_dlgIdList.Add("657 121 372 339 715 105 329 745 96 145 313 686 527 646 30 181 1 954 478 335 334 567 89 466 655 361 135 937 25 196 719 271 153 457 509 212 90 163 894 1 22 1132 541 145 486 76 691 420 401 721 80 650 364 99 239 139 780 1111 1 45 85 1036 60 976 1 190 1 891 294 33 62 1094 1012 1 180 1 393 784 274 875 28 477 344 358");
					break;
				case SephirahType.Netzach:
					_dlgIdList.Add("81 865 216 287 658 223 1038 73 54 247 388 474 471 106 588 429 127 591 1101 35 30 772 295 120 693 185 322 597 25 504 927 76 138 919 57 188 745 163 234 85 701 387 77 397 670 721 301 150 856 191 120 708 334 116 239 139 780 385 703 91 622 19 524 813 335 12 1 891 294 33 62 1094 1012 1 180 1 393 784 274 875 28 477 344 358");
					break;
				case SephirahType.Tiphereth:
					_dlgIdList.Add("869 305 16 1053 37 103 1038 73 54 145 313 686 471 106 588 1117 19 64 756 7 406 437 571 120 917 172 72 713 121 336 804 1 384 1063 77 20 6 883 294 782 181 186 761 256 126 532 385 274 827 301 48 727 1 451 161 1012 4 61 201 920 265 901 24 532 115 548");
					break;
				case SephirahType.Gebura:
					_dlgIdList.Add("869 305 16 1053 37 103 1038 73 54 145 313 686 745 67 356 759 106 272 717 1 427 1057 61 39 69 1025 64 211 190 756 998 1 148 374 167 656 581 19 546 391 11 790 1041 1 128 1004 91 30 827 301 48 727 1 451 161 1012 4 61 201 920 265 901 24 532 115 548");
					break;
				case SephirahType.Chesed:
					_dlgIdList.Add("869 305 16 1053 37 103 1038 73 54 145 313 686 145 931 126 973 43 168 1101 35 30 627 358 182 164 913 76 502 425 184 1 1045 144 817 1 308 745 163 234 85 701 387 817 145 216 865 115 190 827 301 48 727 1 451 161 1012 4 61 201 920 265 901 24 532 115 548");
					break;
				case SephirahType.Binah:
					_dlgIdList.Add("821 1 336 474 658 69 779 78 296 85 85 972 1041 115 30 831 22 336 867 273 53 343 799 36 766 201 210 904 215 60");
					break;
				case SephirahType.Hokma:
					_dlgIdList.Add("536 535 76 1 442 742 811 190 162 202 101 880 1041 115 30 831 22 336 867 273 53 343 799 36 766 201 210 904 215 60");
					break;
				case SephirahType.Keter:
					_dlgIdList.Add("215 728 252 77 130 927 56 185 936 1098 3 12 981 187 30 899 227 36 873 41 229 911 226 28 241 1 960 1062 15 72 927 76 138 664 477 6 1149 11 10 499 115 534 217 330 626 406 271 438 378 533 280 799 231 99 161 1012 4 61 201 920 265 901 24 532 115 548 1 891 294 33 62 1094");
					break;
			}
			_dlgIdList.Add("923 145 131 1 491 651 415 649 96 856 145 114 471 106 588 1117 19 64 719 131 294 290 801 50 196 161 840 798 179 196 453 571 147 145 89 924 901 205 54 15 487 617 1 465 696 1091 1 25");
			_dlgIdList.Add("1051 45 56 141 338 663 425 687 47 591 413 124 981 187 30 899 227 36 843 235 94 974 121 16 315 1 840 713 121 336 719 271 153 9 205 968 477 26 670 941 1 210 183 193 800 987 19 174 1124 1 30 1138 7 15 741 121 301 747 38 364 791 121 235 289 137 748 272 211 658 1 137 1014 778 325 72 736 225 224 593 244 312 16 391 754 45 1017 78 63 187 888 46 561 540 721 211 229 699 293 153 283 384 468 711 313 117 955 19 151 409 485 264 845 166 154");
			_dlgIdList.Add("1051 45 56 835 143 213 904 171 68 468 1 690 470 321 410 532 361 288 913 33 194 441 391 336 811 1 345 739 372 90 1 209 936 667 248 230 349 64 756 933 19 224 541 145 486 865 115 190");
			_dlgIdList.Add("989 145 6 801 130 223 607 164 370 160 91 936 982 79 84 55 847 216 603 78 506 1057 61 39 481 127 593 633 265 216 1035 121 12 89 749 308 1050 7 110 436 211 490 153 703 324 1129 31 6 1092 49 48 200 448 526 357 195 627 449 97 648");
			_dlgIdList.Add("41 1025 88 261 78 833 208 25 936 1038 1 90 701 337 126 907 141 98 756 7 406 823 205 88 641 81 440 315 505 336 1035 121 12 540 479 104 1045 11 126 681 196 270");
			CreateDialog();
		}

		public override void ActiveMap(bool b)
		{
			base.ActiveMap(b);
		}

		public override void EnableMap(bool b)
		{
			gameObject.SetActive(b);
		}

		//public override GameObject GetScratch(int lv, Transform parent)
		//{
		//	return null;
		//}

		public override GameObject GetWallCrater()
		{
			return null;
		}

		public override void OnRoundStart()
		{
			mapSize = MapSize.L;
			ChangeMap();
		}

		public override void OnRoundEnd()
		{
			mapSize = MapSize.L;
		}

		public void OnWaveEnd()
		{
		}

		private void CreateDialog()
		{
			_dlgColor = new Color(0.5f, 0f, 0.07f);
			if (_dlgIdList.Count <= 0)
			{
				return;
			}
			string str = RandomUtil.SelectOne(_dlgIdList);
			if (_dlgEffect != null && _dlgEffect.gameObject != null)
			{
				_dlgEffect.FadeOut();
			}
			_dlgEffect = SingletonBehavior<CreatureDlgManagerUI>.Instance.SetDlg(str, _dlgColor, null);
		}

		private void Update()
		{
			if (_dlgEffect != null && _dlgEffect.gameObject != null)
			{
				if (_dlgEffect.DisplayDone)
				{
					for (int i = 0; i < 1; ++i) CreateDialog();
					return;
				}
			}
			else
			{
				for (int i = 0; i < 1; ++i) CreateDialog();
			}
		}

		public void ChangeMap()
		{
			SingletonBehavior<CreatureDlgManagerUI>.Instance.Init(SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject == this);
		}

		private List<string> _dlgIdList;

		private CreatureDlgEffectUI _dlgEffect;

		[SerializeField]
		private Color _dlgColor;
	}
	public class HMI4MapManager : CustomMapManager
	{
		protected override void LateUpdate()
		{
		}

		public override void CustomInit()
		{
			mapBgm = new AudioClip[3];
			mapBgm[0] = Harmony_Patch.battleBGM["401"];
			mapBgm[1] = Harmony_Patch.battleBGM["401"];
			mapBgm[2] = Harmony_Patch.battleBGM["401"];
			Harmony_Patch.Retexture_keter(gameObject, "HMI3", "HMI4");
			mapSize = MapSize.L;
		}

		public override void InitializeMap()
		{
			_bMapInitialized = true;
			_dlgIdList = new List<string>();
			_dlgIdList.Add("诗人有意无限地，长久地搅乱自身的所有感官，获得预见世事的能力……");
			_dlgIdList.Add("裂光芒之肤，横流其血");
			_dlgIdList.Add("我们铭记死者，直至将其遗忘");
			_dlgIdList.Add("一个孤立系统的总混乱度不会减小");
			_dlgIdList.Add("E=mc^2");
			CreateDialog();
		}

		public override void ActiveMap(bool b)
		{
			base.ActiveMap(b);
		}

		public override void EnableMap(bool b)
		{
			gameObject.SetActive(b);
		}

		public override GameObject GetScratch(int lv, Transform parent)
		{
			return null;
		}

		public override GameObject GetWallCrater()
		{
			return null;
		}

		public override void OnRoundStart()
		{
			mapSize = MapSize.L;
			ChangeMap();
		}

		public override void OnRoundEnd()
		{
			mapSize = MapSize.L;
		}

		public void OnWaveEnd()
		{
		}

		private void CreateDialog()
		{
			_dlgColor = new Color(0.7f, 0.7f, 0.6f);
			if (_dlgIdList.Count <= 0)
			{
				return;
			}
			string str = RandomUtil.SelectOne(_dlgIdList);
			if (_dlgEffect != null && _dlgEffect.gameObject != null)
			{
				_dlgEffect.FadeOut();
			}
			_dlgEffect = SingletonBehavior<CreatureDlgManagerUI>.Instance.SetDlg(str, _dlgColor, null);
		}

		private void Update()
		{
			if (_dlgEffect != null && _dlgEffect.gameObject != null)
			{
				if (_dlgEffect.DisplayDone)
				{
					for (int i = 0; i < 1; ++i) CreateDialog();
					return;
				}
			}
			else
			{
				for (int i = 0; i < 1; ++i) CreateDialog();
			}
		}

		public void ChangeMap()
		{
			SingletonBehavior<CreatureDlgManagerUI>.Instance.Init(SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject == this);
		}

		private List<string> _dlgIdList;

		private CreatureDlgEffectUI _dlgEffect;

		[SerializeField]
		private Color _dlgColor;
	}
	public class HMI5MapManager : CustomMapManager
	{
		protected override void LateUpdate()
		{
		}

		public override void CustomInit()
		{
			mapBgm = new AudioClip[3];
			mapBgm[0] = Harmony_Patch.battleBGM["402"];
			mapBgm[1] = Harmony_Patch.battleBGM["402"];
			mapBgm[2] = Harmony_Patch.battleBGM["402"];
			Harmony_Patch.Retexture_keter(gameObject, "HMI3", "HMI5");
			mapSize = MapSize.L;
		}

		public override void InitializeMap()
		{
			_bMapInitialized = true;
			_dlgIdList = new List<string>();
			_dlgIdList.Add("颜色只存在于有光的地方");
			_dlgIdList.Add("IN GI RUM IMUS NOC TE ET CON SUMI MUR IGNI RUM IMUS NOC TE ET CON SUMI MUR");
			_dlgIdList.Add("变化、混沌、秘密、自然");
			_dlgIdList.Add("日出为血");
			_dlgIdList.Add("每个颜色都更加明亮，然而现在它们全都开始流失色泽，褪至纯白");
			_dlgIdList.Add("何等的奇观");
			_dlgIdList.Add("起初是梦境，然后是幻象，而今是一切");
			CreateDialog();
		}

		public override void ActiveMap(bool b)
		{
			base.ActiveMap(b);
		}

		public override void EnableMap(bool b)
		{
			gameObject.SetActive(b);
		}

		public override GameObject GetScratch(int lv, Transform parent)
		{
			return null;
		}

		public override GameObject GetWallCrater()
		{
			return null;
		}

		public override void OnRoundStart()
		{
			mapSize = MapSize.L;
			ChangeMap();
		}

		public override void OnRoundEnd()
		{
			mapSize = MapSize.L;
		}

		public void OnWaveEnd()
		{
		}

		private void CreateDialog()
		{
			_dlgColor = new Color(0.3f, 0.35f, 0.3f);
			if (_dlgIdList.Count <= 0)
			{
				return;
			}
			string str = RandomUtil.SelectOne(_dlgIdList);
			if (_dlgEffect != null && _dlgEffect.gameObject != null)
			{
				_dlgEffect.FadeOut();
			}
			_dlgEffect = SingletonBehavior<CreatureDlgManagerUI>.Instance.SetDlg(str, _dlgColor, null);
		}

		private void Update()
		{
			if (_dlgEffect != null && _dlgEffect.gameObject != null)
			{
				if (_dlgEffect.DisplayDone)
				{
					for (int i = 0; i < 1; ++i) CreateDialog();
					return;
				}
			}
			else
			{
				for (int i = 0; i < 1; ++i) CreateDialog();
			}
		}

		public void ChangeMap()
		{
			SingletonBehavior<CreatureDlgManagerUI>.Instance.Init(SingletonBehavior<BattleSceneRoot>.Instance.currentMapObject == this);
		}

		private List<string> _dlgIdList;

		private CreatureDlgEffectUI _dlgEffect;

		[SerializeField]
		private Color _dlgColor;
	}
}
