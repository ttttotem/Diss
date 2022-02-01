using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voxell;
using Voxell.NLP.SentenceDetect;
using Voxell.Inspector;
using Voxell.NLP.PosTagger;
using Voxell.NLP.Tokenize;
using Voxell.NLP.Stem;
using Voxell.NLP.NameFind;

public class NLP : MonoBehaviour
{
    [StreamingAssetFilePath] public string splitterModel;
    [StreamingAssetFilePath] public string tokenizerModel;
    [StreamingAssetFilePath] public string posTaggerModel;
    [StreamingAssetFilePath] public string tagDict;
    [StreamingAssetFolderPath] public string nameFinderModel;
    

    private EnglishMaximumEntropyTokenizer tokenizer;
    private EnglishMaximumEntropyPosTagger posTagger;

    private EnglishMaximumEntropySentenceDetector sentenceDetector;

    private EnglishNameFinder nameFinder;

    private RegexStemmer regexStemmer;

    public string testSentence;
    public string[] sentences;
    public string[] tokens;
    public string[] posTags;
    public string[] stemmedTokens;

    public string[] models = new string[]
    { "date", "location", "money", "organization", "percentage", "person", "time" };

    [TextArea(1, 5), InspectOnly] public string ner;

    public static NLP instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one nlp in scene");
            return;
        }
        instance = this;
    }

    void Start()
    {
        sentenceDetector = new EnglishMaximumEntropySentenceDetector(FileUtilx.GetStreamingAssetFilePath(splitterModel));

        tokenizer = new EnglishMaximumEntropyTokenizer(FileUtilx.GetStreamingAssetFilePath(tokenizerModel));

        //Tag dict holds common tags, perhaps users answers add to this dict
        posTagger = new EnglishMaximumEntropyPosTagger(
          FileUtilx.GetStreamingAssetFilePath(posTaggerModel),
          FileUtilx.GetStreamingAssetFilePath(tagDict));

        //Patterns altered in Voxell.NLP.Stem
        regexStemmer = new RegexStemmer();
        regexStemmer.CreatePattern();

        nameFinder = new EnglishNameFinder(FileUtilx.GetStreamingAssetFilePath(nameFinderModel));
    }

    public string[] SplitSentences(string paragraph)
    {
        if(paragraph == null || sentenceDetector == null)
        {
            return null;
        }
        sentences = sentenceDetector.SentenceDetect(paragraph);
        return sentences;
    }

    public void POSTagger(string sentence)
    {
        if(sentence == null || tokenizer == null)
        {
            return;
        }
        tokens = tokenizer.Tokenize(sentence);
        posTags = posTagger.Tag(tokens);
    }

    public void Stem(string sentence)
    {
        if(sentence == null || tokenizer == null || regexStemmer == null)
        {
            return;
        }
        tokens = tokenizer.Tokenize(sentence);
        stemmedTokens = new string[tokens.Length];
        // stem
        for (int t = 0; t < tokens.Length; t++)
            stemmedTokens[t] = regexStemmer.Stem(tokens[t]);
    }

    public void NER(string sentence)
    {
        
        ner = nameFinder.GetNames(models, sentence);
    }

    [Button]
    public void UpdateEditor()
    {
        POSTagger(testSentence);
        Stem(testSentence);
        NER(testSentence);
    }


}
