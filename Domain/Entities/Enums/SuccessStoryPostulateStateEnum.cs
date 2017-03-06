namespace Domain.Entities.Enums
{
    /// <summary>
    /// Representa el enumerado para los estados de la postulación de un caso de éxito.
    /// </summary>
    public enum SuccessStoryPostulateStateEnum
    {
        /// <summary>
        /// Define el valor para el estado nuevo
        /// </summary>
        New = 0,

        /// <summary>
        /// Define el valor para el estado pendiente.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Define el valor para el estado rechazado.
        /// </summary>
        Rejected = 2,

        /// <summary>
        /// Define el valor para el estado publicado.
        /// </summary>
        Published = 3
    }
}