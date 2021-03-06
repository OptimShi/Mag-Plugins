using System;
using System.Collections.Generic;

using Mag.Shared.Constants;
using Mag.Shared.Spells;

namespace Mag.Shared
{
	/// <summary>
	/// Instantiate this object with the item you want info for.
	/// ToString() this object for the info.
	/// </summary>
	public class ItemInfo
	{
		private readonly MyWorldObject mwo;

		public ItemInfo(MyWorldObject myWorldObject)
		{
			mwo = myWorldObject;
		}

		public override string ToString()
		{
			return ToString(true, true);
		}

		public string ToString(bool showBuffedValues, bool showValueAndBurden)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			if (mwo.Values(IntValueKey.MaterialType) > 0)
			{
				if (Dictionaries.MaterialInfo.ContainsKey(mwo.Values(IntValueKey.MaterialType)))
					sb.Append(Dictionaries.MaterialInfo[mwo.Values(IntValueKey.MaterialType)] + " ");
				else
					sb.Append("unknown material " + mwo.Values(IntValueKey.MaterialType) + " ");
			}

			sb.Append(mwo.Name);

			if (mwo.Values((IntValueKey)353) > 0)
			{
				if (Dictionaries.MasteryInfo.ContainsKey(mwo.Values((IntValueKey)353)))
					sb.Append(" (" + Dictionaries.MasteryInfo[mwo.Values((IntValueKey)353)] + ")");
				else
					sb.Append(" (Unknown mastery " + mwo.Values((IntValueKey)353) + ")");
			}

			int set = mwo.Values((IntValueKey)265, 0);
			if (set != 0)
			{
				sb.Append(", ");
				if (Dictionaries.AttributeSetInfo.ContainsKey(set))
					sb.Append(Dictionaries.AttributeSetInfo[set]);
				else
					sb.Append("Unknown set " + set);
			}

			if (mwo.Values(IntValueKey.ArmorLevel) > 0)
				sb.Append(", AL " + mwo.Values(IntValueKey.ArmorLevel));

