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
    public string description;

    public Minion(string _name, Rarity _rarity, int _mana, int _attack, int _health, Position _position, string description)
    {
        this.name = _name;
        this.rarity = _rarity;
        this.mana = _mana;
        this.attack = _attack;
        this.health = _health;
        this.position = _position;
        this.description = description;
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
    public string description;

    public StandartCard(string _name, Rarity _rarity, int _attack, int _health, int _mana, string description)
    {
        this.name = _name;
        this.rarity = _rarity;
        this.mana = _mana;
        this.attack = _attack;
        this.health = _health;
        this.description = description;
    }
}

[System.Serializable]
public class Spell
{
    public string name;
    public Rarity rarity;
    public int mana;
    public string description;

    public Spell(string _name, Rarity _rarity, int _mana, string description)
    {
        this.name = _name;
        this.rarity = _rarity;
        this.mana = _mana;
        this.description = description;
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
            new Minion("Centaur Archer", Rarity.Common, 3, 3, 2, Position.Backline, "Can attack immediately. Deals 3 damage."),
            new Minion("Minotaur Warrior", Rarity.Common, 4, 5, 4, Position.Frontline, "Can attack the turn after it is placed. Deals 5 damage."),
            new Minion("Siren", Rarity.Rare, 5, 3, 4, Position.Backline, "Targets a weak enemy minion and damages the opponent. Deals 4 damage once."),
            new Minion("Nemean Lion", Rarity.Epic, 6, 4, 7, Position.Frontline, "Takes and deals only 1 damage in fights."),
            new Minion("Hydra", Rarity.Epic, 7, 5, 8, Position.Frontline, "Also damages the minions next to its target."),
            new Minion("Pegasus Rider", Rarity.Rare, 4, 3, 3, Position.Versatile, "Can attack immediately. Ignores the first hit it takes."),
            new Minion("Greek Hoplite", Rarity.Common, 2, 2, 3, Position.Frontline, "Can attack immediately. Deals 3 damage."),
            new Minion("Gorgon", Rarity.Epic, 8, 4, 5, Position.Backline, "Freezes all enemies for one turn."),
            new Minion("Chimera", Rarity.Rare, 5, 6, 5, Position.Frontline, "Damages all enemies for 2 when placed. Can’t attack again."),
            new Minion("Athena", Rarity.Legendary, 9, 6, 6, Position.Backline, "Fills the front row with Hoplites."),
            new Minion("Heracles", Rarity.Legendary, 8, 4, 4, Position.Frontline, "Gets stronger (+2 damage/health) for each monster killed before it’s placed."),
            new Minion("Stormcaller", Rarity.Common, 3, 2, 4, Position.Backline, "Increases spell power by +1 while on the field."),
            new Minion("Odyssean Navigator", Rarity.Common, 4, 3, 3, Position.Backline, "Draws a card when placed. If it’s a spell, costs 1 less. Safe from attacks on its first turn."),
            new Minion("Oracle's Emissary", Rarity.Rare, 3, 2, 4, Position.Backline, "Reveals a spell next turn. Heals Zeus for 4 if the spell costs 4 or more."),
            new Minion("Lightning Forger", Rarity.Rare, 5, 3, 3, Position.Backline, "Adds +3 power to Zeus when placed.")
            

        };

