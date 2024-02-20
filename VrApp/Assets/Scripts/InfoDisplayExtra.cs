using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class InfoDisplayExtra : MonoBehaviour
{
    public class Data
    {
        public string Humidity    { get; set; }
        public string Pressure    { get; set; }
        public string Temperature { get; set; }
    }

    public TextMeshProUGUI humidityText;
    public TextMeshProUGUI pressureText;
    public TextMeshProUGUI temperatureText;

    public float refreshInterval = 1f; // Refresh interval in seconds
                                                //Klfb64a6c72b0e7e
    private string uri = "http://193.136.194.15:5000/GetData/";
    private bool isRefreshing = false;
    
    private string uuid = "";

    private IEnumerator refreshData = null;
    

    void Start()
    {

        uuid = gameObject.transform.parent.parent.parent.GetComponent<ObjectInformation>().Uuid;
        Debug.Log(">>>>>" + uuid);
        
    }

    void OnEnable()
    {
        refreshInterval = 1f;
        refreshData = RefreshData();
        StartCoroutine(refreshData);
        Debug.Log("awds");
    
    
    }

    void OnDisable()
    {
        Debug.Log("disable");
        StopCoroutine(refreshData);
    }



    IEnumerator RefreshData()
    {
        do {
            if (!isRefreshing)
            {
                Debug.Log("1");
                isRefreshing = true;    

                using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + uuid))
                {
                    Debug.Log("2");
                    yield return webRequest.SendWebRequest();

                    Debug.Log("3");
                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.LogError(string.Format("Something went wrong: {0}", webRequest.error));
                            // Additional error handling logic or feedback
                            break;
                        case UnityWebRequest.Result.Success:
                            refreshInterval = 60f;
                            Debug.Log("4");
                            Data data = JsonConvert.DeserializeObject<Data>(webRequest.downloadHandler.text);
                            Debug.Log(webRequest.downloadHandler.text);
                            Debug.Log(data.Humidity);
                            humidityText.text = data.Humidity;
                            pressureText.text = data.Pressure;
                            temperatureText.text = data.Temperature;

                            break;
                    }
                    
                    isRefreshing = false;
                }

            }

            yield return new WaitForSeconds(refreshInterval);
        }  while (true);
      
    }
}
