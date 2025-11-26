using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models
{
    public class BarberSchedule
    {
        public int Id { get; set; }                          // BARBERO_HORARIO.id
        public int BarberId { get; set; }                    // BARBERO_HORARIO.barbero_id 
        public DayOfWeek DayOfWeek { get; set; }             // BARBERO_HORARIO.dia_semana
        public TimeOnly? OpenAt { get; set; }                // BARBERO_HORARIO.abre 
        public TimeOnly? CloseAt { get; set; }               // BARBERO_HORARIO.cierra
        public bool IsClosed { get; set; } = false;          // BARBERO_HORARIO.cerrado
    }
}
