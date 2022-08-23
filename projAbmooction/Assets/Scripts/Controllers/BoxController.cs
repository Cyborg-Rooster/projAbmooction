using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class BoxController : MonoBehaviour
{
    [SerializeField] GameObject BoxImage;
    [SerializeField] GameObject BoxLabel;
    [SerializeField] Sprite[] BoxSprites;
    [SerializeField] DialogBoxBuilderController Builder;

    Box Box;
    int ID;

    public void SetBox(Box box, int id)
    {
        ID = id;
        if (box == null) ClearBox();
        else SetTypedBox(box);
    }

    public void OnClick()
    {
        if (Box == null) StartCoroutine(OnBoxIsNull());
        else
        {
            if (Box.Active)
            {
                if (Box.ActualTime.TotalSeconds > 1) StartCoroutine(OnBoxAreActive());
                else
                {
                    //open the box
                }
            }
            else StartCoroutine(OnBoxAreDisactive());
        }
    }

    private IEnumerator OnBoxIsNull()
    {
        yield return Builder.ShowImage(Strings.lblSlots, Strings.BuyRegularBox, Strings.yes, Strings.no, BoxSprites[0]);
        if(Builder.LastButtonState == ButtonPressed.Yes)
        {
            if(GameData.Coins >= 5000)
            {
                GameData.Coins -= 5000;
                Box box = new Box()
                {
                    ID = this.ID,
                    Type = 1,
                    EndTime = new Firebase.Firestore.Timestamp()
                };

                FirebaseManager.SaveBox(box);
                SetBox(box, ID);
                GameData.Save();
            }
        }
    }

    private IEnumerator OnBoxAreActive()
    {
        yield return Builder.ShowImage
        (
            Strings.lblSlots, Strings.SeeAnADAndDecreaseTime, Strings.yes, Strings.no, BoxSprites[0]
        );
        if (Builder.LastButtonState == ButtonPressed.Yes)
        {

        }
    }

    private IEnumerator OnBoxAreDisactive()
    {
        yield return Builder.ShowImage
        (
            Strings.lblSlots, Strings.OpenBox, Strings.yes, Strings.no, BoxSprites[ID]
        );

        if(Builder.LastButtonState == ButtonPressed.Yes)
        {
            Box.Active = true;
            SetBox(Box, ID);
        }
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
        TimeSpan decreaseMilliseconds = TimeSpan.FromMilliseconds(actualTime.Milliseconds);
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
