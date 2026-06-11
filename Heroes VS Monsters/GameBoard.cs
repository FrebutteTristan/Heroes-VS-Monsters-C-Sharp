using System;
using System.Collections.Generic;
using System.Text;
using Heroes_VS_Monsters.Characters;
using Heroes_VS_Monsters.Fights;

namespace Heroes_VS_Monsters
{
    class GameBoard
    {
        private const int SIZE = 15;

        private int _heroX;
        private int _heroY;

        private Heroes _hero;

        private List<MonsterPosition> _monsters;

        private Random _random = new Random();

        private int _defeatedMonsters = 0;

        private List<string> _rightPanel = new List<string>();


        private const int MAP_COL_WIDTH = 46;
        private const int RIGHT_COL_START = 48;
        private const int RIGHT_COL_WIDTH = 60;
        private const int PANEL_START_ROW = 3;
        private const int MAP_START_ROW = 3; 
        private const int MAP_START_COL = 2;


        public GameBoard(Heroes hero)
        {
            _hero = hero;
            _heroX = SIZE / 2;
            _heroY = SIZE / 2;
            _monsters = new List<MonsterPosition>();
            PlaceMonsters();
        }

        private void PlaceMonsters()
        {
            List<Monsters> monstersList = new List<Monsters>
            {
                new Wolf("Croc-d'Acier"), new Wolf("Fenrir"),   new Wolf("Lycan"),
                new Wolf("Morana"),       new Wolf("Kaelen"),
                new Goblin("Zog"),        new Goblin("Zora"),   new Goblin("Razz"),
                new Goblin("Morve"),      new Goblin("Casse-Dent"),
                new Orc("Gauthak"),       new Orc("Ugurth"),    new Orc("Vola"),
                new Dragon("Venomfang"), new Dragon("Aurinax")
            };

            foreach (Monsters monster in monstersList)
            {
                int x, y, tentatives = 0;
                do
                {
                    x = _random.Next(0, SIZE);
                    y = _random.Next(0, SIZE);
                    tentatives++;
                    if (tentatives > 1000) break;
                }
                while (!PositionValid(x, y));
                _monsters.Add(new MonsterPosition(monster, x, y));
            }
        }

        private bool PositionValid(int x, int y)
        {
            if (Math.Abs(x - _heroX) <= 2 && Math.Abs(y - _heroY) <= 2) return false;
            foreach (MonsterPosition mp in _monsters)
                if (Math.Abs(x - mp.X) <= 1 && Math.Abs(y - mp.Y) <= 1) return false;
            return true;
        }

        public void Play()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            _rightPanel.Add("");
            _rightPanel.Add("══ Journal ══════════════════════");
            _rightPanel.Add("");
            _rightPanel.Add("Bienvenue dans la Forêt !");
            _rightPanel.Add("Tu sens que tu n'es pas seul...");
            _rightPanel.Add("Essaie de trouver ton chemin.");

           
            RenderOnce();
         
            RenderFull();

            while (!_hero.Dead() && _monsters.Count > 0)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                int newX = _heroX, newY = _heroY;
                if (key.Key == ConsoleKey.LeftArrow) newX--;
                else if (key.Key == ConsoleKey.RightArrow) newX++;
                else if (key.Key == ConsoleKey.UpArrow) newY--;
                else if (key.Key == ConsoleKey.DownArrow) newY++;
                else continue;

                if (newX < 0 || newX >= SIZE || newY < 0 || newY >= SIZE) continue;

                _heroX = newX;
                _heroY = newY;

                CheckAdjacentsFights();

                if (!_hero.Dead() && _monsters.Count > 0)
                    RenderFull();
            }

