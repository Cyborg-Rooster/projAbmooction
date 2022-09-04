using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class DialogBoxWaitingController : MonoBehaviour
{
    [SerializeField] GameObject Label;
    [SerializeField] GameObject Content;

    public void SetWaiting()
    {
        UIManager.SetText(Label, Strings.Reloading);
        UIManager.SetText(Content, Strings.Waiting);
    }
}
