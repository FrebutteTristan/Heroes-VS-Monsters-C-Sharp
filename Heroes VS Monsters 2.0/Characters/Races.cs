using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Heroes_VS_Monsters_2.Dices;

namespace Heroes_VS_Monsters_2.Characters
{
    class Human : Heroes
    {
        public override string Race => "Humain";
        public override int TotalStrength => Strength + 1 + (Sword != null ? Sword.Bonus : 0);
        public override int TotalStamina => Stamina + 1 + (Armor != null ? Armor.Bonus : 0);

        public Human(string name, int stre, int stam) : base(name, stre, stam) { }
    }

    class Dwarf : Heroes
    {
        public override string Race => "Nain";
        public override int TotalStrength => Strength + (Sword != null ? Sword.Bonus : 0);
        public override int TotalStamina => Stamina + 2 + (Armor != null ? Armor.Bonus : 0);

        public Dwarf(string name, int stre, int stam) : base(name, stre, stam) { }
    }

    class Elf : Heroes
    {
        public override string Race => "Elfe";
        public int Arcana { get; }
        public int TotalArcana => Arcana + 2 + (MagicStaff != null ? MagicStaff.Bonus : 0);
        public override int TotalStrength => 0;
        public override int TotalStamina => Stamina + (Armor != null ? Armor.Bonus : 0);

        public Elf(string name, int arc, int stam) : base(name, noStrength: true, stam)
        {
            Arcana = arc;
        }

        public override void Hit(Characters target)
        {
            Dice dice4 = new Dice(1, 4);
            int damages = dice4.Roll() + StatsModifier(TotalArcana);
            if (damages < 0) damages = 0;
            target.TakeDamages(damages);
            Console.WriteLine($"  {Name} lance un sort sur {target.Name} pour {damages} dégâts ! (PV restants : {target.HP})");
        }

        public override string HitLog(Characters target)
        {
            Dice dice4 = new Dice(1, 4);
            int damages = dice4.Roll() + StatsModifier(TotalArcana);
            if (damages < 0) damages = 0;
            target.TakeDamages(damages);
            return $" ✨{Name} → {target.Name} : -{damages} (PV: {target.HP})";
        }

        public override string ToString()
        {
            return $"[Elfe] {Name} - Endurance: {TotalStamina}, Arcane: {Arcana} (valeur finale: {TotalArcana}), PV: {HP}" +
                   $" | Or: {Gold}, Cuir: {Leather}";
        }
    }
}
