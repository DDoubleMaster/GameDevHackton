using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BallAgent : Agent
{
	[SerializeField] BallParameters BP_Ball;
	[SerializeField] Transform target;

	Rigidbody _rb;

	public override void Initialize()
	{
		_rb = GetComponent<Rigidbody>();
	}

	public override void OnEpisodeBegin()
	{
		if (transform.position.y < 0)
		{
			_rb.angularVelocity = Vector3.zero;
			_rb.linearVelocity = Vector3.zero;
			transform.localPosition = new Vector3(0, 0.5f, 0);
		}

		target.localPosition = new Vector3(Random.Range(-4, 4), 0.5f, Random.Range(-4, 4));
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.position);
		sensor.AddObservation(transform.rotation);
		sensor.AddObservation(target.position);
		sensor.AddObservation(_rb.angularVelocity);
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		Vector3 move = Vector3.zero;
		move.x = actions.ContinuousActions[0];
		move.z = actions.ContinuousActions[1];
		_rb.AddForce(move * BP_Ball.speed, ForceMode.Force);

		float distance = Vector3.Distance(transform.position, target.position);
		if (distance < 1.5f)
		{
			SetReward(1f);
			EndEpisode();
		}

		if (transform.localPosition.y < 0)
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
