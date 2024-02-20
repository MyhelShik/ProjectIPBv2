using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInformation : MonoBehaviour
{
    [Header("Object Information")]
    [SerializeField] string information = "";
    [SerializeField] InformationDisplay informationDisplay;

    
    private string _uuid;
    public string Uuid
    {
        get { return _uuid; }
        set { _uuid = value; }
    }   

    public void ShowInformation() 
    {
        if (information.Trim().Length == 0)
            information = "[REDACTED]";
        informationDisplay.DisplayInformation(_uuid);
    }

    public void SetInformation(string information) 
    {
        this.information = information;

    }

    public void SetInformationDisplay(InformationDisplay informationDisplay) 
    {
        this.informationDisplay = informationDisplay;

    }
}
