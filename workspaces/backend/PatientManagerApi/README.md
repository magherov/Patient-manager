# Patient Manager API

## Requisiti
- .NET 9.0 SDK (o versione successiva)

## Installazione
1. Clonare il repository o copiare i file del progetto
2. Aprire un terminale nella cartella `PatientManagerApi`
3. Eseguire il comando per ripristinare i pacchetti:
```bash
dotnet restore
```

## Esecuzione
Per avviare l'applicazione in modalità sviluppo:
```bash
dotnet run --launch-profile "http"
```

L'applicazione sarà disponibile all'indirizzo:
- API: http://localhost:5034/api/test
- Swagger UI: http://localhost:5034/swagger

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