using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour {
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    [HideInInspector]
    public bool gripButtonDown = false;
    [HideInInspector]
    public bool gripButtonUp = false;
    [HideInInspector]
    public bool gripButtonPressed = false;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    [HideInInspector]
    public bool triggerButtonDown = false;
    [HideInInspector]
    public bool triggerButtonUp = false;
    [HideInInspector]
    public bool triggerButtonPressed = false;
    [HideInInspector]
    public bool triggerButtonRelease = false;
    private bool triggerButtonReleased;
    public float oldTriggerVelocity = 0.0f;
    [HideInInspector]
    public float triggerVelocity = 0.0f;
    public float triggerReleaseThreshold = 0.1f;

    private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    [HideInInspector]
    public bool touchPadDown = false;
    [HideInInspector]
    public bool touchPadUp = false;
    [HideInInspector]
    public bool touchPadPressed = false;
    [HideInInspector]
    public Vector2 touchPadAxis;

    [HideInInspector]
    public bool holdingObject = false;
    public bool debug = false;
    public bool active = false;
    public bool menu = false;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    // Use this for initialization
    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update() {
        if (controller == null) {
            print("controller is not initialized");
            return;
        }
        gripButtonDown = controller.GetPressDown(gripButton);
        gripButtonUp = controller.GetPressUp(gripButton);
        gripButtonPressed = controller.GetPress(gripButton);

        triggerButtonDown = controller.GetPressDown(triggerButton);
        triggerButtonUp = controller.GetPressUp(triggerButton);
        triggerButtonPressed = controller.GetPress(triggerButton);
        triggerVelocity = controller.GetAxis(triggerButton).x;
        touchPadDown = controller.GetPressDown(touchPad);
        touchPadUp = controller.GetPressUp(touchPad);
        touchPadPressed = controller.GetPress(touchPad);
        touchPadAxis = controller.GetAxis(touchPad);
        debugInput();
    }

    void FixedUpdate() {
        detectChange();
    }

    void detectChange() {
        float triggerChange = oldTriggerVelocity - triggerVelocity;
        oldTriggerVelocity = triggerVelocity;
        triggerButtonRelease = false;
        if (!triggerButtonReleased) {
            if (triggerChange != 0f) {
                if (triggerChange > triggerReleaseThreshold) {
                    triggerButtonRelease = true;
                    triggerButtonReleased = true;
                }
            }
        }
        else {
            if (triggerVelocity == 0) {
                triggerButtonReleased = false;
            }
        }
    }



    void debugInput() {
        if (debug && active) {
            print(controller.GetAxis(triggerButton));
        }
    }

}


class ColorHSV : System.Object {

    float h = 0.0f;

    float s = 0.0f;

    float v = 0.0f;

    float a = 0.0f;



    /**

    * Construct without alpha (which defaults to 1)

    */

    public ColorHSV(float h, float s, float v) {

        this.h = h;

        this.s = s;

        this.v = v;

        this.a = 1.0f;

    }



    /**

    * Construct with alpha

    */

    public ColorHSV(float h, float s, float v, float a) {

        this.h = h;

        this.s = s;

        this.v = v;

        this.a = a;

    }



    /**

    * Create from an RGBA color object

    */

    public ColorHSV(Color color) {

        float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);

        float max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);

        float delta = max - min;



        // value is our max color

        this.v = max;



        // saturation is percent of max

        if (!Mathf.Approximately(max, 0))

            this.s = delta / max;

        else {

            // all colors are zero, no saturation and hue is undefined

            this.s = 0;

            this.h = -1;

            return;

        }



        // grayscale image if min and max are the same

        if (Mathf.Approximately(min, max)) {

            this.v = max;

            this.s = 0;

            this.h = -1;

            return;

        }



        // hue depends which color is max (this creates a rainbow effect)

        if (color.r == max)

            this.h = (color.g - color.b) / delta;         // between yellow  magenta

        else if (color.g == max)

            this.h = 2 + (color.b - color.r) / delta; // between cyan  yellow

        else

            this.h = 4 + (color.r - color.g) / delta; // between magenta  cyan



        // turn hue into 0-360 degrees

        this.h *= 60;

        if (this.h < 0)

            this.h += 360;

    }



    /**

    * Return an RGBA color object

    */

    public Color ToColor() {

        // no saturation, we can return the value across the board (grayscale)

        if (this.s == 0)

            return new Color(this.v, this.v, this.v, this.a);



        // which chunk of the rainbow are we in?

        float sector = this.h / 60;



        // split across the decimal (ie 3.87f into 3 and 0.87f)

        int i;
        i = (int)Mathf.Floor(sector);

        float f = sector - i;



        float v = this.v;

        float p = v * (1 - s);

        float q = v * (1 - s * f);

        float t = v * (1 - s * (1 - f));



        // build our rgb color

        Color color = new Color(0, 0, 0, this.a);



        switch (i) {

            case 0:

                color.r = v;

                color.g = t;

                color.b = p;

                break;

            case 1:

                color.r = q;

                color.g = v;

                color.b = p;

                break;

            case 2:

                color.r = p;

                color.g = v;

                color.b = t;

                break;

            case 3:

                color.r = p;

                color.g = q;

                color.b = v;

                break;

            case 4:

                color.r = t;

                color.g = p;

                color.b = v;

                break;

            default:

                color.r = v;

                color.g = p;

                color.b = q;

                break;

        }
        return color;
    }
}