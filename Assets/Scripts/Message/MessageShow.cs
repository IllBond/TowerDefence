using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageShow : MonoBehaviour
{
    private Coroutine oldCoroutine;
    [SerializeField] private bool isDown;

    [SerializeField] private RectTransform MessageWindow;
    [SerializeField] private RectTransform MessageWindowNewPos;

    [SerializeField] private Vector3 UpPos;
    [SerializeField] private Vector3 DownPos;

    [SerializeField] private Text messageText;

    private string _message = "";
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
        }
    }

    public void Update()
    {

        if (isDown)
        {
            MessageWindow.position = Vector3.Lerp(MessageWindow.position, DownPos,Time.deltaTime * 5);
        }
        else
        {
            MessageWindow.position = Vector3.Lerp(MessageWindow.position, UpPos, Time.deltaTime * 5);
        }
    }


    private void Awake()
    {
        UpPos = MessageWindow.position;
        DownPos = MessageWindowNewPos.position;
    }



    public void ShowMessage(string text)
    {
        if (oldCoroutine != null)
        {
            StopCoroutine(oldCoroutine);
        }

        oldCoroutine = StartCoroutine(SetMessage(text));
    }

    IEnumerator SetMessage(string text)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        isDown = true;
        Message = text;
        messageText.text = "" + Message;
        yield return new WaitForSecondsRealtime(2.5f);

        isDown = false;
    }

}
