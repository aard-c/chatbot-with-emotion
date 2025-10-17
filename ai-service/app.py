import gradio as gr
from transformers import pipeline

sentiment_pipeline = pipeline("sentiment-analysis")

def analyze_sentiment(text):
    result = sentiment_pipeline(text)[0]
    label = result['label']
    if "POS" in label:
        sentiment = "positive"
    elif "NEG" in label:
        sentiment = "negative"
    else:
        sentiment = "neutral"
    return {"sentiment": sentiment, "score": round(result['score'], 3)}

iface = gr.Interface(fn=analyze_sentiment, inputs="text", outputs="json")
iface.launch()
