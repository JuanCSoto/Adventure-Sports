// <copyright file="UserSettingRepository.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>UserSetting</c>
    /// </summary>
    public sealed class UserSettingRepository : DataRepository<UserSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public UserSettingRepository(ISession session)
            : base(session, "GXUserSetting")
        {
            this.Entity = new UserSetting();
        }

        /// <summary>
        /// Load the information from the table <c>UserSetting</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new UserSetting(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>UserSetting</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new UserSetting(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>UserSetting</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>UserSetting</c></returns>
        public override List<UserSetting> GetAll()
        {
            List<UserSetting> col = new List<UserSetting>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new UserSetting(this.Session.Reader));
            }

            return col;
        }
    }
}