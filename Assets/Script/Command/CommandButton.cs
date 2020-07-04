using System.Collections;
using System.Collections.Generic;
using Script.Command;
using UnityEngine;

public class CommandButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoButton()
    {
        Debug.Log("Do");
        CommandManager.Instance().DoCommand();
    }

    public void UnDoButton()
    {
        Debug.Log("UnDo");
        CommandManager.Instance().UnDoCommand();

    }
}
