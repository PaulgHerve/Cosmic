using UnityEngine;
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

    float scale;

    void Awake()
    {
        effects = GetComponentInChildren<Star_Effects>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponentInChildren<SphereCollider>();
        planet_Manager = GetComponentInChildren<Planet_Manager>();

        collider.enabled = false;
    }
	
	void Update ()
    {
        enable_Rotation = Galaxy.enable_Rotation;

        //Scale_To_Camera();

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

    public void Generate(float galaxySize, float armVal, float armInc, float armDensity)
    {
        float center = 512 + (12 * (galaxySize / 3));
        float x = (16f) * (6 + galaxySize / 12);
        float y = (12f) * (6 + galaxySize / 12);
        float z = (256) * (6 + galaxySize / 12);
        z *= Random.Range(-1, 1.01f);
        x *= Random.Range(-1, 1.01f);
        y *= Random.Range(-1, 1.01f);

        //Creates a space in the center for a supermassive
        if (z >= 0) { z += center; }
        else if (z < 0) { z -= center; }

        Vector3 startPos = new Vector3(x, y, z);
        star_Type = (starType)Determine_Star_Type();
        int spriteIndex = (int)star_Type;

        sprite.sprite = Galaxy_Generator.Get_Star_Sprite(spriteIndex);

        InitializeScale((starType)spriteIndex);
        SetStarEffectColors((starType)spriteIndex);

        InitializeOrbitalCenter();
        transform.localPosition = startPos;

        rDistance = transform.localPosition.magnitude;

        float percent = (360.000f * armVal);
        float roll = Random.Range(0.000f, armDensity);        
        float maxVal = (360.000f * armInc) * roll;

        percent += Random.Range(0.000f, maxVal);

        roll = Random.Range(0.000f, .510f);
        if (roll < .5f * armInc)
        {
            roll = Random.Range(0.000f, 1.001f);
            maxVal = (360.000f * armInc) * roll;

            percent += Random.Range(0, maxVal);
        }

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
        float x = 2;
        float y = 0f;
        float z = 0;

        movement = new Vector3(x, y, z);
        movement /= Mathf.Sqrt(rDistance);
    }

    private void InitializeMovement(float speed)
    {
        float x = speed;
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
        int t = (int)sType;
        Color32 glowColor = Icon_Controller.Get_Star_Haze_Color(sType);
        Color32 hazeColor = Icon_Controller.Get_Star_Glow_Color(sType);

        effects.Set_Glow(glowColor);
        effects.Set_Haze(hazeColor);
    }

    //Rotates Camera Around a centerpoint
    private void RotateAroundGalaxyCenter()
    {
        Vector3 targetPos = orbitalCenter;           

        Vector3 relative = targetPos - transform.localPosition;
        Quaternion current = transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        transform.Translate(movement);        

        transform.localRotation = Quaternion.Slerp(current, rotation, 1f);
        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, rDistance);
    }

    //Rotates Camera Around a centerpoint at a controlled speed
    private void RotateAroundGalaxyCenter(Vector3 speed)
    {
        Vector3 targetPos = orbitalCenter;

        Vector3 relative = targetPos - transform.localPosition;
        Quaternion current = transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        transform.Translate(speed);

        transform.localRotation = Quaternion.Slerp(current, rotation, 1f);
        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, rDistance);
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

        Vector3 relative = targetPos - transform.localPosition;
        Quaternion current = transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        for (int i = 0; i < degrees; i++)
        {
            for (int n = 0; n < num; n++)
            {
                relative = targetPos - transform.localPosition;
                current = transform.localRotation;
                rotation = Quaternion.LookRotation(relative);

                transform.Translate(m);

                transform.localRotation = Quaternion.Slerp(current, rotation, 1f);
                transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, rDistance);                
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

    public void View_Star_On()
    {
        planet_Manager.View_System();

        sprite.sortingOrder = 4;
    }

    public void View_Star_Off()
    {
        planet_Manager.Hide_System();

        sprite.sortingOrder = 1;
    }
}
