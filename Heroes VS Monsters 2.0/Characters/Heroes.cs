using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Heroes_VS_Monsters_2.Items;

namespace Heroes_VS_Monsters_2.Characters
{
    abstract class Heroes : Characters
    {
        public int Gold { get; private set; }
        public int Leather { get; private set; }
        public Item Sword { get; private set; }
        public Item Armor { get; private set; }
        public Item MagicStaff { get; private set; }

        public abstract string Race { get; }

        public Heroes(string name, int stre, int stam) : base(name, stre, stam)
        {
            Gold = 0; Leather = 0; Sword = null; Armor = null; MagicStaff = null;
        }

        public Heroes(string name, bool noStrength, int stam) : base(name, noStrength, stam)
        {
            Gold = 0; Leather = 0; Sword = null; Armor = null; MagicStaff = null;
        }

        public abstract int TotalStrength { get; }
        public abstract int TotalStamina { get; }

        
        public void CollectLoot(Monsters monster, List<string> log)
        {
            if (monster.Gold > 0)
            {
                Gold += monster.Gold;
                log.Add($" 💰 +{monster.Gold} or  (Total: {Gold})");
            }
            if (monster.Leather > 0)
            {
                Leather += monster.Leather;
                log.Add($" 🧱 +{monster.Leather} cuir  (Total: {Leather})");
            }
        }

        
        public void CollectDropChance(List<string> log)
        {
            Item itemDrop = ItemsManagement.DropChance(Race);

            if (itemDrop == null)
            {
                log.Add(" Aucun objet trouvé sur le corps.");
                return;
            }

            log.Add($" 🎁 OBJET TROUVÉ : {itemDrop} !");

            Item existing = GetInventory(itemDrop.Type);

            if (existing == null)
            {
                EquipItem(itemDrop);
                log.Add($" ✅ {Name} équipe : {itemDrop}");
            }
            else
            {
                _pendingItem = itemDrop;
                log.Add($" ⚠️  Tu possèdes déjà : {existing}");
                log.Add($" Remplacer par {itemDrop} ?");
                log.Add(" [Ecris 'oui' pour changer ou 'non'pour garder]");
                log.Add("Appuie sur 'Enter'");
            }
        }

        private Item _pendingItem = null;
        public Item PendingItem => _pendingItem;

        public bool HasPendingItem => _pendingItem != null;

        public void AcceptPendingItem(List<string> log)
        {
            if (_pendingItem != null)
            {
                EquipItem(_pendingItem);
                log.Add($" ✅ {Name} équipe : {_pendingItem} !");
                _pendingItem = null;
            }
        }

        public void RejectPendingItem(List<string> log)
        {
            if (_pendingItem != null)
            {
                log.Add($" {Name} garde son ancien objet.");
                _pendingItem = null;
            }
        }

        public void LootDrop(Monsters monster)
        {
            if (monster.Gold > 0) { Gold += monster.Gold; Console.WriteLine($"{Name} ramasse {monster.Gold} pièces d'or !"); }
            if (monster.Leather > 0) { Leather += monster.Leather; Console.WriteLine($"{Name} ramasse {monster.Leather} morceaux de cuir !"); }
        }

        public void DropChance()
        {
            Item itemDrop = ItemsManagement.DropChance(Race);
            if (itemDrop == null) { Console.WriteLine("Aucun objet trouvé."); return; }
            Item inventory = GetInventory(itemDrop.Type);
            if (inventory == null) { EquipItem(itemDrop); }
            else
            {
                Console.WriteLine($"Veux-tu remplacer {inventory} par {itemDrop} ? (oui/non) : ");
                string answer = Console.ReadLine()!;
                if (answer == "oui") EquipItem(itemDrop);
            }
        }

        public void GetRest()
        {
            Rest();
            Console.WriteLine($" {Name} se repose et récupère tous ses PV.");
        }

        private Item GetInventory(TypeItem type)
        {
            if (type == TypeItem.Sword) return Sword;
            else if (type == TypeItem.Armor) return Armor;
            else return MagicStaff;
        }

        private void EquipItem(Item item)
        {
            if (item.Type == TypeItem.Sword) Sword = item;
            else if (item.Type == TypeItem.Armor) Armor = item;
            else MagicStaff = item;
        }

        public void DisplayInventory()
        {
            Console.WriteLine($"\n  --- Objets récupérés durant l'aventure ---");
            Console.WriteLine($"  Or : {Gold} pièces | Cuir : {Leather} morceaux");
            Console.WriteLine($"  Épée     : {(Sword != null ? Sword.ToString() : "aucune")}");
            Console.WriteLine($"  Armure   : {(Armor != null ? Armor.ToString() : "aucune")}");
            Console.WriteLine($"  Bâton    : {(MagicStaff != null ? MagicStaff.ToString() : "aucun")}");
        }

        public override string ToString()
        {
            return $"[{Race}] " + base.ToString() +
                   $" | Force : {TotalStrength}, Endurance : {TotalStamina}" +
                   $" | Or: {Gold}, Cuir: {Leather}";
        }
    }
}
