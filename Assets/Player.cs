using UnityEngine;

public class Player : Car
{
	private void FixedUpdate()
	{
		Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
	}
}
