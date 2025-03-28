﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class VerticalGroupController : MonoBehaviour
{
    [SerializeField] GameObject ItemBoxPrefab;

    [SerializeField] Sprite SprDouble;
    [SerializeField] Sprite SprShield;
    [SerializeField] Sprite SprMagnet;
    [SerializeField] Sprite SprSlowMotion;

    public void AddItemBox(string item)
    {
        ItemBoxController i = ReturnIfExistItemBox(item);
        if (i != null) i.SetSliderValue(1f);
        else AddObject(item);
    }

    ItemBoxController ReturnIfExistItemBox(string item)
    {
        ItemBoxController obj = null;
        for(int i = 0; i < transform.childCount; i++)
        {
            ItemBoxController o = transform.GetChild(i).GetComponent<ItemBoxController>();
            if (o.itemBoxName == item)
            { 
                obj = o;
                break;
            }
        }
        return obj;
    }

    private void AddObject(string item)
    {
        GameObject obj = ItemBoxPrefab;
        ItemBoxController i = obj.GetComponent<ItemBoxController>();

        i.itemBoxName = item;
        i.VerticalGroupController = this;

        if (item == GameData.Doubled.Name) 
        { 
            i.itemSprite = SprDouble;
            i.time = GameData.Doubled.Time;
        }
        else if (item == GameData.Magnetic.Name) 
        { 
            i.itemSprite = SprMagnet;
            i.time = GameData.Magnetic.Time;
        }
        else if (item == GameData.Shield.Name) 
        { 
            i.itemSprite = SprShield;
            i.time = GameData.Shield.Time;
        }
        else
        { 
            i.itemSprite = SprSlowMotion;
            i.time = GameData.SlowMotion.Time;
        }

        Instantiate
        (
            obj,
            transform
        );
    }
}
