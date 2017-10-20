using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public enum starType { BLACK_HOLE, BLUE_GIANT, BLUE_SUPER, PULSAR, RED_DWARF, RED_GIANT, WHITE_DWARF, YELLOW_SUN }

    private starType star_Type;
    SpriteRenderer sprite;

    private float rDistance;
    private Vector3 orbitalCenter;
    private float oscillation;
    private Vector3 movement;
    private bool enable_Rotation;

    float scale;

    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
	
	void Update ()
    {
        enable_Rotation = Galaxy.enable_Rotation;

        if (enable_Rotation)
        {
            RotateAroundGalaxyCenter();
            //RotateWiggle();
        }
    }

    public void Generate(float galaxySize, float armVal)
    {
        int passes = 5;

        float center = 48 + (10 * (galaxySize / 3));
        oscillation = Random.Range(-1, 1.01f) * (1 + galaxySize);
        float deviation = .5f;
        float starDensity = .5f;
        float armWidth = (3 / deviation) * (3 + galaxySize / 12);
        float x = (2f) * (3 + galaxySize / 12);
        float y = (4f) * (3 + galaxySize / 12);
        float z = (48f) * (3 + galaxySize / 12);
        z *= Random.Range(0, 1.01f);
        x *= Random.Range(-1, 1.01f);
        y *= Random.Range(-1, 1.01f);

        for (int i = 0; i < passes; i++)
        {
            //x = PullToPoint(x, 0, armWidth, starDensity);
            //y = PullToPoint(y, 0, .25f, starDensity);
            //z = PullToPoint(z, center, .25f, starDensity);
        }

        //Creates a space in the center for a supermassive
        if (z >= 0) { z += center; }
        else if (z < 0) { z -= center; }

        Vector3 startPos = new Vector3(x, y, z);
        int spriteIndex = Determine_Star_Type();

        sprite.sprite = GalaxyGenerator.Get_Star_Sprite(spriteIndex);

        InitializeScale((starType)spriteIndex);
        InitializeOrbitalCenter();
        transform.localPosition = startPos;

        rDistance = transform.localPosition.magnitude;

        //transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, rDistance);

        float percent = 360.00f * armVal;
        //float r = Random.Range(0.00f, percent);
        IEnumerator thing = RotateAroundGalaxyCenter(percent);
        StartCoroutine(thing); 

        InitializeMovement();
    }

    private int Determine_Star_Type()
    {
        int index = 0;
        float roll = Random.Range(0, 101);

        if (roll < 6) { index = 0; }
        else if (roll < 16) { index = 1; }
        else if (roll < 26) { index = 2; }
        else if (roll < 31) { index = 3; }
        else if (roll < 41) { index = 4; }
        else if (roll < 46) { index = 5; }
        else if (roll < 56) { index = 6; }
        else { index = 7; }

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

        if (t == 0)      { scaleMult = .15f; }                  //Black Hole
        else if (t == 1) { scaleMult = .50f; }                  //Blue 
        else if (t == 2) { scaleMult = .80f; }                  //Blue Super
        else if (t == 3) { scaleMult = .20f; }                  //Pulsar
        else if (t == 4) { scaleMult = .25f; }                  //Red Dwarf
        else if (t == 5) { scaleMult = 1.0f; }                  //Red Giant
        else if (t == 6) { scaleMult = .2f; }                   //White Dwarf
        else if (t == 7) { scaleMult = .40f; }                  //Yellow

        scaler *= scaleMult;

        sprite.transform.localScale *= scaler;
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

    //Rotates Camera Around a centerpoint
    private IEnumerator RotateAroundGalaxyCenter(float degrees)
    {
        //Constrains degrees to between 0 - 360
        if (degrees < 0)                    { degrees = 0; }
        if (degrees > 360)                  { degrees = 360; }

        float move = (rDistance / 10) / 3;
        Vector3 m = new Vector3(-move, 0, 0);
        Vector3 targetPos = orbitalCenter;

        Vector3 relative = targetPos - transform.localPosition;
        Quaternion current = transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        for (float i = 0.0f; i < degrees; i += .1f)
        {
            transform.Translate(m);

            transform.localRotation = Quaternion.Slerp(current, rotation, 1f);
            transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, rDistance);

            yield return new WaitForSeconds(.001f);
        }
    }

    public void Rotate90Degrees()
    {
        RotateAroundGalaxyCenter(90f);
    }

    private void RotateWiggle()
    {
        float currentRotation = transform.localRotation.y;
        float currentY = (currentRotation * oscillation / rDistance);

        Vector3 newPos = transform.localPosition;

        newPos.y += currentY;

        transform.localPosition = newPos;
    }

    private float PullToPoint(float currentVal, float center, float reduction, float probability)
    {
        //probability is constrained between 0 and 1;
        //reduction is constrained between 0 and 1;

        float newVal = currentVal; 
        int roll = Random.Range(0, 100);

        if (probability > 1) { probability = 1; }
        else if (probability < 0) { probability = 0; }

        if (reduction > 1) { reduction = 1; }
        else if (reduction < 0) { reduction = 0; }

        if (probability > roll)
        {
            newVal -= (currentVal - center) * reduction;
        }

        return newVal;
    }

    public starType Get_Star_Type()
    {
        return star_Type;
    }
}
