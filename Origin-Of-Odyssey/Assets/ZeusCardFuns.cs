using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ZeusCardFuns 
{
  
  public static void HeraclesFun(GameObject selectedCard, PlayerController PC )
  { 
    selectedCard.GetComponent<CardInformation>().CardHealth = (int.Parse(selectedCard.GetComponent<CardInformation>().CardHealth) + (2 * PC.DeadMonsterCound)).ToString();
    selectedCard.GetComponent<CardInformation>().CardDamage += (2 * PC.DeadMonsterCound);
    selectedCard.GetComponent<CardInformation>().SetInformation();

  }

   public static void LightningBoltFun(GameObject selectedCard, PlayerController PC )
  { 

                     PC.ManaCountText.text =  PC.Mana.ToString() + "/10";
                     PC.OwnManaBar.fillAmount =  PC.Mana / 10f;
                     PC.CompetitorManaBar.fillAmount = PC._GameManager.ManaCount / 10;
                     PC.CompetitorManaCountText.text = ( PC._GameManager.ManaCount) + "/10".ToString();
                     PC.DeckCardCount--;

                     PC.StackDeck();

                    if ( PC.PV.IsMine)
                    {
                        PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("DeleteCompatitorDeckCard", RpcTarget.All);
                        PC.CompetitorPV.GetComponent<PlayerController>().PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                        PC. PV.RPC("RefreshPlayersInformation", RpcTarget.All);
                    }
                    int index = Array.IndexOf(GameObject.Find("Area").GetComponent<CardsAreaCreator>().FrontAreaCollisions, selectedCard.transform.parent.gameObject);

                    PC._CardProgress.SecoundTargetCard = true;
                    PC._CardProgress.SetAttackerCard(index);


                    selectedCard.SetActive(false);

                   
                    selectedCard = null;
                    PC.lastHoveredCard = null;

                    PC.UsedSpell();
                    Debug.LogError("USSEEDD A SPEEELLL");
    
                   // GameObject TalkCloud = Instantiate(Resources.Load<GameObject>("TalkCloud"), GameObject.Find("Character").transform);
                   // TalkCloud.transform.GetChild(0).GetComponent<Text>().text = "USSEEDD A SPEEELLL!";

  

    }


}
