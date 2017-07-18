using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    static class BayesClassifier
    {
        static Dictionary<string, double> probabilityCategory = new Dictionary<string, double>();
        static Dictionary<string, Dictionary<string, double>> probabilityWordInCat
            = new Dictionary<string, Dictionary<string, double>>();

        static internal void train()
        {
            foreach (string category in DataFromFiles.categoryFrequency.Keys)
            {
                probabilityCategory.Add(category,
                    1.0 * DataFromFiles.categoryFrequency[category] / DataFromFiles.allFeaturesCount);
                Dictionary<string, double> wordsProbabilities = new Dictionary<string, double>();
                probabilityWordInCat.Add(category, wordsProbabilities);
                foreach (string word in DataFromFiles.eachWordCountInCat[category].Keys)
                {
                    double wordProbability = (DataFromFiles.eachWordCountInCat[category][word] + 1.0) /
                                                (DataFromFiles.allWordCountPerCat[category]
                                                + DataFromFiles.numberOfVocabularies);
                    probabilityWordInCat[category].Add(word, wordProbability);
                }
            }
        }

        static internal List<string> predictCategories()
        {
            List<string> predictedCat = new List<string>();
            foreach (var testCase in DataFromFiles.testingData)
            {
                Tuple<string, double> catWithMaxProbability = new Tuple<string, double>("", Double.MinValue);
                foreach (string category in probabilityCategory.Keys)
                {
                    double probability = Math.Log(probabilityCategory[category]);
                    foreach (string word in testCase.Keys)
                    {
                        double wordProbability;
                        if (!probabilityWordInCat[category].TryGetValue(word, out wordProbability))
                            wordProbability = 1.0 / (DataFromFiles.allWordCountPerCat[category]
                                                    + DataFromFiles.numberOfVocabularies);
                        probability += testCase[word] * Math.Log(wordProbability);
                    }
                    if (probability > catWithMaxProbability.Item2)
                        catWithMaxProbability = new Tuple<string, double>(category, probability);
                }
                predictedCat.Add(catWithMaxProbability.Item1);
            }
            return predictedCat;
        }

        static internal float getAccuracy(List<string> predicted, List<string> actual)
        {
            int correctCount = 0;
            for (int i = 0; i < predicted.Count; i++)
                if (predicted[i].Equals(actual[i]))
                    correctCount++;
            return (float)correctCount / predicted.Count;
        }
    }
}
