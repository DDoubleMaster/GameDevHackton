using UnityEngine;

public class Bot : Car
{
	BridgeML ML;

	private void Start()
	{
		Debug.Log("Start");
		ML = GameObject.Find("BridgeML").GetComponent<BridgeML>();
		BridgeML.InputData inputData = new()
		{
			Raycast1 = 1.0f,
			Raycast2 = 2.0f,
			Raycast3 = 3.0f,
			Raycast4 = 4.0f,
			Raycast5 = 5.0f,
			Raycast6 = 6.0f,
			Raycast7 = 7.0f,
			Raycast8 = 8.0f,
			Distance = 9.0f,
			Reward = 1.0f,
		};
		StartCoroutine(ML.Predict(inputData, Success));
	}

	void Success(BridgeML.OutputData outputData)
	{
		Debug.Log($"Forward: {outputData.Forward}, Right: {outputData.Right}");
	}
}
