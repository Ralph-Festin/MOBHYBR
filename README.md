CurrencyX: A Real-Time Currency Conversion Application

CurrencyX is a mobile application built using .NET MAUI that allows users to:

📊 View live and historical currency exchange rates
🔄 Convert currencies in real-time
🔍 Search and select currencies


📌 Features
1. Home Screen 
   Displays your country’s currency exchange rate to the top 5 currencies worldwide with an option to switch the time periods.
2. Currency Conversion Screen
   Lets users compare between two currencies with an option to select current and past exchange rates.
3. Historical Exchange Rates Data
   Users are able to change time periods to see past exchange rates.


🛠️ Tech Stack
 - Framework: .NET MAUI 
 - Platform: Visual Studio 2022 
 - Programming Language: C# 
 - UI Framework: .NET MAUI (XAML) 
 - API Consumption Library: HttpClient
 - API Source: Currency Conversion and Exchange Rates API
 - Development Tool: Android Emulator / Physical Device


Follow these steps to run the project locally:

1. Clone the Repository
   git clone https://github.com/your-username/currencyx.git
   cd currencyx
2. Open in Visual Studio
   Open Visual Studio 2022 or later
   Select Open a project or solution
   Open the .sln file
3. Restore Dependencies
   Visual Studio should automatically restore packages
   If not, run:
   dotnet restore
4. Build and Run
   Select your target (Android Emulator / Windows Machine)
   Click Run (▶)

   
🔑 API Key Setup

This project uses a currency exchange API from RapidAPI.

⚠️ Important

The API key in the code is for demonstration only. You should replace it with your own.


Steps to Get Your API Key
1. Go to RapidAPI
2. Search for: Currency Conversion and Exchange Rates API
3. Subscribe to the API (free tier available)
4. Copy your API key


Replace API Key

Open:

Services/CurrencyApiService.cs

Find:

private const string ApiKey = "YOUR_API_KEY_HERE";

Replace with:

private const string ApiKey = "your_actual_api_key";
