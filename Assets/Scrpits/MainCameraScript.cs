using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainCameraScript : MonoBehaviour
{
    public Vector3 playerFollowOffset;
    public Vector3 ArcadeFollowOfset;
    public Vector3 ArcadeCamAngle;
    public float camChangeTime;
    private bool _isArcadeFollow;
    private Transform playerTarget;
    public float smoothSpeed;

    private bool isSwerveFollow = false;
    private bool isCatwalkFollow = false;
    // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player").GetComponent<PlayerScript>().IsDead)
        {
            return;
        }
        if (!_isArcadeFollow)
        {
            FollowTargetWithOffset(playerFollowOffset, playerTarget);
        }
        else
        {
            ArcadeCameraAngle(ArcadeFollowOfset, playerTarget);
        }
     
    }
    public void setIsSwerveFollow(bool b)
    {
        isSwerveFollow = b;
    }

    public void setIsCatwalkFollow(bool b)
    {
        isCatwalkFollow = b;
    }

    public IEnumerator setCameraToCatWallkFollow(Transform Target)
    {
        setIsSwerveFollow(false);
        transform.DOMove(new Vector3(Target.position.x, Target.position.y + 10, Target.position.z + -10), 0.3f);
        transform.DORotate(new Vector3(30, 0, 0), 0.3f);
        yield return new WaitForSeconds(0.3f);
        setIsCatwalkFollow(true);
    }

    public IEnumerator setCameraToSwerveFollow(Transform Target)
    {
        setIsCatwalkFollow(false);
        transform.DOMove(new Vector3(7, 10, Target.position.z + -12), 0.3f);
        transform.DORotate(new Vector3(30, -20, 0), 0.3f);
        yield return new WaitForSeconds(0.3f);
        setIsSwerveFollow(true);
    }

    public IEnumerator SetCameraToArcadeFollow(Transform target)
    {
        Vector3 desiredPosition = new Vector3(target.position.x + ArcadeFollowOfset.x, target.position.y + ArcadeFollowOfset.y, target.position.z + ArcadeFollowOfset.z);
        Vector3 desiredRotation = new Vector3(ArcadeCamAngle.x, ArcadeCamAngle.y, ArcadeCamAngle.z);
        transform.DORotate(desiredRotation, camChangeTime);
        transform.DOMove(desiredPosition, camChangeTime);
        yield return new WaitForSeconds(camChangeTime);
        ArcadeCameraAngle(ArcadeFollowOfset, playerTarget);
        _isArcadeFollow = true;

    }

    private void FollowTargetWithOffset(Vector3 followOffset,Transform target)
    {
        Vector3 desiredPosition = new Vector3(target.position.x + followOffset.x, target.position.y + followOffset.y, target.position.z + followOffset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }

    private void ArcadeCameraAngle(Vector3 followOffset, Transform target)
    {
        Vector3 desiredPosition = new Vector3(target.position.x + followOffset.x, target.position.y + followOffset.y, target.position.z + followOffset.z);
        transform.position = desiredPosition;
        transform.rotation = Quaternion.Euler(ArcadeCamAngle);
    }
}
