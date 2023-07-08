using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CursorImageType
{
    Default,
    PalmNormal,
    PalmHighlight
}


public static class CursorHelper
{





    public static void SetVisible(bool enable)
    {
        Cursor.visible = enable;
    }


    public static void SetImage(CursorImageType imageType)
    {
        

    }


}
