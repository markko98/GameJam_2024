using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Start()
    {
        Collectable.Collected += Collect;
        ButtonMashing.MashingFailed += MashFailed;
        ButtonMashing.MashingSuccessful += MashSuccessful;
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
    }

    private void MashSuccessful()
    {
        Debug.Log("Mash sucsessful!");
    }
}
