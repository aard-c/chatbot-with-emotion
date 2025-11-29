# Chatbot With Emotion

This project is an experimental chatbot that performs basic sentiment or emotion analysis on user messages. The system is built with a C# ASP.NET Core backend and a separate HuggingFace/Gradio model that provides the AI response and sentiment evaluation.

The goal of the project is to demonstrate how to integrate a C# backend with a machine learning model served through HuggingFace Spaces or a local Gradio server. It is not designed for production use. The project is currently unfinished and left in a half-completed state, but the repository remains public for learning and reference purposes.

## How It Works

- The frontend sends a message to the backend API.
- The backend forwards this message to a sentiment/emotion analysis model.
- The model returns a response containing:
  - The predicted sentiment
  - Additional AI-generated text (if implemented)
- The backend saves the message and the model output to a SQLite database.
- The frontend displays the result.

The backend originally attempted to connect directly to a HuggingFace Space via HTTP/SSE endpoints. Due to issues with the Space API and connection format, the model was later run locally using Gradio. The integration is still incomplete.

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core with SQLite
- C# HttpClient (planned integration with Gradio/HuggingFace)
- Local HuggingFace Space / Gradio server for model execution
- React frontend (included in the repository)
- Minimal UI for sending and receiving messages

## Current Status

The project is not finished. API integration with the local or remote ML model is incomplete, and the system does not yet provide stable or reliable AI responses. The repository is kept online for documentation, learning, and future continuation.

## Future Improvements

- Complete the model communication layer
- Replace manual HTTP calls with a stable wrapper or SDK
- Improve error handling between backend and ML server
- Add proper emotion classification display in the frontend
- Polish UI and add conversation history
- Deploy the model and backend together in a consistent environment

## Running the Project

Because the project is unfinished, instructions may not fully work, but the basic structure is:

1. Start the backend:
   ```
   dotnet run
   ```

2. Start the frontend:
   ```
   npm install
   npm run dev
   ```

3. Start a local Gradio model (if you want to test ML responses):
   ```
   python app.py
   ```

Full integration between the backend and Gradio is still pending.
