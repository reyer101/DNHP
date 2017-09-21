using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightAnalogRotator : MonoBehaviour {
    Vector2 inputDirection;
    public float smooth;

	// Use this for initialization
	void Start () {
        inputDirection = Vector2.zero;		
	}
	
	// Update is called once per frame
	void Update () {        
        float x = Input.GetAxis("RightAnalogHori");
        float y = Input.GetAxis("RightAnalogVert");
        float angle = Mathf.Atan2(-y, x) * (180f / Mathf.PI);           
        transform.rotation = Quaternion.Slerp(
            transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 10f * smooth * Time.deltaTime);
	}
}