        spells = new List<Spell>
        {
            new Spell("Lightning Bolt", Rarity.Common, 1, "Deal damage to a minion or hero."),
            new Spell("Lightning Storm", Rarity.Rare, 4, "Deal 2 or 3 damage to all enemy minions."),
            new Spell("Olympian Favor", Rarity.Common, 2, "Give a minion +2 health and +2 damage."),
            new Spell("Aegis Shield", Rarity.Epic, 3, "Protect a minion from the first damage each turn."),
            new Spell("Golden Fleece", Rarity.Common, 2, "Fully heal a minion (doesn't increase max health)."),
            new Spell("Labyrinth Maze", Rarity.Common, 5, "Send all enemy front row minions back to their hand. Extra cards are destroyed if their hand is full."),
            new Spell("Divine Ascension", Rarity.Common, 4, "Choose a minion. If it survives, its health doubles (new max health if it exceeds the original).")
            
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
        attackValue = 2f;
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
            new Minion("Mongol Messenger", Rarity.Common, 2, 2, 1, Position.Frontline, "Draw a card when played."),
            new Minion("Khan’s Envoy", Rarity.Rare, 3, 2, 4, Position.Backline, "50% chance to redirect attacks to a random character."),
            new Minion("Mongol Archer", Rarity.Common, 4, 3, 3, Position.Frontline, "Gets +2 attack if placed on the far-left or far-right."),
            new Minion("Steppe Warlord", Rarity.Rare, 2, 2, 2, Position.Frontline, "Your units with Skirmisher gain +2 attack and +1 health."),
            new Minion("Nomadic Scout", Rarity.Common, 5, 4, 5, Position.Frontline, "Reveals enemy secrets when played."),
            new Minion("Keshik Cavalry", Rarity.Rare, 4, 3, 3, Position.Backline, "Can attack the turn it is played. When it dies, summons a 2/2 Keshik on foot."),
            new Minion("Mongol Shaman", Rarity.Common, 2, 1, 3, Position.Backline, "Fully heals a friendly minion when played."),
            new Minion("Eagle Hunter", Rarity.Rare, 4, 3, 4, Position.Backline, "Summons a 2/1 Eagle when played."),
            new Minion("Yurt Builder", Rarity.Common, 5, 3, 4, Position.Backline, "Gain 1 Mana this turn when played. Gain 1 Armor each time your hero power is used."),
            new Minion("Mongol Lancer", Rarity.Rare, 6, 5, 5, Position.Frontline, "Can attack the turn it is played. If it kills a frontline minion, its damage hits the backline."),
            new Minion("Horse Breeder", Rarity.Common, 7, 4, 4, Position.Frontline, "Summons a 2/2 Steppe Horse when you use your hero power."),
            new Minion("Flaming Camel", Rarity.Rare, 3, 3, 2, Position.Backline, "When it dies, deals 2 damage to all enemy frontline minions."),
            new Minion("Kublai Khan", Rarity.Legendary, 7, 7, 7, Position.Frontline, "At the end of your turn, gives your Skirmisher units +2 attack and +2 health."),
            new Minion("General Subutai", Rarity.Legendary, 8, 5, 5, Position.Frontline, "Can attack twice per turn. Does not take damage when attacking."),
            new Minion("Marco Polo", Rarity.Legendary, 6, 4, 6, Position.Backline, "Lets you pick one of three cards from your deck to add to your hand when played.")
            
        };

        spells = new List<Spell>
        {
            new Spell("Horseback Archery", Rarity.Common, 2, "Give a Skirmisher minion +2 Attack."),
            new Spell("Nomadic Tactics", Rarity.Rare, 3, "When an enemy minion attacks, the minions on its left and right also attack the attacking minion."),
            new Spell("Ger Defense", Rarity.Common, 2, "Give +2 Health to a chosen unit and its neighbors until your next turn."),
            new Spell("Steppe Ambush", Rarity.Rare, 3, "When your opponent casts a spell, summon a 3/2 Horse Archer."),
            new Spell("Mongol Fury", Rarity.Common, 4, "Give all your minions +2 Attack this turn."),
            new Spell("Around the Great Wall", Rarity.Epic, 5, "Your minions can attack enemy minions on their backline even if there are minions in front of them."),
            new Spell("Eternal Steppe’s Whisper", Rarity.Rare, 4, "Choose a minion. It can't be killed until your next turn."),
            new Spell("God’s Bane", Rarity.Epic, 3, "Opponent's spells cost +2 Mana next turn.")
            
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
            new Minion("Da Vinci's Glider", Rarity.Common, 1, 1, 1, Position.Backline, "Reveal hidden units."),
            new Minion("Automaton Apprentice", Rarity.Common, 2, 1, 3, Position.Backline, "Whenever you play a spell, this minion gains +1 Health."),
            new Minion("Automaton Duelist", Rarity.Rare, 3, 3, 2, Position.Frontline, "When this minion attacks, it gains +1 Attack."),
            new Minion("Gyrocopter", Rarity.Common, 2, 1, 2, Position.Backline, "Keeps Stealth after attacking."),
            new Minion("Mechanical Lion", Rarity.Rare, 4, 3, 3, Position.Frontline, "If you control another Mech, gain +1/+2."),
            new Minion("Codex Guardian", Rarity.Rare, 3, 2, 2, Position.Backline, "When your hero draws a card, heal your hero for 2 Health."),
            new Minion("Mirror Shield Automaton", Rarity.Epic, 4, 3, 4, Position.Frontline, "When an enemy minion attacks, deal 1 damage to it."),
            new Minion("Grand Cannon", Rarity.Epic, 5, 4, 3, Position.Backline, "At the end of your turn, deal 2 damage to a random enemy minion."),
            new Minion("Tank of the Renaissance", Rarity.Legendary, 7, 5, 8, Position.Frontline, "Can only take 1 damage at a time."),
            new Minion("Anatomist of the Unknown", Rarity.Epic, 4, 3, 4, Position.Backline, "When you play a spell, give a random friendly minion +2/+2."),
            new Minion("Organ Gun", Rarity.Common, 3, 0, 5, Position.Backline, "After using your hero power, deal 3 damage to random enemy minions."),
            new Minion("Piscean Diver", Rarity.Common, 3, 2, 3, Position.Backline, "Can target cards with minions in front."),
            new Minion("Da Vinci's Helix Engineer", Rarity.Rare, 4, 3, 3, Position.Backline, "Draw a card, if it's a minion, reduce its cost by 2."),
            new Minion("Vitruvian Firstborn", Rarity.Epic, 6, 4, 5, Position.Frontline, "Reduce the cost of cards in your deck by 1."),
            new Minion("Eques Automaton", Rarity.Legendary, 8, 6, 6, Position.Frontline, "At the end of your turn, restore it to full Health (buffed max health if any).")
            
        };

