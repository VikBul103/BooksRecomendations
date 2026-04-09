using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace BooksRecommendation
{
    class Program
    {

        static void Main(string[] args)
        {

            MLContext mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);
            EvaluateModel(mlContext, testDataView, model);
            UseModelForSinglePrediction(mlContext, model);
            SaveModel(mlContext, trainingDataView.Schema, model);
        }

        public static (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "ratings-train.csv");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "ratings-test.csv");

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<BooksRating>(trainingDataPath, hasHeader: true, separatorChar: ',');
            IDataView testDataView = mlContext.Data.LoadFromTextFile<BooksRating>(testDataPath, hasHeader: true, separatorChar: ',');

            return (trainingDataView, testDataView);
        }

        public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "user_id")
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "bookIdEncoded", inputColumnName: "book_id"));

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "bookIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));

            Console.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }

        public static void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            Console.WriteLine("=============== Evaluating the model ===============");
            var prediction = model.Transform(testDataView);

            var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }

        public static void UseModelForSinglePrediction(MLContext mlContext, ITransformer model)
        {
            Console.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<BooksRating, BooksRatingPrediction>(model);
            var testInput = new BooksRating { user_id = 53, book_id = 4602 };

            var bookRatingPrediction = predictionEngine.Predict(testInput);
            if (Math.Round(bookRatingPrediction.Score, 1) > 3.5)
            {
                Console.WriteLine("Book " + testInput.book_id + " is recommended for user " + testInput.user_id);
            }
            else
            {
                Console.WriteLine("Book " + testInput.book_id + " is not recommended for user " + testInput.user_id);
            }
        }

        public static void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "BookRecommenderModel.zip");

            Console.WriteLine("=============== Saving the model to a file ===============");
            Console.WriteLine(modelPath);
            mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
        }
    }
}
