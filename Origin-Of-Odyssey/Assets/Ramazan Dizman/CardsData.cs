using System.Collections.Generic;

[System.Serializable]
public abstract class CardStats {
    public string cardName;
    public float hpValue;
    public float dodgeValue;
    public float attackValue;
    public float defenceValue;
    public float criticalValue;
    public int duelsWon;
    public string question;
    public float winRatio;
    public int duelsFought;
    public int hasBeenBought;
    public string description;
    public float manaValue;
    public float time;
    
    public string GetName()
    {
        return cardName;
    }
}

[System.Serializable]
public class HeroPower
{
    public string name;
    public int mana;
    public int damage;

    public HeroPower(string _name, int _mana, int _damage)
    {
        this.name = _name;
        this.mana = _mana;
        this.damage = _damage;
    }
}

[System.Serializable]
public class Minion
{
    public string name;
    public Rarity rarity;
    public int mana;
    public int attack;
    public int health;
    public Position position;

    public Minion(string _name, Rarity _rarity, int _mana, int _attack, int _health, Position _position)
    {
        this.name = _name;
        this.rarity = _rarity;
        this.mana = _mana;
        this.attack = _attack;
        this.health = _health;
        this.position = _position;
    }
}

[System.Serializable]
public class StandartCard
{
    public string name;
    public Rarity rarity;
    public int mana;
    public int attack;
    public int health;

    public StandartCard(string _name, Rarity _rarity, int _attack, int _health, int _mana)
    {
        this.name = _name;
        this.rarity = _rarity;
        this.mana = _mana;
        this.attack = _attack;
        this.health = _health;
    }
}

[System.Serializable]
public class Spell
{
    public string name;
    public Rarity rarity;
    public int mana;

    public Spell(string _name, Rarity _rarity, int _mana)
    {
        this.name = _name;
        this.rarity = _rarity;
        this.mana = _mana;
    }
}


public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public enum Position
{
    Backline,
    Frontline,
    Versatile
}


[System.Serializable]
public class ZeusCard : CardStats
{
    public List<Minion> minions;
    public List<Spell> spells;
    public HeroPower heroPower;

    public ZeusCard()
    {
        cardName = "Zeus";
        hpValue = 15f;
        dodgeValue = 4f;
        attackValue = 1f;
        defenceValue = 6f;
        criticalValue = 5f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;

        minions = new List<Minion>
        {
            new Minion("Centaur Archer", Rarity.Common, 3, 3, 2, Position.Backline),
            new Minion("Minotaur Warrior", Rarity.Common, 4, 5, 4, Position.Frontline),
            new Minion("Siren", Rarity.Rare, 5, 3, 4, Position.Backline),
            new Minion("Nemean Lion", Rarity.Epic, 6, 4, 7, Position.Frontline),
            new Minion("Hydra", Rarity.Epic, 7, 5, 8, Position.Frontline),
            new Minion("Pegasus Rider", Rarity.Rare, 4, 3, 3, Position.Versatile),
            new Minion("Greek Hoplite", Rarity.Common, 2, 2, 3, Position.Frontline),
            new Minion("Gorgon", Rarity.Epic, 8, 4, 5, Position.Backline),
            new Minion("Chimera", Rarity.Rare, 5, 6, 5, Position.Frontline),
            new Minion("Athena", Rarity.Legendary, 9, 6, 6, Position.Backline),
            new Minion("Heracles", Rarity.Legendary, 8, 4, 4, Position.Frontline),
            new Minion("Stormcaller", Rarity.Common, 3, 2, 4, Position.Backline),
            new Minion("Odyssean Navigator", Rarity.Common, 4, 3, 3, Position.Backline),
            new Minion("Oracle's Emissary", Rarity.Rare, 3, 2, 4, Position.Backline),
            new Minion("Lightning Forger", Rarity.Rare, 5, 3, 3, Position.Backline)
        };

        spells = new List<Spell>
        {
            new Spell("Lightning Bolt", Rarity.Common, 1),
            new Spell("Lightning Storm", Rarity.Rare, 4),
            new Spell("Olympian Favor", Rarity.Common, 2),
            new Spell("Aegis Shield", Rarity.Epic, 3),
            new Spell("Golden Fleece", Rarity.Common, 2),
            new Spell("Labyrinth Maze", Rarity.Common, 5),
            new Spell("Divine Ascention", Rarity.Common, 4)
        };

        heroPower = new HeroPower("Lightning Strike", 2, 1);
    }
}

