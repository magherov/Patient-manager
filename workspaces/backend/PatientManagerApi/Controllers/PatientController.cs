using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagerApi.Models;
using PatientManagerApi.Data;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization; // Per l'ordinamento dinamico

namespace PatientManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PatientController> _logger;

    public PatientController(ApplicationDbContext context, ILogger<PatientController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/patient
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients([FromQuery] PatientQueryParameters queryParams)
    {
        try
        {
            // Validazione dei parametri di ordinamento
            if (!queryParams.IsValidSortDirection())
            {
                return BadRequest("Direzione di ordinamento non valida. Usa 'asc' o 'desc'.");
            }

            if (!queryParams.IsValidSortField())
            {
                return BadRequest("Campo di ordinamento non valido.");
            }

            // Query di base
            IQueryable<Patient> query = _context.Patients;

            // Applica i filtri se specificati
            if (!string.IsNullOrEmpty(queryParams.GivenName))
            {
                query = query.Where(p => p.GivenName.ToLower().Contains(queryParams.GivenName.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParams.FamilyName))
            {
                query = query.Where(p => p.FamilyName.ToLower().Contains(queryParams.FamilyName.ToLower()));
            }

            // Applica l'ordinamento se specificato
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                var sortDirection = (queryParams.SortDirection?.ToLower() ?? "asc") == "desc" ? "descending" : "ascending";
                
                // Questa riga formatta il nome del campo per l'ordinamento nel formato corretto PascalCase
                // Esempio: se queryParams.SortBy = "givenname" o "GIVENNAME" o "givenName"
                // 1. char.ToUpper(queryParams.SortBy[0]) -> prende il primo carattere e lo converte in maiuscolo
                //    es: 'g' diventa 'G'
                // 2. queryParams.SortBy.Substring(1).ToLower() -> prende tutti i caratteri dal secondo in poi e li converte in minuscolo
                //    es: "ivenname" diventa "ivenname"
                // 3. Le due parti vengono concatenate
                //    es: "G" + "ivenname" = "Givenname"
                // Questo è necessario perché le proprietà nel modello Patient sono in PascalCase (es: GivenName)
                var sortField = char.ToUpper(queryParams.SortBy[0]) + queryParams.SortBy.Substring(1).ToLower();
                
                query = query.OrderBy($"{sortField} {sortDirection}");
            }

            var patients = await query.Include(p => p.Parameters).ToListAsync();
            return Ok(patients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei pazienti");
            return StatusCode(500, "Si è verificato un errore durante il recupero dei pazienti");
        }
    }

    // GET: api/patient/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound($"Paziente con ID {id} non trovato");
            }

            return Ok(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del paziente {PatientId}", id);
            return StatusCode(500, "Si è verificato un errore durante il recupero del paziente");
        }
    }

    // POST: api/patient
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Patient>> CreatePatient([FromBody] Patient patient)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Rimuovi l'ID se è stato fornito
            patient.ID = 0;

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Restituisci il paziente con l'ID generato
            return CreatedAtAction(nameof(GetPatient), new { id = patient.ID }, patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione del paziente");
            return StatusCode(500, "Si è verificato un errore durante la creazione del paziente");
        }
    }

    // PUT: api/patient/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdatePatient(int id, Patient patient)
    {
        try
        {
            if (id != patient.ID)
            {
                return BadRequest("L'ID nel percorso non corrisponde all'ID del paziente");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound($"Paziente con ID {id} non trovato");
                }
                throw;
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiornamento del paziente {PatientId}", id);
            return StatusCode(500, "Si è verificato un errore durante l'aggiornamento del paziente");
        }
    }

    // DELETE: api/patient/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePatient(int id)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound($"Paziente con ID {id} non trovato");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione del paziente {PatientId}", id);
            return StatusCode(500, "Si è verificato un errore durante l'eliminazione del paziente");
        }
    }

    private bool PatientExists(int id)
    {
        return _context.Patients.Any(e => e.ID == id);
    }
}