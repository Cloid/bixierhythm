using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyHandler : MonoBehaviour
{
    private void Start()
    {   
        StartCoroutine(fadeIn(this.gameObject.GetComponent<SpriteRenderer>(), 5));
        StartCoroutine(fadeOut(this.gameObject.GetComponent<SpriteRenderer>(), 1));
    }

    IEnumerator fadeOut(SpriteRenderer MyRenderer, float duration)
    {
        float counter = 0;
        //Get current color
        Color spriteColor = MyRenderer.material.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            //Debug.Log(alpha);

            //Change alpha only
            MyRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }

        Destroy(this.gameObject);
        //Debug.Log("Done lerpin");
    }

    IEnumerator fadeIn(SpriteRenderer MyRenderer, float duration)
    {
        float counter = 0;
        //Get current color
        Color spriteColor = MyRenderer.material.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(0, 1, counter / duration);
            //Debug.Log(alpha);

            //Change alpha only
            MyRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            
        }

        yield return null;

        //Destroy(this.gameObject);
        //Debug.Log("Done lerpin");
    }
}
