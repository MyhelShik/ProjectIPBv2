using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class InfoDiplayCanvasInfo : MonoBehaviour
{
    public class HostData
    {

        public string name { get; set; }
    }


    public TextMeshProUGUI nameText;


    void Start()
{
    StartCoroutine(GetHostData("http://193.136.194.15:5000/hosts"));
}

IEnumerator GetHostData(string uri)
{
    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    {
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(string.Format("Something went wrong: {0}", webRequest.error));
            // Additional error handling logic or feedback
            yield break;
        }

        HostData[] hostDataArray = JsonConvert.DeserializeObject<HostData[]>(webRequest.downloadHandler.text);

        HostData hostData = hostDataArray[0];

        nameText.text = hostData.name;
    }
}


}
