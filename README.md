# Biomedical Text Annotation

Custom Annotation system that was used to investigate the effect of gamification on motivating human annotator engagment for crowdsourcing purposes

## Source code outline:
* Game: collection of c# scripts that handle the game logic. Main ones are:
   * "NLPOrgs.cs": Converts hidden model guesses into string locations
   * "Loader.cs": Handles how trap questions vs clean questions are loaded
   * "Points.cs": Handles how points are allocated to users 
   * "TMPDetector.cs": Handles user input for annotation
   * "NLP.cs": Handles ner hidden model and pos tagger
   * "NERTrainer.cs": Trains the NER to work on diseases
   * "SendData.cs": Handles sending users responses to google forms
* Data preparation: "Format converter.ipynb"
* Data processing: "Graphs.ipynb"

## To play with .exe:
  * Click Indv Project Prototype.exe
 
## To build enviroment:
  * Install Github for Unity
  * Fork from https://github.com/2373655r/Diss.git
  * Press play in inspector or file -> build settings -> build


-2373655r
