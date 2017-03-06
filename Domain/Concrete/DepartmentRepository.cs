// <copyright file="DepartmentRepository.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the table <c>Department</c>
    /// </summary>
    public sealed class DepartmentRepository : DataRepository<Department>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public DepartmentRepository(ISession session)
            : base(session, "GXDepartment")
        {
            this.Entity = new Department();
        }

        /// <summary>
        /// Load the information from the table <c>Department</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (Session.Read())
            {
                this.Entity = new Department(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Department</c> to the respective entity according to the identifier 
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new Department(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>Department</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>Department</c></returns>
        public override List<Department> GetAll()
        {
            List<Department> col = new List<Department>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new Department(this.Session.Reader));
            }

            return col;
        }
    }
}