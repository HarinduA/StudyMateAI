using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
// Removed System.Web as HttpUtility is no longer needed with POST request body

class Program
{
    // Google Gemini API URL
    // This API requires an API key for authentication.
    private static readonly string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    // IMPORTANT: Replace with your actual Google Gemini API key.
    // You can get one from Google AI Studio: https://ai.google.dev/
    private static readonly string apiKey = "AIzaSyCFEmF11Jz7me6yiLbri6VB9AqKoES3VQ4"; // <--- REPLACE THIS WITH YOUR ACTUAL API KEY

    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to Student AI Help!");
        Console.WriteLine("Ask any question, and the AI will try to answer.");
        Console.WriteLine("Type 'exit' to quit.\n");

        // Basic check for API key
        if (string.IsNullOrEmpty(apiKey) || apiKey == "AIzaSyCFEmF11Jz7me6yiLbri6VB9AqKoES3VQ4")
        {
            Console.WriteLine("");
            Console.WriteLine("You can get one from: https://ai.google.dev/\n");
            // Optionally, you might want to exit or prompt the user here.
            // For now, we'll continue, but API calls will fail without a valid key.
        }

        while (true)
        {
            Console.Write("You: ");
            string question = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(question))
                continue;

            if (question.Trim().ToLower() == "exit")
                break;

            string answer = await GetAIResponse(question);
            Console.WriteLine("AI: " + answer + "\n");
        }
    }

    static async Task<string> GetAIResponse(string prompt)
    {
        using HttpClient client = new HttpClient();
        // The Gemini API key is passed as a query parameter, not in the Authorization header.
        string requestUrlWithKey = $"{apiUrl}?key={apiKey}";

        // Construct the request body for the Gemini API.
        // The 'contents' array holds the conversation history.
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        string jsonBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        HttpResponseMessage response;

        try
        {
            // Send the POST request to the Gemini API.
            response = await client.PostAsync(requestUrlWithKey, content);
        }
        catch (HttpRequestException ex)
        {
            return $"Request failed: {ex.Message}. Check your internet connection and API URL.";
        }
        catch (Exception ex)
        {
            return $"An unexpected error occurred during the request: {ex.Message}";
        }

        string responseJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            // If the response was not successful, provide the status code and the full response body.
            return $"Error: {response.StatusCode} - {response.ReasonPhrase}. Response Body: {responseJson}";
        }

        try
        {
            // Parse the JSON response from the Gemini API.
            using JsonDocument doc = JsonDocument.Parse(responseJson);
            // Navigate to the generated text within the response structure.
            // Expected path: root.candidates[0].content.parts[0].text
            if (doc.RootElement.TryGetProperty("candidates", out JsonElement candidatesElement) &&
                candidatesElement.ValueKind == JsonValueKind.Array && candidatesElement.GetArrayLength() > 0)
            {
                var candidate = candidatesElement[0];
                if (candidate.TryGetProperty("content", out JsonElement contentElement) &&
                    contentElement.TryGetProperty("parts", out JsonElement partsElement) &&
                    partsElement.ValueKind == JsonValueKind.Array && partsElement.GetArrayLength() > 0)
                {
                    var part = partsElement[0];
                    if (part.TryGetProperty("text", out JsonElement textElement))
                    {
                        string generatedText = textElement.GetString();
                        return generatedText ?? "No answer returned.";
                    }
                }
            }
            return $"Unexpected response format. Full response: {responseJson}";
        }
        catch (JsonException e)
        {
            return $"Error parsing response JSON: {e.Message}. Full response: {responseJson}";
        }
        catch (Exception e)
        {
            return $"An unexpected error occurred while processing the response: {e.Message}. Full response: {responseJson}";
        }
    }
}
