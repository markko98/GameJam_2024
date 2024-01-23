using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testModal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ModalWindow.Instance.InitModal();

        var modalData = new ModalData()
        {
            title = "Test modal",
            message = "Testing a modal window",
            option1 = "Cancel",
            option2 = "Okay",
            option1Callback = WriteDiscard,
            option2Callback = WriteConfirm,
            hideCallback = WriteHide,
        };

        ModalWindow.Instance.ShowModal(modalData);
    }


    private void WriteDiscard()
    {
        Debug.Log("DISCARD");
    }

    private void WriteConfirm()
    {
        Debug.Log("CONFIRM");
    }
    private void WriteHide()
    {
        Debug.Log("HIDE");
    }
}
