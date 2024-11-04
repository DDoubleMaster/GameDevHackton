using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BallAgent : Agent
{
	[SerializeField] BallParameters BP_Ball;
	[SerializeField] Transform target;

	Vector3 direction;
	float distance;

	public override void OnEpisodeBegin()
	{
		/*Vector2 onCircle = Random.insideUnitCircle.normalized * 20;
		Vector3 offset = new Vector3(onCircle.x, 0, onCircle.y);
		target.localPosition = transform.localPosition + offset;*/

		if (distance > 50f)
			transform.localPosition = Vector3.zero;

		Vector2 insideCircle = Random.insideUnitCircle * 20f;
		Vector3 offset = new Vector3(insideCircle.x, 0, insideCircle.y);
		target.localPosition = transform.localPosition + offset;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.position);
		sensor.AddObservation(target.position);
		sensor.AddObservation(direction.x);
		sensor.AddObservation(direction.z);
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		direction.x = Mathf.Lerp(direction.x, actions.ContinuousActions[0], 0.1f);
		direction.z = Mathf.Lerp(direction.z, actions.ContinuousActions[1], 0.1f);
		transform.Translate(direction * Time.deltaTime * BP_Ball.speed);

		distance = Vector3.Distance(transform.position, target.position);
		if (distance < 2f)
		{
			SetReward(1f);
			EndEpisode();
		}
		else if (distance > 30f)
		{
			SetReward(-0.1f);
			EndEpisode();
		}
		else if (Vector3.Distance(Vector3.zero, transform.position) > 100f)
		{
			SetReward(-1.5f);
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
