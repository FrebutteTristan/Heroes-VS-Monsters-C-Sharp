using System;
using System.Collections.Generic;
using System.Text;
using Heroes_VS_Monsters_2.Items;

namespace Heroes_VS_Monsters_2.Items
{
    static class ItemsManagement
    {
        private static Random _random = new Random();

        public static Item DropChance(string HerosRace)
        {

            if (PercentageChance(20))
            {
                if (HerosRace != "Elfe")
                {
                    return GenerateSword();
                }
            }

            if (PercentageChance(20))
            {
                return GenerateArmor();
            }

            if (PercentageChance(20))
            {

                if (HerosRace == "Elfe")
                {
                    return GenerateStaff();
                }
            }

            return null;
        }

        private static Item GenerateSword()
        {
            int itemDraw = _random.Next(1, 101);

            if (itemDraw <= 70)
                return new Item("Épée en bois", TypeItem.Sword, 2);
            else if (itemDraw <= 85)
                return new Item("Épée en fer", TypeItem.Sword, 3);
            else
                return new Item("Épée en adamantine", TypeItem.Sword, 5);
        }


        private static Item GenerateArmor()
        {
            int itemDrow = _random.Next(1, 101);

            if (itemDrow <= 70)
                return new Item("Armure en tissu", TypeItem.Armor, 2);
            else if (itemDrow <= 85)
                return new Item("Armure en cuir", TypeItem.Armor, 3);
            else
                return new Item("Armure en adamantium", TypeItem.Armor, 5);
        }


        private static Item GenerateStaff()
        {
            int itemDrow = _random.Next(1, 101);

            if (itemDrow <= 70)
                return new Item("Bâton de feu", TypeItem.MagicStaff, 2);
            else if (itemDrow <= 85)
                return new Item("Bâton de foudre", TypeItem.MagicStaff, 3);
            else
                return new Item("Bâton du pouvoir", TypeItem.MagicStaff, 5);
        }



        private static bool PercentageChance(int percentage)
        {
            return _random.Next(1, 101) <= percentage;
        }

    }
}

