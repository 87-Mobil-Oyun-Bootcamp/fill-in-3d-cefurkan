using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAreaController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            other.GetComponent<Renderer>().material.color = Color.yellow;

            if (other.gameObject)
            {
                other.gameObject.layer = LayerMask.NameToLayer("CollectedBlock");
            }

            var blockController = other.GetComponent<BlockController>();

            if (blockController)
            {
                blockController.BlockState = BlockState.Collected;
            }
        }
    }
}
