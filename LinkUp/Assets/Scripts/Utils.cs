using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static float[] BUTTON_SIZE = { 100,80,50 };

    public static float[] BUTTON_MARGIN = { 1, 2, 3 };

    public const int MAP_SMALL = 100, MAP_MIDDLE = 101, MAP_LARGE = 102;

    /*
    public static Sprite GetSpriteOfID(int id)
    {
        switch (id)
        {

        }
        return null;
    }*/

    public static float GetTileSize(int i)
    {
        return BUTTON_SIZE[i-100];
    }

    public static GameObject GetTilePrefab(int i)
    {
        if(i == MAP_SMALL)
        {
            return GameObject.Find("TilePrefab_MAP_SMALL");
        }
        else if(i ==MAP_MIDDLE)
        {
            return GameObject.Find("TilePrefab_MAP_MIDDLE");
        }
        else
        {
            return GameObject.Find("TilePrefab_MAP_LARGE");
        }
        
    }
}
