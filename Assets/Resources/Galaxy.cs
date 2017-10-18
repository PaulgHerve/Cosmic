using UnityEngine;
using System.Collections;

public class Galaxy : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
        Rotate();
	}

    void Rotate()
    {
        transform.Rotate(0, -.1f, 0, Space.Self);
    }
}
