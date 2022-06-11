using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight, swipeDown, swipeUp,doubleTap;
    private bool hold = false;
    private Vector2 startTouch, swipeDelta;
    private bool isDraging = false;
    private float counter = 0;
    private bool isHoldingEnable = false;

    private void Update()
    {
        doubleTap = tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            counter += Time.deltaTime;
            if (counter > 0.5f)
            {
                isHoldingEnable = true;
            }
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
            if (isHoldingEnable)
            {
                hold = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            isHoldingEnable = false;
            Reset();
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length > 0)
        {
            counter += Time.deltaTime;
            if (counter > 0.1f)
            {
                isHoldingEnable = true;
            }

            if (Input.touches[0].phase == TouchPhase.Began)
            {
                Debug.Log("Tapped");
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                isHoldingEnable = false;
                counter = 0;
                Reset();
            }
            else if (Input.touches[0].phase == TouchPhase.Stationary && isHoldingEnable)
            {
                hold = true;
            }
        }
        #endregion

        //Calculate the Distance 
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }

        }

        //Deadzone Check
        if (swipeDelta.magnitude > 125)
        {
            //Which Direction
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //left or right
                if (x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }

            Reset();
        }

        for(int i = 0; i < Input.touchCount; ++i)
        {
            if(Input.GetTouch(i).phase == TouchPhase.Began)
            {
                if(Input.GetTouch(i).tapCount == 2)
                {
                    doubleTap = true;
                    Debug.Log("Double Tap");
                }
                else
                {
                    doubleTap = false;
                }
            }
        }


    }
    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        hold = false;
        isDraging = false;
        counter = 0;
    }

    public Vector2 StartTouch { get => startTouch; }
    public bool Tap { get => tap; set => tap = value; }
    public bool SwipeLeft { get => swipeLeft; set => swipeLeft = value; }
    public bool SwipeRight { get => swipeRight; set => swipeRight = value; }
    public bool SwipeDown { get => swipeDown; set => swipeDown = value; }
    public bool SwipeUp { get => swipeUp; set => swipeUp = value; }
    public bool Hold { get => hold; set => hold = value; }
    public bool DoubleTap { get => doubleTap; set => doubleTap = value; }


    public static bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 0.2f;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;
        }
        return result;
    }

}