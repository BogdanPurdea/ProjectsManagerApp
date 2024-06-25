# Project Management ASP.NET Core and Angular Application

## Overview
This application is a project management system built with ASP.NET Core and Angular. It supports basic CRUD operations for managing projects, and features user and admin account capabilities. It uses ASP.NET Core Identity for authentication, JWT Authentication tokens for security, and Entity Framework for data persistence.

## Features

### User and Admin Account
Users can register and login to an account. Admin account has the privilege of user management.

### Project Management
Users can create, read, update, and delete (CRUD) projects. 

### Real Time Presence and Private Live Messaging
The application uses SignalR to provide real-time presence and a private live messaging service between users.

### Drag and Drop Photo Upload
Users can upload their profile photos or any necessary images with a drag and drop functionality.

### Filtering, Sorting, and Paging
The application provides convenient features like filtering, sorting, and paging of data for easier data management.

### JWT Authentication Tokens
For secure access to the software's resources, the application uses JWT authentication tokens.

## Latest Deployed Version
The latest deployed version of the application can be accessed [here](https://projectmanagementwebapp.azurewebsites.net/).

**Note:** The data associated with the deployed version is mocked data meant to demonstrate the functional features.

## TODO
- **Frontend Implementation for Creation of Projects**: Implement frontend functionality to allow users to create projects directly from the application interface.
- **Frontend Implementation for Deletion of Projects**: Implement frontend functionality to allow users to delete projects directly from the application interface.
  
## Technologies Used

- **ASP.NET Core**: This is used for building the server-side of the application.
- **ASP.NET Core Identity**: Utilized for managing users and their roles.
- **Angular**: A framework for building the client-side of the application.
- **SignalR**: Used for creating real-time web functionality.
- **AutoMapper**: A simple little library built to map objects.
- **JWT**: A compact URL-safe means of identifying users.
- **Entity Framework**: The data access technology used to handle data persistence.
