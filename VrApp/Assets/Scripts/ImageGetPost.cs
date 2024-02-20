using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ImageGetPost : MonoBehaviour
{
    public Texture2D targetTexture1;
    public Texture2D targetTexture2;

    public RawImage imageObject1;
    public RawImage imageObject2;

    public float refreshInterval = 120f; // Refresh interval in seconds

    private string uri = "http://193.136.194.15:5000/graph/";
    private bool isRefreshing = false;

    private string uuid = "";
    private IEnumerator refreshImages = null;


   

    void Start()
    {

        uuid = gameObject.transform.parent.parent.parent.parent.GetComponent<ObjectInformation>().Uuid;
        Debug.Log(">>>>>" + uuid);
        
    }

    void OnEnable()
    {
        refreshInterval = 1f;
        refreshImages = RefreshImages();
        StartCoroutine(refreshImages);
        Debug.Log("awds");
    
    
    }

    void OnDisable()
    {
        Debug.Log("disable");
        StopCoroutine(refreshImages);
    }

    IEnumerator RefreshImages()
    {
        do
        {
            if (!isRefreshing)
            {
                isRefreshing = true;  
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + uuid))
            {
                yield return webRequest.SendWebRequest();

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(string.Format("Something went wrong: {0}", webRequest.error));
                        // Additional error handling logic or feedback
                        break;
                    case UnityWebRequest.Result.Success:
                        refreshInterval = 300f;
                        string response = webRequest.downloadHandler.text;
                        GraphData graphData = JsonUtility.FromJson<GraphData>(response);

                        if (graphData != null)
                        {
                            byte[] imageBytes1 = System.Convert.FromBase64String(graphData.image);
                            targetTexture1 = new Texture2D(2, 2); // Adjust the size as per your requirements
                            targetTexture1.LoadImage(imageBytes1);

                            byte[] imageBytes2 = System.Convert.FromBase64String(graphData.image2);
                            targetTexture2 = new Texture2D(2, 2); // Adjust the size as per your requirements
                            targetTexture2.LoadImage(imageBytes2);

                            UpdateVisuals();
                        }
                        break;
                }

                isRefreshing = false;
            }
            yield return new WaitForSeconds(refreshInterval);
            }
        } while (true);
    }

    private void UpdateVisuals()
    {
        if (imageObject1 != null)
        {
            imageObject1.texture = targetTexture1;
        }

        if (imageObject2 != null)
        {
            imageObject2.texture = targetTexture2;
        }
    }

    [System.Serializable]
    public class GraphData
    {
        public string image;
        public string image2;
    }
}
