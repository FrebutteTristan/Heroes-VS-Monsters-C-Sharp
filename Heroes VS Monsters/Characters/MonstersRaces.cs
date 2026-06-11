using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes_VS_Monsters.Characters
{
    class Wolf : Monsters
    {
        public Wolf(string name) : base(name, Stre: 4, Stam: 7, HP: 6)
        {
            Random random = new Random();
            Leather = random.Next(1, 11);
            Gold = 0;
        }

        public override string ToString()
        {
            return $"[Loup] " + base.ToString() + $" | Cuir: {Leather}";
        }
    }


    class Goblin : Monsters
    {
        public Goblin(string name) : base(name, Stre: 9, Stam: 9, HP: 9)
        {
            Random random = new Random();
            Gold = random.Next(2, 16);
            Leather = 0;
        }

        public override string ToString()
        {
            return $"[Gobelin] " + base.ToString() + $" | Or: {Gold}";
        }
    }


    class Orc : Monsters
    {
        public Orc(string name) : base(name, Stre: 13, Stam: 13, HP: 14)
        {
            Random random = new Random();
            Gold = random.Next(15, 51);
            Leather = 0;
        }

        public override string ToString()
        {
            return $"[Orque] " + base.ToString() + $" | Or: {Gold}";
        }
    }


    class Dragon : Monsters
    {
        public Dragon(string name) : base(name, Stre: 15, Stam: 15, HP: 18)
        {
            Random random = new Random();
            Gold = random.Next(30, 101);
            Leather = random.Next(15, 51);
        }

        public override string ToString()
        {
            return $"[Dragonnet] " + base.ToString() + $" | Or: {Gold}, Cuir: {Leather}";
        }
    }
}
