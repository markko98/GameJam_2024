using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Boss boss;

    public bool isMagazineCollected;
    public bool isPaperCollected;

    void Start()
    {
        Collectable.Collected += Collect;

        MatchTextByTyping.OnTextTypedCorrectly = TypingSuccessful;
        MatchTextByTyping.OnTextTypedIncorrectly = TypingFailed;
        // TODO - subrscibe to on time run out
        //GameProgressTracker.OnTimeRunOut += GameOver;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Collect(string type)
    {
        if (type == "Magazine")
        {
            isMagazineCollected = true;
        }
        else if (type == "ToiletPaper")
        {
            isPaperCollected = true;
        }
        // TODO
        Debug.Log(type + " is collected!");
    }

    private void TypingFailed()
    {
        Debug.Log("Typing failed!");
        StartCoroutine(WaitAndFaint());
    }

    private void TypingSuccessful()
    {
        Debug.Log("Typing sucsessful!");
        player.StopLookingAtBoss();
        boss.StopTalking();
    }

    private IEnumerator WaitAndFaint()
    {
        yield return new WaitForSeconds(3f);
        player.Fart();
        yield return new WaitForSeconds(2f);
        boss.Faint();
    }

    public bool IsCollectionCompleted()
    {
        return isMagazineCollected && isPaperCollected;
    }

    public void GameOver()
    {
        // TODO
        Debug.Log("Congratulations, You shit well!");
    }
}
