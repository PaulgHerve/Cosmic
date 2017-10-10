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

    public float cY;
    public float cR;

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
        float center = 20 + (50 * (galaxySize / 3));
        oscillation = Random.Range(-1, 1.01f) * (1 + galaxySize);
        int passes = 2;
        float deviation = .5f;
        float x = (10 / deviation) * (3 + galaxySize / 3);
        float y = (.25f / deviation) * (3 + galaxySize / 3);
        float z = (25 / deviation) * (3 + galaxySize / 3);
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
        int spriteIndex = Random.Range(0, System.Enum.GetNames(typeof(starType)).Length);

        sprite.sprite = GalaxyGenerator.Get_Star_Sprite(spriteIndex);

        transform.position = startPos;
        rDistance = Mathf.Abs(z);

        transform.localPosition = Vector3.ClampMagnitude(transform.position, rDistance);

        InitializeMovement();

        isRotating = true;
    }

    private void InitializeMovement()
    {
        int passes = 2;
        float deviation = .1f;
        float x = 1f;
        float y = 0;
        float z = 0;

        for (int i = 0; i < passes; i++)
        {
            x = SlopeFloat(x, deviation);
        }

        movement = new Vector3(x, y, z);
    }

    //Rotates Camera Around a centerpoint
    private void RotateAroundGalaxyCenter()
    {
        Vector3 targetPos = new Vector3(0, 0, 0);

        Vector3 relative = targetPos - transform.position;
        Quaternion current = transform.localRotation;
        Quaternion rotation = Quaternion.LookRotation(relative);

        transform.Translate(movement);        

        transform.localRotation = Quaternion.Slerp(current, rotation, 1);
        transform.localPosition = Vector3.ClampMagnitude(transform.position, rDistance);
    }

    private void RotateWiggle()
    {
        float currentRotation = transform.localRotation.y;
        float currentY = (currentRotation * oscillation / rDistance);

        Vector3 newPos = transform.localPosition;

        newPos.y += currentY;
        cY = currentY;
        cR = currentRotation;

        transform.position = newPos;
    }

    private float SlopeFloat(float max, float deviation)
    {
        //deviation is constrained between 0 and 1;

        if (deviation > 1) { deviation = 1; }
        else if (deviation < 0) { deviation = 0; }

        float rand1 = max - Random.Range(0, (max * deviation));
        float rand2 = max - Random.Range(0, (max * deviation));

        max -= rand1;
        max += rand2;

        return max;
    }

    public starType Get_Star_Type()
    {
        return star_Type;
    }
}
