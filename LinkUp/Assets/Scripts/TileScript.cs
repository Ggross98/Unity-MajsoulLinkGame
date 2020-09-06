using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileScript : MonoBehaviour
{

    public int id;

    public bool clear = false;

    public Button button;

    public RectTransform rt;

    public Vector2Int pos;
    public int x, y;

    private float totalSize, imageSize;

    void Awake()
    {
        button = GetComponent<Button>();
        rt = GetComponent<RectTransform>();
        imageSize = rt.rect.width;
    }

    public void SetSize(float s)
    {
        totalSize  = s;
    }

    public void SetClear(bool c)
    {
        clear = c;
        if (!c)
        {
            button.image.color = new Color(0, 0, 0, 0);
        }
        else
        {
            button.image.color = Color.white ;
        }
        
    }

    public void SetID(int i)
    {
        id = i;
        //button.image.sprite = Utils.GetSpriteOfID(id);
    }
    public void SetImage(Sprite s)
    {
        GetComponent<Image>().sprite = s;
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        pos = new Vector2Int(x, y);

        rt.anchoredPosition  = new Vector2(x*totalSize+totalSize /2, y*totalSize+ totalSize / 2);

    }
    public void SetPosition(Vector2Int v)
    {
        SetPosition (v.x, v.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
