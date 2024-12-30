using Microsoft.AspNetCore.SignalR;
using PatientManagerApi.Models;

namespace PatientManagerApi.Hubs;

public class AlarmHub : Hub
{
    // Metodo chiamato dal server per notificare tutti i client di un aggiornamento
    public async Task NotifyPatientsUpdate()
    {
        await Clients.All.SendAsync("PatientsUpdated");
    }
} 