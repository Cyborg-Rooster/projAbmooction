using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FadeController : MonoBehaviour
{
    Animator Animator;
    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public IEnumerator StartFade(bool fadeIn)
    {
        if (fadeIn) Animator.Play("fadeIn");
        else
        {
            Animator.Play("fadeOut");
            yield return new WaitForSeconds(1f);
        }

        Animator.speed = 1;
        yield return new WaitForSeconds(.5f);
        Animator.speed = 0;
    }

    public void SetSpeed(int speed)
    {
        Animator.speed = speed;
    }
}