        spells = new List<Spell>
        {
            new Spell("Tome of Confusion", Rarity.Common, 2, "Confuses an enemy minion, causing it to attack a random character."),
            new Spell("Artistic Inspiration", Rarity.Rare, 4, "Gives your minions Deathrattle: Restore your hero power upon death."),
            new Spell("Da Vinci’s Blueprint", Rarity.Common, 3, "Draw a minion from your deck, if it's a Mech, give it +3/+2."),
            new Spell("Anatomical Insight", Rarity.Rare, 3, "Double the damage dealt by a target enemy unit this turn."),
            new Spell("Symmetrical Strategy", Rarity.Epic, 6, "Reset enemy stats by removing all buffs from enemy minions."),
            new Spell("Tabula Aeterna", Rarity.Epic, 6, "Return an enemy minion to your hand, reducing its cost by 1."),
            new Spell("Mona Lisa’s Smile", Rarity.Rare, 4, "Take control of an enemy minion for this turn.")
            

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
            new Minion("Viking Raider", Rarity.Common, 2, 2, 1, Position.Frontline, "Can attack this turn. Grants 1 mana if it kills an enemy minion."),
            new Minion("Runestone Mystic", Rarity.Common, 3, 2, 4, Position.Backline, "Increases spell damage by 1. Draw a spell card when played."),
            new Minion("Fenrir's Spawn", Rarity.Common, 4, 3, 3, Position.Frontline, "If your opponent has more minions, gain +3/+2."),
            new Minion("Shieldmaiden Defender", Rarity.Common, 2, 2, 2, Position.Frontline, "Gains +1 Health for each enemy frontline minion."),
            new Minion("Draugr Warrior", Rarity.Common, 5, 4, 5, Position.Frontline, "Costs 1 less mana for each card in your opponent’s hand."),
            new Minion("Norn Weaver", Rarity.Rare, 4, 3, 3, Position.Backline, "If you guess the card your opponent will draw, add a copy of it to your hand."),
            new Minion("Skald Bard", Rarity.Common, 2, 1, 3, Position.Backline, "Draw a card when played."),
            new Minion("Valkyrie's Chosen", Rarity.Rare, 4, 3, 4, Position.Backline, "Summons two 1/1 Warrior Spirits."),
            new Minion("Mimir's Seer", Rarity.Rare, 5, 3, 4, Position.Backline, "Draw two cards. Reduces the cost of drawn spells by 1 mana."),
            new Minion("Frost Giant", Rarity.Rare, 6, 5, 5, Position.Frontline, "Freezes an enemy minion upon play."),
            new Minion("Einherjar Caller", Rarity.Rare, 7, 4, 4, Position.Frontline, "Resummons a friendly minion that died this game."),
            new Minion("Dwarven Blacksmith", Rarity.Common, 3, 3, 2, Position.Backline, "At the end of each turn, grants +2 Attack to a friendly minion."),
            new Minion("Naglfar", Rarity.Epic, 7, 7, 7, Position.Frontline, "Costs 3 less mana if 6 or more minions have died this game."),
            new Minion("Heimdallr", Rarity.Legendary, 8, 5, 5, Position.Frontline, "Summons three 1/1 Warriors."),
            new Minion("Brokk and Sindri", Rarity.Legendary, 6, 4, 6, Position.Backline, "Gains 1 charge each turn. At 3 charges, deals 2 damage to enemy frontline and summons Thor."),
            new Minion("Thor", Rarity.Legendary, 8, 8, 8, Position.Frontline, "At the end of each turn, deals 1 damage to all enemies.")
            
        };

