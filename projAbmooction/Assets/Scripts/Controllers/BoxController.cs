using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class BoxController : MonoBehaviour
{
    [SerializeField] GameObject BoxImage;
    [SerializeField] GameObject BoxLabel;
    [SerializeField] Sprite[] BoxSprites;

    Box Box;
    int ID;

    public void SetBox(Box box, int id)
    {
        ID = id;
        if (box == null) ClearBox();
        else SetTypedBox(box);
    }


    private void ClearBox()
    {
        UIManager.SetImage(BoxImage, BoxSprites[3]);
        UIManager.SetText(BoxLabel, Strings.Empty);
        Box = null;
    }

    private void SetTypedBox(Box box)
    {
        Box = box;
        if (!box.Active) UIManager.SetText(BoxLabel, Strings.Open);
        else
        {
            CalculateTime();
            InvokeRepeating(nameof(DecreaseTime), 1f, 1f);
        }
        UIManager.SetImage(BoxImage, BoxSprites[box.Type - 1]);
    }

    private void CalculateTime()
    {
        TimeSpan actualTime = Box.EndTime.ToDateTime() - (GameData.DateTimeNow - new TimeSpan(3, 0, 0));
        TimeSpan decreaseMilliseconds = new TimeSpan(0, 0, 0, 0, actualTime.Milliseconds);
        Box.ActualTime = actualTime - decreaseMilliseconds;
    }

    private void DecreaseTime()
    {
        if(Box.ActualTime.TotalSeconds > 1)
        {
            Box.ActualTime.Subtract(new TimeSpan(0, 0, 1));
            UIManager.SetText(BoxLabel, Box.ActualTime.ToString());
        }
        else
        {
            CancelInvoke();
            Box.Active = true;
            UIManager.SetText(BoxLabel, Strings.GetReward);
        }
    }
}
