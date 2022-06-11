using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxScript : MonoBehaviour
{
    public float collectSpeed;
    public float boxHandPosition;
    private BoxCollider bc;
    private MeshRenderer mr;

    private void Start()
    {
         bc = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();

    }

    public void PlayerCollision(Transform handTarget)
    {
        bc.enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.transform.SetParent(handTarget);
        transform.DOLocalMove(new Vector3(0, 0, 0),0.1f);
        GemController.Instance.addSessionGem(1);

    }

    public IEnumerator ThrowBox(Transform handTarget, Transform wallBoxTransform)
    {
        this.GetComponent<MeshRenderer>().enabled = true;

        //Set Parents
        this.transform.SetParent(handTarget);

        //Set Player Collect Point
        GameObject.Find("Player").SendMessage("removeBoxFromList", this.gameObject);

        //Move Position
        transform.DOLocalMove(new Vector3(0,0,0), boxHandPosition);
        yield return new WaitForSeconds(boxHandPosition);


        //Anim Started
        GameObject.Find("Player").SendMessage("setThrowingAnim", true);

        //RigidBody
        this.gameObject.AddComponent<Rigidbody>();
        //Wait For Anim End
        yield return new WaitForSeconds(0.29f);

        //Set Box Collider
        this.transform.SetParent(null);
        bc.enabled = true;
        

        //Animation Finished and Box Deleted From Player Boxes
        GameObject.Find("Player").SendMessage("setThrowingAnim",false);

        //DO Path
        Vector3[] pathOfBox = { handTarget.position + Vector3.right, new Vector3(wallBoxTransform.position.x, (wallBoxTransform.position.y + handTarget.position.y) / 2, (handTarget.position.z + wallBoxTransform.position.z) / 2), wallBoxTransform.position };

        transform.DOPath(pathOfBox, 0.4f);
        yield return new WaitForSeconds(0.01f);
        bc.isTrigger = false;

        //Wait For Player New Shoot
        yield return new WaitForSeconds(0.4f);
        GameObject.Find("Player").GetComponent<PlayerScript>().CanShoot = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "WallBox" || collision.gameObject.tag == "EndWall")
        {
            bc.isTrigger = true;
        }
    }

    public void SetMeshRenderer(bool b)
    {
        mr.enabled = b;
    }
}
