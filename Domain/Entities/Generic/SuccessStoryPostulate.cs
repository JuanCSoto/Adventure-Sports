namespace Domain.Entities.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Representa la clase <see cref="Domain.Entities.Generic.SuccessStoryPostulate"/>.
    /// </summary>
    public class SuccessStoryPostulate
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Domain.Entities.Generic.SuccessStoryPostulate"/>.
        /// </summary>        
        public SuccessStoryPostulate()
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Domain.Entities.Generic.SuccessStoryPostulate"/>.
        /// </summary>
        /// <param name="dataRecord"></param>
        public SuccessStoryPostulate(IDataRecord dataRecord)
        {
            this.Id = Convert.ToInt32(dataRecord["Id"]);
            this.UserId = Convert.ToInt32(dataRecord["UserId"]);
            this.CategoryId = Convert.ToInt32(dataRecord["CategoryId"]);
            this.ResponsibleNames = Convert.ToString(dataRecord["ResponsibleNames"]);
            this.ResponsibleEmail = Convert.ToString(dataRecord["ResponsibleEmail"]);
            this.ResponsibleJobTitle = Convert.ToString(dataRecord["ResponsibleJobTitle"]);
            this.ResponsibleOrganization = Convert.ToString(dataRecord["ResponsibleOrganization"]);
            this.CityId = Convert.ToInt32(dataRecord["CityId"]);
            this.ResponsibleEntities = Convert.ToString(dataRecord["ResponsibleEntities"]);
            this.Name = Convert.ToString(dataRecord["Name"]);
            this.CreationDate = Convert.ToDateTime(dataRecord["CreationDate"]);
            this.Description = Convert.ToString(dataRecord["Description"]);
            this.ConcreteProblems = Convert.ToString(dataRecord["ConcreteProblems"]);
            this.InnovativeUrbanSolution = Convert.ToString(dataRecord["InnovativeUrbanSolution"]);
            this.Documents = Convert.ToString(dataRecord["Documents"]);
            this.State = Convert.ToByte(dataRecord["State"]);
            this.SuccessStoryId = dataRecord["SuccessStoryId"] != DBNull.Value ? Convert.ToInt32(dataRecord["SuccessStoryId"]) : this.SuccessStoryId;
            this.CategoryName = Convert.ToString(dataRecord["CategoryName"]);
            this.CityName = Convert.ToString(dataRecord["CityName"]);
            this.CountryName = Convert.ToString(dataRecord["CountryName"]);
            this.LanguageId = dataRecord["LanguageId"] != DBNull.Value ? Convert.ToInt32(dataRecord["LanguageId"]) : this.LanguageId;
        }

        /// <summary>
        /// Obtiene o establece el id de la postulación del caso de éxito.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el id del usuario desde donde se postuló el caso de éxito.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el id de la categoría del caso de éxito.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la persona o compañía responsable del proyecto.
        /// </summary>
        public string ResponsibleNames { get; set; }

        /// <summary>
        /// Obtiene o establece el email de la persona o compañía responsable del proyecto.
        /// </summary>
        public string ResponsibleEmail { get; set; }

        /// <summary>
        /// Obtiene o establece el titulo (profesión) de la persona o compañía responsable del proyecto.
        /// </summary>
        public string ResponsibleJobTitle { get; set; }

        /// <summary>
        /// Obtiene o establece la organización responsable del proyecto.
        /// </summary>
        public string ResponsibleOrganization { get; set; }

        /// <summary>
        /// Obtiene o establece el id de la ciudad para la cual se postuló el caso de éxito.
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Obtiene o establece las entidades responsables del caso de éxito.
        /// </summary>
        public string ResponsibleEntities { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del caso de éxito.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha en la que se postuló del caso de éxito.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del caso de éxito.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece los problemas a los cuales se les quiere dar solución con el caso de éxito.
        /// </summary>
        public string ConcreteProblems { get; set; }

        /// <summary>
        /// Obtiene o establece el por qué el caso de éxito es una solución urbana innovadora.
        /// </summary>
        public string InnovativeUrbanSolution { get; set; }

        /// <summary>
        /// Obtiene o establece los identificadores de las palabras claves.
        /// </summary>
        public string IdsTag { get; set; }

        /// <summary>
        /// Obtiene o establece información relacionada con el caso de éxito.
        /// </summary>
        public string Documents { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de la postulación del caso de éxito.
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// Obtiene o establece el id del caso de éxito.
        /// </summary>
        public int? SuccessStoryId { get; set; }

        /// <summary>
        /// Obtiene o establece el lenguaje actual.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la ciudad.
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del país.
        /// </summary>
        public string CountryName { get; set; }
    }
}
