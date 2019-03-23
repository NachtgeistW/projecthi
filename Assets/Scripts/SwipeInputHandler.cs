﻿using System;
using UnityEngine;

namespace Rayark.Hi
{
    public class SwipeInputHandler : MonoBehaviour
    {
        public event Action<Vector2> OnSwipe;

        private Vector2 _touchBeginPosition;
        private Vector3 _mouseBeginPosition;

        void Update()
        {
#if UNITY_ANDROID || UNITY_IOS
            _TouchSwipe();
#else
            _MouseSwipe();
#endif
        }

        private void _TouchSwipe()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _touchBeginPosition = touch.position;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Vector2 swipeDiff = touch.position - _touchBeginPosition;
                    if(OnSwipe != null)
                    {
                        OnSwipe(swipeDiff);
                    }
                }
            }
        }

        private void _MouseSwipe()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseBeginPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector3 swipeDiff = Input.mousePosition - _mouseBeginPosition;
                if (OnSwipe != null)
                {
                    OnSwipe(swipeDiff);
                }
            }
        }
    }
}