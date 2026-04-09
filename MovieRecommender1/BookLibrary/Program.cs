// See https://aka.ms/new-console-template for more information
using Microsoft.ML;
using Microsoft.ML.Trainers;


class Program
{
    static void Main(String[] args)
    {
        Console.WriteLine("Hello, World!");
        MLContext mlContext = new MLContext();
        var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "BookRecommenderModel.zip");
        DataViewSchema modelSchema;
        ITransformer trainedModel = mlContext.Model.Load(modelPath, out modelSchema);
    }

    
}
