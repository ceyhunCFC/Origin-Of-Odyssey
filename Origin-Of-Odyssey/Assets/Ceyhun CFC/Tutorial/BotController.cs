using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotController : MonoBehaviour
{
   TutorialPlayerController _TutorialPlayerController;
   void Start()
   {
     _TutorialPlayerController=GetComponent<TutorialPlayerController>(); 

      for (int i = 0; i < 3; i++)
      {
        _TutorialPlayerController.CreateAnCompetitorCard();
      }

     Instantiate(Resources.Load<GameObject>("TutorialCompetitorHeroCard 1"), GameObject.Find("CompetitorHeroPivot").transform);
   
   }
   
   public void BotAttack()
   {

      _TutorialPlayerController.CreateAnCompetitorCard();
   
     

      _TutorialPlayerController.WhoseTurnText.text = "Enemy Turn";

      StartCoroutine(Waiter());
       
   }

  int FindEmptyAreaBox()
   {
    
      

      List<int> EmptyIndexes = new List<int>();

      for (int i = 0; i < GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions.Length; i++)
      {
        if(GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[i].transform.childCount==0)
        {
           EmptyIndexes.Add(i);
           print("Eklendi");
        }
        
      }
     
      return  EmptyIndexes[UnityEngine.Random.Range(0, EmptyIndexes.Count)];

   }


   void CreateCardFromBot()
   {

    _TutorialPlayerController.RemoveAnCompetitorCard();
     GameObject CardCurrent = Instantiate(_TutorialPlayerController.CardPrefabInGame, GameObject.Find("Area").GetComponent<CardsAreaCreator>().BackAreaCollisions[FindEmptyAreaBox()].transform);
     CardCurrent.tag = "CompetitorCard";

   
     CardCurrent.transform.localScale = new Vector3(1,1,0.04f);
     CardCurrent.transform.localPosition = Vector3.zero;
     CardCurrent.transform.localEulerAngles = new Vector3(45,0,180);

      _TutorialPlayerController.CreateInfoCard(CardCurrent);


      if(CardCurrent.GetComponent<CardInformation>().CardHealth=="")
      {
          Destroy(CardCurrent);
      }

   

   }

   IEnumerator Waiter()
   {
      yield return new WaitForSeconds(1);
      Debug.Log("SIRA SENDE");

      if(UnityEngine.Random.Range(0, 10)<6)
      {
         CreateCardFromBot();
      }
      
      _TutorialPlayerController.BeginerFunction();
      
   }
}
