using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;

public class CarAgent : Car
{
	private readonly MLContext _mlContext;
	private ITransformer _model;
	private EstimatorChain<RegressionPredictionTransformer<Microsoft.ML.Trainers.LinearRegressionModelParameters>> _pipeline;

	public CarAgent()
	{
		_mlContext = new();
		string[] inputColumn = new[]
		{
			"Raycast1", "Raycast2", "Raycast3",
			"Raycast4", "Raycast5", "Raycast6",
			"Raycast7", "Raycast8", "Distance"
		};
		_pipeline = _mlContext.Transforms.Concatenate("Input", inputColumn)
			.Append(_mlContext.Transforms.NormalizeMinMax("Input"))
			.Append(_mlContext.Regression.Trainers.Sdca("Reward", "Input"));
	}

	public OutputData Predict(InputData inputData)
	{
		PredictionEngine<InputData, OutputData> predictionEngine =
			_mlContext.Model.CreatePredictionEngine<InputData, OutputData>(_model);
		OutputData prediction =
			predictionEngine.Predict(inputData);
		return prediction;
	}

	public void UpdateModel(InputData inputData, float reward)
	{
		IDataView data = _mlContext.Data
			.LoadFromEnumerable(new List<InputData> { inputData });

		if (_model == null)
			_model = _pipeline.Fit(data);
		else
		{
			IDataView transformedData = _model.Transform(data);
			_model = _pipeline.Fit(transformedData);
		}
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
	}

	public class OutputData
	{
		public float Forward { get; set; }
		public float Right { get; set; }
	}
}