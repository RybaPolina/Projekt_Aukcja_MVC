# Auctions – Internetowy System Aukcyjny (ASP.NET MVC)

## Spis treści

1. [Opis projektu](#opis-projektu)
2. [Funkcjonalności](#funkcjonalności)
3. [Struktura plików i klas](#struktura-plików-i-klas)
4. [Instalacja i uruchomienie](#instalacja-i-uruchomienie)
5. [Konfiguracja bazy danych SQL Server](#konfiguracja-bazy-danych-sql-server)
6. [Przykładowe dane wejściowe](#przykładowe-dane-wejściowe)

---

## Opis projektu

**"Auctions"** to aplikacja internetowa umożliwiająca tworzenie, zarządzanie i udział w aukcjach. Projekt został stworzony w architekturze MVC z wykorzystaniem ASP.NET Core, Entity Framework oraz Identity do obsługi użytkowników.

---

## Funkcjonalności

- Rejestracja i logowanie użytkowników (ASP.NET Identity).
- Tworzenie, edycja i usuwanie własnych aukcji.
- Licytowanie aukcji przez innych użytkowników.
- Automatyczne zakończenie aukcji i wyłonienie zwycięzcy.
- System komentarzy pod aukcjami.
- Widoki dostosowane do ról użytkowników (np. właściciel aukcji może edytować i zakończyć aukcję).
- Widoczność najwyższej oferty oraz autora ostatniego podbicia.
- Paginacja na liście aukcji.

---

## Struktura plików i klas

```
Projekt_Aukcja_MVC-master/
│
├── Auctions.sln                         # Plik rozwiązania Visual Studio
├── README.md                            # Opis projektu
│
├── Auctions/                            # Główna aplikacja ASP.NET
│   ├── Controllers/                     # Kontrolery MVC
│   ├── Data/                            # EF Core, serwisy, migracje
│   ├── Models/                          # Modele danych
│   ├── Views/                           # Widoki Razor
│   ├── Areas/Identity/                  # Rejestracja, logowanie itd.
│   ├── appsettings.json                 # Plik konfiguracji połączenia z SQL Server
│   └── Program.cs                       # Punkt wejściowy
```

---

## Instalacja i uruchomienie

### Wymagania:
- Visual Studio 2022 (lub VS Code z SDK .NET 6+)
- .NET 6 SDK lub nowszy
- **SQL Server (np. Express lub Developer Edition)**

### Krok po kroku:

1. **Pobierz projekt lub rozpakuj ZIP:**
   ```bash
   cd Projekt_Aukcja_MVC-master
   ```

2. **Otwórz plik `Auctions.sln` w Visual Studio.**

3. **Przywróć zależności NuGet:**
   Visual Studio zrobi to automatycznie, albo:
   ```bash
   dotnet restore
   ```

4. **Zmień konfigurację połączenia z bazą danych:**

   W pliku `Auctions/appsettings.json` znajduje się linia:

   ```json
   "DefaultConnection": "Data Source=PROFITWAY-POLA\SQLEXPRESS;Initial Catalog=Auctions_Data;Integrated Security=True;..."
   ```

   Zmień `Data Source` na swoją instancję SQL Server, np.:

   - Dla lokalnego serwera Express:
     ```json
     "Data Source=localhost\SQLEXPRESS;Initial Catalog=Auctions_Data;Integrated Security=True;"
     ```
   - Dla autoryzacji SQL:
     ```json
     "Data Source=localhost;Initial Catalog=Auctions_Data;User ID=SA;Password=TwojeHaslo;"
     ```

5. **Wykonaj migracje bazy danych:**
   Z poziomu Visual Studio – otwórz **Package Manager Console**:
   ```powershell
   Update-Database
   ```

   Lub przez CLI:
   ```bash
   dotnet ef database update --project Auctions
   ```

6. **Uruchom aplikację (F5 lub):**
   ```bash
   dotnet run --project Auctions
   ```

7. **Dostęp:**
   Aplikacja będzie dostępna pod:
   ```
   http://localhost:5000 lub https://localhost:5001
   ```

---

## Konfiguracja bazy danych SQL Server

- Jeśli nie masz zainstalowanego SQL Server:
  - Pobierz [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
  - Zainstaluj z domyślną nazwą instancji `SQLEXPRESS`.
  - Skonfiguruj SQL Server Management Studio (SSMS), aby utworzyć i zarządzać bazą.

---

## Przykładowe dane wejściowe

- Nie są wymagane dodatkowe dane wejściowe.
- Wszystkie dane (aukcje, oferty, komentarze) dodaje się poprzez formularze w aplikacji.

---
