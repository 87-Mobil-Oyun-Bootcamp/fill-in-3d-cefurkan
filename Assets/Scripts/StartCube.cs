using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCube : MonoBehaviour
{
    public Rigidbody rb;
    public bool isDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        
        var comp2 = other.gameObject.GetComponent<FillAreaController>();
        if (comp2)
        {

            GetComponent<Collider>().enabled = false;
            comp2.trigger.enabled = false;
            comp2.transform.position += new Vector3(0f, 0.3f);
            StartCoroutine(FillWithLerp());

        }

        IEnumerator FillWithLerp()
        {
            var timer = 0f;
            while (true)
            {
                timer += Time.deltaTime * 4;
                transform.localPosition = Vector3.Lerp(transform.localPosition, comp2.transform.localPosition, timer);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, timer);

                if (timer >= 1f)
                {
                    comp2.mR.material.color = comp2.targetColor;
                    Destroy(gameObject);
                    isDestroyed = true;
                    LevelManager.Instance.blocksFromImage.Remove(other.gameObject);
                    other.gameObject.GetComponent<ColorChanger>().enabled = false;
                    break;
                }

                yield return new WaitForEndOfFrame();

            }
        }
    }


}
