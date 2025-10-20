import React, { useState } from "react";
import axios from "axios";

const ChatScreen = () => {
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState("");

  const sendMessage = async () => {
    if (!input.trim()) return;

    const newMessage = {
      text: input,
      userId: 1, // temporary, later dynamic if added users
    };

    try {
      
      const res = await axios.post("http://localhost:5286/api/messages", newMessage);

      
      const { sentiment, score } = res.data;

 
      setMessages([
        ...messages,
        { sender: "You", text: input },
        { sender: "AI", text: `Sentiment: ${sentiment} (Score: ${score})` },
      ]);

      setInput("");
    } catch (err) {
      console.error(err);
      alert("Error sending message!");
    }
  };

  return (
    <div style={styles.container}>
      <h2>💬 Emotion Chatbot</h2>

      <div style={styles.chatBox}>
        {messages.map((msg, i) => (
          <div key={i} style={msg.sender === "You" ? styles.userMsg : styles.aiMsg}>
            <b>{msg.sender}:</b> {msg.text}
          </div>
        ))}
      </div>

      <div style={styles.inputContainer}>
        <input
          type="text"
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="Type a message..."
          style={styles.input}
          onKeyDown={(e) => e.key === "Enter" && sendMessage()}
        />
        <button onClick={sendMessage} style={styles.button}>Send</button>
      </div>
    </div>
  );
};

// Styles
const styles = {
  container: {
    width: "400px",
    margin: "40px auto",
    fontFamily: "Arial, sans-serif",
    textAlign: "center",
  },
  chatBox: {
    border: "1px solid #ccc",
    borderRadius: "8px",
    padding: "10px",
    height: "300px",
    overflowY: "auto",
    marginBottom: "10px",
    background: "#fafafa",
  },
  inputContainer: {
    display: "flex",
  },
  input: {
    flex: 1,
    padding: "8px",
    border: "1px solid #ccc",
    borderRadius: "4px",
  },
  button: {
    marginLeft: "6px",
    padding: "8px 14px",
    background: "#007bff",
    color: "white",
    border: "none",
    borderRadius: "4px",
    cursor: "pointer",
  },
  userMsg: {
    textAlign: "right",
    color: "#333",
    marginBottom: "6px",
  },
  aiMsg: {
    textAlign: "left",
    color: "#007bff",
    marginBottom: "6px",
  },
};

export default ChatScreen;
