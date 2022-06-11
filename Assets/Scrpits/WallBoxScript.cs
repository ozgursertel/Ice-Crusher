using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBoxScript : MonoBehaviour
{
    ParticleSystem hitParticleSystem;
    ParticleSystem blastParticleSystem;
    Rigidbody rb;
    BoxCollider bc;
    MeshRenderer meshRenderer;
    private bool _needWakeUp = false; 
    // Start is called before the first frame update
    void Start()
    {
        hitParticleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
        blastParticleSystem = transform.GetChild(1).GetComponent<ParticleSystem>();
        blastParticleSystem.Stop();
        hitParticleSystem.Stop();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        if(rb.IsSleeping() && _needWakeUp)
        {
            rb.WakeUp();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Acorn")
        {
            SiblingRigidBodyWakeUp();
            rb.AddForce(Vector3.up * 7);
            bc.enabled = false;
            meshRenderer.enabled = false;
            hitParticleSystem.Play();
            blastParticleSystem.Play();
            AudioManager.instance.Play("Box Destroy");

        }
    }
    private void SiblingRigidBodyWakeUp()
    {
        if(transform.parent != null)
        {
            if (transform.GetSiblingIndex() + 1 < transform.parent.childCount)
            {
                transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<WallBoxScript>().RigidBodyWakeUp();
            }
        }
    }
    public void RigidBodyWakeUp()
    {
        rb.WakeUp();
        _needWakeUp = true;
    }
}
