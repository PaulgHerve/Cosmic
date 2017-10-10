using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	void Awake () {
        if (isLocalPlayer)
        {
            Destroy(this);
        }
        else
        {
            Camera.main.GetComponent<CameraController>().SetPlayer(this);
        }
	}
	
	void Update ()
    {
        
	}    
}
