using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{

    public Toggle map_small, map_middle, map_large;
    public Toggle mode_classic, mode_gravity;
    public Slider ic_slider;
    public Text ic_slider_text;

    public const int MAP_SMALL = 100, MAP_MIDDLE = 101, MAP_LARGE = 102;
    public const int MODE_CLASSIC = 200, MODE_GRAVITY = 201;

    public void Awake()
    {
        ic_slider.onValueChanged.AddListener(
            delegate { ic_slider_text.text = (int)ic_slider.value + ""; }

            );
    }

    public void Start()
    {
        if (PlayerPrefs.HasKey("MAP_SIZE"))
        {
            int ms = PlayerPrefs.GetInt("MAP_SIZE");
            if(ms == MAP_SMALL)
            {
                map_small.isOn = true;
            }else if(ms == MAP_MIDDLE)
            {
                map_middle.isOn = true;
            }
            else
            {
                map_large.isOn = true;
            }
        }

        if (PlayerPrefs.HasKey("IMAGE_COUNT"))
        {
            int ms = PlayerPrefs.GetInt("IMAGE_COUNT");
            ic_slider.value = ms;
            ic_slider_text.text = ms + "";
        }

        if (PlayerPrefs.HasKey("GAME_MODE"))
        {
            int ms = PlayerPrefs.GetInt("GAME_MODE");
            if(ms == MODE_CLASSIC)
            {
                mode_classic.isOn = true;
            }
            else
            {
                mode_gravity.isOn = true;
            }
        }
    }

    public void StartGame()
    {
        int mapsize = MAP_SMALL;
        int imagecount = 10;
        int gamemode = MODE_CLASSIC;

        if (map_middle.isOn) mapsize = MAP_MIDDLE;
        else if (map_large.isOn) mapsize = MAP_LARGE;

        if (mode_classic.isOn) gamemode = MODE_CLASSIC;
        else gamemode = MODE_GRAVITY;

        imagecount = (int)ic_slider.value;

        PlayerPrefs.SetInt("MAP_SIZE", mapsize);
        PlayerPrefs.SetInt("IMAGE_COUNT", imagecount);
        PlayerPrefs.SetInt("GAME_MODE", gamemode);
        SceneManager.LoadScene("Game");
    }

    //public void Change

    public void ExitGame()
    {
        Application.Quit();
    }



}
