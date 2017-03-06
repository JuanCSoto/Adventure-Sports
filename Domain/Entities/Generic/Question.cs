// <copyright file="Question.cs" company="Dasigno">
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
    /// <c>Question</c> object mapped table <c>Question</c>.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Question"/> class
        /// </summary>
        public Question()
        {
            this.Type = TypeQuestion.Abierta;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Question"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public Question(IDataRecord obj)
        {
            this.ContentId = Convert.ToInt32(obj["ContentId"]);
            this.StartDate = Convert.ToDateTime(obj["StartDate"]);
            this.EndDate = Convert.ToDateTime(obj["EndDate"]);
            this.Type = (TypeQuestion)Convert.ToInt16(obj["Type"]);
            this.Description = Convert.ToString(obj["Description"]);
            this.DescriptionIngles = Convert.ToString(obj["Description2"]);
            this.XCoordinate = obj["XCoordinate"] != DBNull.Value ? Convert.ToSingle(obj["XCoordinate"]) : this.XCoordinate;
            this.YCoordinate = obj["YCoordinate"] != DBNull.Value ? Convert.ToSingle(obj["YCoordinate"]) : this.YCoordinate;
            this.Prize = obj["Prize"] != DBNull.Value ? Convert.ToString(obj["Prize"]) : this.Prize;
            this.PrizeIngles = obj["Prize2"] != DBNull.Value ? Convert.ToString(obj["Prize2"]) : this.PrizeIngles;
            this.Recommended = Convert.ToBoolean(obj["Recommended"]);
        }

        /// <summary>
        /// Defines the type of the question
        /// </summary>
        public enum TypeQuestion : short
        {
            /// <summary>
            /// Question type image
            /// </summary>
            Seleccion_Multiple = 0,

            /// <summary>
            /// Question type video
            /// </summary>
            Ubicacion = 1,

            /// <summary>
            /// Question type open
            /// </summary>
            Abierta = 2
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
        [Required(ErrorMessage = "Debes ingresar la fecha final de la pregunta")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the challenge end date
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@EndDate")]
        [Required(ErrorMessage = "Debes ingresar la fecha final de la pregunta")]
        [DateGreaterThan("StartDate", "La ficha final de la pregunta debe ser superior a la fecha de inicio")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the type question
        /// </summary>
        [InfoDatabase(DbType.Int16, "@Type")]
        public TypeQuestion Type { get; set; }

        /// <summary>
        /// Gets or sets the question description
        /// </summary>
        [InfoDatabase(DbType.String, "@Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la descripción de la pregunta")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the question description second language
        /// </summary>
        [InfoDatabase(DbType.String, "@Description2")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Debes ingresar la descripción de la pregunta")]
        public string DescriptionIngles { get; set; }

        /// <summary>
        /// Gets or sets the question x coordinate
        /// </summary>
        [InfoDatabase(DbType.Single, "@XCoordinate")]
        public float? XCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the question y coordinate
        /// </summary>
        [InfoDatabase(DbType.Single, "@YCoordinate")]
        public float? YCoordinate { get; set; }
        
        /// <summary>
        /// Gets or sets the question prize
        /// </summary>
        [InfoDatabase(DbType.String, "@Prize")]
        public string Prize { get; set; }

        /// <summary>
        /// Gets or sets the question prize second language
        /// </summary>
        [InfoDatabase(DbType.String, "@Prize2")]
        public string PrizeIngles { get; set; }

        /// <summary>
        /// Gets or sets if the question is recommended
        /// </summary>
        [InfoDatabase(DbType.Boolean, "@Recommended")]
        public bool? Recommended { get; set; }

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
