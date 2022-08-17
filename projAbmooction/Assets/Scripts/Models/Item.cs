using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Item
{
    public float Time { get; set; }
    public int Level { get; set; }
    public int Price { get; set; }

    public int ID { get; set; }

    public string Name { get => Strings.items[ID]; }

    public Item(int id, int level, int price, float time)
    {
        ID = id;
        Time = time;
        Price = price;
        Level = level;
    }
}
