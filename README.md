# 🛒 SiM Workwear — Online Store

An ASP.NET Core MVC web application for browsing and managing a product catalog for **SiM Workwear** — a Bulgarian supplier of workwear and personal protective equipment. Built as an academic project demonstrating full-stack web development with .NET 8, Entity Framework Core, and SQLite.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Features](#features)
- [Architecture & Design Principles](#architecture--design-principles)
- [Database](#database)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Known Issues](#known-issues)
- [TODOs & Future Improvements](#todos--future-improvements)

---

## Overview

SiM Workwear Online Store is a full-stack MVC web application where administrators can manage a product catalog (add, edit, delete products with categories and images), and visitors can browse the catalog, search by keyword, and filter by category. The app features a responsive layout, a hero landing page, About and Contact pages, and a sticky footer.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC 8.0 |
| Language | C# 12 (.NET 8) |
| ORM | Entity Framework Core 9 |
| Database | SQLite (via `Microsoft.Data.Sqlite`) |
| Authentication Package | `Microsoft.AspNetCore.Identity.EntityFrameworkCore` 8.0 *(installed but not yet wired up)* |
| Frontend | Razor Views (`.cshtml`) |
| CSS Framework | Bootstrap 5.3.2 (CDN) |
| Icons | Bootstrap Icons (CDN) |
| Client-side Validation | jQuery Validation + Unobtrusive Validation |
| Build System | MSBuild / `dotnet CLI` |

---

## Project Structure

```
OnlineStore/
├── Controllers/
│   ├── HomeController.cs          # Handles Home, About, Contact, Privacy, Error
│   └── ProductsController.cs      # Full CRUD for products + catalog with search/filter
├── Models/
│   ├── Product.cs                 # Product entity with data annotations
│   ├── Category.cs                # Category entity (One-to-Many with Products)
│   ├── ApplicationDbContext.cs    # EF Core DbContext
│   └── ErrorViewModel.cs         # Used by the Error page
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml           # Landing page with hero banner
│   │   ├── About.cshtml           # Company info page
│   │   ├── Contact.cshtml         # Contact details page
│   │   └── Privacy.cshtml         # Privacy policy (hidden from nav)
│   ├── Products/
│   │   ├── Catalog.cshtml         # Public product catalog with search & filter
│   │   ├── Index.cshtml           # Admin product list with CRUD actions
│   │   ├── Create.cshtml          # Add new product form
│   │   ├── Edit.cshtml            # Edit existing product form
│   │   ├── Details.cshtml         # Product detail view
│   │   └── Delete.cshtml          # Delete confirmation page
│   └── Shared/
│       ├── _Layout.cshtml         # Main layout with navbar and footer
│       ├── _ValidationScriptsPartial.cshtml
│       ├── Error.cshtml
│       ├── _ViewImports.cshtml
│       └── _ViewStart.cshtml
├── wwwroot/
│   ├── css/site.css               # Custom styles
│   ├── js/site.js                 # Custom scripts
│   ├── images/
│   │   ├── home-bg.png            # Hero banner background
│   │   └── default-image.jpg      # Fallback product image
│   └── lib/                       # jQuery, Bootstrap, jQuery Validation (local copies)
├── Properties/
│   └── launchSettings.json        # Dev server configuration
├── appsettings.json               # App configuration (connection string, logging)
├── appsettings.Development.json   # Development overrides
├── Program.cs                     # App entry point and DI configuration
├── OnlineStore.csproj             # Project file and NuGet dependencies
└── OnlineStore.db                 # SQLite database file (auto-created on first run)
```

---

## Features

### Public-facing
- **Hero Landing Page** — full-width banner image with overlay text and a call-to-action button linking to the catalog.
- **Product Catalog** — card-based grid view showing product name, truncated description, price, category badge, and image (with fallback to a default image if none is provided).
- **Search** — keyword search across product name and description using `EF.Functions.Like` for database-level pattern matching.
- **Category Filter** — dropdown filter to narrow catalog results by category.
- **About Page** — company description for SiM Workwear.
- **Contact Page** — email, phone number, and physical address.
- **Responsive Layout** — Bootstrap 5 grid and navbar collapse ensure usability on all screen sizes.
- **Sticky Footer** — footer stays at the bottom of the viewport using flexbox (`min-vh-100`, `flex-grow-1`, `mt-auto`).

### Admin / Management
- **Product List (Index)** — tabular view of all products with inline Edit, Details, and Delete action buttons using Bootstrap Icons.
- **Create Product** — form with fields for name, description, price, category (dropdown), and image URL. Includes server-side model validation.
- **Edit Product** — pre-populated form to update any product field.
- **Delete Product** — confirmation page before permanent deletion.
- **CSRF Protection** — all POST actions are decorated with `[ValidateAntiForgeryToken]`.

---

## Architecture & Design Principles

### MVC Pattern
The application strictly follows the **Model-View-Controller** pattern provided by ASP.NET Core:
- **Models** hold data structure and validation rules only.
- **Controllers** handle HTTP requests, interact with the database via EF Core, and pass data to views.
- **Views** are responsible solely for rendering HTML using Razor syntax.

### Dependency Injection
`ApplicationDbContext` is registered in `Program.cs` via `builder.Services.AddDbContext<>()` and injected into controllers through constructor injection — following the standard ASP.NET Core DI pattern.

### Data Annotations for Validation
Models use `[Required]`, `[StringLength]`, and `[Range]` attributes to enforce validation at both the server (ModelState) and client (jQuery Unobtrusive Validation) levels. Error messages are written in Bulgarian to match the target audience.

### Eager Loading
The `Products` controller uses `.Include(p => p.Category)` to eagerly load the related `Category` navigation property, avoiding N+1 query problems when displaying product lists.

### Null Safety
Models use the C# `required` keyword alongside nullable reference types (`?`) to prevent `NullReferenceException` at compile time. The `Category` navigation property is marked nullable (`Category?`) since it is optional at the object level.

### ANTIFORGERY Tokens
All state-changing POST endpoints (`Create`, `Edit`, `Delete`) are decorated with `[ValidateAntiForgeryToken]` to protect against Cross-Site Request Forgery attacks.

### Database Auto-Creation
`dbContext.Database.EnsureCreated()` is called at startup to automatically create the SQLite database and schema on first run — removing the need for manual migration commands in a development context.

---

## Database

The app uses **SQLite** with a single file database (`OnlineStore.db`) stored in the project root.

### Tables

**Categories**

| Column | Type | Notes |
|---|---|---|
| Id | INTEGER | Primary key, auto-increment |
| Name | TEXT | Required, max 100 chars |

**Products**

| Column | Type | Notes |
|---|---|---|
| Id | INTEGER | Primary key, auto-increment |
| Name | TEXT | Required, max 100 chars |
| Description | TEXT | Required, max 500 chars |
| Price | DECIMAL | Required, range 0.01–10,000 |
| CategoryId | INTEGER | Foreign key → Categories.Id |
| ImageUrl | TEXT | Optional, stores a URL string |

### Relationship
`Category` → `Product` is a **One-to-Many** relationship. A category has a `List<Product>` collection property; a product has a `CategoryId` foreign key and a nullable `Category?` navigation property.

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Any IDE: Visual Studio 2022+, VS Code with C# extension, or JetBrains Rider

### Running the App

```bash
# Clone the repository
git clone https://github.com/MaximSimeonovIvanov/Online-Store.git
cd Online-Store

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The app will start and the SQLite database (`OnlineStore.db`) will be created automatically on first run. Open your browser at `https://localhost:5001` or `http://localhost:5000`.

> **Note:** The database is created empty. You will need to manually insert `Category` records (e.g. via a SQLite browser or a data seed) before you can add products, as the category dropdown requires at least one entry.

---

## Configuration

The connection string is defined in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=OnlineStore.db"
  }
}
```

To change the database file location, update the `Data Source` path. No other configuration is required to run the application locally.

---

## Known Issues

- **Case-sensitive search** — The `EF.Functions.Like` query uses `.ToLower()` on the search term but SQLite's `LIKE` operator is already case-insensitive for ASCII characters. However, **Bulgarian Cyrillic characters are not handled** by SQLite's default `LIKE`, meaning searches for Bulgarian words may return inconsistent results depending on casing. A previous attempt to fix this was made (visible in the commit history) but was ultimately reverted.

- **No data seeding** — The database starts completely empty. There is no seed data for categories or products, so the catalog page will be blank on first run and the Create Product form cannot be used until categories are inserted manually.

- **Image handling is URL-only** — Products can only have an image by providing an external URL. There is no file upload functionality. If the URL is broken or empty, the app falls back to `default-image.jpg`.

- **Admin routes are publicly accessible** — The Products `Index`, `Create`, `Edit`, and `Delete` routes are not protected by authentication. Any visitor who knows the URL (`/Products/Create`, etc.) can add or delete products.

- **`UseStaticFiles()` called twice** — `Program.cs` registers `app.UseStaticFiles()` twice, which is redundant. It does not cause errors but should be cleaned up.

- **Debug `Console.WriteLine` statements left in production code** — The `Create` POST action in `ProductsController` contains `Console.WriteLine` calls used during development that were never removed.

- **Create/Edit forms lack Bootstrap styling** — Unlike the Catalog and Index views which are fully styled with Bootstrap classes, the `Create.cshtml` and `Edit.cshtml` forms use bare `<input>` and `<label>` tags without Bootstrap form classes (`form-control`, `form-label`, etc.), making them visually inconsistent with the rest of the app.

- **Details page is minimal** — `Details.cshtml` shows only the product name, description, and price as plain `<div>` elements with no styling, no category display, and no image.

- **Commented-out code** — Several blocks of code are commented out in `Program.cs` and `_Layout.cshtml` (e.g. the old Products default route, the Privacy nav link, local CSS imports), which adds noise and should be removed or documented.

---

## TODOs & Future Improvements

### High Priority
- [ ] **Seed initial data** — Add a `DbInitializer` or EF Core `HasData()` seeding for default categories (e.g. Workwear, PPE, Footwear, Accessories) so the app is usable immediately after cloning.
- [ ] **Add authentication & authorization** — Wire up the already-installed `Microsoft.AspNetCore.Identity` package to protect admin routes (`Create`, `Edit`, `Delete`) behind a login. Add an Admin role and restrict CRUD operations to authenticated admins only.
- [ ] **Fix Bulgarian search** — Configure SQLite with a custom collation or use `.ToLower()` consistently on both sides of the comparison at the application level to handle Cyrillic case-insensitive search correctly.

### Medium Priority
- [ ] **Style Create and Edit forms** — Apply Bootstrap `form-control`, `form-label`, `mb-3`, and `btn` classes consistently to match the rest of the UI.
- [ ] **Improve the Details page** — Display the product image, category badge, full description, and an "Add to Cart" placeholder. Match the styling of the Catalog cards.
- [ ] **File upload for images** — Replace the image URL input with a proper file upload field using `IFormFile`, saving images to `wwwroot/images/products/`.
- [ ] **Combine search and category filter** — Currently they are two separate `<form>` elements that reset each other. Merge them into a single form so a user can search within a category simultaneously.
- [ ] **Pagination** — Add pagination to the Catalog and Index views to handle large product lists gracefully.

### Low Priority / Polish
- [ ] **Remove debug `Console.WriteLine` calls** from `ProductsController`.
- [ ] **Remove duplicate `UseStaticFiles()`** call in `Program.cs`.
- [ ] **Clean up commented-out code** across `Program.cs` and `_Layout.cshtml`.
- [ ] **Add a shopping cart** — Implement a session-based or database-backed cart so visitors can select products and submit an order inquiry.
- [ ] **Add a Category management UI** — Currently categories can only be managed directly in the database. A simple CRUD controller and views for `Category` would make the app fully self-contained.
- [ ] **Add EF Core Migrations** — Replace `EnsureCreated()` with proper `dotnet ef migrations` to support schema evolution without data loss.
- [ ] **Add unit tests** — Write xUnit tests for controller actions and model validation logic.
- [ ] **Localization** — The UI is currently in Bulgarian. Consider extracting strings into resource files for easier maintenance or future multi-language support.

---

## License

This project is licensed under the terms found in the `LICENSE` file in the repository root.

---

*Built with ASP.NET Core 8 · Entity Framework Core 9 · SQLite · Bootstrap 5*
