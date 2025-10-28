using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class BarbershopSchedule
{
    public int Id { get; set; }                          // BARBERIA_HORARIO.id
    public int BarbershopId { get; set; }                // BARBERIA_HORARIO.barberia_id (FK BARBERIA)
    public DayOfWeek DayOfWeek { get; set; }             // BARBERIA_HORARIO.dia_semana (0=domingo..6=sábado)
    public TimeOnly OpenAt { get; set; }                 // BARBERIA_HORARIO.abre
    public TimeOnly CloseAt { get; set; }                // BARBERIA_HORARIO.cierra
    public bool IsClosed { get; set; } = false;          // BARBERIA_HORARIO.cerrado
}
