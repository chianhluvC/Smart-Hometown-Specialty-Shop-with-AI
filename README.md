# Smart Hometown Specialty Shop with AI Chatbot

![Screenshot of homepage](https://github.com/user-attachments/assets/a5ac41c7-d4ac-4c92-9133-0303933d2223)
![Screenshot of product page](https://github.com/user-attachments/assets/4315041a-9574-469b-bcf2-957306fb07e5)

## Overview

An intelligent e-commerce platform specializing in traditional Vietnamese hometown specialties. Built with ASP.NET Core MVC (.NET 7) using Razor Views, featuring a smart AI chatbot powered by DeepSeek V3 API and secure online payments through VNPay.

## 🔧 Technologies Used

- **Backend**: ASP.NET Core MVC (.NET 7), Entity Framework Core, SQL Server
- **Frontend**: Razor Views (.cshtml), Bootstrap 5, jQuery 3.6+
- **AI Integration**: DeepSeek V3 API
- **Payment Processing**: VNPay API

## 💡 Key Features

### 🤖 AI Chatbot – Smart Specialty Recommender
- Powered by DeepSeek V3 API
- Natural language conversation to discover hometown specialties
- Intelligent product recommendations based on user preferences
- Dark/light mode support
- Implemented as a Razor ViewComponent

### 🛒 Shopping Cart
- Seamless add, update, and remove functionality
- Responsive design with Bootstrap and jQuery

### 💳 VNPay Payment Integration
- Secure payment processing
- Flexible integration (sandbox/production)

### 📊 Admin Dashboard
- Real-time order and revenue statistics
- Product performance analytics
- Customer behavior insights

## 🚀 Getting Started

### Prerequisites
- .NET 7 SDK
- SQL Server
- VNPay merchant credentials
- DeepSeek V3 API key (via OpenRouter.ai)

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-username/smartdelight.ai.git
   cd smartdelight.ai
   ```

2. **Configure appsettings.json**
   ```json
   {
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
   }
   ```

3. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

5. **Access the Website**
   Open your browser and go to: `https://localhost:5001`

## 💻 Frontend Setup

All UI components use Bootstrap 5 and jQuery 3.6+, included via CDN in `_Layout.cshtml`:

```html
<!-- Bootstrap CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">

<!-- jQuery and Bootstrap JS -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
```

## 💬 Example Chatbot Conversation

**User**: "Tìm đặc sản Hà Nội"  
**Bot**: "Đây là gợi ý của tôi dựa trên..."

## 📝 License

MIT License – feel free to use and modify this project for learning or business purposes.
