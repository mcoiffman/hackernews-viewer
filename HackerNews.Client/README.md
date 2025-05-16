# Hacker News Viewer

A simple full-stack web application that displays top stories from Hacker News.

- ✅ Frontend: Angular (search, pagination, clean UI)
- ✅ Backend: .NET Web API (calls Hacker News API, uses caching)

This is a showcase project with clear code, comments, and tests — built for performance and readability.

---

## 🚀 How to Run the App Locally

### 🔧 Prerequisites

1. [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
2. [Node.js + npm](https://nodejs.org) (v18+ recommended)
3. Angular CLI (install using `npm install -g @angular/cli`)
4. Git Bash (or any terminal)

---

## 🛠 Backend: .NET API Setup

1. Open a terminal in the root project folder (where `HackerNews.Api.sln` is located)

2. Run the API:

   ```bash
   cd HackerNews.Api
   dotnet run
   ```

3. The API will start on something like:

   ```
   https://localhost:7192
   ```

   You can test it in your browser:

   - https://localhost:7192/api/stories

---

## 🌐 Frontend: Angular App Setup

1. Open a new terminal window

2. Navigate to the Angular project folder:

   ```bash
   cd HackerNews.Client
   ```

3. Install dependencies:

   ```bash
   npm install
   ```

4. Start the Angular app:

   ```bash
   ng serve
   ```

5. Open your browser:

   - http://localhost:4200

You should now see a table of top Hacker News stories with search and pagination.

---

## ✅ Running Unit Tests

### API Tests

1. From the solution root:

   ```bash
   dotnet test
   ```

2. Tests are located in the `HackerNews.Tests` project

---

## ✨ Features

- Clean, responsive UI (Angular 19)
- Search stories by title
- Pagination and page size control
- Caching on backend for performance
- Unit tests for backend services

---

## 🙌 Author Notes

This project was created as part of a job application to demonstrate my ability to:
- Build full-stack apps
- Consume public APIs cleanly
- Write unit tests
- Follow best practices with modern tooling

Thanks for checking it out!
