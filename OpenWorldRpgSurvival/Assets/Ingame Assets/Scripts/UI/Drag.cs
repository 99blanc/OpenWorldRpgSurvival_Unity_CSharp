using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drag : MonoBehaviour
{
    public static Drag instance;
    public Slot drag;

    [SerializeField] public Image imageItem;

    private void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image image)
    {
        imageItem.sprite = image.sprite;
        SetColor(1);
    }

    public void SetColor(float alpha)
    {
        Color color = imageItem.color;
        color.a = alpha;
        imageItem.color = color;
    }
}
