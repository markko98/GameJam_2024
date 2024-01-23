using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Boss boss;

    void Start()
    {
        Collectable.Collected += Collect;
        ButtonMashing.MashingFailed += MashFailed;
        ButtonMashing.MashingSuccessful += MashSuccessful;
        // TODO - subrscibe to on time run out
        //GameProgressTracker.OnTimeRunOut += GameOver;
    }

    private void GameOver()
    {
        // TODO - implement shitting
        Debug.Log("GAME OVER");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Collect(string type)
    {
        Debug.Log(type + " is collected!");
    }

    private void MashFailed()
    {
        Debug.Log("Mash failed!");
        IEnumerator coroutine = WaitAndFaint();
        StartCoroutine(coroutine);
    }

    private void MashSuccessful()
    {
        Debug.Log("Mash sucsessful!");
    }

    private IEnumerator WaitAndFaint()
    {
        yield return new WaitForSeconds(3f);
        player.Fart();
        yield return new WaitForSeconds(1f);
        boss.Faint();
    }
}
