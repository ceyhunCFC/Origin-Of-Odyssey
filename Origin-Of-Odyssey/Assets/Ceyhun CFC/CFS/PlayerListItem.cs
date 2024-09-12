using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
	[SerializeField] Text text;
	Player player;

    public string[] playerNames = new string[]
    {
        "ShadowHunter", "DragonSlayer", "StormBreaker", "NightOwl", "PhoenixFire",
        "SteelTitan", "SilverFang", "MysticMage", "DarkKnight", "LoneWolf",
        "BlazeWarrior", "ThunderStrike", "FrostDragon", "StormCaller", "IronClad",
        "GhostRider", "WildCat", "VortexMaster", "EagleEye", "RedPhoenix",
        "FallenAngel", "ThunderBolt", "SlyFox", "LightningBolt", "WarriorKing",
        "RogueShadow", "DemonHunter", "StarGazer", "WraithLord", "VengefulSpirit",
        "CelestialKnight", "ShadowBlade", "RagingInferno", "MysticKnight", "FrostBite",
        "EbonDragon", "SpectralWarrior", "SunSeeker", "IronFist", "SkyWalker",
        "DragonHeart", "SilentStorm", "VoidCaster", "SteelPhoenix", "BloodRaven",
        "StormHunter", "ArcaneWarrior", "GrimReaper", "DuskKnight", "ViperStrike",
        "EclipseMage", "IronGiant", "SpectralKnight", "ShadowRanger", "StormWarden",
        "LunarEclipse", "InfernoBeast", "MysticSorcerer", "PhantomBlade", "WraithHunter",
        "DemonLord", "CelestialMage", "StarChaser", "FrostGuardian", "ShadowStalker",
        "InfernalKnight", "VoidKnight", "ThunderGuard", "SpectralRanger", "MoonlightWarrior",
        "DragonRider", "BloodKnight", "StormFury", "ShadowAssassin", "CelestialFury",
        "NightmareMage", "StormCaster", "RogueWarrior", "EclipseKnight", "DuskMage",
        "FallenWarrior", "MysticRanger", "SteelSorcerer", "ThunderMage", "IronWarden",
        "PhantomRanger", "InfernalSorcerer", "FrostWarden", "WraithSorcerer", "DragonGuard",
        "LunarMage", "ShadowLord", "StormRider", "CelestialSorcerer", "IronRanger",
        "EbonWarrior", "DemonMage", "BloodSorcerer", "SpectralSorcerer", "VoidWarrior",
        "SunGuard", "EclipseRanger", "MysticLord", "StormKnight", "LunarKnight",
        "PhantomWarrior", "CelestialKnight", "WraithWarden", "InfernoMage", "FrostRanger"
    };

    private void Awake()
    {
        text.text = playerNames[Random.Range(0, playerNames.Length)];
    }

    public void SetUp(Player _player)
	{
		player = _player;
		text.text = _player.NickName;
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (player == otherPlayer)
		{
			Destroy(gameObject);
		}
	}

	public override void OnLeftRoom()
	{
		Destroy(gameObject);
	}


}