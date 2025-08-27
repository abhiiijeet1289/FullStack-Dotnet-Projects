# Bookstore Management Application 📚 (ADO.NET + .NET 9.0)

This is a **Bookstore Management Application** built using **ASP.NET Core Razor Pages, ADO.NET, and SQL Server**.  
It demonstrates **connected and disconnected architectures, stored procedures, parameterized queries, and DataSet/DataTable operations**.

## 🚀 Features
- Connected architecture with `SqlConnection`, `SqlCommand`, `SqlDataReader`
- Disconnected architecture with `SqlDataAdapter`, `DataSet`, `DataTable`
- CRUD operations using stored procedures
- Parameterized queries (SQL injection prevention)
- Layered architecture with Repository + Service pattern
- ASP.NET Core Razor Pages for UI
- Unit & integration testing included

## 📂 Project Structure
- `BookstoreApp/` → Source code (Models, Data, Services, Razor Pages)
- `Database/` → SQL schema + stored procedures
- `Documentation/` → Full project documentation (detailed ADO.NET explanation)


## 🗄️ Database
Run the SQL scripts from the `Database/` folder to create and seed the **Books** table along with stored procedures.

## ⚙️ Technology Stack
- .NET 9.0
- ASP.NET Core Razor Pages
- ADO.NET
- SQL Server

## 📖 Full Documentation
See [BookstoreApp_Documentation.md](Documentation/BookstoreApp_Documentation.md)

---

💡 This project was developed as part of the **Wipro NGA .NET Cohort Assignment**.
