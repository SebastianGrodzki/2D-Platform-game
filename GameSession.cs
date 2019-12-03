using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour {

    //**configuration structure**

    [SerializeField] int player_chances = 3;
    [SerializeField] int result = 0;

    [SerializeField] Text player_chances_text;
    [SerializeField] Text result_text;

    //**number game sessions**

    private void wake_up()
    {
        int number_game_sessions = FindObjectsOfType<GameSession>().Length;
        if (number_game_sessions > 1)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    //**use this for initialization**

    void Start ()
    {
        //**convert results to text**

        player_chances_text.text = player_chances.ToString();
        result_text.text = result.ToString();
	}
	
    public void points_result (int add_points)
    {
        result += add_points;
        result_text.text = result.ToString();
    }

    //**conditions for granting further opportunities**

    public void player_death_control()
    {
        if (player_chances > 1)
        {
            take_chance();
        }
        else
        {
            reset_start_again();
        }
    }

    //**chances**

    private void take_chance()
    {
        player_chances--;
        var active_scene_index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(active_scene_index);
        player_chances_text.text = player_chances.ToString();
    }

    //**try again**

    private void reset_start_again()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
