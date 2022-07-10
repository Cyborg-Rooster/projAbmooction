using System.Collections;
using UnityEngine;

public class SpriteEffectController : MonoBehaviour
{
    [SerializeField] Material WhiteMaterial;

    SpriteRenderer spriteRenderer;
    Material SpriteDefault;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteDefault = spriteRenderer.material;
    }

    public void BlinkBlank(float RepeatTimeInSeconds)
    {
        StartCoroutine(BlinkWithColorWhite(RepeatTimeInSeconds));
    }

    public void BlinkOppacity(float RepeatTimeInSeconds)
    {
        StartCoroutine(BlinkTheOppacity(RepeatTimeInSeconds));
    }

    public void ChangeAlphaNum(bool visible)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeAlphaNumber(visible));
    }

    public void StopEffects()
    {
        StopAllCoroutines();
        spriteRenderer.material = SpriteDefault;
        ChangeOppacity(new Color
        (
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            1f
        ));
    }

    #region "Blink materials"
    private void ChangeToMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    IEnumerator BlinkWithColorWhite(float RepeatTimeInSeconds)
    {
        float time = RepeatTimeInSeconds;
        while(time > 0)
        {
            yield return BlinkMaterialForTime();
            time -= 0.125f;
        }
    }

    IEnumerator BlinkMaterialForTime()
    {
        ChangeToMaterial(WhiteMaterial);
        yield return new WaitForSeconds(0.125f);
        ChangeToMaterial(SpriteDefault);
    }
    #endregion

    #region "Blink Oppacity"
    private void ChangeOppacity(Color NewColor)
    {
        spriteRenderer.color = NewColor;
    }

    IEnumerator BlinkTheOppacity(float RepeatTimeInSeconds)
    {
        float time = RepeatTimeInSeconds;
        while (time > 0)
        {
            yield return BlinkOppacityForTime();
            time -= 0.5f;
        }
    }

    IEnumerator BlinkOppacityForTime()
    {
        ChangeOppacity(new Color
        (
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            0f
        ));

        yield return new WaitForSeconds(0.25f);

        ChangeOppacity(new Color
        (
            spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            1
        ));

        yield return new WaitForSeconds(0.25f);
    }
    #endregion

    #region "Change Alpha"
    IEnumerator ChangeAlphaNumber(bool visible)
    {
        Color C = spriteRenderer.color;
        for (float alpha = AlphaNumber(visible); AlphaCondition(alpha, visible); alpha = CheckAlpha(alpha, visible))
        {
            C.a = alpha;
            spriteRenderer.color = C;
            yield return new WaitForSeconds(.2f);
        }
    }

    private float AlphaNumber(bool visible)
    {
        if (visible) return 0;
        else return .5f;
    }

    private bool AlphaCondition(float alpha, bool visible)
    {
        if (visible) return alpha <= .5f;
        else return alpha >= 0;
    }
    private float CheckAlpha(float color, bool visible)
    {
        if (visible) return color += 0.1f;
        else return color -= 0.1f;
    }
    #endregion
}
