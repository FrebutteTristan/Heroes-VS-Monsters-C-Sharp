using System;
using System.Text;

namespace Heroes_VS_Monsters_2.Dices
{
    class Dice
    {
        private static Random _random = new Random();

        public int Minimum { get; }
        public int Maximum { get; }

        public Dice(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public int Roll()
        {
            return _random.Next(Minimum, Maximum + 1);
        }
    }
}
