using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapData.Preset;
public enum Map_State
{
    None = 0,
    Destructible = 1,//”j‰ó‰Â”\
    Indestructible = 2,//”j‰ó•s‰Â
}

[CreateAssetMenu(fileName = "MapData", menuName = "OriginalScriptableObjects/MapData")]
[System.Serializable]

public class MapData : ScriptableObject
{
    public Preset[] preset;
    
    [System.Serializable]
    public class Preset
    {
        public Area[] area;
        public Height[] height;

        [System.Serializable]
        public class Height
        {
            public Map_State[] width;
        }


        [System.Serializable]
        public class Area
        {
            public Vector2Int posLeftDown;
            public Vector2Int posRightUp;
        }
    }

    public MapData Clone()
    {
        return Instantiate(this);
    }
}
