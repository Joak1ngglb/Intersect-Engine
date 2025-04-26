using Intersect.Enums;
using Intersect.GameObjects;

public static class SearchHelper
{
    /// <summary>
    /// Comprueba si un ítem cumple los filtros de búsqueda: nombre, tipo y subtipo.
    /// </summary>
    /// <param name="item">El ítem a testear.</param>
    /// <param name="searchTerm">El texto a buscar en el nombre.</param>
    /// <param name="typeFilter">Filtro opcional por tipo de ítem (Enums.ItemType).</param>
    /// <param name="subtypeFilter">Filtro opcional por subtipo (por ejemplo, una cadena o un enum).</param>
    /// <returns>True si el ítem pasa todos los filtros.</returns>
    public static bool IsSearchable(
        ItemBase item,
        string searchTerm,
        ItemType? typeFilter = null,
        string subtypeFilter = null
    )
    {
        // 1) Filtro por tipo de ítem
        if (typeFilter.HasValue && item.ItemType != typeFilter.Value)
        {
            return false;
        }

        // 2) Filtro por subtipo
        if (!string.IsNullOrEmpty(subtypeFilter) &&
            !string.Equals(item.Subtype, subtypeFilter, StringComparison.CurrentCultureIgnoreCase))
        {
            return false;
        }

        // 3) Filtro por nombre (el que ya teníamos)
        return IsNameMatch(item.Name, searchTerm);
    }

    // Extraemos la lógica original de búsqueda por nombre a un método aparte
    private static bool IsNameMatch(string name, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return true;
        }
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        // Prefijo o sufijo (ignore case)
        if (name.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
            name.EndsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase))
        {
            return true;
        }

        // Subcadena solo si el término > 3 caracteres
        if (searchTerm.Length > 3 &&
            name.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) >= 0)
        {
            return true;
        }

        return false;
    }
}
