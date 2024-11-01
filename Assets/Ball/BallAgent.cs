using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BallAgent : Agent
{
	[SerializeField] BallParameters BP_Ball;
	[SerializeField] Transform target;

	Vector3 direction;

	public override void OnEpisodeBegin()
	{
		target.localPosition = transform.localPosition + new Vector3(Random.Range(-4, 4), 0.5f, Random.Range(-4, 4));
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.position);
		sensor.AddObservation(transform.rotation);
		sensor.AddObservation(target.position);
		sensor.AddObservation(direction);
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		direction.x = Mathf.Lerp(direction.x, actions.ContinuousActions[0], 0.1f);
		direction.z = Mathf.Lerp(direction.z, actions.ContinuousActions[1], 0.1f);
		transform.Translate(direction * Time.deltaTime * BP_Ball.speed);

		float distance = Vector3.Distance(transform.position, target.position);
		if (distance < 1.5f)
		{
			SetReward(1f);
			EndEpisode();
		}
		else if (distance > 10f)
		{
			SetReward(-2f);
			EndEpisode();
		}
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		ActionSegment<float> ao = actionsOut.ContinuousActions;
		ao[0] = Input.GetAxis("Horizontal");
		ao[1] = Input.GetAxis("Vertical");
	}
}
