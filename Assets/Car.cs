using UnityEngine;

public class Car : MonoBehaviour
{
	[SerializeField] float speed = 5;
	[SerializeField] float rotate = 120;

	public void Move(float forward, float right)
	{
		transform.Translate(Vector3.forward * Time.deltaTime * forward * speed);

		if (forward != 0f)
			transform.Rotate(Vector3.up * Time.deltaTime * right * forward * rotate);
	}
}
