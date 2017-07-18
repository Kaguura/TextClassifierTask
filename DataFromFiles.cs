using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace task1
{
    static class DataFromFiles
    {
        internal static int numberOfVocabularies;
        internal static int allFeaturesCount;
        internal static Dictionary<string, int> categoryFrequency;
        internal static Dictionary<string, int> allWordCountPerCat;
        internal static Dictionary<string, Dictionary<string, int>> eachWordCountInCat;
        internal static List<Dictionary<string, int>> testingData;
        internal static List<string> actualCategories;

        internal static void readTrainingFiles(string x_train_path, string y_train_path)
        {
            categoryFrequency = new Dictionary<string, int>();
            allWordCountPerCat = new Dictionary<string, int>();
            eachWordCountInCat = new Dictionary<string, Dictionary<string, int>>();
            List<string> wordLines = new List<string>();
            List<string> categoriesByLine = new List<string>();

            using (var reader = new StreamReader(x_train_path, Encoding.GetEncoding(1251)))
            {
                HashSet<string> allWords = new HashSet<string>();
                //Skip first line with column names
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Replace(";", " ");
                    line = line.ToLower();

                    wordLines.Add(line);
                    foreach (string word in line.Split())
                    {
                        allWords.Add(word);
                    }
                }
                numberOfVocabularies = allWords.Count;
            }

            using (var reader = new StreamReader(y_train_path, Encoding.GetEncoding(1251)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.ToLower();

                    categoriesByLine.Add(line);
                }
            }

            //Fill dictionary with frequencies of each word in each category
            for (int i = 0; i < categoriesByLine.Count; i++)
            {
                Dictionary<string, int> wordCount;
                if (!eachWordCountInCat.TryGetValue(categoriesByLine[i], out wordCount))
                {
                    wordCount = new Dictionary<string, int>();
                    eachWordCountInCat.Add(categoriesByLine[i], wordCount);
                    allWordCountPerCat.Add(categoriesByLine[i], 0);
                    categoryFrequency.Add(categoriesByLine[i], 1);
                }
                else categoryFrequency[categoriesByLine[i]]++;

                foreach (string word in wordLines[i].Split())
                {
                    int count;
                    if (wordCount.TryGetValue(word, out count))
                        wordCount[word]++;
                    else wordCount.Add(word, 1);
                    eachWordCountInCat[categoriesByLine[i]] = wordCount;
                    allWordCountPerCat[categoriesByLine[i]]++;
                }
            }
            allFeaturesCount = categoriesByLine.Count;
        }

        internal static void readTestingFile(string x_test_path)
        {
            testingData = new List<Dictionary<string, int>>();
            List<string> wordLines = new List<string>();

            using (var reader = new StreamReader(x_test_path, Encoding.GetEncoding(1251)))
            {
                //Skip first line with column names
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Replace(";", " ");
                    line = line.ToLower();

                    wordLines.Add(line);
                }
            }

            //Fill dictionary with frequencies of each word in test case
            foreach (string line in wordLines)
            {
                Dictionary<string, int> wordCount = new Dictionary<string, int>();
                foreach (string word in line.Split())
                {
                    int count;
                    if (wordCount.TryGetValue(word, out count))
                        wordCount[word]++;
                    else wordCount.Add(word, 1);
                }
                testingData.Add(wordCount);
            }
        }

        internal static void readActualCategoriesFile(string actual_cat_path)
        {
            actualCategories = new List<string>();
            using (var reader = new StreamReader(actual_cat_path, Encoding.GetEncoding(1251)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.ToLower();

                    actualCategories.Add(line);
                }
            }
        }
    }
}
