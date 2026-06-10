using System;
using System.Collections.Generic;
using System.Text;

namespace Heroes_VS_Monsters_2.Characters
{
    abstract class Monsters : Characters
    {
        public int Gold { get; protected set; }
        public int Leather { get; protected set; }

        public Monsters(string name, int Stre, int Stam, int HP) : base(name, Stre, Stam, HP)
        {
            Gold = 0;
            Leather = 0;
        }


    }
}
