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

    private void ChangeToMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    IEnumerator BlinkWithColorWhite(float RepeatTimeInSeconds)
    {
        float time = RepeatTimeInSeconds;
        while(time > 0)
        {
            yield return BlinkForTime();
            time -= 0.125f;
        }
    }

    IEnumerator BlinkForTime()
    {
        ChangeToMaterial(WhiteMaterial);
        yield return new WaitForSeconds(0.125f);
        ChangeToMaterial(SpriteDefault);
    }
}
