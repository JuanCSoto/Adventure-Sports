// <copyright file="SystemNotificationType.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Domain.Entities.Basic
{
    /// <summary>
    /// enumeration for the system notification template
    /// </summary>
    public enum SystemNotificationType
    {
        /// <summary>
        /// receive a like for an idea
        /// </summary>
        RECEIVE_IDEA_LIKE = -176,

        /// <summary>
        /// receive a comment for a idea
        /// </summary>
        RECEIVE_IDEA_COMMENT = -177,

        /// <summary>
        /// an idea is shared in the web
        /// </summary>
        RECEIVE_IDEA_SHARE = -178,

        /// <summary>
        /// receive a like in a related idea
        /// </summary>
        RECEIVE_IDEA_RELATED_LIKE = -179,

        /// <summary>
        /// receive a comment in a related idea
        /// </summary>
        RECEIVE_IDEA_RELATED_COMMENT = -180,

        /// <summary>
        /// a related idea is edited
        /// </summary>
        EDIT_RELATED_IDEA = -181,

        /// <summary>
        /// the user idea gets into the top 10 of the pulses ideas
        /// </summary>
        IDEA_TOP_10 = -182,

        /// <summary>
        /// the user idea gets into the top 5 of the sites ideas
        /// </summary>
        POPULAR_IDEA_TOP_5 = -183,

        /// <summary>
        /// the user idea gets into the top 5 of the sites ideas
        /// </summary>
        VOTED_IDEA_TOP_5 = -184,

        /// <summary>
        /// there is a new process in the site
        /// </summary>
        NEW_PROCESS = -185,

        /// <summary>
        /// change the date of a process so it turn to active again
        /// </summary>
        RE_ACTIVATE_PROCESS = -186,

        /// <summary>
        /// finishing process
        /// </summary>
        FINISHING_PROCESS = -187,

        /// <summary>
        /// finished process
        /// </summary>
        FINISHED_PROCESS = -188,

        /// <summary>
        /// the user gets into the top 5 users of the site
        /// </summary>
        ACTIVE_USER_TOP_5 = -189,

        /// <summary>
        /// the user archives a new rank
        /// </summary>
        NEW_RANK = -190,

        /// <summary>
        /// postulate story
        /// </summary>
        POSTULATESTORY = -175,

        /// <summary>
        /// publicated story
        /// </summary>
        PUBLICATEDESTORY = -174,

        /// <summary>
        /// rejected story
        /// </summary>
        REJECTEDSTORY = -173
    }
}
