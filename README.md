# 🛒 Supermarket Management Application

## Overview

This repository contains a Supermarket Management Application built using C#, WPF, and SQL Server. The application is structured on the MVVM model and aims to provide a robust system for managing supermarket operations efficiently.

## Features

### General Features
- **📦 Products Management**: Store information about products, including product name, barcode, category, and manufacturer.
- **🏭 Manufacturers Management**: Manage manufacturers' details, including name and country of origin.
- **📂 Product Categories**: Support for various product categories such as food, clothing, stationery, etc.
- **📊 Stock Management**: Handle product stocks with details like quantity, unit of measure, date of procurement, expiry date, purchase price, and selling price.
- **👤 User Management**: Store information about users, including username, password, and user type.
- **🧾 Receipts Management**: Manage receipts with details like the issuance date, cashier, list of sold products with quantities and subtotals, and total amount collected.

### Administrator Functionalities
- **🛠️ CRUD Operations**: Add, modify, delete, and view users, categories, manufacturers, products, and stocks. Note that data is logically deleted (becomes inactive) rather than physically removed from the database.
- **📥 Stock Entry**: Manual entry of purchase price; the selling price is auto-calculated based on a predefined formula.
- **🔒 Price Validation**: Ensure purchase prices are immutable post-entry and selling prices cannot be set below the purchase price.
- **✅ Form Validation**: Validate all input fields in the forms for addition, modification, and deletion.
- **🔍 Data Search and Reporting**:
  - List products by a selected manufacturer and category.
  - Display the value of each product category based on current selling prices.
  - View the total amount collected by a user for a selected month, broken down by day.
  - Display the details of the largest receipt of the day, selected via a calendar interface.

### Cashier Functionalities
- **🔎 Product Search**: Search for products by name, barcode, expiry date, manufacturer, and category.
- **🧾 Receipt Management**: Issue and view receipt details. The subtotal and total amounts are calculated at the time of display.
  - Example:
    ```
    2 x Mineral Water .......... 8 lei
    3 x Milka Chocolate ..... 30 lei
    Total .............................. 38 lei
    ```
- **🔒 Immutable Receipts**: Once a receipt is confirmed, it cannot be modified.
- **📊 Stock Management**: Automatically manage stock quantities upon product sales. Stocks are depleted in the order they were received and marked inactive when empty or expired.

### Additional Features
- **🖼️ Barcode Display**: Display product barcodes as images using the ZXing.Net package.
- **🌍 Country API Integration**: Automatically populate the country of origin for manufacturers using data from the [CountriesNow API](https://countriesnow.space/).

## Technology Stack
- **💻 Programming Language**: C#
- **🖼️ Framework**: WPF (Windows Presentation Foundation)
- **💾 Database**: SQL Server
- **🛠️ ORM**: Entity Framework

## Installation and Setup
1. Clone the repository:
   ```sh
   git clone https://github.com/alexandru-pestritu/WPF_Supermarket
   ```
2. Open the solution file in Visual Studio.
3. Update the connection string in App.config to point to your SQL Server instance.
4. Build and run the application.
