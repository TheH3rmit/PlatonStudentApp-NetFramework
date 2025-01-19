# System Rejestracji Studentów

**Autor**: Jakub Pleban  
**Numer indeksu**: 61998  
**Kurs**: Projektowanie Aplikacji Wielowarstwowych  

---

## Opis (Polski)

To jest wielowarstwowa aplikacja internetowa stworzona w ramach projektu na przedmiot *Projektowanie Aplikacji Wielowarstwowych*. **System Rejestracji Studentów** pozwala użytkownikom zarządzać kursami i uczestniczyć w nich w zależności od roli: Administrator, Student lub Nauczyciel.

---

## Tech Stack

- **Frontend**: ASP.NET Web Forms, Bootstrap
- **Backend**: ASP.NET Framework
- **Baza danych**: Microsoft SQL Server
- **Hosting**: IIS lub Azure App Service
- **Języki programowania**: C#, SQL

---

## Funkcjonalności

### Funkcje Administratora:
- Zarządzanie użytkownikami (dodawanie, edytowanie, usuwanie).
- Podgląd szczegółów użytkowników, w tym studentów, nauczycieli i administratorów.
- Zarządzanie kursami (dodawanie, edytowanie, usuwanie).

### Funkcje Nauczyciela:
- Podgląd przypisanych kursów.
- Podgląd studentów zapisanych na ich kursy.
- Zarządzanie ocenami studentów.

### Funkcje Studenta:
- Podgląd dostępnych kursów.
- Zapis na kursy.
- Podgląd ocen i szczegółów zapisanych kursów.
- Wypisanie się z kursów.

### Ogólne:
- Kontrola dostępu oparta na rolach dla Administratora, Studenta i Nauczyciela.
- Uwierzytelnianie użytkowników z dashboardami specyficznymi dla ról.

---

## Domyślne Konta Użytkowników

Do logowania w aplikacji można użyć poniższych danych:

| **Rola**       | **Nazwa użytkownika** | **Hasło**    |
|----------------|------------------------|--------------|
| Administrator  | admin1                | admin1       |
| Student 1      | student1              | student1     |
| Student 2      | student2              | student2     |
| Nauczyciel 1   | teacher1              | teacher1     |
| Nauczyciel 2   | teacher2              | teacher2     |

---

## Jak Uruchomić

1. **Konfiguracja bazy danych**:
   - Przywróć bazę danych SQL Server dostarczoną z projektem.
   - Zaktualizuj connection string w pliku `Web.config`, podając dane dostępu do swojej bazy danych.

2. **Uruchamianie lokalne**:
   - Otwórz projekt w Visual Studio.
   - Zbuduj projekt, aby rozwiązać zależności.
   - Uruchom aplikację za pomocą IIS Express.

---

## Uwagi

- Upewnij się, że connection string do bazy danych jest prawidłowo skonfigurowany w pliku `Web.config`.
- Sprawdź, czy środowisko hostingu obsługuje ASP.NET Framework i ma zainstalowany wymagany runtime.


---

# Student Registration System

**Author**: Jakub Pleban  
**Student ID**: 61998  
**Course**: Projektowanie Aplikacji Wielowarstwowych  

---

## Overview

This is a multi-layered web application designed as a project for the *Projektowanie Aplikacji Wielowarstwowych* course. The **Student Registration System** allows users to manage and interact with courses in various roles such as Admin, Student, or Teacher.

---

## Tech Stack

- **Frontend**: ASP.NET Web Forms, Bootstrap
- **Backend**: ASP.NET Framework
- **Database**: Microsoft SQL Server
- **Deployment**: IIS or Azure App Service
- **Languages**: C#, SQL

---

## Functionalities

### Admin Features:
- Manage users (add, edit, delete).
- View user details, including students, teachers, and admins.
- Manage courses (add, edit, delete).

### Teacher Features:
- View courses assigned to them.
- View students enrolled in their courses.
- Update and manage grades for students.

### Student Features:
- View available courses.
- Enroll in courses.
- View grades and details for enrolled courses.
- Unenroll from courses.

### General:
- Role-based access control for Admin, Student, and Teacher roles.
- User authentication with role-specific dashboards.

---

## Default User Accounts

To access the application, use the following default credentials:

| **Role**     | **Username** | **Password** |
|--------------|--------------|--------------|
| Admin        | admin1       | admin1       |
| Student 1    | student1     | student1     |
| Student 2    | student2     | student2     |
| Teacher 1    | teacher1     | teacher1     |
| Teacher 2    | teacher2     | teacher2     |

---

## How to Run

1. **Setup Database**:
   - Restore the SQL Server database provided with the project.
   - Update the connection string in the `Web.config` file with your database credentials.

2. **Run Locally**:
   - Open the project in Visual Studio.
   - Build the project to resolve dependencies.
   - Run the application using IIS Express.

---

## Notes

- Ensure that the database connection string is configured correctly in the `Web.config`.
- Make sure your hosting environment supports ASP.NET Framework and has the required runtime installed.
