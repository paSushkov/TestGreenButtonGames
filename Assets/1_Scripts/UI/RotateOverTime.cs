using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
public class RotateOverTime : MonoBehaviour
{
    private RectTransform rectTransform;
    
    [SerializeField]
    private float rotationSpeed = 0f;
    private Vector3 axis = Vector3.forward;

    public static RotateOverTime CreateComponent(GameObject where, Vector3 aroundAxis, float speed)
    {
        RotateOverTime component = where.AddComponent<RotateOverTime>();
        component.axis = aroundAxis;
        component.rotationSpeed = speed;
        return component;
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(axis, Time.deltaTime* rotationSpeed);
    }


}
