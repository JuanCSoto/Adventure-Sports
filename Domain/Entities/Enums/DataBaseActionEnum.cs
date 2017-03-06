namespace Domain.Entities.Enums
{
    /// <summary>
    /// Representa el enumerado para las acciones de base de datos.
    /// </summary>
    public enum DataBaseActionEnum
    {
        /// <summary>
        /// Define el valor para la acción Select.
        /// </summary>
        Select = 0,

        /// <summary>
        /// Define el valor para la acción Update.
        /// </summary>
        Update = 1,

        /// <summary>
        /// Define el valor para la acción Insert.
        /// </summary>
        Insert = 2,

        /// <summary>
        /// Define el valor para la acción Delete.
        /// </summary>
        Delete = 3,

        /// <summary>
        /// Define el valor para la acción SelectByPrimaryKey.
        /// </summary>
        SelectByPrimaryKey = 4
    }
}
