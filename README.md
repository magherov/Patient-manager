# Patient Manager

Sistema di gestione pazienti con frontend Angular e backend .NET.

## Tecnologie Utilizzate

### Backend (.NET)
- **.NET 8**: Framework di sviluppo principale
- **Entity Framework Core**: ORM per la gestione del database
- **SQLite**: Database relazionale
- **SignalR**: Comunicazione real-time
- **JWT Authentication**: Autenticazione basata su token
- **Swagger/OpenAPI**: Documentazione API
- **AutoMapper**: Mappatura oggetti
- **Microsoft.AspNetCore.Identity**: Gestione utenti e ruoli

### Frontend (Angular)
- **Angular 17**: Framework frontend
- **TailwindCSS**: Framework CSS utility-first
- **Prime ng**: Componenti lato ui
- **SignalR**: Client per comunicazione real-time
- **TypeScript**: Linguaggio di programmazione
- **SCSS**: Preprocessore CSS
- **PostCSS**: Tool per la trasformazione CSS

## Requisiti di Sistema

### Per il Backend
- .NET 8.0 SDK (o versione successiva)
- Un IDE compatibile (Visual Studio, VS Code, ecc.)

### Per il Frontend
- Node.js (v18 o superiore)
- npm (incluso con Node.js)

## Avvio in Locale

### Backend

1. Aprire un terminale nella cartella `workspaces/backend/PatientManagerApi`

2. Ripristinare i pacchetti:
```bash
dotnet restore
```

3. Applicare le migrazioni del database:
```bash
dotnet ef database update
```

4. Avviare il server:
```bash
dotnet run --launch-profile "http"
```

Il backend sarà disponibile su:
- API: http://localhost:5034/api
- Swagger UI: http://localhost:5034/swagger

### Frontend

1. Aprire un terminale nella cartella `workspaces/frontend`

2. Installare le dipendenze:
```bash
npm install
```

3. Avviare il server di sviluppo:
```bash
npm start
```
o
```bash
ng serve
```

Il frontend sarà disponibile su: http://localhost:4200

Vengono creati due User di default:
- admin = Username: Admin e password: admin
- user + Username: user e password: admin

## Struttura del Progetto

### Backend (`workspaces/backend/PatientManagerApi`)
- `Controllers/`: Controller API REST
- `Models/`: Modelli di dominio e DTOs
- `Services/`: Servizi dell'applicazione
- `Data/`: Contesto del database e configurazioni
- `Hubs/`: Hub SignalR per comunicazione real-time
- `Migrations/`: Migrazioni del database

### Frontend (`workspaces/frontend`)
- `src/app/components/`: Componenti Angular
- `src/app/services/`: Servizi per comunicazione API
- `src/app/dialogs/`: Componenti dialog
- `src/app/guard/`: Guard per le rotte
- `src/app/interceptors/`: Interceptor HTTP
- `src/app/utils/`: Funzioni di utilità
- `src/assets/`: Asset statici (immagini, icone)
- `src/environments/`: File di configurazione ambiente

## API Endpoints Principali
- `POST /api/auth/login`: Autenticazione utente
- `GET /api/patients`: Lista pazienti
- `POST /api/patients`: Creazione nuovo paziente
- `GET /api/patients/{id}`: Dettagli paziente
- `PUT /api/patients/{id}`: Aggiornamento paziente
- `DELETE /api/patients/{id}`: Eliminazione paziente