[System.Serializable]
public class GenghisCard : CardStats
{
    public List<Minion> minions;
    public List<Spell> spells;
    public HeroPower heroPower;
    public GenghisCard()
    {
        cardName = "Genghis";
        hpValue = 11f;
        dodgeValue = 4f;
        attackValue = 6f;
        defenceValue = 5f;
        criticalValue = 7f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;

        minions = new List<Minion>
        {
            new Minion("Mongol Messenger", Rarity.Common, 2, 2, 1, Position.Frontline),
            new Minion("Khan’s Envoy", Rarity.Rare, 3, 2, 4, Position.Backline),
            new Minion("Mongol Archer", Rarity.Common, 4, 3, 3, Position.Frontline),
            new Minion("Steppe Warlord", Rarity.Rare, 2, 2, 2, Position.Frontline),
            new Minion("Nomadic Scout", Rarity.Common, 5, 4, 5, Position.Frontline),
            new Minion("Keshik Cavalry", Rarity.Rare, 4, 3, 3, Position.Backline),
            new Minion("Mongol Shaman", Rarity.Common, 2, 1, 3, Position.Backline),
            new Minion("Eagle Hunter", Rarity.Rare, 4, 3, 4, Position.Backline),
            new Minion("Yurt Builder", Rarity.Common, 5, 3, 4, Position.Backline),
            new Minion("Mongol Lancer", Rarity.Rare, 6, 5, 5, Position.Frontline),
            new Minion("Horse Breeder", Rarity.Common, 7, 4, 4, Position.Frontline),
            new Minion("Flaming Camel", Rarity.Rare, 3, 3, 2, Position.Backline),
            new Minion("Kublai Khan", Rarity.Legendary, 7, 7, 7, Position.Frontline),
            new Minion("General Subutai", Rarity.Legendary, 8, 5, 5, Position.Frontline),
            new Minion("Marco Polo", Rarity.Legendary, 6, 4, 6, Position.Backline)
        };

        spells = new List<Spell>
        {
            new Spell("Horseback Archery", Rarity.Common, 2),
            new Spell("Nomadic Tactics", Rarity.Rare, 3),
            new Spell("Ger Defense", Rarity.Common, 2),
            new Spell("Steppe Ambush", Rarity.Rare, 3),
            new Spell("Mongol Fury", Rarity.Common, 4),
            new Spell("Around the Great Wall", Rarity.Epic, 5),
            new Spell("Eternal Steppe’s Whisper", Rarity.Rare, 4),
            new Spell("God’s Bane", Rarity.Epic, 3)
        };

        heroPower = new HeroPower("Mongol Messenger", 2, 2);
    }
}

