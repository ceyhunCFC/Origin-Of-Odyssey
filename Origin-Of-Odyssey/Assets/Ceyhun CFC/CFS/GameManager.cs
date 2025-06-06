using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ender.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [HideInInspector] public static GameManager Instance;
    [HideInInspector] public bool Turn = false; // FALSE IS MASTER TURN - TRUE IS OTHER TURN
    [HideInInspector] public float ManaCount = 1;

    [HideInInspector] public string MasterPlayerName = "";
    [HideInInspector] public string OtherPlayerName = "";

    [HideInInspector] public int MasterHealth = 30;
    [HideInInspector] public int OtherHealth = 30;

    [HideInInspector] public int MasterAttackDamage = 0;
    [HideInInspector] public int OtherAttackDamage = 0;

    [HideInInspector] public string[] MasterDeck;
    [HideInInspector] public string[] OtherDeck;

    [HideInInspector] public string MasterMainCard = "";
    [HideInInspector] public string OtherrMainCard = "";

    [HideInInspector] public int TurnCount = 0;
    public PlayerController PlayerController;


    public Text MasterPlayerNameText;
    public Text OtherPlayerNameText;
    public Text MasterDeckText;
    public Text OtherDeckText;
    public Text WhoTurnText;
    public Text WinningName;

    public GameObject winPanel, losePanel;
    PhotonView PV;

   

    private void Awake()
    {
        /*if (Instance == null)
            Instance = this;
        else
         //   Destroy(gameObject);
        */
      //  DontDestroyOnLoad(gameObject);
        PV = GetComponent<PhotonView>();
        
    }

    public void MasterDamanage(int Damage)
    {
        PV.RPC("RPC_MasterDamanage", RpcTarget.AllBufferedViaServer,Damage);
    }

    [PunRPC]
    async void RPC_MasterDamanage(int Damage)
    {
        if (PlayerController.OwnNameText.text==MasterPlayerName)
            await PlayerController.HeroParticleController.PlayHit();
        if (MasterHealth  > 0)
        {
            MasterHealth -= Damage;
            if (MasterHealth<0)
            {
                MasterHealth = 0;
            }
            PlayerController.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }

        if (MasterHealth > 0)
        {
             return;
        }
        if (PlayerController.OwnNameText.text==MasterPlayerName)
        {
            losePanel.SetActive(true);
            Sprite heroSprite = PlayerController.CompetitorMainCard switch
            {
                "Zeus" => PlayerController.heroIcons[0],
                "Odin" => PlayerController.heroIcons[1],
                "Genghis" => PlayerController.heroIcons[2],
                "Anubis" => PlayerController.heroIcons[3],
                "Dustin" => PlayerController.heroIcons[4],
                "Leonardo Da Vinci" => PlayerController.heroIcons[5],
                _ => PlayerController.CompetitorHeroIcon.sprite
            };
            losePanel.GetComponent<WinLosePanel>().Initialize(PlayerController.OwnNameText.text, PlayerController.OwnMainCard, heroSprite);
        }
        else
        {
            winPanel.SetActive(true);
            Sprite heroSprite = PlayerController.CompetitorMainCard switch
            {
                "Zeus" => PlayerController.heroIcons[0],
                "Odin" => PlayerController.heroIcons[1],
                "Genghis" => PlayerController.heroIcons[2],
                "Anubis" => PlayerController.heroIcons[3],
                "Dustin" => PlayerController.heroIcons[4],
                "Leonardo Da Vinci" => PlayerController.heroIcons[5],
                _ => PlayerController.CompetitorHeroIcon.sprite
            };
            winPanel.GetComponent<WinLosePanel>().Initialize(PlayerController.OwnNameText.text, PlayerController.OwnMainCard, heroSprite);
        }
    }

    public void OtherDamanage(int Damage)
    {
        PV.RPC("RPC_OtherDamanage", RpcTarget.AllBufferedViaServer,Damage);
    }

    [PunRPC]
    async void RPC_OtherDamanage(int Damage)
    {
        if (PlayerController.OwnNameText.text==OtherPlayerName)
            await PlayerController.HeroParticleController.PlayHit();
        if (OtherHealth > 0)
        {
            OtherHealth -= Damage;
            if (OtherHealth<0)
            {
                OtherHealth = 0;
            }
            PlayerController.PV.RPC("RefreshPlayersInformation", RpcTarget.All);
        }

        if (OtherHealth > 0)
        {
             return;
        }
            
        if (PlayerController.OwnNameText.text==OtherPlayerName)
        {
            losePanel.SetActive(true);
            Sprite heroSprite = PlayerController.CompetitorMainCard switch
            {
                "Zeus" => PlayerController.heroIcons[0],
                "Odin" => PlayerController.heroIcons[1],
                "Genghis" => PlayerController.heroIcons[2],
                "Anubis" => PlayerController.heroIcons[3],
                "Dustin" => PlayerController.heroIcons[4],
                "Leonardo Da Vinci" => PlayerController.heroIcons[5],
                _ => PlayerController.CompetitorHeroIcon.sprite
            };
            losePanel.GetComponent<WinLosePanel>().Initialize(PlayerController.OwnNameText.text, PlayerController.OwnMainCard, heroSprite);
        }
        else
        {
            winPanel.SetActive(true);
            Sprite heroSprite = PlayerController.CompetitorMainCard switch
            {
                "Zeus" => PlayerController.heroIcons[0],
                "Odin" => PlayerController.heroIcons[1],
                "Genghis" => PlayerController.heroIcons[2],
                "Anubis" => PlayerController.heroIcons[3],
                "Dustin" => PlayerController.heroIcons[4],
                "Leonardo Da Vinci" => PlayerController.heroIcons[5],
                _ => PlayerController.CompetitorHeroIcon.sprite
            };
            winPanel.GetComponent<WinLosePanel>().Initialize(PlayerController.OwnNameText.text, PlayerController.OwnMainCard, heroSprite);
        }

    }

    public void OtherHeal(int Heal)
    {
        PV.RPC("RPC_OtherDamanage", RpcTarget.AllBufferedViaServer, Heal);
    }

    [PunRPC]
    void RPC_OtherHeal(int Heal)
    {
        if (OtherHealth + Heal > 30)
        {
            OtherHealth = 30;
        }
        else
        {
            OtherHealth += Heal;
        }

    }

    public void MasterHeal(int Heal)
    {
        PV.RPC("RPC_MasterHeal", RpcTarget.AllBufferedViaServer, Heal);
    }

    [PunRPC]
    void RPC_MasterHeal(int Heal)
    {
        if (MasterHealth + Heal > 30)
        {
            MasterHealth = 30;
        }
        else
        {
            MasterHealth += Heal;
        }

    }


    public void OtherAddAttackDamage(int AddDamage)
    {
        PV.RPC("RPC_OtherAddAttackDamage", RpcTarget.AllBufferedViaServer, AddDamage);
    }

    [PunRPC]
    void RPC_OtherAddAttackDamage(int AddDamage)
    {
        print(OtherAttackDamage);
        OtherAttackDamage += AddDamage;
        print(OtherAttackDamage);

    }

    public void MasterAddAttackDamage(int AddDamage)
    {
        PV.RPC("RPC_MasterAddAttackDamage", RpcTarget.AllBufferedViaServer, AddDamage);
    }

    [PunRPC]
    void RPC_MasterAddAttackDamage(int AddDamage)
    {
        print(MasterAttackDamage);
        MasterAttackDamage += AddDamage;
        print(MasterAttackDamage);

    }


    private void LoadMainMenu()
    {
        LeaveRoomAndReturnToMainMenu();
    }

    public void LeaveRoomAndReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom(); // Odayı terk et
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby"); // Ana menü sahnesine geç
    }

    public override void OnLeftLobby()
    {
       // SceneManager.LoadScene("LoginScene"); // Ana menü sahnesine geç
        //Destroy(GameObject.Find("RoomManager").gameObject);
    }

    public void AddMana()
    {
        PV.RPC("RPC_AddMana", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void RPC_AddMana()
    {
        if (ManaCount<=9)
        {
            ManaCount++;
        }
        
    }

    public void FinishTurn(bool turn)
    {

        PV.RPC("RPC_FinishTurn", RpcTarget.AllBufferedViaServer, turn);
    }

    [PunRPC]
    void RPC_FinishTurn(bool turn)
    {

        Turn = turn;
        TurnCount++;
        WhoTurnText.text += ", " + turn.ToString();
    }
    
    public void SendData(string ID,string Nickname,string[] Deck)
    {
        PV.RPC("RPC_SendData", RpcTarget.All,ID, Nickname,Deck);
    }

    [PunRPC]
    void RPC_SendData(string ID, string Nickname, string[] Deck)
    {
        if (ID=="1")
        {
            MasterPlayerName = Nickname;
            MasterDeck = Deck;
           // MasterMainCard = "ZeusCard";
            MasterMainCard = Deck[0];
            MasterHealth = FindHeroHealth(Deck[0]);
            MasterAttackDamage = FindHeroAttackDamage(Deck[0]);

            // MasterPlayerNameText.text = MasterPlayerName;
            //  MasterDeckText.text = MasterDeck.ToString();
        }
        else if (ID == "2")
        {
            OtherPlayerName = Nickname;
            OtherDeck = Deck;
           // OtherrMainCard = "ZeusCard";
            OtherrMainCard = Deck[0];
            OtherHealth = FindHeroHealth(Deck[0]);
            OtherAttackDamage = FindHeroAttackDamage(Deck[0]);


            //  OtherPlayerNameText.text = OtherPlayerName;
            //   OtherDeckText.text = OtherDeck.ToString();
        }

       
    }

    int FindHeroHealth(string HeroName)
    {
        switch (HeroName)
        {
            case "Zeus":
                ZeusCard zeusCard = new ZeusCard();
                return (int) zeusCard.hpValue;

            case "Genghis":
                GenghisCard genghisCard = new GenghisCard();
                return (int) genghisCard.hpValue;
            case "Odin":
                OdinCard odinCard = new OdinCard();
                return (int) odinCard.hpValue;
            case "Anubis":
                AnubisCard anubisCard = new AnubisCard();
                return (int) anubisCard.hpValue;
            case "Leonardo Da Vinci":
                LeonardoCard leonardoCard = new LeonardoCard();
                return (int) leonardoCard.hpValue;

        }

        return 10;

    }

    int FindHeroAttackDamage(string HeroName)
    {
        switch (HeroName)
        {
            case "Zeus":
                ZeusCard zeusCard = new ZeusCard();
                return (int)zeusCard.attackValue;

            case "Genghis":
                GenghisCard genghisCard = new GenghisCard();
                return (int)genghisCard.attackValue;
            case "Odin":
                OdinCard odinCard = new OdinCard();
                return (int)odinCard.attackValue;
            case "Anubis":
                AnubisCard anubisCard = new AnubisCard();
                return (int)anubisCard.attackValue;
            case "Leonardo Da Vinci":
                LeonardoCard leonardoCard = new LeonardoCard();
                return (int)leonardoCard.attackValue;

        }

        return 60;

    }


}
