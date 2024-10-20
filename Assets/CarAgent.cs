using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class CarAgent : Agent
{
	[SerializeField] float speed = 5;
	[SerializeField] float rotateSpeed = 120;
	[SerializeField] float radius = 5;

	public Transform target;

	public override void OnEpisodeBegin()
	{
		if (transform.position.y < 0)
		{
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}

		target.localPosition = new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.position);
		sensor.AddObservation(transform.rotation);
		sensor.AddObservation(target.position);
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		float forward = actions.ContinuousActions[0];
		float right = actions.ContinuousActions[1];

		transform.Translate(Vector3.forward * Time.deltaTime * forward * speed);

		if (forward != 0)
			transform.Rotate(Vector3.up * Time.deltaTime * forward * right * rotateSpeed);

		float distance = Vector3.Distance(transform.position, target.position);
		if (distance < 2.5f)
		{
			SetReward(1f + (forward/10));
			EndEpisode();
		} 

		if (transform.position.y < 0)
		{
			SetReward(-2f);
			EndEpisode();
		}
    }

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<float> ao = actionsOut.ContinuousActions;
		ao[0] = Input.GetAxis("Vertical");
		ao[1] = Input.GetAxis("Horizontal");
	}
}
