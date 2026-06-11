using System;
using System.Collections.Generic;
using System.Text;
using Heroes_VS_Monsters.Dices;

namespace Heroes_VS_Monsters.Characters
{
    abstract class Characters
    {
        public int Strength { get; }
        public int Stamina { get; }

        private int _HP;
        public int HP
        {
            get { return _HP; }
        }

        public string Name { get; }

        public Characters(string name, int stre, int stam)
        {
            Name = name;
            Strength = stre;
            Stamina = stam;
            _HP = Stamina + StatsModifier(Stamina);
        }

        public Characters(string name, int stre, int stam, int HP)
        {
            Name = name;
            Strength = stre;
            Stamina = stam;
            _HP = HP;
        }

        public Characters(string name, bool noStrength, int stam)
        {
            Name = name;
            Strength = 0;
            Stamina = stam;
            _HP = Stamina + StatsModifier(Stamina);
        }

        public static int StatsModifier(int value)
        {
            if (value <= 4)
                return -1;
            else if (value <= 9)
                return 0;
            else if (value <= 14)
                return 1;
            else if (value <= 19)
                return 2;
            else
                return 3;
        }

        public static int RollDice()
        {
            int[] results = new int[4];
            Dice dice6 = new Dice(1, 6);
            for (int i = 0; i < 4; i++)
                results[i] = dice6.Roll();
            Array.Sort(results);
            return results[1] + results[2] + results[3];
        }

        public virtual void Hit(Characters target)
        {
            Dice dice4 = new Dice(1, 4);
            int damages = dice4.Roll() + StatsModifier(Strength);
            if (damages < 0) damages = 0;
            target.TakeDamages(damages);
            Console.WriteLine($" {Name} frappe {target.Name} pour {damages} dégâts ! (PV restants : {target.HP})");
        }

        public virtual string HitLog(Characters target)
        {
            Dice dice4 = new Dice(1, 4);
            int damages = dice4.Roll() + StatsModifier(Strength);
            if (damages < 0) damages = 0;
            target.TakeDamages(damages);
            return $" {Name} → {target.Name} : -{damages} dmg (PV: {target.HP})";
        }

        public void TakeDamages(int damages)
        {
            _HP -= damages;
        }

        public void Rest()
        {
            _HP = Stamina + StatsModifier(Stamina);
        }

        public bool Dead()
        {
            return _HP <= 0;
        }

        public override string ToString()
        {
            return $"{Name} - Force : {Strength}, Endurance : {Stamina}, PV : {HP}";
        }
    }
}
