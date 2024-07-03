import torch
import re
import string
#import nltk

#from nltk.corpus import stopwords
#from nltk.tokenize import word_tokenize
from flask import Flask, jsonify, request
from transformers import RobertaTokenizer, RobertaForSequenceClassification

app = Flask(__name__)

device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
loaded_model_articles = RobertaForSequenceClassification.from_pretrained("TrainedModels/robertaArticles/").to(device)
loaded_tokenizer_articles = RobertaTokenizer.from_pretrained("TrainedModels/robertaArticles/")
loaded_model_tweets = RobertaForSequenceClassification.from_pretrained("TrainedModels/robertaTweets/").to(device)
loaded_tokenizer_tweets = RobertaTokenizer.from_pretrained("TrainedModels/robertaTweets/")

# nltk.download('stopwords')
# nltk.download('punkt')
#
#
# def remove_stopwords(text):
#     stop_words2 = set(stopwords.words('english'))
#     tokens = word_tokenize(text)
#     filtered_tokens = [word for word in tokens if word.lower() not in stop_words2]
#     filtered_text = ' '.join(filtered_tokens)
#     return filtered_text


def process_article_text(text):
    text = text.lower().split();
    text = " ".join(text) #remove extra whitespaces
    text = re.sub('\n', '', text) #remove newlines
    text = re.sub(r"[^0-9a-zA-Z]+",' ',text) #Replace any non-alphanumeric characters
    text = re.sub('https?://\S+|www\.\S+', '', text) #remove URLs
    text = re.sub('[%s]' % re.escape(string.punctuation), '', text) #remove punctuation
    text = re.sub('\w*\d\w*', '', text) #remove words containing digits
    return text


def process_tweet_text(text):
    text = text.lower().split()
    text = " ".join(text) #remove extra whitespaces
    text = re.sub('\n', '', text) #remove newlines
    return text


def predict(model, tokenizer, text):
    inputs = tokenizer(text, return_tensors="pt", padding=True, truncation=True).to(device)
    # Perform inference
    with torch.no_grad():
        outputs = model(**inputs)
        logits = outputs.logits

    # Apply softmax to get probabilities
    probs = torch.softmax(logits, dim=1)

    # Get predicted label
    predicted_label_id = torch.argmax(probs, dim=1).item()

    # Map label id to label
    label_map = {0: "Real", 1: "Fake"}
    predicted_label = label_map[predicted_label_id]

    print("Predicted label:", predicted_label)
    return jsonify({'label': predicted_label})


@app.route('/predictArticle', methods = ['POST'])
def predict_article():
    data = request.json
    print(data)

    if 'text' not in data or 'title' not in data:
        return jsonify({'error': 'Missing parameters: text and title are required'}), 400

    article_text = data.get('text', '')
    article_title = data.get('title', '')

    article_combined = article_title + " " + article_text
    #article_combined = remove_stopwords(article_combined)
    #article_combined = process_article_text(article_combined)
    print(article_combined)

    return predict(loaded_model_articles, loaded_tokenizer_articles, article_combined)


@app.route('/predictTweet', methods = ['POST'])
def predict_tweet():
    data = request.json

    if 'text' not in data:
        return jsonify({'error': 'Missing parameters: text is required'}), 400

    tweet_text = data.get('text', '')

    #tweet_text = remove_stopwords(tweet_text)
    tweet_text = process_tweet_text(tweet_text)
    print(tweet_text)

    return predict(loaded_model_tweets, loaded_tokenizer_tweets, tweet_text)


if __name__ == '__main__':
    app.run()
