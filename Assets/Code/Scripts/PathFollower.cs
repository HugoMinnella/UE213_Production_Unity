﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour, InputSystem_Actions.IVehicleActions
    {
        [Header("Path Manager")]
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public Camera camera;
        public float speed = 5;
        public float widthOffset;
        public float offsetSpeed = 1.0f;
        public float heightOffset = 0.0f;
        public Vector3 vehiculeRotation;
        public float offset { get; set; }
        public float timeToTravel { get; set; }

        private float distanceTravelled;
        private float currentOffset;
        private InputSystem_Actions controls;
        private bool canRotate = true;


        public void OnEnable()
        {
            if (controls == null)
            {
                controls = new InputSystem_Actions();
                // Tell the "gameplay" action map that we want to get told about
                // when actions get triggered.
                controls.Vehicle.SetCallbacks(this);
            }
            controls.Vehicle.Enable();
        }

        public void OnDisable()
        {
            controls.Vehicle.Disable();
        }

        void InputSystem_Actions.IVehicleActions.OnLeft(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                float tempOffset = offset - widthOffset;
                offset = Mathf.Max(tempOffset, -widthOffset);
            }
        }

        void InputSystem_Actions.IVehicleActions.OnRight(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                float tempOffset = offset + widthOffset;
                offset = Mathf.Min(tempOffset, widthOffset);
            }
        }

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }

            currentOffset = offset;
        }

        void Update()
        {

            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;

                currentOffset = Mathf.MoveTowards(currentOffset, offset, Time.deltaTime * offsetSpeed);

                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction) + (transform.right * currentOffset) + (transform.up * heightOffset);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction) * Quaternion.Euler(vehiculeRotation.x, vehiculeRotation.y, vehiculeRotation.z);

            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        public void ComputeTravelTime()
        {
            timeToTravel = (pathCreator.path.length / ((speed * Time.deltaTime) * (1f / Time.deltaTime)));
        }

        public void RotateCamera()
        {   
            if(canRotate == true)
            {
                canRotate = false;
                camera.transform.Rotate(0, 0, 180);
                StartCoroutine(ResetCanRotate(5f));
            }   
        }

        IEnumerator ResetCanRotate(float duration)
        {
            yield return new WaitForSeconds(duration);
            camera.transform.Rotate(0, 0, 180);
            canRotate = true;
        }

    }
}