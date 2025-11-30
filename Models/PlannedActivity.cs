using System;

namespace SpoonTrack.Models
{
    public class PlannedActivity
    {
        public int Id { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public int SpoonCost { get; set; } // Quantos spoons gasta
        public DateTime PlannedDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? Notes { get; set; }
    }

    // Atividades pré-definidas com custos típicos
    public static class CommonActivities
    {
        public static readonly (string Name, int Cost)[] Activities = 
        {
            ("Tomar banho", 2),
            ("Vestir", 1),
            ("Cozinhar refeição", 3),
            ("Ir às compras", 4),
            ("Trabalhar 2h", 4),
            ("Reunião 1h", 3),
            ("Limpar casa", 5),
            ("Conduzir 30min", 2),
            ("Evento social", 4),
            ("Médico", 3),
            ("Exercício leve", 2),
            ("Telefonema importante", 1),
            ("Descanso ativo", -1), // Recupera 1 spoon
            ("Sesta", -2) // Recupera 2 spoons
        };
    }
}
