using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCube : MonoBehaviour
{
    public Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
       
        var comp2 = other.gameObject.GetComponent<BlockController>();
        Debug.Log(comp2);
        if (comp2)
        {
            GetComponent<Collider>().enabled = false;
            comp2.trigger.enabled = false;
            comp2.transform.position += new Vector3(0f, 0.3f);
            StartCoroutine(OsmanAga());
        }

        IEnumerator OsmanAga()
        {
            var timer = 0f;
            while (true)
            {
                timer += Time.deltaTime * 4;
                transform.localPosition = Vector3.Lerp(transform.localPosition, comp2.transform.localPosition, timer);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, timer);

                if(timer>=1f)
                {
                    comp2.mR.material.color = comp2.targetColor;
                    Destroy(gameObject);
                    break;
                }
                yield return new WaitForEndOfFrame();

            }
        }
    }

  
}
