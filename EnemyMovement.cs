using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    //**enemy configuration**

    [SerializeField] float move_speed_parameter = 1f;

    //**reference**

    Rigidbody2D inflexible_body;

	//**use this for initialization**

	void Start ()
    {
        inflexible_body = GetComponent<Rigidbody2D>();
	}

    //**actualization > once / frame**

    void Update()
    {
        if (enemy_movement())
        {
            inflexible_body.velocity = new Vector2(move_speed_parameter, 0f);
        }
        else
        {
            inflexible_body.velocity = new Vector2(-move_speed_parameter, 0f);
        }
	}

    bool enemy_movement()
    {
        return transform.localScale.x > 0;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(inflexible_body.velocity.x)), 1f);
    }
}
