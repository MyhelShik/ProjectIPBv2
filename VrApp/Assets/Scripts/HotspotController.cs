using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using UnityEngine.Networking;
public class HotspotController : MonoBehaviour
{
    [Header("Hotspot Initialization Config:")]
    [Tooltip("JSON file location with hotspot informations and map.")]
    [SerializeField] string filePath = "Assets/HotspotData.json";
    [Tooltip("All materials with the 360� images used in this hotspot.")]
    [SerializeField] List<Material> scenaryMaterials;
    [SerializeField] GameObject canvasMapping;
    [SerializeField] GameObject mappingButton;
    [SerializeField] GameObject mappingButtonInMenu;
    [SerializeField] GameObject canvasInfo;
    //[SerializeField] GameObject canvasInfo2; //added instance canvas

    [SerializeField] string typeTextID = "text";
    
    [SerializeField] string typeImageID = "image";
    [SerializeField] string typeVideoID = "video";
    [SerializeField] string typeTransitionID = "transition";

    //var
    private string jsonText;
    private GameObject hotspotScenary;
    private MeshRenderer scenaryMeshRenderer;
    private Hotspot hotspot;

    private Camera camera;

    private byte actualIndex = 0;

    void Start() 
    {

        camera = FindObjectOfType<Camera>();

        StartCoroutine(InitializeHotspot("http://193.136.194.15:5000/hosts"));
        //InitializeHotspot();

    }


    IEnumerator InitializeHotspot(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
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
                    Hotspot[] hostDataArray = JsonConvert.DeserializeObject<Hotspot[]>(webRequest.downloadHandler.text);
                    if (hostDataArray.Length > 0)
                    {

                        hotspot =  hostDataArray[0];

                        CreateHotspotScenary();

                        canvasMapping = Instantiate(canvasMapping, gameObject.transform);
                        canvasMapping.GetComponent<Canvas>().worldCamera = camera;
                        

                        scenaryMeshRenderer = hotspotScenary.GetComponent<MeshRenderer>();
                        scenaryMeshRenderer.material = FindMaterial(hotspot.images[actualIndex].materialName);

                        gameObject.name = "Hotspot Controller [" + hotspot.name + "]";

                        MapImage();

                        //materialNameText.text = hostData.materialName;
                        //nameText.text = hostData.name;
                    }
                    break;
            }
        }
    }

    private void InitializeHotspotLocal() 
    {
        /*StreamReader reader = new StreamReader(filePath);
        jsonText = reader.ReadToEnd();
        reader.Close();

        if (jsonText.Trim().Length == 0) 
        {
            Debug.LogError("JSON file is empty.");
            return;

        }*/

        
        

        //hotspot = JsonUtility.FromJson<Hotspot>(jsonText);


        // \/


        //CreateHotspotScenary();

        //canvasMapping = Instantiate(canvasMapping, gameObject.transform);
        //canvasMapping.GetComponent<Canvas>().worldCamera = camera;

        //scenaryMeshRenderer = hotspotScenary.GetComponent<MeshRenderer>();
        //scenaryMeshRenderer.material = FindMaterial(hotspot.images[actualIndex].materialName);

        //gameObject.name = "Hotspot Controller [" + hotspot.name + "]";

        //MapImage();

    }

    private void CreateHotspotScenary() 
    {
        hotspotScenary = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hotspotScenary.name = "Hotspot Scenary";
        hotspotScenary.transform.parent = gameObject.transform;
        hotspotScenary.transform.localScale = new Vector3(8,8,8);
        
    }

    private void MapImage() 
    {
        byte count = 0;

        foreach (Mapping map in hotspot.images[actualIndex].mapping) 
        {

            GameObject newMappingButton = Instantiate(mappingButton, canvasMapping.transform);
            newMappingButton.transform.position = new Vector3(map.positionX, map.positionY, map.positionZ);
            ObjectInformation objectinformation = newMappingButton.GetComponent<ObjectInformation>();
            objectinformation.Uuid = map.uuidsensor;
            Debug.Log("> " + objectinformation.Uuid.ToString());

 
            

            newMappingButton.name = newMappingButton.name + " | " + count;

            newMappingButton.transform.LookAt(camera.transform, Vector3.one);
            

            if (map.type.Equals(typeTextID))
                InstantiateCanvasInfo(ref count, map, newMappingButton);

            //if(map.type.Equals(typeTextID2))
                //InstantiateCanvasInfo2(ref count, map, newMappingButton);
    

        }
    }

    private byte InstantiateCanvasInfo(ref byte count, Mapping map, GameObject newMappingButton) 
    {
        GameObject newCanvasInfo = Instantiate(canvasInfo.gameObject, newMappingButton.transform);

        newCanvasInfo.name = newCanvasInfo.name + " | " + count++;

        newMappingButton.GetComponent<ObjectInformation>().SetInformation(map.information);
        newMappingButton.GetComponent<ObjectInformation>().SetInformationDisplay(newCanvasInfo.GetComponent<InformationDisplay>());
        return count;
    }
            //new function avoid
    /*private byte InstantiateCanvasInfo2(ref byte count, Mapping map, GameObject newMappingButton) 
    {
        GameObject newCanvasInfo = Instantiate(canvasInfo2.gameObject, newMappingButton.transform);

        newCanvasInfo.name = newCanvasInfo.name + " | " + count++;

        newMappingButton.GetComponent<ObjectInformation>().SetInformation(map.information);
        newMappingButton.GetComponent<ObjectInformation>().SetInformationDisplay(newCanvasInfo.GetComponent<InformationDisplay>());
        return count;
    }
    */

    private Material FindMaterial(string materialName) 
    {
        foreach (Material mat in scenaryMaterials) 
        {
            if (mat.name.Contains(materialName)) 
            {
                return mat;

            }
        }
        return null;

    }
}
