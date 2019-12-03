using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    Player player;

    //**use this for initialization**

    void Start ()
    {
        player = FindObjectOfType<Player>();
	}

    //**actualization > once / frame**

    void Update()
    {
        //*camera position**

        Vector2 newCamPos = new Vector2(player.transform.position.x, player.transform.position.y);
        transform.position = new Vector3(newCamPos.x, newCamPos.y, transform.position.z);
	}
}
