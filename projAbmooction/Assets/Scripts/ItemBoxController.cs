using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxController : MonoBehaviour
{
    public float time;
    public string name;
    public Sprite itemSprite;
    public VerticalGroupController VerticalGroupController;

    [Header("Childrens")]
    [SerializeField] SpriteRenderer ItemSpriteRenderer;
    [SerializeField] GameObject TxtItemName;
    [SerializeField] GameObject Slider;

    float count;

    void Start()
    {
        ItemSpriteRenderer.sprite = itemSprite;
        UIManager.SetText(TxtItemName, name);

        count = 1 / time;
        InvokeRepeating("DecreaseSlider", 1f, 1f);
    }

    // Update is called once per frame
    public void SetSliderValue(float value)
    {
        UIManager.SetSliderValue(Slider, value);
    }

    private void DecreaseSlider()
    {
        if (UIManager.ChangeSlider(Slider, true, count) == 0) Destroy(gameObject);
    }
}