[System.Serializable]
public class LeonardoCard : CardStats
{
    public List<Minion> minions;
    public List<Spell> spells;
    public HeroPower heroPower;
    public LeonardoCard()
    {
        cardName = "Leonardo Da Vinci";
        hpValue = 12f;
        dodgeValue = 4f;
        attackValue = 5f;
        defenceValue = 6f;
        criticalValue = 6f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;

        minions = new List<Minion>
        {
            new Minion("Da Vinci's Glider", Rarity.Common, 1, 1, 1, Position.Backline),
            new Minion("Automaton Apprentice", Rarity.Common, 2, 1, 3, Position.Backline),
            new Minion("Automaton Duelist", Rarity.Rare, 3, 3, 2, Position.Frontline),
            new Minion("Gyrocopter", Rarity.Common, 2, 1, 2, Position.Backline),
            new Minion("Mechanical Lion", Rarity.Rare, 4, 3, 3, Position.Frontline),
            new Minion("Codex Guardian", Rarity.Rare, 3, 2, 2, Position.Backline),
            new Minion("Mirror Shield Automaton", Rarity.Epic, 4, 3, 4, Position.Frontline),
            new Minion("Grand Cannon", Rarity.Epic, 5, 4, 3, Position.Backline),
            new Minion("Tank of the Renaissance", Rarity.Legendary, 7, 5, 8, Position.Frontline),
            new Minion("Anatomist of the Unknown", Rarity.Epic, 4, 3, 4, Position.Backline),
            new Minion("Organ Gun", Rarity.Common, 3, 0, 5, Position.Backline),
            new Minion("Piscean Diver", Rarity.Common, 3, 2, 3, Position.Backline),
            new Minion("Da Vinci's Helix Engineer", Rarity.Rare, 4, 3, 3, Position.Backline),
            new Minion("Vitruvian Firstborn", Rarity.Epic, 6, 4, 5, Position.Frontline),
            new Minion("Eques Automaton", Rarity.Legendary, 8, 6, 6, Position.Frontline)
        };

        spells = new List<Spell>
        {
            new Spell("Tome of Confusion", Rarity.Common, 2),
            new Spell("Artistic Inspiration", Rarity.Rare, 4),
            new Spell("Da Vinci’s Blueprint", Rarity.Common, 3),
            new Spell("Anatomical Insight", Rarity.Rare, 3),
            new Spell("Symmetrical Strategy", Rarity.Epic, 6),
            new Spell("Tabula Aeterna", Rarity.Epic, 6),
            new Spell("Mona Lisa’s Smile", Rarity.Rare, 4)
        };

        heroPower = new HeroPower("Tome of Confusion", 2, 2);
    }
}
[System.Serializable]
public class OdinCard : CardStats
{
    public List<Minion> minions;
    public List<Spell> spells;
    public HeroPower heroPower;
    public OdinCard()
    {
        cardName = "Odin";
        hpValue = 14f;
        dodgeValue = 4f;
        attackValue = 6f;
        defenceValue = 7f;
        criticalValue = 5f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;

        minions = new List<Minion>
        {
            new Minion("Viking Raider", Rarity.Common, 2, 2, 1, Position.Frontline),
            new Minion("Runestone Mystic", Rarity.Common, 3, 2, 4, Position.Backline),
            new Minion("Fenrir's Spawn", Rarity.Common, 4, 3, 3, Position.Frontline),
            new Minion("Shieldmaiden Defender", Rarity.Common, 2, 2, 2, Position.Frontline),
            new Minion("Draugr Warrior", Rarity.Common, 5, 4, 5, Position.Frontline),
            new Minion("Norn Weaver", Rarity.Rare, 4, 3, 3, Position.Backline),
            new Minion("Skald Bard", Rarity.Common, 2, 1, 3, Position.Backline),
            new Minion("Valkyrie's Chosen", Rarity.Rare, 4, 3, 4, Position.Backline),
            new Minion("Mimir's Seer", Rarity.Rare, 5, 3, 4, Position.Backline),
            new Minion("Frost Giant", Rarity.Rare, 6, 5, 5, Position.Frontline),
            new Minion("Einherjar Caller", Rarity.Rare, 7, 4, 4, Position.Frontline),
            new Minion("Dwarven Blacksmith", Rarity.Common, 3, 3, 2, Position.Backline),
            new Minion("Naglfar", Rarity.Epic, 7, 7, 7, Position.Frontline),
            new Minion("Heimdallr", Rarity.Legendary, 8, 5, 5, Position.Frontline),
            new Minion("Brokk and Sindri", Rarity.Legendary, 6, 4, 6, Position.Backline)
        };

        spells = new List<Spell>
        {
            new Spell("Rune Magic", Rarity.Rare, 2),
            new Spell("Winter's Chill", Rarity.Rare, 3),
            new Spell("Gjallarhorn Call", Rarity.Epic, 6),
            new Spell("Mimir's Wisdom", Rarity.Epic, 4),
            new Spell("Viking Raid", Rarity.Epic, 5),
            new Spell("Sleipnir’s Gallop", Rarity.Epic, 4),
            new Spell("The Allfather’s Decree", Rarity.Epic, 5)
        };

        heroPower = new HeroPower("Wisdom of the Allfather", 2, 1);
    }
}
[System.Serializable]
public class DustinCard : CardStats
{
    public List<Minion> minions;
    public List<Spell> spells;
    public HeroPower heroPower;
    public DustinCard()
    {
        cardName = "Dustin";
        hpValue = 11f;
        dodgeValue = 5f;
        attackValue = 5f;
        defenceValue = 7f;
        criticalValue = 6f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;

        minions = new List<Minion>
        {
            new Minion("Mutant Behemoth", Rarity.Rare, 6, 6, 8, Position.Frontline),
            new Minion("Scavenger Raider", Rarity.Common, 3, 3, 2, Position.Frontline),
            new Minion("Toxic Stalker", Rarity.Rare, 4, 2, 5, Position.Backline),
            new Minion("Wasteland Sniper", Rarity.Rare, 5, 3, 3, Position.Backline),
            new Minion("Radiated Hulk", Rarity.Epic, 7, 7, 7, Position.Frontline),
            new Minion("Engineer of the Ruins", Rarity.Rare, 4, 2, 4, Position.Backline),
            new Minion("Lone Cyborg", Rarity.Epic, 5, 4, 4, Position.Frontline),
            new Minion("Desert Nomad", Rarity.Common, 2, 2, 3, Position.Frontline),
            new Minion("Rogue AI Drone", Rarity.Rare, 3, 1, 1, Position.Backline),
            new Minion("Mutant Swarm", Rarity.Common, 6, 1, 1, Position.Frontline),
            new Minion("Dune Raider", Rarity.Rare, 5, 4, 5, Position.Frontline),
            new Minion("Warlord", Rarity.Epic, 7, 6, 6, Position.Frontline),
            new Minion("Salvage Colossus", Rarity.Legendary, 8, 8, 8, Position.Frontline),
            new Minion("Claire", Rarity.Legendary, 10, 8, 8, Position.Frontline)
        };

        spells = new List<Spell>
        {
            new Spell("Radioactive Fallout", Rarity.Common, 3),
            new Spell("Scrap Shield", Rarity.Rare, 2),
            new Spell("Shockwave Impulse", Rarity.Common, 4),
            new Spell("Survival Instincts", Rarity.Rare, 3),
            new Spell("Mutagenic Mist", Rarity.Epic, 6),
            new Spell("Garage Raid", Rarity.Common, 4),
            new Spell("Mutated Blood Sample", Rarity.Common, 2),
            new Spell("X Factor", Rarity.Epic, 5)
        };

        heroPower = new HeroPower("Summon a random mutant", 2, 1);
    }
}
[System.Serializable]
public class AnubisCard : CardStats
{
    public List<Minion> minions;
    public List<Spell> spells;
    public HeroPower heroPower;
    public AnubisCard()
    {
        cardName = "Anubis";
        hpValue = 10f;
        dodgeValue = 3f;
        attackValue = 7f;
        defenceValue = 6f;
        criticalValue = 6f;
        duelsWon = 0;
        question = "";
        winRatio = 0f;
        duelsFought = 0;
        hasBeenBought = 0;
        description = "";
        manaValue = 0f;
        time = 0f;

        minions = new List<Minion>
        {
            new Minion("Sandstone Scribe", Rarity.Common, 2, 1, 3, Position.Backline),
            new Minion("Tomb Protector", Rarity.Common, 3, 2, 4, Position.Frontline),
            new Minion("Necropolis Acolyte", Rarity.Common, 2, 2, 2, Position.Backline),
            new Minion("Desert Bowman", Rarity.Common, 3, 3, 2, Position.Backline),
            new Minion("Sphinx Riddler", Rarity.Common, 4, 4, 5, Position.Backline),
            new Minion("Osiris’ Bannerman", Rarity.Rare, 5, 4, 4, Position.Frontline),
            new Minion("Sun Charioteer", Rarity.Common, 4, 3, 2, Position.Frontline),
            new Minion("Crypt Warden", Rarity.Rare, 4, 3, 3, Position.Backline),
            new Minion("Falcon-Eyed Hunter", Rarity.Common, 3, 2, 3, Position.Backline),
            new Minion("Canopic Preserver", Rarity.Rare, 4, 3, 3, Position.Backline),
            new Minion("Royal Mummy", Rarity.Rare, 6, 5, 5, Position.Frontline),
            new Minion("Temple Guardian", Rarity.Rare, 5, 4, 6, Position.Frontline),
            new Minion("Chaos Scarab", Rarity.Epic, 7, 6, 6, Position.Frontline),
            new Minion("Bata", Rarity.Legendary, 8, 5, 5, Position.Frontline),
            new Minion("Osiris", Rarity.Legendary, 10, 20, 20, Position.Frontline)
        };

        spells = new List<Spell>
        {
            new Spell("Book of the Dead", Rarity.Rare, 6),
            new Spell("Sun Disk Radiance", Rarity.Epic, 3),
            new Spell("Plague of Locusts", Rarity.Epic, 5),
            new Spell("River's Blessing", Rarity.Rare, 4),
            new Spell("Pyramid's Might", Rarity.Epic, 7),
            new Spell("Scales of Anubis", Rarity.Epic, 4),
            new Spell("Gates of Duat", Rarity.Epic, 8)
        };

        heroPower = new HeroPower("Summon a 2/2 mummy", 2, 2);
    }
}

