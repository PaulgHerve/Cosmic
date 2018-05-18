using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Star : MonoBehaviour {

    public enum starType { BLACK_HOLE, BLUE_GIANT, BLUE_SUPER, PULSAR, RED_DWARF, RED_GIANT, WHITE_DWARF, YELLOW_SUN }

    private starType star_Type;

    Planet_Manager planet_Manager;
    Star_Effects effects;
    SpriteRenderer sprite;
    new SphereCollider collider;

    private float rDistance;
    private Vector3 orbitalCenter;
    private Vector3 movement;
    private bool enable_Rotation;
    private float age = 0;
    private Vector3 baseScale;
    private Canvas canvas;
    private RectTransform rect;

    IEnumerator currentAnimation = null;

    float scale;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        baseScale = rect.localScale;
        effects = GetComponentInChildren<Star_Effects>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponentInChildren<SphereCollider>();
        planet_Manager = GetComponentInChildren<Planet_Manager>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;

        collider.enabled = false;
    }
	
	void Update ()
    {
        enable_Rotation = Galaxy.enable_Rotation;

        Scale_To_Camera();

        if (enable_Rotation)
        {
            RotateAroundGalaxyCenter();
            collider.enabled = false;
        }
        else
        {
            collider.enabled = true;
        }     
    }

    public void Generate(float galaxySize, float armVal, float armInc, float armDensity, string newName)
    {
        float center = 512 + (12.00f * (galaxySize / 3.00f));
        float x = (20.00f) * (6.00f + galaxySize / 12.00f);
        float y = (12.00f) * (6.00f + galaxySize / 12.00f);
        float z = (256.00f) * (6.00f + galaxySize / 12.00f);
        z *= Random.Range(-1.00f, 1.01f);
        x *= Random.Range(-1.00f, 1.01f);
        y *= Random.Range(-1.00f, 1.01f);

        //Creates a space in the center for a supermassive
        if (z >= 0) { z += center; }
        else if (z < 0) { z -= center; }

        Vector3 startPos = new Vector3(x, y, z);
        star_Type = (starType)Determine_Star_Type();
        int spriteIndex = (int)star_Type;

        sprite.sprite = Galaxy_Generator.Get_Star_Sprite(spriteIndex);
        name = newName;

        Initialize_Name();
        InitializeScale((starType)spriteIndex);
        SetStarEffectColors((starType)spriteIndex);

        InitializeOrbitalCenter();
        rect.localPosition = startPos;

        rDistance = Vector3.Magnitude(new Vector3(rect.localPosition.x, 0, rect.localPosition.z));

        float percent = (360.00000f * armVal);
        float roll = Random.Range(0.00000f, armDensity);        
        float maxVal = (360.00000f * armInc) * roll;

        percent += Random.Range(0.00000f, maxVal);

        IEnumerator thing = RotateAroundGalaxyCenter(percent);
        StartCoroutine(thing);

        InitializeMovement();

        for (int i = 0; i < 240; i++)
        {
            RotateAroundGalaxyCenter(256 * movement);
        }

        Generate_Solar_System();        
    }

    //Generates a group of celestial bodies associated to a host star
    public void Generate_Solar_System()
    {
        planet_Manager.Initialize(this);
    }

    private int Determine_Star_Type()
    {
        int index = 0;
        float roll = Random.Range(0, 101);

        if (roll < 4) { index = 0; }                            //Black Hole
        else if (roll < 15) { index = 1; }                      //Blue
        else if (roll < 20) { index = 2; }                      //Blue Super
        else if (roll < 26) { index = 3; }                      //Pulsar
        else if (roll < 41) { index = 4; }                      //Red Dwarf
        else if (roll < 44) { index = 5; }                      //Red Giant
        else if (roll < 56) { index = 6; }                      //White Dwarf
        else { index = 7; }                                     //Yellow

        return index;
    }

    private void InitializeMovement()
    {
        float x = 2.000f;
        float y = 0.000f;
        float z = 0.000f;

        movement = new Vector3(x, y, z);
        movement /= Mathf.Sqrt(rDistance);
    }

    private void InitializeMovement(float speed)
    {
        float x = speed * 1.000f;
        float y = 0;
        float z = 0;
        
        movement = new Vector3(x, y, z);
        movement *= 10;
        movement /= Mathf.Sqrt(rDistance);
    }

    private void InitializeOrbitalCenter()
    {
        orbitalCenter = new Vector3(0, 0, 0);
    }

    private void InitializeScale(starType sType)
    {
        int t = (int)sType;
        float scaleMult = 1;
        float scaler = Random.Range(.5f, 1.01f);

        if (t == 0)      { scaleMult = .40f; }                          //Black Hole
        else if (t == 1) { scaleMult = .50f; }                          //Blue 
        else if (t == 2) { scaleMult = .80f; }                          //Blue Super
        else if (t == 3) { scaleMult = .70f; }                          //Pulsar
        else if (t == 4) { scaleMult = .25f; }                          //Red Dwarf
        else if (t == 5) { scaleMult = .85f; }                          //Red Giant
        else if (t == 6) { scaleMult = .25f; }                          //White Dwarf
        else if (t == 7) { scaleMult = .40f; }                          //Yellow

        scaler *= scaleMult;

        sprite.transform.localScale *= scaler;
        scale = sprite.transform.localScale.x;
    }

    private void Initialize_Name()
    {
        Text nameText = GetComponentInChildren<Text>();

        nameText.text = name;
    }

    private void Scale_To_Camera()
    {
        float cameraScale = CameraController.GetCameraScaleRatio();

        if (cameraScale < .25f)
        {
            cameraScale = .25f;
        }

        if (cameraScale > .8f)
        {
            cameraScale = .8f;
        }

        float scaler = 2 * scale * cameraScale;

        if (scaler != sprite.transform.localScale.x)
        {
            sprite.transform.localScale = new Vector3(scaler, scaler, scaler);
        }      
    }

    private void SetStarEffectColors(starType sType)
    {
        Color32 glowColor = Icon_Controller.Get_Star_Haze_Color(sType);
        Color32 hazeColor = Icon_Controller.Get_Star_Glow_Color(sType);

        glowColor.a -= 4;

        effects.Set_Glow(glowColor);
        effects.Set_Haze(hazeColor);
    }

    //Rotates Camera Around a centerpoint
    private void RotateAroundGalaxyCenter()
    {
        Vector3 targetPos = orbitalCenter;           

        Vector3 relative = targetPos - rect.localPosition;
        Quaternion current = rect.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        rect.Translate(movement);

        rect.localRotation = Quaternion.Slerp(current, rotation, 1f);

        Vector3 newPos = new Vector3(rect.localPosition.x, 0, rect.localPosition.z);
        newPos = Vector3.ClampMagnitude(newPos, rDistance);
        newPos.y = rect.localPosition.y;

        rect.localPosition = newPos;
    }

    //Rotates Camera Around a centerpoint at a controlled speed
    private void RotateAroundGalaxyCenter(Vector3 speed)
    {
        Vector3 targetPos = orbitalCenter;

        Vector3 relative = targetPos - rect.localPosition;
        Quaternion current = rect.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        rect.Translate(speed);

        rect.localRotation = Quaternion.Slerp(current, rotation, 1f);

        Vector3 newPos = new Vector3(rect.localPosition.x, 0, rect.localPosition.z);
        newPos = Vector3.ClampMagnitude(newPos, rDistance);
        newPos.y = rect.localPosition.y;

        rect.localPosition = newPos;
    }

    //Rotates Camera Around a centerpoint
    private IEnumerator RotateAroundGalaxyCenter(float degrees)
    {
        //Constrains degrees to between 0 - 360
        if (degrees < 0)                    { degrees = 0; }

        float num = 5;

        degrees *= .500f;
        degrees /= num;

        float move = (2 * rDistance * Mathf.PI) / 180;
        Vector3 m = new Vector3(move, 0, 0);
        Vector3 targetPos = orbitalCenter;

        Vector3 relative = targetPos - rect.localPosition;
        Quaternion current = rect.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        for (int i = 0; i < degrees; i++)
        {
            for (int n = 0; n < num; n++)
            {
                relative = targetPos - rect.localPosition;
                current = rect.localRotation;
                rotation = Quaternion.LookRotation(relative);

                rect.Translate(m);

                rect.localRotation = Quaternion.Slerp(current, rotation, 1f);

                Vector3 newPos = new Vector3(rect.localPosition.x, 0, rect.localPosition.z);
                newPos = Vector3.ClampMagnitude(newPos, rDistance);
                newPos.y = rect.localPosition.y;

                rect.localPosition = newPos;
            }

            yield return new WaitForEndOfFrame();           
        }
    }

    public void AnimateAge(float runs)
    {
        Vector3 speed = movement;
        float dif = Mathf.Abs(runs);
        float age_Change = (runs / dif);

        if (runs < 0)
        {
            speed *= -1;
        }

        if (dif > 24)
        {
            dif /= 12.000f;
            speed *= 12.000f;
            age_Change *= 12.000f;
        }

        else if (dif > 12)
        {
            dif /= 6.000f;
            speed *= 6.000f;
            age_Change *= 6.000f;
        }

        for (int i = 0; i < dif; i++)
        {
            age += age_Change;

            RotateAroundGalaxyCenter(512 * speed);
        }
    }

    public starType Get_Star_Type()
    {
        return star_Type;
    }

    public void Update_Location(float newAge)
    {
        float dif = newAge - age;

        AnimateAge(dif);
    }

    private void Set_Name_Layer_Forward()
    {
        canvas.sortingOrder = 6;
    }

    private void Set_Name_Layer_Back()
    {
        canvas.sortingOrder = 1;
    }

    public void Star_View_On()
    {
        planet_Manager.Check_System();

        sprite.sortingOrder = 5;
        Set_Name_Layer_Forward();
        Set_Scale(2);
    }

    public void System_View_On()
    {
        planet_Manager.View_System();

        sprite.sortingOrder = 4;
        Set_Name_Layer_Forward();
        Set_Scale(4);
    }

    public void System_View_Off()
    {
        planet_Manager.Hide_System();

        sprite.sortingOrder = 1;
        Set_Name_Layer_Back();
        Decrease_Scale();
    }

    private void Set_Scale(float newScale)
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }

        IEnumerator thing = Animate_Change_Scale(newScale);

        currentAnimation = thing;

        StartCoroutine(thing);
    }

    private void Decrease_Scale()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }

        IEnumerator thing = Animate_Change_Scale(baseScale.x);

        currentAnimation = thing;

        StartCoroutine(thing);
    }

    private IEnumerator Animate_Change_Scale(float newScale)
    {
        float ticks = 3;
        Vector3 currentScale = rect.localScale;
        Vector3 intendedScale = new Vector3(newScale, newScale, newScale);
        Vector3 dif = intendedScale - currentScale;
        Vector3 step = dif / ticks;
        
        for (int i = 0; i < ticks; i++)
        {
            rect.localScale += step;

            yield return new WaitForSeconds(.02f);
        }
    }
}