        spells = new List<Spell>
        {
            new Spell("Rune Magic", Rarity.Rare, 2, "Choose one: Deal 2 damage, Heal 2 Health, or Draw a card."),
            new Spell("Winter's Chill", Rarity.Rare, 3, "Deal 3 damage to an enemy minion and freeze it and its neighbors, dealing 1 damage to them."),
            new Spell("Gjallarhorn Call", Rarity.Epic, 6, "Summon one of the three great warriors. (Heimdallr reduces the cost to 0 mana if on the field.)"),
            new Spell("Mimir's Wisdom", Rarity.Epic, 4, "Copy three cards from your opponent's deck and add them to your hand."),
            new Spell("Viking Raid", Rarity.Epic, 5, "Summon three 2/2 Vikings with Charge."),
            new Spell("Sleipnir’s Gallop", Rarity.Epic, 4, "Give a friendly minion +3/+3 and Charge. Return it to your hand at the end of the turn."),
            new Spell("The Allfather’s Decree", Rarity.Epic, 5, "Equip a 4/2 Gungnir, which gains 'Immune' when attacking this turn.")
            

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
            new Minion("Mutant Behemoth", Rarity.Rare, 6, 6, 8, Position.Frontline, "Reduce the attack of all enemy frontline minions by 2 for 1 turn."),
            new Minion("Scavenger Raider", Rarity.Common, 3, 3, 2, Position.Frontline, "Give your Hero a 2/2 weapon or draw a card."),
            new Minion("Toxic Stalker", Rarity.Rare, 4, 2, 5, Position.Backline, "Cannot be targeted by spells."),
            new Minion("Wasteland Sniper", Rarity.Rare, 5, 3, 3, Position.Backline, "Deal 2 damage to a selected character."),
            new Minion("Radiated Hulk", Rarity.Epic, 7, 7, 7, Position.Frontline, "If you control another Mutant, deal 1 damage to all characters at the end of the turn."),
            new Minion("Engineer of the Ruins", Rarity.Rare, 4, 2, 4, Position.Backline, "Heal all friendly characters for 2."),
            new Minion("Lone Cyborg", Rarity.Epic, 5, 4, 4, Position.Frontline, "If this is your only frontline minion, gain +3/+3."),
            new Minion("Desert Nomad", Rarity.Common, 2, 2, 3, Position.Frontline, ""),
            new Minion("Rogue AI Drone", Rarity.Rare, 3, 1, 1, Position.Backline, "Has immunity to damage while attacking. Return this card to your hand when it dies."),
            new Minion("Mutant Swarm", Rarity.Common, 6, 1, 1, Position.Frontline, "Summons 5 copies of itself. When it kills an enemy, summon a 2/2 Mutant Newborn."),
            new Minion("Dune Raider", Rarity.Rare, 5, 4, 5, Position.Frontline, "Summon two 1/1 Scavengers when it dies."),
            new Minion("Warlord", Rarity.Epic, 7, 6, 6, Position.Frontline, "Summon three 3/3 Wasteland Warriors when it dies."),
            new Minion("Salvage Colossus", Rarity.Legendary, 8, 8, 8, Position.Frontline, "Equip a 5/2 Salvaged Weapon when it dies."),
            new Minion("Claire", Rarity.Legendary, 10, 8, 8, Position.Frontline, "Attack all enemy minions. Deals 2 damage to itself after attacking. Deal 5 damage to your Hero when it dies.")
            
        };

