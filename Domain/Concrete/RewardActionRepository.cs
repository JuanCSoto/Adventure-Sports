// <copyright file="RewardActionRepository.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Concrete
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;
    using Domain.Entities;

    /// <summary>
    /// Class responsible for interaction with the board <c>RewardAction</c>
    /// </summary>
    public sealed class RewardActionRepository : DataRepository<RewardAction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RewardActionRepository"/> class
        /// </summary>
        /// <param name="session">Represents the interaction with the repository of information</param>
        public RewardActionRepository(ISession session)
            : base(session, "GXRewardAction")
        {
            this.Entity = new RewardAction();
        }

        /// <summary>
        /// Load the information from the table <c>RewardAction</c> to the respective entity according to the parameters send
        /// </summary>
        public override void Load()
        {
            base.Load();
            while (this.Session.Read())
            {
                this.Entity = new RewardAction(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>RewardAction</c> to the respective entity according to the identifier
        /// </summary>
        public override void LoadByKey()
        {
            base.LoadByKey();
            while (this.Session.Read())
            {
                this.Entity = new RewardAction(this.Session.Reader);
            }
        }

        /// <summary>
        /// Load the information from the table <c>RewardAction</c> to the list of entities according to the parameters send
        /// </summary>
        /// <returns>list of the entities type <c>RewardAction</c></returns>
        public override List<RewardAction> GetAll()
        {
            List<RewardAction> col = new List<RewardAction>();
            this.BindParameters(0);
            this.Session.ExecuteReader();
            while (this.Session.Read())
            {
                col.Add(new RewardAction(this.Session.Reader));
            }

            return col;
        }

        /// <summary>
        /// Update the user points
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>return the user points</returns>
        public int UserMedallosUpdate(int userId)
        {
            this.Session.CreateCommand("CTUserMedallosUpdate", true);
            this.Session.AddParameter("@UserId", userId, System.Data.ParameterDirection.Input, System.Data.DbType.Int32);
            int result = 0;
            object scalar = this.Session.ExecuteScalar();
            int.TryParse(scalar.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Set a new reward according to the user action
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="userAction">user action</param>
        /// <param name="medallos">point to give</param>
        /// <param name="maxPerDay">times per day this can happen</param>
        /// <returns>true if the action was successful false if not</returns>
        public bool SetUserRewardAction(int userId, RewardAction.UserActionType userAction, ref int medallos, int maxPerDay, bool onlyOne)
        {
            bool result = false;

            Session.Begin();

            RewardActionRepository reward = new RewardActionRepository(Session);
            reward.Entity.UserAction = (int)userAction;
            reward.Entity.UserId = userId;
            if (!onlyOne)
            {
                reward.Entity.Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            }

            int total = reward.GetAll().Count;
            if (total < maxPerDay || total == 0)
            {
                reward.Entity.Medallos = medallos;
                reward.Entity.Date = DateTime.Now;
                reward.Insert();
                int totalMedallos = this.UserMedallosUpdate(userId);

                UserRepository user = new UserRepository(Session);
                user.Entity.UserId = userId;
                user.LoadByKey();
                user.Entity.Medallos = totalMedallos;
                user.Update();

                result = true;
                Session.Commit();
                medallos = totalMedallos;
            }
            else
            {
                Session.RollBack();
            }

            return result;
        }
    }
}