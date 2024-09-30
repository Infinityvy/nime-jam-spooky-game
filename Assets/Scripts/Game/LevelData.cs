using Models.Items;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class LevelData
{
    public LevelData(int money, int quota, int dueIn, List<(BaseItem, Vector3)> storedItems)
    {
        this.money = money;
        this.quota = quota;
        this.dueIn = dueIn;
        this.storedItems = storedItems;
    }

    public int money;
    public int quota;
    public int dueIn;
    public List<(BaseItem, Vector3)> storedItems;
}
