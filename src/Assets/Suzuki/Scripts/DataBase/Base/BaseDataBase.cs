using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDataBase<T> : ScriptableObject where T : BaseData
{
    [SerializeField] List<T> itemList = new List<T>();

    // プロパティ
    public List<T> ItemList
    {
        get { return itemList; }
    }
}
