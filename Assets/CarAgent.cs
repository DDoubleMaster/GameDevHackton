using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class CarAgent : Agent
{
	[SerializeField] float speed = 5;
	[SerializeField] float rotateSpeed = 120;
	public Transform target;
	private Vector3 mainPos;
	private Vector3 targetPos;

	public override void Initialize()
	{
		mainPos = transform.position;
	}

	public override void OnEpisodeBegin()
	{
		float radius = 5;
		Debug.Log("New Episode");
		// —брос позиции агента
		if (transform.localPosition.y < 0)
		{
			transform.position = mainPos;
			transform.localRotation = Quaternion.identity;
		}
		else
		{
			transform.localPosition = new Vector3(Random.Range(-radius, radius), 0.5f, Random.Range(-radius, radius));
			transform.localRotation = Quaternion.identity;
		}

		// —брос позиции цели
		target.localPosition = new Vector3(Random.Range(-radius, radius), 0.5f, Random.Range(-radius, radius));
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.localPosition);
		sensor.AddObservation(target.localPosition);
		sensor.AddObservation(transform.localRotation);
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		float move = actions.ContinuousActions[0];
		float rotate = actions.ContinuousActions[1];
		transform.Translate(Vector3.forward * Time.deltaTime * move * speed);

		if (move != 0)
		{
			transform.Rotate(Vector3.up * Time.deltaTime * rotate * rotateSpeed);
		}

		float distance = Vector3.Distance(transform.localPosition, target.localPosition);

		if (distance < 1f)
		{
			SetReward(2.0f);
			EndEpisode();
		}

		if (transform.localPosition.y < 0)
		{
			SetReward(-1.0f);
			EndEpisode();
		}
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
		continuousAction[0] = Input.GetAxis("Vertical");
		continuousAction[1] = Input.GetAxis("Horizontal");
	}
}