        spells = new List<Spell>
        {
            new Spell("Radioactive Fallout", Rarity.Common, 3, "Shuffles all enemy minions and changes their positions."),
            new Spell("Scrap Shield", Rarity.Rare, 2, "Increase the maximum health of a friendly minion by 3."),
            new Spell("Shockwave Impulse", Rarity.Common, 4, "Prevents enemy frontline from attacking for one turn. Deals 2 damage to all mechanical characters."),
            new Spell("Survival Instincts", Rarity.Rare, 3, "Makes your Hero immune to damage for the next turn."),
            new Spell("Mutagenic Mist", Rarity.Epic, 6, "Deals 2 damage to all enemy minions and gives them -1 Attack for one turn."),
            new Spell("Garage Raid", Rarity.Common, 4, "Gives your Hero a random vehicle (Warlord, Dune Raider)."),
            new Spell("Mutated Blood Sample", Rarity.Common, 2, "Deals 1 damage to a minion and gives it +2 Attack. At the start of the turn, it randomly attacks a character."),
            new Spell("X Factor", Rarity.Epic, 5, "Take control of an enemy minion and give it +2/+2. At the end of the turn, it returns to the enemy Hero's hand. Can be used twice.")
            
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
            new Minion("Sandstone Scribe", Rarity.Common, 2, 1, 3, Position.Backline, "Add a 'Scroll of Death' to your hand (2 Mana: Summon 2 1/2 Mummies)."),
            new Minion("Tomb Protector", Rarity.Common, 3, 2, 4, Position.Frontline, "Give all friendly Undead minions +1 Health."),
            new Minion("Necropolis Acolyte", Rarity.Common, 2, 2, 2, Position.Backline, "Heal a character for 2 Health."),
            new Minion("Desert Bowman", Rarity.Common, 3, 3, 2, Position.Backline, "Deal 1 damage to a character."),
            new Minion("Sphinx Riddler", Rarity.Common, 4, 4, 5, Position.Backline, "Reveal 3 cards, draw one if you choose the correct one."),
            new Minion("Osiris' Bannerman", Rarity.Rare, 5, 4, 4, Position.Frontline, "If Osiris is on the board, gain +2/+2."),
            new Minion("Sun Charioteer", Rarity.Common, 4, 3, 2, Position.Frontline, "Deals damage to adjacent units."),
            new Minion("Crypt Warden", Rarity.Rare, 4, 3, 3, Position.Backline, "Summon 3 1/1 Mummies after death."),
            new Minion("Falcon-Eyed Hunter", Rarity.Common, 3, 2, 3, Position.Backline, "Deal 3 damage to a minion in the backline."),
            new Minion("Canopic Preserver", Rarity.Rare, 4, 3, 3, Position.Backline, "Destroy a friendly minion to gain its stats."),
            new Minion("Royal Mummy", Rarity.Rare, 6, 5, 5, Position.Frontline, "Deal 3 damage to all characters after death."),
            new Minion("Temple Guardian", Rarity.Rare, 5, 4, 6, Position.Frontline, "Whenever you summon an Undead, this minion gains +1/+1."),
            new Minion("Chaos Scarab", Rarity.Epic, 7, 6, 6, Position.Frontline, "Cannot take damage while attacking."),
            new Minion("Bata", Rarity.Legendary, 8, 5, 5, Position.Frontline, "Take control of an enemy Beast. Summon a copy of that Beast after death."),
            new Minion("Osiris", Rarity.Legendary, 10, 20, 20, Position.Frontline, "Shuffle Osiris' 5 body parts into your opponent's deck. When all are drawn, resurrect a 20/20 Osiris.")
            };


