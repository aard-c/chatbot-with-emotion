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


app = gr.Interface(
    fn=analyze_sentiment,
    inputs=gr.Textbox(label="Enter text"),
    outputs="json",
)


app.launch(
    server_name="0.0.0.0",
    server_port=7860,
    show_api=True,
    share=True,        
    ssr_mode=False,      
    mcp_server=True
)

