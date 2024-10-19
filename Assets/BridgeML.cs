using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BridgeML : MonoBehaviour
{
	private const string PredictUrl = "https://localhost:7038/api/ML/predict";

	public IEnumerator Predict(InputData inputData, System.Action<OutputData> callback)
	{
		Debug.Log("Called");
		string json = JsonUtility.ToJson(inputData);
		UnityWebRequest request = UnityWebRequest.PostWwwForm(PredictUrl, json);
		request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.SendWebRequest();
		Debug.Log("Sended");
		if (request.result == UnityWebRequest.Result.Success)
		{
			OutputData outputData = JsonUtility.FromJson<OutputData>(request.downloadHandler.text);
			Debug.Log(outputData);
			Debug.Log("Success");
			callback(outputData);
		}
		else
			Debug.LogError(request.error);
	}

	public class InputData
	{
		public float Raycast1 { get; set; }
		public float Raycast2 { get; set; }
		public float Raycast3 { get; set; }
		public float Raycast4 { get; set; }
		public float Raycast5 { get; set; }
		public float Raycast6 { get; set; }
		public float Raycast7 { get; set; }
		public float Raycast8 { get; set; }
		public float Distance { get; set; }
		public float Reward { get; set; }
	}

	public class OutputData
	{
		public float Forward { get; set; }
		public float Right { get; set; }
	}
}