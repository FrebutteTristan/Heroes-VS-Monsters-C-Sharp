using System;
using System.Collections.Generic;
using System.Text;
using Heroes_VS_Monsters_2.Characters;

namespace Heroes_VS_Monsters_2.Fights
{
    class Fight
    {
        public static (bool victory, List<string> combatLog, List<string> lootLog) StartFight(Heroes hero, Monsters monster)
        {
            List<string> combatLog = new List<string>();
            List<string> lootLog = new List<string>();

            combatLog.Add("════════════════════════════════");
            combatLog.Add($" ⚔️  {hero.Name} VS {monster.Name}");
            combatLog.Add("════════════════════════════════");
            combatLog.Add($" {hero.Name}: {hero.HP} PV");
            combatLog.Add($" {monster.Name}: {monster.HP} PV");
            combatLog.Add("────────────────────────────────");

            int turn = 1;

            while (!hero.Dead() && !monster.Dead())
            {
                combatLog.Add("");
                combatLog.Add($" --- Tour {turn} ---");
                combatLog.Add("");

                string heroHit = hero.HitLog(monster);
                combatLog.Add(heroHit);

                if (monster.Dead())
                {
                    combatLog.Add($" 💀 {monster.Name} est mort !");
                    break;
                }

                string monsterHit = monster.HitLog(hero);
                combatLog.Add(monsterHit);

                if (hero.Dead())
                {
                    combatLog.Add($" 💀 {hero.Name} est mort !");
                    break;
                }

                turn++;
            }

            combatLog.Add("────────────────────────────────");

            if (!hero.Dead())
            {
                combatLog.Add($" 🏆 {hero.Name} a gagné !");

                hero.CollectLoot(monster, lootLog);
                hero.CollectDropChance(lootLog);
                hero.Rest();

                return (true, combatLog, lootLog);
            }
            else
            {
                combatLog.Add($" ☠️  {hero.Name} a perdu...");
                return (false, combatLog, lootLog);
            }
        }
    }
}

