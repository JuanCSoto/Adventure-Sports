// <copyright file="SuccessStoryPostulatePaging.cs" company="Intergrupo">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>
namespace Domain.Entities.FrontEnd
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Representa la clase <see cref="Domain.Entities.Generic.SuccessStoryPostulatePaging"/>.
    /// </summary>
    public class SuccessStoryPostulatePaging
    {
                /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Domain.Entities.Generic.SuccessStoryPostulatePaging"/>.
        /// </summary>        
        public SuccessStoryPostulatePaging()
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Domain.Entities.Generic.SuccessStoryPostulatePaging"/>.
        /// </summary>
        /// <param name="dataRecord">Data Record</param>
        public SuccessStoryPostulatePaging(IDataRecord dataRecord)
        {
            this.Id = Convert.ToInt32(dataRecord["Id"]);
            this.Name = Convert.ToString(dataRecord["Name"]);
            this.CreationDate = Convert.ToDateTime(dataRecord["CreationDate"]);
            this.State = Convert.ToByte(dataRecord["State"]);
            this.CityName = Convert.ToString(dataRecord["CityName"]);
            this.CountryName = Convert.ToString(dataRecord["CountryName"]);
        }

        /// <summary>
        /// Obtiene o establece el id de la postulación del caso de éxito.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del caso de éxito.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha en la que se postuló del caso de éxito.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Obtiene o establece el estado de la postulación del caso de éxito.
        /// </summary>
        public byte State { get; set; }

        /// <summary>
        /// Gets or sets nombre de la ciudad.
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets nombre del pais.
        /// </summary>
        public string CountryName { get; set; }
    }
}
