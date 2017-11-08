using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public enum starType { BLACK_HOLE, BLUE_GIANT, BLUE_SUPER, PULSAR, RED_DWARF, RED_GIANT, WHITE_DWARF, YELLOW_SUN }

    private starType star_Type;
    SpriteRenderer sprite;
    SphereCollider collider;

    private float rDistance;
    private Vector3 orbitalCenter;
    private float oscillation;
    private Vector3 movement;
    private bool enable_Rotation;

    float scale;

    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponentInChildren<SphereCollider>();

        collider.enabled = false;
    }
	
	void Update ()
    {
        enable_Rotation = Galaxy.enable_Rotation;

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
        float center = 48 + (10 * (galaxySize / 3));
        oscillation = Random.Range(-1, 1.01f) * (1 + galaxySize);
        float x = (3f) * (3 + galaxySize / 12);
        float y = (2f) * (3 + galaxySize / 12);
        float z = (64) * (3 + galaxySize / 12);
        z *= Random.Range(-1, 1.01f);
        x *= Random.Range(-1, 1.01f);
        y *= Random.Range(-1, 1.01f);

        //Creates a space in the center for a supermassive
        if (z >= 0) { z += center; }
        else if (z < 0) { z -= center; }

        Vector3 startPos = new Vector3(x, y, z);
        int spriteIndex = Determine_Star_Type();

        sprite.sprite = Galaxy_Generator.Get_Star_Sprite(spriteIndex);

        InitializeScale((starType)spriteIndex);
        InitializeOrbitalCenter();
        transform.localPosition = startPos;

        rDistance = transform.localPosition.magnitude;

        float percent = (360.00f * armVal);
        float roll = Random.Range(0, armDensity);        
        float maxVal = (360.00f * armInc) * roll;

        percent += Random.Range(0, maxVal);

        roll = Random.Range(0.00f,.51f);
        if (roll < .5f * armInc)
        {
            roll = Random.Range(0, 1.01f);
            maxVal = (360.00f * armInc) * roll;

            percent += Random.Range(0, maxVal);
        }

        IEnumerator thing = RotateAroundGalaxyCenter(percent);
        StartCoroutine(thing);         
    }

    private int Determine_Star_Type()
    {
        int index = 0;
        float roll = Random.Range(0, 101);

        if (roll < 6) { index = 0; }                    //Black Hole
        else if (roll < 16) { index = 1; }              //Blue
        else if (roll < 21) { index = 2; }              //Blue Super
        else if (roll < 26) { index = 3; }              //Pulsar
        else if (roll < 41) { index = 4; }              //Red Dwarf
        else if (roll < 44) { index = 5; }              //Red Giant
        else if (roll < 56) { index = 6; }              //White Dwarf
        else { index = 7; }                             //Yellow

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

        if (t == 0)      { scaleMult = .30f; }                  //Black Hole
        else if (t == 1) { scaleMult = .50f; }                  //Blue 
        else if (t == 2) { scaleMult = .80f; }                  //Blue Super
        else if (t == 3) { scaleMult = .50f; }                  //Pulsar
        else if (t == 4) { scaleMult = .25f; }                  //Red Dwarf
        else if (t == 5) { scaleMult = .90f; }                  //Red Giant
        else if (t == 6) { scaleMult = .25f; }                  //White Dwarf
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

            //yield return new WaitForSeconds(.005f);            
        }

        //yield return new WaitForSeconds(.25f);

        InitializeMovement();

        for (int i = 0; i < 240; i++)
        {
            for (int n = 0; n < 10; n++)
            {
                RotateAroundGalaxyCenter();
            }

            yield return new WaitForSeconds(.01f);
        }
    }

    private void RotateWiggle()
    {
        float currentRotation = transform.localRotation.y;
        float currentY = (currentRotation * oscillation / rDistance);

        Vector3 newPos = transform.localPosition;

        newPos.y += currentY;

        transform.localPosition = newPos;
    }

    public starType Get_Star_Type()
    {
        return star_Type;
    }
}