			if (mwo.Values(IntValueKey.ImbuedEffect) > 0)
			{
				sb.Append(",");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 1) == 1) sb.Append(" CS");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 2) == 2) sb.Append(" CB");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 4) == 4) sb.Append(" AR");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 8) == 8) sb.Append(" SlashRend");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 16) == 16) sb.Append(" PierceRend");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 32) == 32) sb.Append(" BludgeRend");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 64) == 64) sb.Append(" AcidRend");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 128) == 128) sb.Append(" FrostRend");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 256) == 256) sb.Append(" LightRend");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 512) == 512) sb.Append(" FireRend");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 1024) == 1024) sb.Append(" MeleeImbue");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 4096) == 4096) sb.Append(" MagicImbue");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 8192) == 8192) sb.Append(" Hematited");
				if ((mwo.Values(IntValueKey.ImbuedEffect) & 536870912) == 536870912) sb.Append(" MagicAbsorb");
			}

			if (mwo.Values(IntValueKey.NumTimesTinkered) > 0)
				sb.Append(", Tinks " + mwo.Values(IntValueKey.NumTimesTinkered));

			if (mwo.Values(IntValueKey.MaxDamage_Decal) != 0 && mwo.Values(DoubleValueKey.Variance_Decal) != 0)
				sb.Append(", " + (mwo.Values(IntValueKey.MaxDamage_Decal) - (mwo.Values(IntValueKey.MaxDamage_Decal) * mwo.Values(DoubleValueKey.Variance_Decal))).ToString("N2") + "-" + mwo.Values(IntValueKey.MaxDamage_Decal));
			else if (mwo.Values(IntValueKey.MaxDamage_Decal) != 0 && mwo.Values(DoubleValueKey.Variance_Decal) == 0)
				sb.Append(", " + mwo.Values(IntValueKey.MaxDamage_Decal));

			if (mwo.Values(IntValueKey.ElementalDamageBonus, 0) != 0)
				sb.Append(", +" + mwo.Values(IntValueKey.ElementalDamageBonus));

			if (mwo.Values(DoubleValueKey.DamageBonus_Decal, 1) != 1)
				sb.Append(", +" + Math.Round(((mwo.Values(DoubleValueKey.DamageBonus_Decal) - 1) * 100)) + "%");

			if (mwo.Values(DoubleValueKey.ElementalDamageMod, 1) != 1)
				sb.Append(", +" + Math.Round(((mwo.Values(DoubleValueKey.ElementalDamageMod) - 1) * 100)) + "%vs. Monsters");

			if (mwo.Values(DoubleValueKey.AttackBonus_Decal, 1) != 1)
				sb.Append(", +" + Math.Round(((mwo.Values(DoubleValueKey.AttackBonus_Decal) - 1) * 100)) + "%a");

			if (mwo.Values(DoubleValueKey.WeaponDefense, 1) != 1)
				sb.Append(", " + Math.Round(((mwo.Values(DoubleValueKey.WeaponDefense) - 1) * 100)) + "%md");

			if (mwo.Values(DoubleValueKey.WeaponMagicDefense, 1) != 1)
				sb.Append(", " + Math.Round(((mwo.Values(DoubleValueKey.WeaponMagicDefense) - 1) * 100), 1) + "%mgc.d");

			if (mwo.Values(DoubleValueKey.WeaponMissileDefense, 1) != 1)
				sb.Append(", " + Math.Round(((mwo.Values(DoubleValueKey.WeaponMissileDefense) - 1) * 100), 1) + "%msl.d");

			if (mwo.Values(DoubleValueKey.ManaConversionMod) != 0)
				sb.Append(", " + Math.Round((mwo.Values(DoubleValueKey.ManaConversionMod) * 100)) + "%mc");

			if (showBuffedValues && (mwo.ObjectClass == (int)ObjectClass.MeleeWeapon || mwo.ObjectClass == (int)ObjectClass.MissileWeapon || mwo.ObjectClass == (int)ObjectClass.WandStaffOrb))
			{
				sb.Append(", (");

				// (Damage)
				if (mwo.ObjectClass == (int)ObjectClass.MeleeWeapon)
					sb.Append(mwo.CalcedBuffedTinkedDoT.ToString("N1") + "/" + mwo.GetBuffedIntValueKey((int)IntValueKey.MaxDamage_Decal));

				if (mwo.ObjectClass == (int)ObjectClass.MissileWeapon)
					sb.Append(mwo.CalcedBuffedMissileDamage.ToString("N1"));

				if (mwo.ObjectClass == (int)ObjectClass.WandStaffOrb)
					sb.Append(((mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.ElementalDamageMod) - 1) * 100).ToString("N1"));

				// (AttackBonus/MeleeDefenseBonus/ManaCBonus)
				sb.Append(" ");

				if (mwo.Values(DoubleValueKey.AttackBonus_Decal, 1) != 1)
					sb.Append(Math.Round(((mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.AttackBonus_Decal) - 1) * 100)).ToString("N1") + "/");

				if (mwo.Values(DoubleValueKey.WeaponDefense, 1) != 1)
					sb.Append(Math.Round(((mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.WeaponDefense) - 1) * 100)).ToString("N1"));

				if (mwo.Values(DoubleValueKey.ManaConversionMod) != 0)
					sb.Append("/" + Math.Round(mwo.GetBuffedDoubleValueKey((int)DoubleValueKey.ManaConversionMod) * 100));

				sb.Append(")");
			}

			if (mwo.Spells.Count > 0)
			{
				List<int> sortedSpellIds = new List<int>();

				foreach (var spell in mwo.Spells)
					sortedSpellIds.Add(spell);

				sortedSpellIds.Sort();
				sortedSpellIds.Reverse();

				foreach (int spellId in sortedSpellIds)
				{
					Spell spell = SpellTools.GetSpell(spellId);

					if (spell == null)
						continue;

					// If the item is not loot generated, show all spells
					if (!mwo.IntValues.ContainsKey((int)IntValueKey.MaterialType))
						goto ShowSpell;

					// Always show Minor/Major/Epic Impen
					if (spell.Name.Contains("Minor Impenetrability") || spell.Name.Contains("Major Impenetrability") || spell.Name.Contains("Epic Impenetrability") || spell.Name.Contains("Legendary Impenetrability"))
						goto ShowSpell;

					// Always show trinket spells
					if (spell.Name.Contains("Augmented"))
						goto ShowSpell;

					if (mwo.Values(IntValueKey.ResistMagic, 0) != 0)
					{
						// Show banes and impen on unenchantable equipment
						if (spell.Name.Contains(" Bane") || spell.Name.Contains("Impen") || spell.Name.StartsWith("Brogard"))
							goto ShowSpell;
					}
					else
					{
						// Hide banes and impen on enchantable equipment
						if (spell.Name.Contains(" Bane") || spell.Name.Contains("Impen") || spell.Name.StartsWith("Brogard"))
							continue;
					}

					//Debug.WriteToChat(spellById.Name + ", Difficulty: " + spellById.Difficulty + ", Family: " + spellById.Family + ", Generation: " + spellById.Generation + ", Type: " + spellById.Type + ", " + spellById.Unknown1 + " " + spellById.Unknown2 + " " + spellById.Unknown3 + " " + spellById.Unknown4 + " " + spellById.Unknown5 + " " + spellById.Unknown6 + " " + spellById.Unknown7 + " " + spellById.Unknown8 + " " + spellById.Unknown9 + " " + spellById.Unknown10);
					// <{Mag-Tools}>: Major Coordination,				Difficulty: 15,		Family: 267,	Generation: 1, Type: 1,		0	1		1 2572 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Epic Magic Resistance,			Difficulty: 20,		Family: 299,	Generation: 1, Type: 1,		0	1		1 4704 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Epic Life Magic Aptitude,			Difficulty: 20,		Family: 357,	Generation: 1, Type: 1,		0	1		1 4700 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Epic Endurance,					Difficulty: 20,		Family: 263,	Generation: 1, Type: 1,		0	1		1 4226 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Essence Glutton,					Difficulty: 30,		Family: 279,	Generation: 1, Type: 1,		0	0		1 2666 -2.07525870829232E+20 0 0 0 0 0

					// <{Mag-Tools}>: Might of the Lugians,				Difficulty: 300,	Family: 1,		Generation: 1, Type: 1,		0	0		1 2087 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Executor's Blessing,				Difficulty: 300,	Family: 115,	Generation: 1, Type: 1,		0	0		1 2053 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Regeneration Other Incantation,	Difficulty: 400,	Family: 93,		Generation: 1, Type: 1,		5	0.25	1 3982 -2.07525870829232E+20 0 0 0 0 0

					// Focusing stone
					// <{Mag-Tools}>: Brilliance,						Difficulty: 250,	Family: 15,		Generation: 1, Type: 1,		5	0.25	1 2348 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Concentration,					Difficulty: 100,	Family: 13,		Generation: 1, Type: 1,		0	0		1 2347 -2.07525870829232E+20 0 0 0 0 0
					// <{Mag-Tools}>: Malediction,						Difficulty: 50,		Family: 284,	Generation: 1, Type: 1,		0	0		1 2346 -2.07525870829232E+20 0 0 0 0 0

					// Weapon buffs
					// <{Mag-Tools}>: Elysa's Sight,					Difficulty: 300,	Family: 152,	Generation: 1, Type: 1,		25	0		1 2106 -2.07525870829232E+20 0 0 0 0 0 (Attack Skill)
					// <{Mag-Tools}>: Infected Caress,					Difficulty: 300,	Family: 154,	Generation: 1, Type: 1,		25	0		1 2096 -2.07525870829232E+20 0 0 0 0 0 (Damage)
					// <{Mag-Tools}>: Infected Spirit Caress,			Difficulty: 300,	Family: 154,	Generation: 1, Type: 1,		25	0		1 3259 -2.07525870829232E+20 0 0 0 0 0 (Damage)
					// <{Mag-Tools}>: Cragstone's Will,					Difficulty: 300,	Family: 156,	Generation: 1, Type: 1,		25	0		1 2101 -2.07525870829232E+20 0 0 0 0 0 (Defense)
					// <{Mag-Tools}>: Atlan's Alacrity,					Difficulty: 300,	Family: 158,	Generation: 1, Type: 1,		25	0		1 2116 -2.07525870829232E+20 0 0 0 0 0 (Speed)
					// <{Mag-Tools}>: Mystic's Blessing,				Difficulty: 300,	Family: 195,	Generation: 1, Type: 1,		25	0		1 2117 -2.07525870829232E+20 0 0 0 0 0 (Mana C)
					// <{Mag-Tools}>: Vision of the Hunter,				Difficulty: 500,	Family: 325,	Generation: 1, Type: 1,		25	0		1 2968 -2.07525870829232E+20 0 0 0 0 0 (Damage Mod)

					if ((spell.Family >= 152 && spell.Family <= 158) || spell.Family == 195 || spell.Family == 325)
					{
						// This is a weapon buff

						// Lvl 6
						if (spell.Difficulty == 250)
							continue;

						// Lvl 7
						if (spell.Difficulty == 300)
							goto ShowSpell;

						// Lvl 8+
						if (spell.Difficulty >= 400)
							goto ShowSpell;

						continue;
					}

					// This is not a weapon buff.

					// Filter all 1-5 spells
					if (spell.Name.EndsWith(" I") || spell.Name.EndsWith(" II") || spell.Name.EndsWith(" III") || spell.Name.EndsWith(" IV") || spell.Name.EndsWith(" V"))
						continue;

					// Filter 6's
					if (spell.Name.EndsWith(" VI"))
						continue;

					// Filter 7's
					if (spell.Difficulty == 300)
						continue;

					// Filter 8's
					if (spell.Name.Contains("Incantation"))
						continue;

				ShowSpell:

					sb.Append(", " + spell.Name);
				}
			}

			// Wield Lvl 180
			if (mwo.Values(IntValueKey.WieldDifficulty) > 0)
			{
				// I don't quite understand this.
				if (mwo.Values(IntValueKey.WieldRequirements) == 7 && mwo.Values(IntValueKey.WieldSkillType) == 1)
					sb.Append(", Wield Lvl " + mwo.Values(IntValueKey.WieldDifficulty));
				else
				{
					if (Dictionaries.SkillInfo.ContainsKey(mwo.Values(IntValueKey.WieldSkillType)))
						sb.Append(", " + Dictionaries.SkillInfo[mwo.Values(IntValueKey.WieldSkillType)] + " " + mwo.Values(IntValueKey.WieldDifficulty));
					else
						sb.Append(", Unknown skill: " + mwo.Values(IntValueKey.WieldSkillType) + " " + mwo.Values(IntValueKey.WieldDifficulty));
				}
			}

			// Summoning Gem
			if (mwo.Values((IntValueKey)369) > 0)
				sb.Append(", Lvl " + mwo.Values((IntValueKey)369));

			// Melee Defense 300 to Activate
			// If the activation is lower than the wield requirement, don't show it.
			if (mwo.Values(IntValueKey.ItemSkillLevelLimit) > 0 && (mwo.Values(IntValueKey.WieldSkillType) != mwo.Values(IntValueKey.AppraisalItemSkill) || mwo.Values(IntValueKey.WieldDifficulty) < mwo.Values(IntValueKey.ItemSkillLevelLimit)))
			{
				if (Dictionaries.SkillInfo.ContainsKey(mwo.Values(IntValueKey.AppraisalItemSkill)))
					sb.Append(", " + Dictionaries.SkillInfo[mwo.Values(IntValueKey.AppraisalItemSkill)] + " " + mwo.Values(IntValueKey.ItemSkillLevelLimit) + " to Activate");
				else
					sb.Append(", Unknown skill: " + mwo.Values(IntValueKey.AppraisalItemSkill) + " " + mwo.Values(IntValueKey.ItemSkillLevelLimit) + " to Activate");
			}

			// Summoning Gem
			if (mwo.Values((IntValueKey)366) > 0 && mwo.Values((IntValueKey)367) > 0)
			{
				if (Dictionaries.SkillInfo.ContainsKey(mwo.Values((IntValueKey)366)))
					sb.Append(", " + Dictionaries.SkillInfo[mwo.Values((IntValueKey)366)] + " " + mwo.Values((IntValueKey)367));
				else
					sb.Append(", Unknown skill: " + mwo.Values((IntValueKey)366) + " " + mwo.Values((IntValueKey)367));
			}

			// Summoning Gem
			if (mwo.Values((IntValueKey)368) > 0 && mwo.Values((IntValueKey)367) > 0)
			{
				if (Dictionaries.SkillInfo.ContainsKey(mwo.Values((IntValueKey)368)))
					sb.Append(", Spec " + Dictionaries.SkillInfo[mwo.Values((IntValueKey)368)] + " " + mwo.Values((IntValueKey)367));
				else
					sb.Append(", Unknown skill spec: " + mwo.Values((IntValueKey)368) + " " + mwo.Values((IntValueKey)367));
			}

			if (mwo.Values(IntValueKey.ItemDifficulty) > 0)
				sb.Append(", Diff " + mwo.Values(IntValueKey.ItemDifficulty));

			if (mwo.ObjectClass == (int)ObjectClass.Salvage)
			{
				if (mwo.Values(DoubleValueKey.SalvageWorkmanship_Decal) > 0)
					sb.Append(", Work " + mwo.Values(DoubleValueKey.SalvageWorkmanship_Decal).ToString("N2"));
			}
			else
			{
				if (mwo.Values(IntValueKey.ItemWorkmanship) > 0 && mwo.Values(IntValueKey.NumTimesTinkered) != 10) // Don't show the work if its already 10 tinked.
					sb.Append(", Craft " + mwo.Values(IntValueKey.ItemWorkmanship));
			}

			if (mwo.ObjectClass == (int)ObjectClass.Armor && mwo.Values(IntValueKey.ResistMagic, 0) != 0)
			{
				sb.Append(", [" +
					mwo.Values(DoubleValueKey.SlashProt_Decal).ToString("N1") + "/" +
					mwo.Values(DoubleValueKey.PierceProt_Decal).ToString("N1") + "/" +
					mwo.Values(DoubleValueKey.BludgeonProt_Decal).ToString("N1") + "/" +
					mwo.Values(DoubleValueKey.ColdProt_Decal).ToString("N1") + "/" +
					mwo.Values(DoubleValueKey.FireProt_Decal).ToString("N1") + "/" +
					mwo.Values(DoubleValueKey.AcidProt_Decal).ToString("N1") + "/" +
					mwo.Values(DoubleValueKey.LightningProt_Decal).ToString("N1") + "]");
			}

			if (showValueAndBurden)
			{
				if (mwo.Values(IntValueKey.Value) > 0)
					sb.Append(", Value " + String.Format("{0:n0}", mwo.Values(IntValueKey.Value)));

				if (mwo.Values(IntValueKey.EncumbranceVal) > 0)
					sb.Append(", BU " + mwo.Values(IntValueKey.EncumbranceVal));
			}

			if (mwo.TotalRating > 0)
			{
				sb.Append(", [");
				bool first = true;
				if (mwo.DamRating > 0) { sb.Append("D " + mwo.DamRating); first = false; }
				if (mwo.DamResistRating > 0) { if (!first) sb.Append(", "); sb.Append("DR " + mwo.DamResistRating); first = false; }
				if (mwo.CritRating > 0) { if (!first) sb.Append(", "); sb.Append("C " + mwo.CritRating); first = false; }
				if (mwo.CritDamRating > 0) { if (!first) sb.Append(", "); sb.Append("CD " + mwo.CritDamRating); first = false; }
				if (mwo.CritResistRating > 0) { if (!first) sb.Append(", "); sb.Append("CR " + mwo.CritResistRating); first = false; }
				if (mwo.CritDamResistRating > 0) { if (!first) sb.Append(", "); sb.Append("CDR " + mwo.CritDamResistRating); first = false; }
				if (mwo.HealBoostRating > 0) { if (!first) sb.Append(", "); sb.Append("HB " + mwo.HealBoostRating); first = false; }
				if (mwo.VitalityRating > 0) { if (!first) sb.Append(", "); sb.Append("V " + mwo.VitalityRating); first = false; }
				sb.Append("]");
			}

			if (mwo.ObjectClass == (int)ObjectClass.Misc && mwo.Name.Contains("Keyring"))
				sb.Append(", Keys: " + mwo.Values(IntValueKey.NumKeys) + ", Uses: " + mwo.Values(IntValueKey.Structure));

			return sb.ToString();
		}
	}
}
