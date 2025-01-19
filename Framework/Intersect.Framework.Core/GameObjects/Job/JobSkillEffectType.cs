namespace Intersect.Enums;
public enum JobSkillEffectType
{
    // Recolección
    IncreaseResourceYield,       // Incrementa la cantidad de recursos recolectados
    IncreaseRareDropChance,      // Aumenta la probabilidad de obtener objetos raros
    ReduceGatheringTime,         // Reduce el tiempo necesario para recolectar

    // Fabricación
    IncreaseSuccessRate,         // Aumenta la tasa de éxito en fabricación
    DecreaseFailureRate,         // Reduce la tasa de fallos en fabricación
    ReduceCraftingCost,          // Reduce el costo de materiales para fabricar
    UnlockExclusiveRecipes,      // Desbloquea recetas exclusivas

    // Bonificaciones Generales
    IncreaseJobExperienceGain,   // Incrementa la experiencia obtenida en el trabajo
    ReduceEnergyConsumption,     // Reduce el consumo de energía al realizar acciones del trabajo
    IncreaseDurabilityUsage,     // Mejora la durabilidad de herramientas usadas en el trabajo

    // Personalización de Equipos
    UnlockJobSpecificGear,       // Desbloquea equipo exclusivo del trabajo
    ImproveToolEfficiency,       // Mejora la eficiencia de herramientas específicas del trabajo

    // Otros
    IncreaseJobProgressSpeed,    // Acelera el progreso general en el trabajo
    UnlockUniqueInteractions,    // Desbloquea interacciones únicas (eventos o mecánicas especiales)
    None,
}
