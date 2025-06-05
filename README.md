# Student AI Helper - Backend Console Application
This project is a .NET console application serving as the backend for an AI-powered student helper. It demonstrates how to integrate with Large Language Models (LLMs) to provide real-time, AI-generated answers to user queries.

Features
AI Integration: Connects to the Google Gemini API (gemini-2.0-flash model) to leverage advanced natural language generation capabilities.
Console Interface: Provides a simple command-line interface for testing AI responses.
Robust Error Handling: Includes mechanisms to catch and display API-related errors (e.g., service unavailability, network issues).
Scalable Foundation: Designed as a standalone backend component, capable of being integrated with various frontend applications (like the React chat screen).
Technologies Used
Backend: .NET (C#)
HTTP Client: HttpClient for API requests.
JSON Handling: System.Text.Json for serializing requests and deserializing AI responses.
AI API: Google Gemini API
Setup and Running
Follow these steps to get the backend application running on your local machine.

Prerequisites
.NET SDK (Version 6.0 or higher recommended)
1. Get Your Google Gemini API Key
This application requires an API key to communicate with the Google Gemini model.

Go to Google AI Studio.
Sign in with your Google account.
Generate a new API Key.
2. Configure the API Key
    Open the Program.cs file in the project.
    Locate the apiKey variable:
    C#

    private static readonly string apiKey = "YOUR_GEMINI_API_KEY_HERE"; // <--- REPLACE THIS WITH YOUR ACTUAL API KEY
    Replace "YOUR_GEMINI_API_KEY_HERE" with the actual API key you obtained from Google AI Studio.

3. Run the Application

        dotnet run
