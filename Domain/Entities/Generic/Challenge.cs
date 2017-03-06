// <copyright file="Challenge.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Diego Toro</author>
namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// <c>Challenge</c> object mapped table <c>Challenge</c>.
    /// </summary>
    public class Challenge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Challenge"/> class
        /// </summary>
        public Challenge()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Challenge"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Challenge(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.StartDate = Convert.ToDateTime(obj["StartDate"]);
            this.EndDate = Convert.ToDateTime(obj["EndDate"]);
            this.Type = (TypeChallenge)Convert.ToInt16(obj["Type"]);
            this.Status = (StatusChallenge)Convert.ToInt16(obj["Status"]);
            this.Budget = obj["Budget"] != DBNull.Value ? Convert.ToSingle(obj["Budget"]) : this.Budget;
            this.Description = Convert.ToString(obj["Description"]);
            this.DescriptionIngles = Convert.ToString(obj["Description2"]);
            this.Prize = obj["Prize"] != DBNull.Value ? Convert.ToString(obj["Prize"]) : this.Prize;
            this.PrizeIngles = obj["Prize2"] != DBNull.Value ? Convert.ToString(obj["Prize2"]) : this.PrizeIngles;
            this.XCoordinate = obj["XCoordinate"] != DBNull.Value ? Convert.ToSingle(obj["XCoordinate"]) : this.XCoordinate;
            this.YCoordinate = obj["YCoordinate"] != DBNull.Value ? Convert.ToSingle(obj["YCoordinate"]) : this.YCoordinate;
            this.People = obj["People"] != DBNull.Value ? Convert.ToInt32(obj["People"]) : this.People;
            this.Recommended = Convert.ToBoolean(obj["Recommended"]);
            this.Followers = Convert.ToInt32(obj["Followers"]);
        }

        /// <summary>
        /// Defines the type of the challenge
        /// </summary>
        public enum TypeChallenge : short
        {
            /// <summary>
            /// Challenge type citizen participation
            /// </summary>
            Participacion_Ciudadana = 0,

            /// <summary>
            /// Challenge type city challenge
            /// </summary>
            Reto_Ciudad = 1
        }

        /// <summary>
        /// Defines the status of the challenge
        /// </summary>
        public enum StatusChallenge : short
        {
            /// <summary>
            /// Challenge Status active
            /// </summary>
            En_Curso = 0,

            /// <summary>
            /// Challenge Status finished
            /// </summary>
            Finalizado = 1,

            /// <summary>
            /// Challenge Status successfully finished
            /// </summary>
            Finalizado_Exitoso = 2,

            /// <summary>
            /// Challenge Status unsuccessfully finished
            /// </summary>
            Finalizado_No_Exitoso = 3,

            /// <summary>
            /// Challenge Status resumed
            /// </summary>
            Reanudado = 4,

            /// <summary>
            /// Challenge Status canceled
            /// </summary>
            Cancelado = 5
        }

        /// <summary>
        /// Gets or sets the identifier of the content
        /// </summary>
        [InfoDatabase(DbType.Int32, "@ContentId")]
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the challenge start date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@StartDate")]
        [Required(ErrorMessage = "Debes ingresar la fecha de inicio del desafio")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the challenge end date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@EndDate")]
        [Required(ErrorMessage = "Debes ingresar la fecha final del desafio")]
        [DateGreaterThan("StartDate", "La ficha final del reto debe ser superior a la fecha de inicio")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the challenge type
        /// </summary>
        [InfoDatabase(DbType.Int16, "@Type")]
        public TypeChallenge Type { get; set; }

        /// <summary>
        /// Gets or sets the challenge status
        /// </summary>
        [InfoDatabase(DbType.Int16, "@Status")]
        public StatusChallenge Status { get; set; }

        /// <summary>
        /// Gets or sets the challenge budget
        /// </summary>
        [InfoDatabase(DbType.Currency, "@Budget")]
        [Range(0d, float.MaxValue)]
        public float? Budget { get; set; }

        /// <summary>
        /// Gets or sets the challenge description
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la descripción del desafio")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the challenge description second language
        /// </summary>
        [InfoDatabase(DbType.String, "@Description2")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la descripción del desafio segundo lenguaje")]
        public string DescriptionIngles { get; set; }

        /// <summary>
        /// Gets or sets the challenge prize
        /// </summary>
        [InfoDatabase(DbType.String, "@Prize")]
        public string Prize { get; set; }

        /// <summary>
        /// Gets or sets the challenge prize second language
        /// </summary>
        [InfoDatabase(DbType.String, "@Prize2")]
        public string PrizeIngles { get; set; }

        /// <summary>
        /// Gets or sets the challenge x coordinate
        /// </summary>
        [InfoDatabase(DbType.Single, "@XCoordinate")]
        public float? XCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the challenge y coordinate
        /// </summary>
        [InfoDatabase(DbType.Single, "@YCoordinate")]
        public float? YCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the amount of people needed for the challenge
        /// </summary>
        [InfoDatabase(DbType.Int32, "@People")]
        public int? People { get; set; }

        /// <summary>
        /// Gets or sets if the challenge is recommended
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Recommended")]
        public bool? Recommended { get; set; }

        /// <summary>
        /// Gets or sets the followers
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Followers")]
        public int? Followers { get; set; }

        /// <summary>
        /// Obtiene o establece los identificadores de los tags.
        /// </summary>
        [InfoDatabase(DbType.String, "@ExistingTags")]
        public string ExistingTags { get; set; }

        /// <summary>
        /// Obtiene o establece los nombres de los nuevos tags.
        /// </summary>
        [InfoDatabase(DbType.String, "@NewTags")]
        public string NewTags { get; set; }
    }
}
