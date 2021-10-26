using System.ComponentModel.DataAnnotations;

namespace net_registri_log.Shared.Models.Enums
{
    public enum OperazioneLogsEnum
    {
        [Display(Name = "Creato", Description = "Risorsa creata")]
        Creato,
        [Display(Name = "CreatoImportazione", Description = "Risorsa creata da una importazione")]
        CreatoImportazione,
        [Display(Name = "Modificato", Description = "Risorsa modificata")]
        Modificato,
        [Display(Name = "ModificatoImportazione", Description = "Risorsa modificata da una importazione")]
        ModificatoImportazione,
        [Display(Name = "Rimosso", Description = "Risorsa eliminata")]
        Rimosso,
        [Display(Name = "Eliminato", Description = "Risorsa rimossa")]
        Eliminato,
        [Display(Name = "LogClient", Description = "Log inviati dal client")]
        LogClient,
        [Display(Name = "AuditApi", Description = "Log api")]
        AuditApi,
    }

}
