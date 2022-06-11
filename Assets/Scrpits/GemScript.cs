using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public GameObject plusOneText;
    private bool rotating;
    private void Start()
    {
        plusOneText.SetActive(false);
        rotating = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotating)
        {
            transform.Rotate(Vector3.up, 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            GemController.Instance.addSessionGem(1);
            rotating = false;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            plusOneText.SetActive(true);
            
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
