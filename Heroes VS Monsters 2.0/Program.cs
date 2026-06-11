using System;
using Heroes_VS_Monsters.Characters;
using Heroes_VS_Monsters.Dices;

namespace Heroes_VS_Monsters
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;



            ConsoleHelper.WriteColored(@"



                                                   Tu te réveilles avec une migraine intense...    

            
                                        Devant toi, tu peux apercevoir un panneau couvert de mousse... :", ConsoleColor.Yellow);


            ConsoleHelper.WriteColored(@"
                                           ╔═══════════════════════════════════════════════════╗
                                           ║                                                   ║
                                           ║                 FORET KRYPTGARDEN                 ║
                                           ║                                                   ║
                                           ╚═══════════════════════╗══╔════════════════════════╝
                                                                   ║══║
                                                                   ║══║
                                                                   ║══║
                                                                   ║══║
                                                                   ║══║
                                                                   ╚══╝", ConsoleColor.Green);


          ConsoleHelper.WriteColored(@"
                                   La douleur te fait baisser les yeux vers une flaque d'eau à tes pieds...
                                        
            
                                   Quelle race vois-tu ?
            
                           1. 🧑 Humain (+1 Force, +1 Endurance) - peut s'équiper d'une épée et d'une armure
                           2. 🪖 Nain   (+2 Endurance)           - peut s'équiper d'une épée et d'une armure
                           3. 🧝 Elfe   (+2 Arcane)              - peut s'équiper d'un bâton magique et d'une armure
            
                             1, 2 ou 3 :
            ", ConsoleColor.Yellow);
            
            string raceSelection = Console.ReadLine()!;

            ConsoleHelper.WriteColored("               En te massant les tempes, tu arrives à retrouver tes esprits, quel est ton nom ?", ConsoleColor.Yellow);
            string heroName = Console.ReadLine()!;
            Console.Clear();

            Heroes hero;

            if (raceSelection == "1")
            {
                int stre = RollDice("⚔️  Force", heroName);
                Console.WriteLine();
                int stam = RollDice("🛡️  Endurance", heroName);
                hero = new Human(heroName, stre, stam);

            }
            else if (raceSelection == "2")
            {
                int stre = RollDice("⚔️  Force", heroName);
                Console.WriteLine();
                int stam = RollDice("🛡️  Endurance", heroName);
                hero = new Dwarf(heroName, stre, stam);

            }
            else
            {
                int arc = RollDice("✨  Arcane", heroName);
                Console.WriteLine();
                int stam = RollDice("🛡️  Endurance", heroName);
                hero = new Elf(heroName, arc, stam);

            }

            Console.WriteLine(@"
                                Votre héros a été créé !
            ");
            Console.WriteLine(hero.ToString());

            GameBoard map = new GameBoard(hero);
            map.Play();
        }


        static int RollDice(string stats, string heroName)
        {
            Dice dice6 = new Dice(1, 6);
            int[] results = new int[4];


            ConsoleHelper.WriteColored(@$"  
              ╔══════════════════════════════════════════╗
               🎲 Lancer des dés pour : {stats}   
              ╚══════════════════════════════════════════╝
               
           Tu vas lancer 4 dés 6.Seuls les 3 meilleurs résultats
           seront additionnés pour déterminer ta/ton {stats}.
            ", ConsoleColor.Cyan);
          

            for (int i = 0; i < 4; i++)
            {
                Console.Write($"           Appuie sur 'Enter'' pour lancer le dé ({i + 1}/4)...");
                Console.ReadLine();

                results[i] = dice6.Roll();

                Console.WriteLine($"            🎲 Dé {i + 1} : {ShowDice(results[i])}  →  {results[i]}");
                Console.WriteLine();
            }

            int[] copy = (int[])results.Clone();
            Array.Sort(copy);

            int smallerDice = copy[0];
            int sum = copy[1] + copy[2] + copy[3];

            ConsoleHelper.WriteColored(@"
              ──────────────────────────────────────────
                       Récapitulatif des 4 dés :
            ", ConsoleColor.Cyan);

            bool ignoreSmallerDice = false;
            for (int i = 0; i < 4; i++)
            {
                if (results[i] == smallerDice && !ignoreSmallerDice)
                {
                    Console.Write("");
                    ignoreSmallerDice = true;
                }

            }

            ConsoleHelper.WriteColored(@$" 
               ❌ Le dé le plus faible ({smallerDice}) est ignoré.

               ✅ Somme des 3 meilleurs : {copy[1]} + {copy[2]} + {copy[3]} = {sum}

               🏆 {stats} de {heroName} : {sum}
             ══════════════════════════════════════════", ConsoleColor.Yellow);
            Console.ReadLine();
            Console.Clear();


            return sum;
        }

        static string ShowDice(int value)
        {
            switch (value)
            {
                case 1: return "⚀";
                case 2: return "⚁";
                case 3: return "⚂";
                case 4: return "⚃";
                case 5: return "⚄";
                case 6: return "⚅";
                default: return "?";
            }
        }

    }
}