# 📝 ASP.NET Full Blog API

This is a **Blog API** built with **.NET 9**, **ASP.NET Core**, and **PostgreSQL**.  
It is fully Dockerized and includes the following services:

- **myblog** → The ASP.NET Core Web API (main service)  
- **db** → PostgreSQL database  
- **migrator** → Runs Entity Framework Core migrations automatically at startup  

---

## 🚀 Getting Started

### Prerequisites
Make sure you have the following installed:
- [Docker](https://docs.docker.com/get-docker/)  
- [Docker Compose](https://docs.docker.com/compose/install/)  

---

### 1. Clone the repository
```bash
git clone https://github.com/husseinpenart/asp-full-blog-api.git
cd asp-full-blog-api


2. Run with Docker Compose

This will start all services (API + Database + Migrator):

docker compose up -d

3. Access the services

API → http://localhost:5203

PostgreSQL → localhost:5433

Default database credentials (from docker-compose.yml):

User: husseinpenart

Password: 13741995hussein

Database: myblog

⚙️ Configuration

You can change the environment variables inside docker-compose.yml.

Database (db service)
environment:
  POSTGRES_USER: your-username
  POSTGRES_PASSWORD: your-password
  POSTGRES_DB: your-database

API (myblog service)
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - ASPNETCORE_URLS=http://+:8080
  - ConnectionStrings__defaultConnection=Host=db;Username=your-username;Password=your-password;Database=your-database

📦 Running API Only (from Docker Hub)

If you only want to pull the API image without Compose:

docker pull husseinasadi/myblog-asp-api:latest


Run the container (make sure you have your own PostgreSQL instance running):

docker run -d -p 5203:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__defaultConnection="Host=<db-host>;Username=<user>;Password=<pass>;Database=myblog" \
  husseinasadi/myblog-asp-api:latest

✅ Notes

Change default database credentials before using in production.

The migrator service automatically applies EF Core migrations when the app starts.

Data is persisted in ./postgres-data (volume defined in docker-compose.yml).

📖 License

This project is licensed under the MIT License – feel free to use it and contribute.

💡 With this setup, you get a ready-to-use Blog API with Docker in just one command:

docker compose up -d