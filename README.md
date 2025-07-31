# Book Heaven – Complete Bookstore Management System

## Description
**Book Heaven** is a modern desktop-based bookstore management system built using **WPF (Windows Presentation Foundation)** and powered by a **MySQL** relational database. It is designed to streamline bookstore operations through an intuitive user interface and a robust data management backend.

The system enables efficient handling of books, suppliers, purchases, sales, memberships, and employees. Its normalized database schema ensures data integrity through foreign key constraints and supports seamless interaction with the WPF frontend.

---

## Key Features
- **Books Management**  
  Add, edit, and view books with price, author, quantity, and cover image support.
- **Supplier Tracking**  
  Full profile management of book suppliers, including contact and location data.
- **Purchasing System**  
  Track book purchases from suppliers with quantity, ETA, and receive status.
- **Sales Records**  
  Log and analyze book sales linked to employees and members.
- **Member Management**  
  Manage customer memberships with valid periods and contact information.
- **Employee Registry**  
  Store staff details including address, phone, and hire date.

---

## Visuals

### Database Schema Diagram

![Database Diagram](assets/database-diagram.png)
> This diagram illustrates the entity relationships between tables such as `books`, `suppliers`, `purchases`, `sales`, `members`, and `employees`.

---

### WPF Application Screenshots

### Main Dashboard
![Dashboard Screenshot](assets/dashboard.png)

### Manage Books Interface
![Book Management Screenshot](assets/book-management.png)

### Sales Reporting View

![Sales Screenshot](assets/sales-overview.png)
> These visuals highlight the smooth WPF user interface and how Book Heaven simplifies bookstore management.

---

## Technologies Used

| Layer        | Technology                  |
|--------------|-----------------------------|
| Frontend     | WPF (XAML + C#)             |
| Backend      | C# (.NET)                   |
| Database     | MySQL                       |


---

## Ideal Use Case
**Book Heaven** is best suited for small to medium-sized bookstores that need a desktop application with offline-first capability, fast performance, and a user-friendly interface to manage their inventory and customer relations.
