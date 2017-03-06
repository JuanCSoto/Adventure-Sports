// <copyright file="EmailNotificationType.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.Basic
{
    /// <summary>
    /// enumeration for the email notification template
    /// </summary>
    public enum EmailNotificationType
    {
        /// <summary>
        /// receive a number of likes in a idea
        /// </summary>
        RECEIVE_N_IDEA_LIKE = -191,

        /// <summary>
        /// new process
        /// </summary>
        NEW_PROCESS = -192,

        /// <summary>
        /// finishing process
        /// </summary>
        FINISHING_PROCESS = -193,

        /// <summary>
        /// finished process
        /// </summary>
        FINISHED_PROCESS = -194,

        /// <summary>
        /// a user idea is blocked
        /// </summary>
        IDEA_BLOCKED = -195,

        /// <summary>
        /// user leave notification to administrator
        /// </summary>
        USER_LEAVE_ADMIN = -196,

        /// <summary>
        /// user leave notification to user
        /// </summary>
        USER_LEAVE_USER = -197,

        /// <summary>
        /// administrator leave notification to administrator
        /// </summary>
        ADMIN_KICKOUT_ADMIN = -198,

        /// <summary>
        /// administrator leave notification to user
        /// </summary>
        ADMIN_KICKOUT_USER = -199,

        /// <summary>
        /// user change of rank
        /// </summary>
        PROMOTION = -200,

        /// <summary>
        /// postulate story
        /// </summary>
        POSTULATESTORY = -201,

        /// <summary>
        /// publicated story
        /// </summary>
        PUBLICATEDESTORY = -202,

        /// <summary>
        /// rejected story
        /// </summary>
        REJECTEDSTORY = -203
    }
}
