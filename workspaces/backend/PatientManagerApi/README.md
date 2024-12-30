# Patient Manager API

## Tecnologie e Librerie
- **.NET 8**: Framework di sviluppo principale
- **Entity Framework Core**: ORM per la gestione del database
- **SQLite**: Database relazionale
- **SignalR**: Comunicazione real-time
- **JWT Authentication**: Autenticazione basata su token
- **Swagger/OpenAPI**: Documentazione API
- **AutoMapper**: Mappatura oggetti
- **Microsoft.AspNetCore.Identity**: Gestione utenti e ruoli

## Requisiti
- .NET 8.0 SDK (o versione successiva)
- Un IDE compatibile (Visual Studio, VS Code, ecc.)

## Installazione
1. Clonare il repository o copiare i file del progetto
2. Aprire un terminale nella cartella `PatientManagerApi`
3. Eseguire il comando per ripristinare i pacchetti:
```bash
dotnet restore
```

4. Applicare le migrazioni del database:
```bash
dotnet ef database update
```

## Esecuzione
Per avviare l'applicazione in modalità sviluppo:
```bash
dotnet run --launch-profile "http"
```

L'applicazione sarà disponibile all'indirizzo:
- API: http://localhost:5034/api
- Swagger UI: http://localhost:5034/swagger

## Struttura del Progetto
- `Controllers/`: Controller API REST
- `Models/`: Modelli di dominio e DTOs
- `Services/`: Servizi dell'applicazione
- `Data/`: Contesto del database e configurazioni
- `Hubs/`: Hub SignalR per comunicazione real-time
- `Migrations/`: Migrazioni del database

## Build per il Deploy
Per creare una versione pubblicabile dell'applicazione:
```bash
dotnet publish -c Release -o ./publish
```

Per eseguire la versione pubblicata:
```bash
cd publish
dotnet PatientManagerApi.dll
```

## API Endpoints Principali
- `POST /api/auth/login`: Autenticazione utente
- `GET /api/patients`: Lista pazienti
- `POST /api/patients`: Creazione nuovo paziente
- `GET /api/patients/{id}`: Dettagli paziente
- `PUT /api/patients/{id}`: Aggiornamento paziente
- `DELETE /api/patients/{id}`: Eliminazione paziente
