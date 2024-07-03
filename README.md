# Fake news detection using ML algorithms

This project implements and compares various machine learning models’ ability to correctly label misinformation.
Two types of models were used:
- Traditional: Logistic regression, Support vector machine, or Multinomial Naïve Bayes.
- Transformer: BERT, ALBERT, or RoBERTa.

The models were trained in Python, using Google Collab.
The trained models are loaded the Flask server, which exposes API routes that can be called for predicting an input.
The .NET web application is used to take the input from the user, call the API mentioned above, and return the predicted label.


Datasets used for training can be found at the following links:

- https://www.kaggle.com/code/ohseokkim/fake-news-easy-nlp-text-classification/input
- https://www.kaggle.com/code/therealsampat/fake-news-detection/input?select=Fake.csv
- https://www.kaggle.com/code/ruchi798/how-do-you-recognize-fake-news/input
- https://raw.githubusercontent.com/susanli2016/NLP-with-Python/master/data/corona_fake.csv
- https://data.mendeley.com/datasets/zwfdmp5syg/1
- https://www.unb.ca/cic/datasets/truthseeker-2023.html

For the Flask API to work, make sure to also add the trained models which will be used for the prediction.
