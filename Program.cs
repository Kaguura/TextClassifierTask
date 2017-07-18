using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataFromFiles.readTrainingFiles(@"..\..\X_train.csv",
                                            @"..\..\y_train.csv");
            BayesClassifier.train();

            //get accuracy of training data
            DataFromFiles.readTestingFile(@"..\..\X_train.csv");
            var predictedValues = BayesClassifier.predictCategories();
            DataFromFiles.readActualCategoriesFile(@"..\..\y_train.csv");
            Console.WriteLine(BayesClassifier.getAccuracy(predictedValues,
                                            DataFromFiles.actualCategories));

            //predict categories testing data.
            //can't get accuracy, actual categories are not given for testing data.
            //that's why, just write predicted categories to file
            DataFromFiles.readTestingFile(@"..\..\X_test.csv");
            predictedValues = BayesClassifier.predictCategories();
            System.IO.File.WriteAllLines(@"..\..\predicted_categories.csv", predictedValues, Encoding.GetEncoding(1251));
        }
    }
}
