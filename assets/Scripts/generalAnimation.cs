using UnityEngine;
using System.Collections;

public class generalAnimation : MonoBehaviour {
	public float speed;
	public float rotateX, rotateY, rotateZ;
	public float moveX, moveY, moveZ;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update (){
		float v = speed * Time.deltaTime;
		float m = Time.deltaTime;
		transform.Rotate(new Vector3(rotateX, rotateY, rotateZ), v, Space.Self);
		transform.Translate(new Vector3(moveX * m, moveY * m, moveZ * m), Space.World);

	}
}
