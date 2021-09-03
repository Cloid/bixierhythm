using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightScript : MonoBehaviour
{
    public Image top;
    public Image middle;
    public Image bottom;
    void Start()
    {
        Color temp1 = top.color;
        temp1.a = 0f;
        top.color = temp1;

        Color temp2 = middle.color;
        temp2.a = 0f;
        middle.color = temp2;

        Color temp3 = bottom.color;
        temp3.a = 0f;
        bottom.color = temp3;
    }
    // Update is called once per frame
    void Update()
    {
        if(top.color.a <= 1)
        {
            Color temp = top.color;
            temp.a += 0.000099f;
            top.color = temp;
        }

        if (middle.color.a <= 1)
        {
            Color temp = middle.color;
            temp.a += 0.000066f;
            middle.color = temp;
        }
        if (bottom.color.a <= 1)
        {
            Color temp = bottom.color;
            temp.a += 0.000050f;
            bottom.color = temp;
        }
    }

    public void brightenGood()
    {
        if(top.color.a >= 0)
        {
            Color temp = top.color;
            temp.a -= 0.2f;
            top.color = temp;
        }
        if (middle.color.a >= 0)
        {
            Color temp = middle.color;
            temp.a -= 0.25f;
            middle.color = temp;
        }
        if (bottom.color.a >= 0)
        {
            Color temp = bottom.color;
            temp.a -= 0.3f;
            bottom.color = temp;
        }

    }

    public void brightenPerfect()
    {
        if (top.color.a >= 0)
        {
            Color temp = top.color;
            temp.a -= 0.3f;
            top.color = temp;
        }
        if (middle.color.a >= 0)
        {
            Color temp = middle.color;
            temp.a -= 0.35f;
            middle.color = temp;
        }
        if (bottom.color.a >= 0)
        {
            Color temp = bottom.color;
            temp.a -= 0.4f;
            bottom.color = temp;
        }

    }

    public void darkenMiss()
    {
        if (top.color.a <= 1)
        {
            Color temp = top.color;
            temp.a += 0.5f;
            top.color = temp;
        }
        if (middle.color.a <= 1)
        {
            Color temp = middle.color;
            temp.a += 0.5f;
            middle.color = temp;
        }
        if (bottom.color.a <= 1)
        {
            Color temp = bottom.color;
            temp.a += 0.5f;
            bottom.color = temp;
        }

    }
}
