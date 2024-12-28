using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PatientManagerApi.Data;
using PatientManagerApi.Hubs;
using PatientManagerApi.Models;

namespace PatientManagerApi.Services;

public class AlarmMonitorService : BackgroundService
{
    private readonly ILogger<AlarmMonitorService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<AlarmHub> _hubContext;
    private readonly Random _random;

    public AlarmMonitorService(
        ILogger<AlarmMonitorService> logger,
        IServiceScopeFactory scopeFactory,
        IHubContext<AlarmHub> hubContext)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
        _random = new Random();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servizio di monitoraggio allarmi avviato");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdatePatientAlarms();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento degli allarmi dei pazienti");
                // Aspetta un po' prima di riprovare in caso di errore
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task UpdatePatientAlarms()
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Recupera tutti i pazienti con i loro parametri
        var patients = await dbContext.Patients
            .Include(p => p.Parameters)
            .ToListAsync();

        var updated = false;

        foreach (var patient in patients)
        {
            // Genera casualmente lo stato di allarme (true/false)
            var hasAlarm = _random.Next(2) == 1;

            // Aggiorna lo stato di allarme per ogni parametro del paziente
            foreach (var parameter in patient.Parameters)
            {
                if (parameter.Alarm != hasAlarm)
                {
                    parameter.Alarm = hasAlarm;
                    updated = true;
                }
            }

            // Se il paziente non ha parametri, ne creiamo uno di default
            if (!patient.Parameters.Any())
            {
                patient.Parameters.Add(new Parameter
                {
                    PatientID = patient.ID,
                    Name = "DefaultParameter",
                    Value = "0",
                    Alarm = hasAlarm
                });
                updated = true;
            }
        }

        // Salva le modifiche nel database solo se ci sono stati aggiornamenti
        if (updated)
        {
            await dbContext.SaveChangesAsync();
            
            // Notifica tutti i client connessi dell'aggiornamento
            await _hubContext.Clients.All.SendAsync("PatientsUpdated");
            
            _logger.LogInformation("Allarmi dei pazienti aggiornati e notificati ai client");
        }
    }
} 