using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public TalkManager talkManager;
    public Text talkText;
    public GameObject talkPanel;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    private void Start()
    {
        isAction = false;
        talkPanel.SetActive(isAction);
    }

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjManager objManager = scanObject.GetComponent<ObjManager>();
        Talk(objManager.id, objManager.isNpc);

        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }

        talkText.text = talkData;
        isAction = true;
        talkIndex++;
    }
}
