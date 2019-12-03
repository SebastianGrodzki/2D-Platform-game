using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //**player state**

    bool player_alive = true;

    //**configuration structure**

    [SerializeField] float run_parameter = 5f;
    [SerializeField] float climb_parameter = 5f;
    [SerializeField] float jump_parameter = 5f;
    
    [SerializeField] Vector2 jump_dead = new Vector2(25f, 25f); //recoil parameters

    //**references to cache components**

    Animator pilot;
    BoxCollider2D player_feet;
    Rigidbody2D inflexible_body;
    CapsuleCollider2D body_bumper;

    float gravity_at_start;

    //**addressing scripts and components**

    void Start ()
    {
        pilot = GetComponent<Animator>(); //steer
        player_feet = GetComponent<BoxCollider2D>();
        inflexible_body = GetComponent<Rigidbody2D>();
        body_bumper = GetComponent<CapsuleCollider2D>();

        gravity_at_start = inflexible_body.gravityScale; 
	}
	
	//**actualization > once / frame**

	void Update()
    {
        if (!player_alive)
        {
            return;
        }

        running_mechanics();
        climb_mechanics();
        jump_mechanics();

        rotation_mechanics(); // <left and right>
        dying_mechanics();
	}

    public void running_mechanics()
    {

        float control_toss = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 player_quickness = new Vector2(control_toss * run_parameter, inflexible_body.velocity.y);
        inflexible_body.velocity = player_quickness;
        print(player_quickness);

        //**player moves horizontally**

        bool player_moves_horizontally = Mathf.Abs(inflexible_body.velocity.x) > Mathf.Epsilon;
        pilot.SetBool("Running", player_moves_horizontally);
    }

    private void climb_mechanics()
    {
        if (!player_feet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            pilot.SetBool("Climbing", false);
            inflexible_body.gravityScale = gravity_at_start;
            return;
        }

        float control_toss = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climb_quickness = new Vector2(inflexible_body.velocity.x, control_toss * climb_parameter);
        inflexible_body.velocity = climb_quickness;
        inflexible_body.gravityScale = 0f;

        //**player moves vertically**

        bool player_moves_vertically = Mathf.Abs(inflexible_body.velocity.y) > Mathf.Epsilon;
        pilot.SetBool("Climbing", player_moves_vertically);
    }

    private void jump_mechanics()
    {
        if(!player_feet.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {

            //**player jump (add and sum quickness)**

            Vector2 jump_quickness = new Vector2(0f, jump_parameter);
            inflexible_body.velocity += jump_quickness;
        }
    }

    private void dying_mechanics()
    {
        if (body_bumper.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            player_alive = false;
            pilot.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = jump_dead;
            FindObjectOfType<GameSession>().player_death_control();
            //print("You are dead.");
        }
    }

    private void rotation_mechanics()
    {

        //**if player is moving horizontally...**

        bool player_moves_horizontally = Mathf.Abs(inflexible_body.velocity.x) > Mathf.Epsilon;
        if (player_moves_horizontally)
        {

            //**...inverse scaling**

            transform.localScale = new Vector2(Mathf.Sign(inflexible_body.velocity.x), 1f);
        }
    }

}
