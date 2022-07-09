using System;
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
            if (o.name == item)
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

        i.name = item;
        i.VerticalGroupController = this;

        if (item == Strings.itemDouble) 
        { 
            i.itemSprite = SprDouble;
            i.time = GameData.DoubledTime;
        }
        else if (item == Strings.itemMagnetic) 
        { 
            i.itemSprite = SprMagnet;
            i.time = GameData.MagneticTime;
        }
        else if (item == Strings.itemShield) 
        { 
            i.itemSprite = SprShield;
            i.time = GameData.ShieldTime;
        }
        else
        { 
            i.itemSprite = SprSlowMotion;
            i.time = GameData.SlowMotionTime;
        }

        Instantiate
        (
            obj,
            transform
        );
    }
}
