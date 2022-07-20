using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ShakeObjectController : MonoBehaviour
{
    public float Duration;
    public float Magnetude;
    public float MinimumX;        
    public float MaximumX;

    public void ShakeObject()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 originalPos = transform.position;

        float elapse = 0.0f;

        while (elapse < Duration)
        {
            float x = Random.Range(MinimumX, MaximumX) * Magnetude;

            transform.position = new Vector3(x, originalPos.y, originalPos.z);

            elapse += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }
}