            DisplayGameEnd();
        }


        private void CheckAdjacentsFights()
        {
            List<MonsterPosition> copy = new List<MonsterPosition>(_monsters);

            foreach (MonsterPosition mp in copy)
            {
                int distX = Math.Abs(_heroX - mp.X);
                int distY = Math.Abs(_heroY - mp.Y);
                bool isAdjacent = (distX + distY == 1);
                bool isOnMonsterCase = (distX == 0 && distY == 0);

                if (!isAdjacent && !isOnMonsterCase) continue;

                _rightPanel.Clear();
                _rightPanel.Add("══ Combat ════════════════════════");
                _rightPanel.Add("");
                _rightPanel.Add($"⚔️  {mp.Monster.Name} surgit !");
                _rightPanel.Add("");
                _rightPanel.Add("[ 'Enter' pour combattre... ]");
                RenderFull();

                Console.CursorVisible = true;
                Console.ReadLine();
                Console.CursorVisible = false;

                var (victory, combatLog, lootLog) = Fight.StartFight(_hero, mp.Monster);

                _rightPanel.Clear();
                foreach (string line in combatLog) _rightPanel.Add(line);
                _rightPanel.Add("");
                _rightPanel.Add("[ 'Enter' pour voir le Loot... ]");
                RenderFull();

                Console.CursorVisible = true;
                Console.ReadLine();
                Console.CursorVisible = false;

                if (victory)
                {
                    _defeatedMonsters++;
                    _monsters.Remove(mp);

                    _rightPanel.Clear();
                    _rightPanel.Add("══ Loot ══════════════════════════");
                    foreach (string line in lootLog) _rightPanel.Add(line);

                    if (_hero.HasPendingItem)
                    {
                        _rightPanel.Add("");
                        _rightPanel.Add("──────────────────────────────────");
                        RenderFull();

                        WriteAt(RIGHT_COL_START, Console.CursorTop + 1, "oui / non : ");
                        Console.CursorVisible = true;
                        string answer = Console.ReadLine()!;
                        Console.CursorVisible = false;

                        List<string> decisionLog = new List<string>();
                        if (answer.Trim().ToLower() == "oui")
                            _hero.AcceptPendingItem(decisionLog);
                        else
                            _hero.RejectPendingItem(decisionLog);

                        foreach (string line in decisionLog) _rightPanel.Add(line);
                    }

                    _rightPanel.Add("");
                    _rightPanel.Add($"Monstres restants : {_monsters.Count}");
                    _rightPanel.Add("");
                    _rightPanel.Add("[ 'Enter' pour reprendre... ]");
                    RenderFull();
                }

                Console.CursorVisible = true;
                Console.ReadLine();
                Console.CursorVisible = false;

                _rightPanel.Clear();
                _rightPanel.Add("══ Journal ══════════════════════");
                if (victory)
                    _rightPanel.Add($"✅ {mp.Monster.Name} vaincu !");
                else
                    _rightPanel.Add($"☠️  {_hero.Name} est mort...");

                if (_hero.Dead()) return;
            }
        }


        private void RenderOnce()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            ConsoleColored.WriteColored(
                @"    ╔══════════════════════════╗              ╔══════════════════════════════════════╗
    ║    Forêt Kryptgarden     ║              ║            Journal / Combat          ║
    ╚══════════════════════════╝              ╚══════════════════════════════════════╝",
                ConsoleColor.Yellow);

         
            WriteAt(MAP_START_COL, MAP_START_ROW, "+" + new string('-', SIZE * 2) + "+");
            WriteAt(MAP_START_COL, MAP_START_ROW + SIZE + 1, "+" + new string('-', SIZE * 2) + "+");

           
            int instrRow = MAP_START_ROW + SIZE + 5;
            WriteAt(0, instrRow, "  ⬆ ⬇ ⬅ ➔ pour te déplacer");
        }


        private void RenderFull()
        {
            for (int lign = 0; lign < SIZE; lign++)
            {
                int row = MAP_START_ROW + 1 + lign;
                StringBuilder sb = new StringBuilder();
                sb.Append("  |");

                for (int col = 0; col < SIZE; col++)
                {
                    if (col == _heroX && lign == _heroY)
                    {
                        sb.Append(GetIconeHero());
                        continue;
                    }

                    bool displayMonster = false;
                    foreach (MonsterPosition mp in _monsters)
                    {
                        if (mp.X == col && mp.Y == lign)
                        {
                            sb.Append(IsAdjacent(col, lign) ? GetIconeMonster(mp.Monster) : ". ");
                            displayMonster = true;
                            break;
                        }
                    }
                    if (!displayMonster) sb.Append(". ");
                }

                sb.Append("|");
                WriteAt(0, row, sb.ToString());
            }

            int statsRow = MAP_START_ROW + SIZE + 2;
            WriteAt(0, statsRow, PadRight(GetStatsLine1(), MAP_COL_WIDTH));
            WriteAt(0, statsRow + 1, PadRight(GetStatsLine2(), MAP_COL_WIDTH));
            WriteAt(0, statsRow + 2, PadRight(GetStatsLine3a(), MAP_COL_WIDTH));
            WriteAt(0, statsRow + 3, PadRight(GetStatsLine3b(), MAP_COL_WIDTH));

            const int maxLines = 60;
            string blank = new string(' ', RIGHT_COL_WIDTH);

            for (int i = 0; i < maxLines; i++)
                WriteAt(RIGHT_COL_START, PANEL_START_ROW + i, blank);

            for (int i = 0; i < _rightPanel.Count && i < maxLines; i++)
            {
                string line = _rightPanel[i];
                if (line.Length > RIGHT_COL_WIDTH)
                    line = line.Substring(0, RIGHT_COL_WIDTH);
                WriteAt(RIGHT_COL_START, PANEL_START_ROW + i, line);
            }

            Console.SetCursorPosition(0, statsRow + 6);
        }

        private void WriteAt(int col, int row, string text)
        {
            try
            {
                Console.SetCursorPosition(col, row);
                Console.Write(text);
            }
            catch 
            {

            }
        }

        private string PadRight(string text, int width)
            => text.Length >= width ? text : text + new string(' ', width - text.Length);

        private bool IsAdjacent(int x, int y)
            => (Math.Abs(x - _heroX) + Math.Abs(y - _heroY) == 1);

        private string GetIconeHero()
        {
            if (_hero is Human) return "🧑";
            if (_hero is Dwarf) return "🪖";
            return "🧝";
        }

        private string GetIconeMonster(Monsters monster)
        {
            if (monster is Wolf) return "🐺";
            if (monster is Goblin) return "👺";
            if (monster is Orc) return "👹";
            return "🐉";
        }

        private string GetStatsLine1()
        {
            string icon = GetIconeHero();
            return $"  {icon} {_hero.Name} [{_hero.Race}]  PV: {_hero.HP}  Or: {_hero.Gold}  Cuir: {_hero.Leather}";
        }

        private string GetStatsLine2()
        {
            if (_hero is Elf elf)
                return $"  Arc: {elf.TotalArcana}  End: {_hero.TotalStamina}  Monstres restants: {_monsters.Count}";
            return $"  For: {_hero.TotalStrength}  End: {_hero.TotalStamina}  Monstres restants: {_monsters.Count}";
        }

        private string GetStatsLine3a()
        {
            string sword = _hero.Sword != null ? _hero.Sword.ToString() : " x";
            string staff = _hero.MagicStaff != null ? _hero.MagicStaff.ToString() : " x";
            return $"  🗡 {sword}  🪄 {staff}";
        }

        private string GetStatsLine3b()
        {
            string armor = _hero.Armor != null ? _hero.Armor.ToString() : " x";
            return $"  🛡 {armor}";
        }

        private void DisplayGameEnd()
        {
            Console.Clear();
            ConsoleColored.WriteColored(@"

                ╔══════════════════════════════════════════╗
                ║              FIN DU JEU                  ║
                ╚══════════════════════════════════════════╝
            ", ConsoleColor.Yellow);

            if (_hero.Dead())
            {
                ConsoleColored.WriteColored(@$"                
                      💀 {_hero.Name} est tombé(e) au combat !
                         Tu as vaincu {_defeatedMonsters} monstre(s).", ConsoleColor.Red);

                ConsoleColored.WriteColored(@$"
                                _____________
                               /             \
                              /               \
                             |                 |
                             |      R.I.P      |
                             |                 |
                             |    ''LOSER''    |
                             |                 |
                             ___________________
                            /                   \
                           /          ~          \
                          /          ( )          \
                         /          (o o)          \
                        /          (  .  )          \
                       /          (  )-(  )          \
                      /            (_____)            \
                     /                                 \
                     -----------------------------------", ConsoleColor.Gray);
            }
            else
            {
                ConsoleColored.WriteColored(@$"                    
                                  🏆 Félicitations {_hero.Name} !
                            Tu as vaincu tous les monstres de la forêt !
                     Tu peux enfin quitter cet endroit maudit et rentrer chez toi.", ConsoleColor.Yellow);

                ConsoleColored.WriteColored($@"
                             (@@)       (@@)       (@@)       (@@)       (@@)
                            (@@@@)     (@@@@)     (@@@@)     (@@@@)     (@@@@)
                           (@@@@@@)   (@@@@@@)   (@@@@@@)   (@@@@@@)   (@@@@@@)", ConsoleColor.DarkGreen);

                ConsoleColored.WriteColored(@$"                              ||         ||         ||         ||         ||
                    --------------------------------------------------------------", ConsoleColor.DarkGray);

                ConsoleColored.WriteColored($@"
                                             0
                                            \_\_
                                              _\\
                                            _/  \_", ConsoleColor.Yellow);

                ConsoleColored.WriteColored(@$"
                    --------------------------------------------------------------
                
                ", ConsoleColor.DarkGray);
            }

            _hero.DisplayInventory();
            Console.WriteLine("  Appuie sur Enter pour quitter...");
            Console.CursorVisible = true;
            Console.ReadLine();
        }
    }

    class MonsterPosition
    {
        public Monsters Monster { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public MonsterPosition(Monsters monster, int x, int y)
        {
            Monster = monster;
            X = x;
            Y = y;
        }
    }
}