        spells = new List<Spell>
        {
            new Spell("Book of the Dead", Rarity.Rare, 6, "Revive the 5 friendly minions that have died this game."),
            new Spell("Sun Disk Radiance", Rarity.Epic, 3, "Give a friendly minion +2/+2. Return this spell to your hand after use."),
            new Spell("Plague of Locusts", Rarity.Epic, 5, "Deal 1 damage to all enemy minions. Summon a 1/1 Locust for each minion that dies."),
            new Spell("River's Blessing", Rarity.Rare, 4, "Heal 10 HP randomly distributed among all friendly characters."),
            new Spell("Pyramid's Might", Rarity.Epic, 7, "Give a friendly minion +4/+4 and give +1/+1 to its adjacent minions."),
            new Spell("Scales of Anubis", Rarity.Epic, 4, "Judge a minion: Destroy it or return it to your opponent's hand."),
            new Spell("Gates of Duat", Rarity.Epic, 8, "Choose one: Freeze all enemy minions for one turn or summon six 2/2 Mummies in the frontline.")
            
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
            new StandartCard("Siege Master Urban", Rarity.Legendary, 5, 8, 8, "Summon a 2/2 Siege Engine with Charge at the start of your turn."),
            new StandartCard("Sphinx of the Sands", Rarity.Legendary, 6, 9, 7, "Show 3 cards to your opponent. If they can't pick the correct one, you take no damage this turn."),
            new StandartCard("Wasteland Giant", Rarity.Legendary, 8, 8, 9, "Whenever this minion takes damage, deal 3 damage to all enemies."),
            new StandartCard("Chronomancer Cleopatra", Rarity.Legendary, 5, 7, 8, "At the end of your turn, add a random spell you've cast to your hand."),
            new StandartCard("Frost Wyrm Fafnir", Rarity.Legendary, 8, 10, 10, "Freeze any minion damaged by this. Summon two 4/5 Frost Drakes when it dies."),
            new StandartCard("Echo of Tomorrow", Rarity.Legendary, 6, 6, 7, "Copy the last spell you cast. It costs 3 less."),
            
            new StandartCard("Templar Knight", Rarity.Epic, 4, 7, 6, "Has Divine Shield."),
            new StandartCard("Cerberus Spawn", Rarity.Epic, 5, 5, 5, "Summon two 1/1 Hellhounds if your opponent has more minions."),
            new StandartCard("Scrapyard Engineer", Rarity.Epic, 3, 5, 4, "Give a random friendly minion +2/+2 at the end of your turn."),
            new StandartCard("Arcane Scholar", Rarity.Epic, 3, 6, 5, "Gain Spell Damage +2. Discover a spell."),
            new StandartCard("Enchanted Golem", Rarity.Epic, 6, 6, 6, "Can't be targeted by spells or hero powers."),
            new StandartCard("Rebel Outcast", Rarity.Epic, 4, 4, 4, "Deal damage equal to this minion's Attack to an enemy minion."),
            new StandartCard("Desert Warlock", Rarity.Epic, 5, 5, 7, "Summon two 2/2 Sand Elementals with 'Your spells cost (1) less.'"),
            new StandartCard("Naiad Protector", Rarity.Epic, 3, 6, 4, "Gain +1 Health whenever you cast a spell."),
            new StandartCard("Ruined City Scout", Rarity.Epic, 2, 4, 3, "Look at the top three cards of your deck. Draw one and shuffle the rest."),
            new StandartCard("Gladiator Champion", Rarity.Epic, 7, 5, 8, "Gain +1/+1 for each enemy minion."),



            new StandartCard("Horse Archer", Rarity.Rare, 3, 3, 2, "Cannot attack heroes."),
            new StandartCard("Forest Nymph", Rarity.Rare, 4, 2, 4, "Restore 2 Health to all friendly minions at the end of your turn."),
            new StandartCard("Urban Ranger", Rarity.Rare, 5, 4, 3, "Deal 2 damage to two random enemy minions."),
            new StandartCard("Byzantine Fire Slinger", Rarity.Rare, 4, 3, 3, "Deal 1 damage to all enemy minions."),
            new StandartCard("Labyrinth Guardian", Rarity.Rare, 6, 5, 6, "Summon a 3/3 Minotaur when it dies."),
            new StandartCard("Toxic Rainmaker", Rarity.Rare, 3, 2, 4, "Give all enemy minions -1 Attack until your next turn."),
            new StandartCard("Crusader's Bishop", Rarity.Rare, 5, 4, 5, "Your spells heal your hero for the damage they deal."),
            new StandartCard("Shadow Assassin", Rarity.Rare, 4, 3, 3, "Return this minion to your hand when it dies."),
            new StandartCard("Scavenger's Daughter", Rarity.Rare, 2, 2, 2, "If you have a weapon equipped, draw a card."),
            new StandartCard("Viking Shield-Maiden", Rarity.Rare, 3, 3, 4, "If you control another Viking, gain +1/+1."),
            new StandartCard("Minotaur Labyrinth Keeper", Rarity.Rare, 5, 4, 6, "Gain +1 Attack whenever this minion takes damage."),
            new StandartCard("Radiated Marauder", Rarity.Rare, 4, 5, 4, "Deal 2 damage to your hero when this minion dies."),
            new StandartCard("Spartan Hoplite", Rarity.Rare, 3, 2, 5, "Adjacent minions gain +1 Attack whenever this minion is attacked."),
            new StandartCard("Minor Oracle of Delphi", Rarity.Rare, 6, 4, 4, "Guess if the next card is a minion or spell. If correct, draw it."),
            new StandartCard("Rogue Mech-Pilot", Rarity.Rare, 7, 6, 5, "Summon two 1/1 Defective Drones."),



            new StandartCard("Legionnaire", Rarity.Common, 2, 3, 2, ""),
            new StandartCard("Merfolk Scout", Rarity.Common, 1, 2, 1, "Look at the top card of your deck."),
            new StandartCard("Rubble Raider", Rarity.Common, 3, 3, 3, "Attack once per turn."),
            new StandartCard("Saxon Bowman", Rarity.Common, 3, 2, 3, "Can attack one additional time each turn."),
            new StandartCard("Desert Conjurer", Rarity.Common, 4, 3, 4, "Add a 'Sandstorm' spell to your hand (Deals 1 damage to all minions)."),
            new StandartCard("Norse Axeman", Rarity.Common, 2, 2, 2, "Deal 1 damage to all enemy minions when it dies."),
            new StandartCard("Jade Monk", Rarity.Common, 2, 4, 3, "Gains +1 Attack when healed."),
            new StandartCard("Plague Carrier", Rarity.Common, 4, 3, 4, "Give an enemy minion -2 Attack until the end of the next turn."),
            new StandartCard("Gaelic Warrior", Rarity.Common, 5, 4, 5, "Dies at the end of the turn."),
            new StandartCard("Oasis Guardian", Rarity.Common, 3, 6, 4, "Restore 2 Health to your hero."),
            new StandartCard("Street Thug", Rarity.Common, 2, 3, 3, "Gains +2 Attack for each weapon card in your hand."),
            new StandartCard("Catacomb Guardian", Rarity.Common, 1, 4, 2, "Gains +1 Health whenever it takes damage."),
            new StandartCard("Sewer Rat", Rarity.Common, 1, 1, 1, "Summon a 1/1 Rat when it dies."),
            new StandartCard("Battle Mage", Rarity.Common, 2, 3, 3, "Spell Damage +1."),
            new StandartCard("Roving Merchant", Rarity.Common, 4, 4, 4, "Draw a card if you control at least 3 other minions."),
            new StandartCard("Dwarven Miner", Rarity.Common, 2, 3, 2, "Equip a 2/1 Dwarven Pickaxe."),
            new StandartCard("Elven Tracker", Rarity.Common, 3, 2, 3, "Has a secret ability."),
            new StandartCard("Berserker Thrall", Rarity.Common, 5, 5, 5, "Attacks a random enemy at the start of your turn."),
            new StandartCard("Ancient Librarian", Rarity.Common, 3, 5, 4, "Silence a minion (removes its buffs and abilities)."),
            new StandartCard("Nomadic Hunter", Rarity.Common, 2, 4, 3, "Can attack immediately if a spell was played this turn."),
            new StandartCard("Apprentice Sorcerer", Rarity.Common, 2, 2, 2, "Discover a random spell costing 3 or less from your deck."),
            new StandartCard("Scrap Collector", Rarity.Common, 1, 5, 3, "Give a random friendly minion +1/+1 at the end of your turn."),
            new StandartCard("Minor Glacial Elemental", Rarity.Common, 1, 3, 2, "Freezes any minion it damages."),
            new StandartCard("Raiding Party", Rarity.Common, 5, 5, 5, "Summon two 1/1 Pirates with Attack."),
            new StandartCard("Mystic Archer", Rarity.Common, 3, 1, 3, "Deal 2 damage."),
            new StandartCard("Wall Builder", Rarity.Common, 0, 4, 2, "Gains +1 Health at the start of your turn."),
            new StandartCard("Tavern Brawler", Rarity.Common, 4, 4, 4, "Gains +1 Health at the end of each turn."),
            new StandartCard("Dune Scout", Rarity.Common, 2, 1, 1, "Can attack immediately if your hero has a weapon."),
            new StandartCard("Canal Lurker", Rarity.Common, 3, 3, 3, ""),
            new StandartCard("Wandering Healer", Rarity.Common, 3, 3, 4, "Restore 3 Health to all friendly characters."),
            new StandartCard("Frontline Militia", Rarity.Common, 2, 2, 2, "Gains +1 Health for each other friendly minion."),
            new StandartCard("Pyromaniac Wizard", Rarity.Common, 3, 3, 3, "Deal 1 damage to all enemy minions."),
            new StandartCard("Desert Nomad", Rarity.Common, 5, 5, 5, "")
            

        };
    }

}






