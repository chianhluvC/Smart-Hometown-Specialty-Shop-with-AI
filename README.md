# Smart Hometown Specialty Shop with AI Chatbot

![Screenshot 2025-04-16 175321](https://github.com/user-attachments/assets/a5ac41c7-d4ac-4c92-9133-0303933d2223)
![Screenshot 2025-04-16 175401](https://github.com/user-attachments/assets/4315041a-9574-469b-bcf2-957306fb07e5)


An intelligent e-commerce website for selling traditional Vietnamese hometown specialties.
Built with ASP.NET Core MVC (.NET 7) using Razor Views (cshtml).
Features a smart AI chatbot powered by DeepSeek V3 API, and supports secure online payment through VNPay.

🔧 Technologies Used
ASP.NET Core MVC (.NET 7)

DeepSeek V3 API 

Razor Views (.cshtml)

Entity Framework Core

SQL Server

Bootstrap 5 (Responsive design)

jQuery 3.6+ (Client-side interactivity)

VNPay API (Payment Gateway)



💡 Key Features
🤖 AI Chatbot – Smart Specialty Recommender
Integrated with DeepSeek V3 API

Chatbot helps users find the most suitable hometown specialties through natural language conversation

Automatically selects the best product based on user preferences and explains the reasoning

Integrated using Razor ViewComponent and supports dark/light mode

🛒 Shopping Cart
Add, update, and remove products

Seamless cart experience using Bootstrap and jQuery

💳 VNPay Payment Integration
Secure online payment processing

Easy integration using VNPay API (sandbox or production)

📊 Admin Dashboard
Real-time statistics for orders and revenue

View top-selling products and customer behavior

🚀 Getting Started
Prerequisites
.NET 7 SDK

SQL Server

VNPay merchant credentials (sandbox or live)

DeepSeek V3 API key (via OpenRouter.ai)

Installation Steps
Clone the Repository

bash
Copy
Edit
git clone https://github.com/your-username/smartdelight.ai.git
cd smartdelight.ai
Configure appsettings.json

json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=HometownDb;Trusted_Connection=True;"
},
"VNPay": {
  "TmnCode": "YOUR_TMN_CODE",
  "HashSecret": "YOUR_HASH_SECRET"
},
"OpenRouter": {
  "ApiKey": "YOUR_API_KEY",
  "BaseUrl": "https://openrouter.ai/api"
}
Apply Database Migrations

bash
Copy
Edit
dotnet ef database update
Run the Application

bash
Copy
Edit
dotnet run
Access the Website
Open your browser and go to: https://localhost:5001

💻 Frontend Setup
All UI components are styled using Bootstrap 5 and jQuery 3.6+. These are included via CDN in _Layout.cshtml:

html
Copy
Edit
<!-- Bootstrap CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">

<!-- jQuery and Bootstrap JS -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>


💬 Example Chatbot Conversation
User: “Tìm đặc sản Hà Nội”
Bot: “Đây là gợi ý của tôi dựa trên...”


📝 License
MIT License – feel free to use and modify this project for learning or business purposes.
