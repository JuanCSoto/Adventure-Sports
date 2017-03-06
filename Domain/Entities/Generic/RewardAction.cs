// <copyright file="RewardAction.cs" company="Dasigno">
//  Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    using System.Data;
    using Domain.Abstract;

    /// <summary>
    /// RewardAction object mapped table <c>RewardAction</c>.
    /// </summary>
    public class RewardAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RewardAction"/> class
        /// </summary>
        public RewardAction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RewardAction"/> class
        /// </summary>
        /// <param name="obj">Provides access to the column values</param>
        public RewardAction(IDataRecord obj)
        {
            this.RewardActionId = Convert.ToInt64(obj["RewardActionId"]);
            this.UserId = Convert.ToInt32(obj["UserId"]);
            this.UserAction = Convert.ToInt32(obj["UserAction"]);
            this.Medallos = Convert.ToInt32(obj["Medallos"]);
            this.Date = Convert.ToDateTime(obj["Date"]);
        }

        /// <summary>
        /// enumerator for the rewarded user actions
        /// </summary>
        public enum UserActionType : int
        {
            /// <summary>
            /// the user login
            /// </summary>
            Login = 1,

            /// <summary>
            /// the user registry
            /// </summary>
            Registry = 2,

            /// <summary>
            /// complete the profile
            /// </summary>
            CompleteProfile = 3,

            /// <summary>
            /// like an idea
            /// </summary>
            LikeIdea = 4,

            /// <summary>
            /// receive a idea like
            /// </summary>
            ReciveLike = 5,

            /// <summary>
            /// vote for a versus
            /// </summary>
            VsIdea = 6,

            /// <summary>
            /// receive a versus vote
            /// </summary>
            ReciveVs = 7,

            /// <summary>
            /// hate the idea
            /// </summary>
            HateIdea = 8,

            /// <summary>
            /// receive a hate idea
            /// </summary>
            ReciveHate = 9,

            /// <summary>
            /// comment an idea
            /// </summary>
            CommentIdea = 10,

            /// <summary>
            /// receive a idea comment
            /// </summary>
            ReciveComment = 11,

            /// <summary>
            /// answer a question
            /// </summary>
            IdeaQuestion = 12,

            /// <summary>
            /// follow a challenge
            /// </summary>
            IdeaChallenge = 13,

            /// <summary>
            /// field fill photo
            /// </summary>
            FillProfilePhoto = 14,

            /// <summary>
            /// field fill gender
            /// </summary>
            FillProfileGender = 15,

            /// <summary>
            /// field fill age
            /// </summary>
            FillProfileAge = 16,

            /// <summary>
            /// field fill age
            /// </summary>
            FillProfileProfession = 17,

            /// <summary>
            /// field fill phone
            /// </summary>
            FillProfilePhone = 18,

            /// <summary>
            /// field fill country
            /// </summary>
            FillProfileCountry = 19,

            /// <summary>
            /// field fill city
            /// </summary>
            FillProfileCity = 20,

            /// <summary>
            /// field fill city
            /// </summary>
            FillProfileInterest = 21
        }

        /// <summary>
        /// Gets or sets the reward action id
        /// </summary>
        [InfoDatabase(DbType.Int64, "@RewardActionId")]
        public long? RewardActionId { get; set; }

        /// <summary>
        /// Gets or sets the  user id
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserId")]
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user action
        /// </summary>
        [InfoDatabase(DbType.Int32, "@UserAction")]
        public int UserAction { get; set; }

        /// <summary>
        /// Gets or sets the point to give
        /// </summary>
        [InfoDatabase(DbType.Int32, "@Medallos")]
        public int? Medallos { get; set; }

        /// <summary>
        /// Gets or sets the date of the reward
        /// </summary>
        [InfoDatabase(DbType.DateTime, "@Date")]
        public DateTime? Date { get; set; }
    }
}