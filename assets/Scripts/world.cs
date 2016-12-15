using UnityEngine;
using System.Collections.Generic;

public class world : MonoBehaviour {
    //private float speed = 1.5f;
    public WandController[] wandControllers;
    private int numInstances = 300;
    private int index = 0;
    private List<GameObject> instances;
    private GameObject currentInstance;
    private world[] worlds;
    private Vector3 holdingStation;
    public GameObject obj;
    private int frames;
    private float[] savedSizes;
    private float currentSize;
    private int numSavedSizes = 3;
    public float speed = 5f;

    // Use this for initialization
    void Start () {
        instances = new List<GameObject>();
        savedSizes = new float[numSavedSizes];
        holdingStation = new Vector3(-999, -999, -999);
        createInstances(obj);
    }

	// Update is called once per frame
	void Update () {
        checkInput();
	}

    void FixedUpdate()
    {
        frames++;
        checkWandControllerInput();
    }

    void checkInput() {
        if (Input.GetKeyDown("space")) {
            clearScene();
        }
    }


    void checkWandControllerInput()
    {
        foreach(WandController controller in wandControllers)
        {
            if (controller.active)
            {
                if (controller.triggerButtonRelease && controller.holdingObject){
                    releaseObject(controller);
                }
                else{
                    if (controller.triggerVelocity > 0.2f) {
                        if (!controller.holdingObject) {
                            createObjectAtController(controller);
                        }else{
                            moveObjectToController(controller);
                            rotateObjectToController(controller);
                            growObject(controller.triggerVelocity);
                            saveSizeHistory(currentSize);
                        }
                    }
                }
                if (controller.triggerVelocity < 0.05f && controller.holdingObject){
                    releaseObject(controller);
                }
                if (controller.touchPadDown) {
                    clearScene();
                }
            }
            if (controller.menu) {
                if (controller.touchPadDown) {
                    clearScene();
                }
            }
        }
    }

    void setInstanceToMaxSavedSize() {
        //used for retaining size once release is realized
        float maxSize = savedSizes[0];
        foreach (float size in savedSizes) {
            if (size > maxSize) {
                maxSize = size;
            }
        }
       setObjectSize(maxSize);
    }

    void growObject(float triggerVelocity){
        currentSize += ((triggerVelocity * triggerVelocity )/200f);

        if(triggerVelocity < .2f){
            float gravity = (.2f - triggerVelocity)/100f;
            currentSize -= gravity;
        }

        currentSize = Mathf.Clamp(currentSize, 0, 1.15f);
        setObjectSize(currentSize);
        Vector3 targetScale = new Vector3(currentSize, currentSize, currentSize);
        currentInstance.transform.localScale = targetScale;
    }

    void setObjectSize(float size) {
        Vector3 targetScale = new Vector3(size, size, size);
        currentInstance.transform.localScale = targetScale;
    }

    void stopAnimation(GameObject obj){
        generalAnimation anim = obj.GetComponent<generalAnimation>();
        anim.moveY = 0f;
    }

    void startAnimation(GameObject obj){
        generalAnimation anim = obj.GetComponent<generalAnimation>();
        anim.moveY = .001f * (1f/obj.transform.lossyScale.y);
    }


    void saveSizeHistory(float size)
    {
        int frameIndex = frames % savedSizes.Length;
        savedSizes[frameIndex] = size;
    }


    void createInstances(GameObject _prefab)
    {
        for (int i = 0; i < numInstances; i++)
        {
            GameObject obj = createInstance(_prefab);
            obj.transform.position = holdingStation;
            shrinkObjectToInitialSize(obj);
            instances.Add(obj);
       }
        currentInstance = instances[0];
    }

    void clearScene(){
        //TODO: refactor - this is for clearing both sets
        foreach(world w in this.GetComponents<world>()){
            w.clearThisScene();
        }
    }

    public void clearThisScene(){
        for(int i = 0; i < numInstances; i++) {
            GameObject obj = instances[i];
            obj.transform.position = holdingStation;
            shrinkObjectToInitialSize(obj);
            stopAnimation(obj);
        }
    }

    GameObject createInstance(GameObject _prefab)
    {
        GameObject instance;
        instance = Instantiate(_prefab) as GameObject;
        return instance;
    }

    void releaseObject(WandController controller)
    {
        controller.holdingObject = false;
        setInstanceToMaxSavedSize();
        startAnimation(currentInstance);
    }

    void createObjectAtController(WandController controller)
    {
        GameObject controllerGo = controller.gameObject;
        createObjectAt(controllerGo.transform.position);
        moveObjectToController(controller);
        rotateObjectToController(controller);
        shrinkObjectToInitialSize(currentInstance);
        controller.holdingObject = true;
        stopAnimation(currentInstance);
    }

    void moveObjectToController(WandController controller)
    {
        moveObjectToPosition(currentInstance, controller.transform.position);
    }

    void rotateObjectToController(WandController controller) {
        currentInstance.transform.rotation = controller.transform.rotation;
    }

    void randomColor() {
        var randomHSV = new ColorHSV(Random.Range(0.0f, 360.0f), Random.Range(0.5f, 0.9f), 1.0f);
        var color = randomHSV.ToColor();
        currentInstance.GetComponent<Renderer>().material.color = color;
    }

    void moveObjectToPosition(GameObject obj, Vector3 location)
    {
        currentInstance.transform.position = location;
    }

    void shrinkObjectToInitialSize(GameObject obj) {
        obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        currentSize = 0.01f;
    }

    void createObjectAt(Vector3 location)
    {
        index++;
        index = (int)Mathf.Repeat(index, numInstances);
        GameObject instance = instances[index];
        currentInstance = instance;
    }


}



