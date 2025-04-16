using PathCreation;
using PathCreation.Examples;
using System;
using UnityEngine;

public class Ennemie : MonoBehaviour
{
    public EnnemieType type;
    public float heightOffset;
    public Int32 beat;
    public float offset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

#if UNITY_EDITOR
    private void OnValidate() => UnityEditor.EditorApplication.delayCall += _OnValidate;

    private void _OnValidate()
    {
        UnityEditor.EditorApplication.delayCall -= _OnValidate;
        if (this == null) return;

        if (transform.parent != null)
        {
            EnnemieCreator creator = transform.parent.GetComponent<EnnemieCreator>();
            if (creator != null)
            {
                creator.updateEnnemie(this);
            }
        }
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        //if (other.)
        {
            
        }

        if (other.CompareTag("Player"))
        {
            PathFollower vehicle = other.GetComponent<PathFollower>();

            if(type == EnnemieType.UpsideDown)
            {
                vehicle.RotateCamera();
            }

            if(type == EnnemieType.Slow)
            {
                vehicle.SlowDown();
            }
        }
    }
}

public enum EnnemieType { 
    UpsideDown,
    Slow
};


