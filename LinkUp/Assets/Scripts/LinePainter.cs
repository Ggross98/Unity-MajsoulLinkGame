using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinePainter : MonoBehaviour
{
    public Image l1, l2, l3;

    public float width;


    public void PaintLine(int lines, Vector2[] pos)
    {
        if (lines < 1 || lines > 3) return;

        if(lines == 1)
        {
            PaintLine(l1, pos[0], pos[1]);
        }
        else if(lines == 2)
        {
            PaintLine(l1, pos[0], pos[1]);
            PaintLine(l2, pos[1], pos[2]);
        }
        else
        {
            PaintLine(l1, pos[0], pos[1]);
            PaintLine(l2, pos[1], pos[2]);
            PaintLine(l3, pos[0], pos[3]);
        }
    }

    public void ClearLines()
    {
        l1.transform.position = new Vector3(2000, 2000);
        l2.transform.position = new Vector3(2000, 2000);
        l3.transform.position = new Vector3(2000, 2000);
    }

    public void PaintLine(Image l, Vector2 p1, Vector2 p2)
    {
        l.rectTransform.anchoredPosition  = p1;
        float z;
        if(p1.x == p2.x)
        {
            if (p1.y > p2.y) z = 180;
            else z = 0;
        }
        else
        {
            if (p1.x > p2.x) z = 90;
            else z = -90;
        }

        l.rectTransform.rotation = Quaternion .AngleAxis (z,Vector3.forward );

        //l.transform.localRotation = Quaternion.AngleAxis(GetAngle(p1,p2), Vector3.forward);
        float distance = Vector2.Distance(p1,p2);
        l.rectTransform.sizeDelta = new Vector2(width, distance);
        l.transform.SetAsLastSibling();
    }

    public float GetAngle(Vector2 pa, Vector2 pb)
    {
        var dir = pb - pa;
        var dirV2 = new Vector2(dir.x, dir.y);
        var angle = Vector2.SignedAngle(dirV2, Vector2.down);
        return angle;
    }
}
