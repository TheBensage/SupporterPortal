## Overview
Prototype site built for demonstrating **Umbraco 13**, **.NET 8**, and a modular front-end with **Vite**, **TypeScript**, **SCSS/Bootstrap**, and **uSync**. Features include:

- Interactive hero section  
- Responsive header/navigation  
- Dynamic listing/ search
- Newsletter signup form with mocked CRM integration  
- Mock CRM membership & Authentication flow

<img width="2880" height="1704" alt="image" src="https://github.com/user-attachments/assets/dd700037-9029-4715-ae34-909bf5cf6931" />

## Tech Stack
- **Backend:** .NET 8, Umbraco 13  
- **Frontend:** Vite, TypeScript, SCSS, Bootstrap  
- **Auth:** Auth0 (OpenID Connect)  
- **Search:** Examine  
- **Content Sync:** uSync (supports multi-environment setup)  

---

## Project Structure
```
SupporterPortal/
  frontend/                          # Vite JS and SCSS bundler project
  SupporterPortal.Web/               # Umbraco project
  SupporterPortal.Infrastructure/    # Infrastructure layer
  SupporterPortal.Application/       # Application layer
  SupporterPortal.Domain/            # Domain layer
  README.md
```

## Frontend

### Vite & SCSS
- Builds all TS components dynamically  
- Outputs JS/CSS to `wwwroot/dist`  
- Custom Bootstrap theme via SCSS

### Dynamic Components
- Lazy-loaded with `import.meta.glob`  
- Initialized on viewport visibility using `IntersectionObserver`  
- Staggered animations via `.is-initialized`  

## Backend & Umbraco
- Umbraco 13 bootstrapped with backoffice, website, delivery API  
- Cookie + OpenID Connect authentication (Auth0)  
- Examine-based search  
- uSync: first-run prompt to install and import content; config-driven for multi-environment setups  

### Layout & Partials
- `_Layout.cshtml` loads `main.css` and `main.js`  
- Header, navigation, hero, block grid, and newsletter partials fully modular  
- Example usage in `HomePage` view:
```csharp
@inherits UmbracoViewPage<HomePage>
@{
    Layout = "_Layout.cshtml";
}
@await Html.PartialAsync("_Hero", Model.PageHero)
@Html.GetBlockGridHtml(Model.PageGrid)
@await Html.PartialAsync("_NewsLetterForm")
```
### Editor Experience
 - Modular document types & compositions
 - Editable page sections via backoffice
 - Reusable front-end components tied to content

### Newsletter & CRM (Mocked)
 - Captures name/email
 - Demonstrates validation, secure transmission, error handling

### CRM Membership Concept
 - Illustrates authentication, membership data, and personalized content
 - Secure API flow

## Getting Started
Prerequisites: .NET 8 SDK, Node 20+, 

Setup:

Frontend
```
npm install
npm run build
```

Backend/Umbraco
```
dotnet restore
dotnet run --project SupporterPortal.Web
```

On first run, if UmbracoDbDSN is missing, uSync prompts to import content/ you will need to install the database.  You will also need to provide your own Auth0 account details

Media is included ... for now!
