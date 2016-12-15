using UnityEngine;
using System.Collections;

public class webCam : MonoBehaviour {
    public string webCamName = "HD Pro Webcam C920";

    // Use this for initialization
    void Start() {
        WebCamTexture webcamTexture = new WebCamTexture();
        WebCamDevice[] devices = WebCamTexture.devices;
        for(int i = 0; i < devices.Length; i++) {
            if(devices[i].name == webCamName) {
                webcamTexture.deviceName = webCamName;
            }
        }
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play(); 
    }

}
