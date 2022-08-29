using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] GameObject BoxImage;
    [SerializeField] GameObject BoxLabel;
    [SerializeField] Sprite[] BoxSprites;
    [SerializeField] DialogBoxBuilderController Builder;
    [SerializeField] BoxRewardController RewardController;

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
                else RewardController.GetReward(Box, this);
            }
            else StartCoroutine(OnBoxAreDisactive());
        }
    }

    private IEnumerator OnBoxIsNull()
    {
        yield return Builder.ShowImage(Strings.lblSlots, Strings.BuyRegularBox, Strings.yes, Strings.no, BoxSprites[0], new Vector3(0.05f, 0.05f, 1));
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
            Strings.lblSlots, Strings.SeeAnADAndDecreaseTime, Strings.yes, Strings.no, BoxSprites[Box.Type - 1], new Vector3(0.05f, 0.05f, 1)
        );
        if (Builder.LastButtonState == ButtonPressed.Yes)
        {

        }
    }

    private IEnumerator OnBoxAreDisactive()
    {
        yield return Builder.ShowImage
        (
            Strings.lblSlots, Strings.OpenBox, Strings.yes, Strings.no, BoxSprites[Box.Type - 1], new Vector3(0.05f, 0.05f, 1)
        );

        if(Builder.LastButtonState == ButtonPressed.Yes)
        {
            Box.Active = true;
            Box.EndTime = Timestamp.FromDateTime(GameData.DateTimeNow + TimeSpan.FromHours(Box.Type));
            FirebaseManager.SaveBox(Box);
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
            InvokeRepeating("DecreaseTime", 0f, 1f);
        }
        UIManager.SetImage(BoxImage, BoxSprites[box.Type - 1]);
    }

    private void CalculateTime()
    {
        TimeSpan actualTime = (Box.EndTime.ToDateTime() - new TimeSpan(3, 0, 0)) - GameData.DateTimeNow;
        TimeSpan decreaseMilliseconds = TimeSpan.FromMilliseconds(actualTime.Milliseconds);
        Box.ActualTime = actualTime - decreaseMilliseconds;
    }

    private void DecreaseTime()
    {
        if(Box.ActualTime.TotalSeconds >= 1)
        {
            Box.ActualTime = Box.ActualTime.Subtract(TimeSpan.FromSeconds(1));

            UIManager.SetText
            (
                BoxLabel, 
                $"{Box.ActualTime.Hours:00}:{Box.ActualTime.Minutes:00}:{Box.ActualTime.Seconds:00}"
            );
        }
        else
        {
            CancelInvoke();
            Box.Active = true;
            UIManager.SetText(BoxLabel, Strings.GetReward);
        }
    }
}
