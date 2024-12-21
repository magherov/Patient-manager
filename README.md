# Patient Manager

Applicazione per la gestione dei pazienti con architettura workspace.

## Struttura del Progetto

Il progetto è organizzato in workspaces:

### Frontend (`/workspaces/frontend`)
- Applicazione Angular
- UI per la gestione dei pazienti
- Utilizza Tailwind CSS per lo styling

### Backend (`/workspaces/backend`)
- API REST in .NET Core
- Gestisce l'autenticazione e le operazioni CRUD sui pazienti
- Database SQLite

## Requisiti di Sviluppo

### Frontend
- Node.js
- npm
- Angular CLI

### Backend
- .NET 8 SDK
- Visual Studio 2022 o VS Code con estensione C#

## Come Iniziare

1. **Backend**
   ```bash
   cd workspaces/backend/PatientManagerApi
   dotnet restore
   dotnet run
   ```

2. **Frontend**
   ```bash
   cd workspaces/frontend
   npm install
   ng serve
   ```

L'applicazione frontend sarà disponibile su `http://localhost:4200`
L'API backend sarà disponibile su `http://localhost:5000`
