    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using UnityEngine.Networking;

    public class InformationDisplay : MonoBehaviour
    {
        [Header("Information Display Config:")]
        [SerializeField] TextMeshProUGUI infomation;
        [SerializeField] Transform canvasRotationPointPrefab;
        [SerializeField] Transform playerCamera;
        [SerializeField] float distance = 1;
        [SerializeField] float offset = 5;

        private Transform canvasRotationPoint;
        private Transform canvasRotationPoint2;

        // Start is called before the first frame update
        void Start()
        {
            playerCamera = FindObjectOfType<Camera>().gameObject.transform;
            canvasRotationPoint = Instantiate(canvasRotationPointPrefab, transform);

            // Calculate the position for the second canvasRotationPoint
            Vector3 offsetPosition = -canvasRotationPoint.right * offset;
        }

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        break;
                }
            }
        }
    
        public void DisplayInformation(string sensorUuid)
        {
            // Debug.Log(sensorUuid);
            // infomation.text = sensorUuid;
            
            // A correct website page.
            StartCoroutine(GetRequest("http://193.136.194.15:5000/GetData/Klfb64a6c72b0e7e"));


            canvasRotationPoint.position = playerCamera.transform.position + playerCamera.transform.forward * distance;
            canvasRotationPoint.position = new Vector3(canvasRotationPoint.position.x, 1f, canvasRotationPoint.position.z);
            canvasRotationPoint.rotation = new Quaternion(0f, playerCamera.transform.rotation.y, 0f, playerCamera.transform.rotation.w);
            canvasRotationPoint.gameObject.SetActive(true);
        }
    }
