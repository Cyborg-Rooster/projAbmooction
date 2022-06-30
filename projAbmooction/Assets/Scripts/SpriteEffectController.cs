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
}
