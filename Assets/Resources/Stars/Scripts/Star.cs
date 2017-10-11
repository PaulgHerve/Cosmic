using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public enum starType { BLACK_HOLE, BLUE_GIANT, BLUE_SUPER, PULSAR, RED_DWARF, RED_GIANT, WHITE_DWARF, YELLOW_SUN }

    private starType star_Type;
    SpriteRenderer sprite;

    public float rDistance;
    private float oscillation;
    private Vector3 movement;
    private bool isRotating = false;

    float scale;

    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start ()
    {
	
	}
	
	void Update ()
    {
        if (isRotating)
        {
            RotateAroundGalaxyCenter();
            RotateWiggle();
        }
    }

    public void Generate(float galaxySize)
    {
        int passes = 3;

        float center = 120 + (20 * (galaxySize / 3));
        oscillation = Random.Range(-1, 1.01f) * (1 + galaxySize);
        float deviation = .5f;
        float x = (15 / deviation) * (3 + galaxySize / 6);
        float y = (3f / deviation) * (3 + galaxySize / 6);
        float z = (36 / deviation) * (3 + galaxySize / 6);
        x = Random.Range(-x, x + 1);
        y = Random.Range(-y, y + 1);
        z = Random.Range(-z, z + 1);
        x *= passes;
        y *= passes;
        z *= passes;

        //One free deviation on the y axis
        y = SlopeFloat(y, deviation);

        for (int i = 0; i < passes; i++)
        {
            x = SlopeFloat(x, deviation);
            y = SlopeFloat(y, deviation);
            z = SlopeFloat(z, deviation);
        }

        //Creates a space in the center for a supermassive
        if (z > 0) { z += center; }
        if (z < 0) { z -= center; }

        Vector3 startPos = new Vector3(x, y, z);
        int spriteIndex = Determine_Star_Type();

        sprite.sprite = GalaxyGenerator.Get_Star_Sprite(spriteIndex);

        InitializeScale((starType)spriteIndex);

        transform.localPosition = startPos;

        rDistance = transform.localPosition.magnitude;

        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, rDistance);

        InitializeMovement(20);

        for (int i = 0; i < 50; i++)
        {
            RotateAroundGalaxyCenter();
        }

        InitializeMovement();

        isRotating = true;
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
        float x = Random.Range(-.02f, .03f);
        float y = 0f;
        float z = 0;

        movement = new Vector3(x, y, z);
    }

    private void InitializeMovement(float speed)
    {
        float deviation = .1f;
        float x = speed;
        float y = 0;
        float z = 0;

        x = SlopeFloat(x, deviation);

        movement = new Vector3(x, y, z);
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
        Vector3 targetPos = new Vector3(0, 0, 0);

        Vector3 relative = targetPos - transform.localPosition;
        Quaternion current = transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        transform.Translate(movement);        

        transform.localRotation = Quaternion.Slerp(current, rotation, 1);
        transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, rDistance);
    }

    private void RotateWiggle()
    {
        float currentRotation = transform.localRotation.y;
        float currentY = (currentRotation * oscillation / rDistance);

        Vector3 newPos = transform.localPosition;

        newPos.y += currentY;

        transform.localPosition = newPos;
    }

    private float SlopeFloat(float max, float deviation)
    {
        //deviation is constrained between 0 and 1;

        if (deviation > 1) { deviation = 1; }
        else if (deviation < 0) { deviation = 0; }

        float rand1 = max - Random.Range(0, (max * deviation));
        float rand2 = max - Random.Range(0, (max * deviation));

        max -= rand1;
        max += rand2 * .8f;

        return max;
    }

    public starType Get_Star_Type()
    {
        return star_Type;
    }
}
