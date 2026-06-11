using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes_VS_Monsters.Items
{
    class Item
    {
        public string Name { get; }
        public TypeItem Type { get; }
        public int Bonus { get; }
        public Item(string name, TypeItem type, int bonus)
        {
            Name = name;
            Type = type;
            Bonus = bonus;
        }

        public override string ToString()
        {
            string statBoost = "";

            if (Type == TypeItem.Sword)
                statBoost = "Force";
            else if (Type == TypeItem.Armor)
                statBoost = "Endurance";
            else
                statBoost = "Arcane";

            return $"{Name} (+{Bonus} {statBoost})";
        }
    }
}
