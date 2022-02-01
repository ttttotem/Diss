using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Voxell;
using Voxell.Inspector;
using Voxell.NLP;
using Voxell.NLP.Classifier;
using Voxell.NLP.Tokenize;
using Voxell.NLP.PosTagger;
using Voxell.NLP.Stem;
using Voxell.NLP.NameFind;
using SharpEntropy;
using SharpEntropy.IO;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Voxell.NLP.Parser;
using Voxell.NLP.Util;

public class NERTrainer : MonoBehaviour
{
    public ClassifyOptions classifyOptions;
    [StreamingAssetFilePath] public string tokenizerModel;
    [StreamingAssetFilePath] public string posTaggerModel;
    [StreamingAssetFilePath] public string tagDict;
    [StreamingAssetFolderPath] public string nameFinderModel;

    public TextAsset dataset;
    public List<Sentence> sentences;
    public List<string> vocabs;
    public string sentenceToClassify;
    [InspectOnly] public string classifiedLabel;
    [InspectOnly] public double highestConfidence;

    public string modelName;

    public string trainingData;

    private NaiveBayesClassifier classifier;
    private EnglishMaximumEntropyTokenizer tokenizer;
    private EnglishMaximumEntropyPosTagger posTagger;
    private RegexStemmer stemmer;

    public void InitializeData()
    {
        // reset data
        sentences.Clear();
        sentences.TrimExcess();
        classifyOptions.labels.Clear();
        classifyOptions.labels.TrimExcess();

        // create tokenizer, pos tagger, and stemmer
        tokenizer = new EnglishMaximumEntropyTokenizer(FileUtilx.GetStreamingAssetFilePath(tokenizerModel));
        posTagger = new EnglishMaximumEntropyPosTagger(
          FileUtilx.GetStreamingAssetFilePath(posTaggerModel),
          FileUtilx.GetStreamingAssetFilePath(tagDict));
        stemmer = new RegexStemmer();
        stemmer.CreatePattern();

        // generate data
        var data = JsonConvert.DeserializeObject<JObject>(dataset.text);
        JToken intents = data["intents"];
        foreach (JToken intent in intents)
            classifyOptions.AddLabel((string)intent["intent"]);

        foreach (JToken intent in intents)
        {
            // convert each sentences into a Sentence class and add it into the list
            foreach (JToken text in intent["text"])
                sentences.Add(new Sentence(
                  ((string)text).ToLower(),
                  (string)intent["intent"],
                  tokenizer, posTagger, stemmer
                ));
        }
    }

    [Button]
    public void TrainNER()
    {
        //Assets/StreamingAssets/Models/Training/data.txt
        //InitializeData();
        // train the model
        string path = FileUtilx.GetStreamingAssetFilePath(Path.Combine(nameFinderModel, trainingData + ".txt"));
        GisModel ner = MaximumEntropyNameFinder.TrainModel(path);
        Debug.Log(path);
        Debug.Log(ner);


        // save the model
        BinaryGisModelWriter writter = new BinaryGisModelWriter();
        writter.Persist(ner, FileUtilx.GetStreamingAssetFilePath(Path.Combine(nameFinderModel, modelName + ".nbin")));
        //using (BinaryWriter binaryWriter = new BinaryWriter(new FileStream(FileUtilx.GetStreamingAssetFilePath(Path.Combine(nameFinderModel, modelName + ".nbin")), FileMode.Create)))
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ner));
        //    binaryWriter.Write(bytes);
        //}
        Debug.Log(FileUtilx.GetStreamingAssetFilePath(Path.Combine(nameFinderModel, modelName + ".nbin")));
    }
}
