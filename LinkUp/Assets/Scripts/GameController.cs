using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public const int MAP_SMALL=100, MAP_MIDDLE=101, MAP_LARGE=102;
    private int mapSize = MAP_SMALL;
    private int imageCount = 10;
    private float tileSize=100;

    private TileScript[,] tileArray;
    private int[,] map;

    private int maxX, maxY;
    private TileScript  selectedTile;

    private GameObject tilePrefab;
    private GameObject selectFrame;

    public Sprite s0, s1, s2, s3, s4, s5, s6, s7, s8, s9,s10,s11,s12,s13,s14;
    private Sprite[] sprites;

    private bool gaming = false;
    private bool pausing = false;

    private float time = 0;

    private LinePainter linePainter;
    private LineInfo lineInfo;
    private float lineTime = 0;

    public const int MODE_CLASSIC = 200, MODE_GRAVITY = 201;
    private int gameMode = MODE_GRAVITY;

    void Start()
    {

        sprites = new Sprite[20];
        sprites[0] = s0;
        sprites[1] = s1;
        sprites[2] = s2;
        sprites[3] = s3;
        sprites[4] = s4;
        sprites[5] = s5;
        sprites[6] = s6;
        sprites[7] = s7;
        sprites[8] = s8;
        sprites[9] = s9;
        sprites[10] = s10;
        sprites[11] = s11;
        sprites[12] = s12;
        sprites[13] = s13;
        sprites[14] = s14;
        GameObject.Find("Start").GetComponent<Button>().onClick.AddListener(Restart);
        GameObject.Find("Exit").GetComponent<Button>().onClick.AddListener(

            delegate { SceneManager.LoadScene("Menu"); }
            
            );
        //int ms, ic;
        if(PlayerPrefs .HasKey("MAP_SIZE"))
        {
            mapSize  = PlayerPrefs.GetInt("MAP_SIZE");
        }
        if(PlayerPrefs .HasKey("IMAGE_COUNT"))
        {
            imageCount = PlayerPrefs.GetInt("IMAGE_COUNT");
        }
        if(PlayerPrefs .HasKey("GAME_MODE"))
        {
            gameMode = PlayerPrefs.GetInt("GAME_MODE");
        }
        linePainter = GameObject.Find("LinePainter").GetComponent<LinePainter>();

        StartGame(mapSize , imageCount ,gameMode);
    }

    void Update()
    {
        if (!gaming) return;

        CheckWin();
        if (!gaming) return;

        if(Input.GetMouseButtonUp(1))
        {
            CancelSelect();
        }

        float dt;
        if(pausing)
        {
            dt = 0;
        }
        else
        {
            dt = Time.deltaTime;
        }
        

        time += dt;
        int minute = (int)time/60;
        int second = (int)(time-minute*60);

        //GameObject.Find("TimePanel").GetComponent<Text>().text = "Time  " + time;
        string showtime1 = string.Format("{0:D2}", minute);
        string showtime2 = string.Format("{0:D2}", second);
        //Debug.Log(showtime);
        GameObject .Find("TimePanel").GetComponent <Text>().text = "Time  "+ showtime1+":"+showtime2  ;

        if(lineTime <= 0)
        {
            linePainter.ClearLines();
        }
        else
        {
            lineTime -= Time.deltaTime;
        }
    }


    public void StartGame(int ms, int ic, int gm)
    {
        this.mapSize = ms;
        this.imageCount = ic;
        tileSize = Utils.GetTileSize(ms);

        gameMode = gm;

        string m;
        if (gm == MODE_CLASSIC) m = "经典";
        else m = "重力";
        GameObject.Find("GameModeText").GetComponent<Text>().text = "模式："+m+"    图片数量"+ic;
        

        maxX = 1600 / (int)tileSize;
        maxY = 800 / (int)tileSize;

        map = new int[maxX + 2, maxY + 2];

        tilePrefab = Utils.GetTilePrefab(ms);
        CreateTiles();

        selectFrame = GameObject.Find("SelectFrame");
        selectFrame.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        selectFrame.GetComponent<RectTransform>().SetAsLastSibling();
        selectFrame.GetComponent<RectTransform>().sizeDelta = new Vector2(tileSize, tileSize);

        gaming = true;

        GameObject.Find("WinText").GetComponent<Text>().text = "";

        Vector2[] pos = new Vector2[2];
        pos[0] = new Vector2(100, 100);
        pos[1] = new Vector2(200, 100);
        
        lineInfo = new LineInfo();

        linePainter.PaintLine(1, pos);
    }

    public void Restart()
    {
        CreateTiles();
        time = 0;
        gaming = true;
        pausing = false;
        GameObject.Find("WinText").GetComponent<Text>().text = "";
    }


    public void CheckWin()
    {
        for(int i = 0; i < maxX; i++)
        {
            for(int j = 0; j < maxY; j++)
            {
                if (tileArray [i,j].button.interactable ) return;
            }
        }

        GameObject.Find("WinText").GetComponent<Text>().text = "完成！";
        gaming = false;
    }


    public void CreateTiles()
    {
        if(tileArray == null)
        {
            tileArray = new TileScript[maxX, maxY];

            for(int i = 0; i < maxX; i++)
            {
                for(int j =0; j < maxY; j++)
                {
                    GameObject newTile = Instantiate(tilePrefab, GameObject.Find("Board").transform);
                    //newTile.transform.SetParent(GameObject.Find("Board").transform);
                    TileScript ts = newTile.GetComponent<TileScript>();
                    ts.SetSize(Utils.GetTileSize(mapSize ));
                    ts.SetPosition(i,j);
                    ts.button.image.sprite = s0;
                    ts.button.onClick.AddListener(

                        delegate { SelectTile(ts); }

                        );
                    tileArray[i, j] = ts;
                }
            }
        }
        else
        {
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    tileArray[i, j].button.interactable = true;
                }
            }
                }
        List<int> source = new List<int>(maxX*maxY);
        for (int i = 0; i < maxX * maxY; i++)
        {
            source.Add (i);
        }
        
        //剩余位置
        int left = maxX * maxY;

        while (left > 0)
        {
            for (int id = 0; id < imageCount; id++)//各种图片轮流
            {
                Debug.Log("id:" + id);

                for(int j = 0; j < 2; j++)//改变两个位置
                {
                    //随机选取可用的位置
                    int index = Random.Range(0, left);
                    int pos = source[index];
                    //将对应方块改变
                    tileArray[pos / maxY, pos % maxY].SetID(id);
                    tileArray[pos / maxY, pos % maxY].SetImage(sprites[id]);
                    source.RemoveAt(index);
                    left--;
                }
               
                if (left <= 0)
                {
                    break;
                }
            }
        }

        
    }


    //点击一个按钮时
    public void SelectTile(TileScript ts)
    {
        if (!gaming) return;
        if (pausing) return;

        //没有选中时，选中该按钮
        if(selectedTile == null)
        {
            //Debug.Log("选中");
            selectedTile = ts;

            selectFrame.GetComponent<Image>().color = Color.white;
            selectFrame.GetComponent<RectTransform>().anchoredPosition = ts.GetComponent<RectTransform>().anchoredPosition;
            //selectedTile.
        }
        else
        {
            //如果重复点击一个按钮，取消选择
            if (ts == selectedTile)
            {
                Debug.Log("重复！");
                CancelSelect();
                return;
            }
            else
            {
                //能够按规则连接则消除
                if(CanConnect (selectedTile , ts))
                {
                    selectedTile.button.interactable = false;
                    ts.button.interactable = false;
                    
                    if (gameMode == MODE_GRAVITY)
                    {
                        if(selectedTile .y > ts.y)
                        {
                            Gravitiy(selectedTile);
                            Gravitiy(ts);
                        }
                        else
                        {
                            
                            Gravitiy(ts);
                            Gravitiy(selectedTile);
                        }

                        
                    }

                    else
                        StartCoroutine(PaintLine());

                    CancelSelect();

                }
                //否则取消选择
                else
                {
                    CancelSelect();
                    return;
                }
            }

            
        }
    }

    public void PauseButton()
    {
        if (!pausing)
        {
            GameObject.Find("Pause").GetComponentInChildren<Text>().text = "继续";
            pausing = true;
        }
        else
        {
            GameObject.Find("Pause").GetComponentInChildren<Text>().text = "暂停";
            pausing = false;
        }
    }


    public IEnumerator PaintLine()
    {
        linePainter.ClearLines();

        Debug.Log("共有" + lineInfo.lines + "条线");
        for(int i = 0; i <= lineInfo.lines; i++)
        {
            Debug.Log(lineInfo.pos[i]);
        }
        linePainter.PaintLine(lineInfo.lines, lineInfo.pos);
        lineTime = 1f;
        yield return null;
    }



    public void CancelSelect()
    {
        selectedTile = null;
        selectFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100, -100);
        selectFrame.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    

    protected bool  CanConnect(TileScript t1, TileScript t2)
    {
        

        if (t1.id != t2.id) return false;

        /*
        for(int i = 0; i < maxX + 2; i++)
        {
            for(int j = 0; j < maxY + 2; j++)
            {
                map[i, j] = -1;
            }
        }
        for(int i = 0; i < maxX; i++)
        {
            for(int j = 0; j < maxY; j++)
            {
                if(tileArray[i,j].button .interactable )
                    map[i + 1, j + 1] = tileArray[i, j].id;
                else
                {
                    map[i + 1, j + 1] = -1;
                }
            }
        }*/

        RefreshMap();

        bool can = false;
        if (CanConnect_OneLine(t1.x + 1, t1.y + 1, t2.x + 1, t2.y + 1)) {
            lineInfo.lines = 1;
            can =  true;
        }
        else if (CanConnect_TwoLine(t1.x + 1, t1.y + 1, t2.x + 1, t2.y + 1)) {

            lineInfo.lines = 2;
            can =  true;
        }
        else if(CanConnect_ThreeLine(t1.x + 1, t1.y + 1, t2.x + 1, t2.y + 1)){
            lineInfo.lines = 3;
            can = true;
        }
            
        return can;
    }

    public Vector2 GetPositionFromArray(int x, int y)
    {
        float _x = (x - 1) * tileSize + tileSize/2;
        float _y;
        if(mapSize ==MAP_LARGE )
           _y = (y - 1) * tileSize+tileSize *1.5f;
        else
            _y = (y - 1) * tileSize + tileSize;
        return new Vector2(_x,_y);
    }



    public void Gravitiy(TileScript ts)
    {
        RefreshMap();
        int x = ts.x;
        int y = ts.y;

        
        for(int i = y; i < maxY; i++)
        {
            if(i == maxY - 1)
            {
                //tileArray[x, i] = ts;
                tileArray[x, i].button.interactable = false;
            }
            else
            {
                //tileArray[x, i] = tileArray[x, i + 1];
                int id = map[x+1,i+2];//上面一格
                if (id == -1)
                {
                    tileArray[x, i].button.interactable = false;
                }
                else
                {
                    tileArray[x, i].SetID(id);
                    tileArray[x, i].SetImage(sprites[id]);
                    //tileArray[x, i].SetPosition(x, i);
                    tileArray[x, i].button.interactable = true;
                }

            }
        }
        /*
        int id = map[x + 1, y+2];//上面一格
        if(id == -1)
        {

        }
        else
        {
            tileArray[x, y].SetID(id);
            tileArray[x, y].SetImage(sprites[id]);
            //tileArray[x, i].SetPosition(x, i);
            tileArray[x, y].button.interactable = true;
        }
        */

    }

    public void RefreshMap()
    {
        for (int i = 0; i < maxX + 2; i++)
        {
            for (int j = 0; j < maxY + 2; j++)
            {
                map[i, j] = -1;
            }
        }
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                if (tileArray[i, j].button.interactable)
                    map[i + 1, j + 1] = tileArray[i, j].id;
                else
                {
                    map[i + 1, j + 1] = -1;
                }
            }
        }
    }



    public bool CanConnect_OneLine(int x1, int y1, int x2, int y2)
    {
        //int x1 = t1.x + 1; int x2 = t2.x + 1;
        //int y1 = t1.y + 1; int y2 = t2.y + 1;

        //一条折线
        bool oneline = true;
        if (x1 == x2)
        {
            for (int i = Mathf.Min(y1, y2) + 1; i < Mathf.Max(y1,y2); i++)
            {
                if (map[x1, i] != -1)
                {
                    oneline = false;
                    break;
                }
            }
            
        }else if(y1 == y2)
        {
            for (int i = Mathf.Min(x1, x2) + 1; i < Mathf.Max(x1, x2); i++)
            {
                if (map[i, y1] != -1)
                {
                    oneline = false;
                    break;
                }
            }
        }
        else
        {
            oneline = false;
        }

        lineInfo.pos[0] = GetPositionFromArray(x1,y1);
        lineInfo.pos[1] = GetPositionFromArray(x2, y2);


        return oneline;
    }

    public bool CanConnect_TwoLine(int x1, int y1, int x2, int y2)
    {
        bool can = false;
        
        if((map[x2,y1]==-1&&CanConnect_OneLine (x1,y1,x2,y1)&&CanConnect_OneLine(x2, y2, x2, y1))){
            lineInfo.pos[0] = GetPositionFromArray(x1, y1);
            lineInfo.pos[1] = GetPositionFromArray(x2, y1);
            lineInfo.pos[2] = GetPositionFromArray(x2, y2);

            can = true;
        }
        else if((map[x1, y2] == -1&&CanConnect_OneLine(x1, y1, x1, y2) && CanConnect_OneLine(x2, y2, x1, y2)))
        {
            lineInfo.pos[0] = GetPositionFromArray(x1, y1);
            lineInfo.pos[1] = GetPositionFromArray(x1, y2);
            lineInfo.pos[2] = GetPositionFromArray(x2, y2);

            can = true;
        }

        return can;
    }

    public bool CanConnect_ThreeLine(int x1, int y1, int x2, int y2)
    {
        
        for(int i = 0; i < maxX + 2; i++)
        {
            if (i == x1) continue;
            if (map[i, y1] != -1) continue;
            if(CanConnect_OneLine (x1,y1,i,y1)&&CanConnect_TwoLine(i, y1, x2, y2))
            {
                lineInfo.pos[3] = GetPositionFromArray(x1, y1);
                
                return true;
            }
        }

        for(int i = 0; i < maxY + 2; i++)
        {
            if (i == y1) continue;
            if (map[x1,i] != -1) continue;
            if (CanConnect_OneLine(x1, y1, x1,i) && CanConnect_TwoLine(x1,i, x2, y2))
            {
                lineInfo.pos[3] = GetPositionFromArray(x1, y1);
                return true;
            }
        }

        return false;
    }
}