public class StandartCards
{
    public List<StandartCard> standartcards;

    public StandartCards()
    {
        standartcards = new List<StandartCard>
        {
            new StandartCard("Siege Master Urban", Rarity.Legendary, 5, 8, 8),
            new StandartCard("Sphinx of the Sands", Rarity.Legendary, 6, 9, 7),
            new StandartCard("Wasteland Giant", Rarity.Legendary, 8, 8, 9),
            new StandartCard("Chronomancer Cleopatra", Rarity.Legendary, 5, 7, 8),
            new StandartCard("Frost Wyrm Fafnir", Rarity.Legendary, 8, 10, 10),
            new StandartCard("Echo of Tomorrow", Rarity.Legendary, 6, 6, 7),

            new StandartCard("Templar Knight", Rarity.Epic, 4, 7, 6),
            new StandartCard("Cerberus Spawn", Rarity.Epic, 5, 5, 5),
            new StandartCard("Scrapyard Engineer", Rarity.Epic, 3, 5, 4),
            new StandartCard("Arcane Scholar", Rarity.Epic, 3, 6, 5),
            new StandartCard("Enchanted Golem", Rarity.Epic, 6, 6, 6),
            new StandartCard("Rebel Outcast", Rarity.Epic, 4, 4, 4),
            new StandartCard("Desert Warlock", Rarity.Epic, 5, 5, 7),
            new StandartCard("Naiad Protector", Rarity.Epic, 3, 6, 4),
            new StandartCard("Ruined City Scout", Rarity.Epic, 2, 4, 3),
            new StandartCard("Gladiator Champion", Rarity.Epic, 7, 5, 8),

            new StandartCard("Horse Archer", Rarity.Rare, 3, 2, 3),
            new StandartCard("Forest Nymph", Rarity.Rare, 2, 4, 4),
            new StandartCard("Urban Ranger", Rarity.Rare, 4, 3, 5),
            new StandartCard("Byzantine Fire Slinger", Rarity.Rare, 3, 3, 4),
            new StandartCard("Labyrinth Guardian", Rarity.Rare, 5, 6, 6),
            new StandartCard("Toxic Rainmaker", Rarity.Rare, 2, 4, 3),
            new StandartCard("Crusader's Bishop", Rarity.Rare, 4, 5, 5),
            new StandartCard("Shadow Assassin", Rarity.Rare, 3, 3, 4),
            new StandartCard("Scavenger's Daughter", Rarity.Rare, 2, 2, 2),
            new StandartCard("Viking Shield-Maiden", Rarity.Rare, 3, 4, 3),
            new StandartCard("Minotaur Labyrinth Keeper", Rarity.Rare, 4, 6, 5),
            new StandartCard("Radiated Marauder", Rarity.Rare, 5, 4, 4),
            new StandartCard("Spartan Hoplite", Rarity.Rare, 2, 5, 3),
            new StandartCard("Minor Oracle of Delphi", Rarity.Rare, 4, 4, 6),
            new StandartCard("Rogue Mech-Pilot", Rarity.Rare, 6, 5, 7),
            new StandartCard("Knight Errant", Rarity.Rare, 4, 3, 4),
            new StandartCard("Banshee Wailer", Rarity.Rare, 3, 5, 5),
            new StandartCard("Apex Predator", Rarity.Rare, 5, 6, 6),

            new StandartCard("Legionnaire", Rarity.Common, 2, 3, 2),
            new StandartCard("Merfolk Scout", Rarity.Common, 1, 2, 1),
            new StandartCard("Rubble Raider", Rarity.Common, 3, 3, 3),
            new StandartCard("Saxon Bowman", Rarity.Common, 3, 2, 3),
            new StandartCard("Desert Conjurer", Rarity.Common, 3, 4, 4),
            new StandartCard("Norse Axeman", Rarity.Common, 2, 2, 2),
            new StandartCard("Jade Monk", Rarity.Common, 2, 4, 3),
            new StandartCard("Plague Carrier", Rarity.Common, 4, 3, 4),
            new StandartCard("Gaelic Warrior", Rarity.Common, 5, 4, 5),
            new StandartCard("Oasis Guardian", Rarity.Common, 3, 6, 4),
            new StandartCard("Street Thug", Rarity.Common, 3, 3, 3),
            new StandartCard("Catacomb Guardian", Rarity.Common, 1, 4, 2),
            new StandartCard("Sewer Rat", Rarity.Common, 1, 1, 1),
            new StandartCard("Battle Mage", Rarity.Common, 2, 3, 3),
            new StandartCard("Roving Merchant", Rarity.Common, 4, 4, 4),
            new StandartCard("Dwarven Miner", Rarity.Common, 2, 3, 2),
            new StandartCard("Elven Tracker", Rarity.Common, 3, 2, 3),
            new StandartCard("Berserker Thrall", Rarity.Common, 5, 5, 5),
            new StandartCard("Ancient Librarian", Rarity.Common, 3, 5, 4),
            new StandartCard("Nomadic Hunter", Rarity.Common, 2, 4, 3),
            new StandartCard("Apprentice Sorcerer", Rarity.Common, 2, 2, 2),
            new StandartCard("Scrap Collector", Rarity.Common, 1, 5, 3),
            new StandartCard("Minor Glacial Elemental", Rarity.Common, 1, 3, 2),
            new StandartCard("Raiding Raiding Party", Rarity.Common, 5, 5, 5),
            new StandartCard("Mystic Archer", Rarity.Common, 3, 1, 3),
            new StandartCard("Wall Builder", Rarity.Common, 0, 4, 2),
            new StandartCard("Tavern Brawler", Rarity.Common, 4, 4, 4),
            new StandartCard("Dune Scout", Rarity.Common, 2, 1, 1),
            new StandartCard("Canal Lurker", Rarity.Common, 3, 3, 3),
            new StandartCard("Wandering Healer", Rarity.Common, 3, 3, 4),
            new StandartCard("Frontline Militia", Rarity.Common, 2, 2, 2),
            new StandartCard("Pyromaniac Wizard", Rarity.Common, 3, 3, 3),
            new StandartCard("Desert Nomad", Rarity.Common, 5, 5, 5),
        };
    }

}






