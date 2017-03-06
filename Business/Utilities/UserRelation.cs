// <copyright file="UserRelation.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno</author>
namespace Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Abstract;
    using Domain.Concrete;

    /// <summary>
    /// user relation system
    /// </summary>
    public static class UserRelation
    {
        /// <summary>
        /// start a new thread for the related content action
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="userRelationId">user relation id</param>
        /// <param name="relationId">relation id</param>
        /// <param name="action">text action</param>
        /// <param name="session">SQL session</param>
        public static void SaveRelationAction(int userId, int? userRelationId, int relationId, string action, ISession session)
        {
            Thread thread = new Thread(() => AsyncSaveRelationAction(userId, userRelationId, relationId, action, session));
            thread.Start();
        }

        /// <summary>
        /// prepare the user relation
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="userRelationId">user relation id</param>
        /// <param name="relationId">relation id</param>
        /// <param name="action">text action</param>
        /// <param name="session">SQL session</param>
        /// <returns>true if the relation was created false if not</returns>
        private static bool AsyncSaveRelationAction(int userId, int? userRelationId, int relationId, string action, ISession session)
        {
            bool result = false;            
            ////session.Begin();

            try
            {
                UserRelationRepository relation = new UserRelationRepository(session);
                IdeaRepository ideaRepository = new IdeaRepository(session);
                IdeaVoteRepository ideaVoteRepository = new IdeaVoteRepository(session);
                CommentRepository commentRepository = new CommentRepository(session);
                ChaellengeFollowerRepository followerRepository = new ChaellengeFollowerRepository(session);
                AnswerRepository answerRepository = new AnswerRepository(session);
                UserRepository userRepository = new UserRepository(session);
                ContentRepository contentRepository = new ContentRepository(session);
                int contentId = 0;
                switch (action)
                {
                    case "idea":
                        ideaRepository.Entity.IdeaId = relationId;
                        ideaRepository.LoadByKey();
                        if (ideaRepository.Entity.ContentId.HasValue)
                        {
                            contentId = ideaRepository.Entity.ContentId.Value;
                        }

                        break;
                    case "comment":
                        commentRepository.Entity.CommentId = relationId;
                        commentRepository.LoadByKey();
                        if (commentRepository.Entity.IdeaId.HasValue && userRelationId.HasValue)
                        {
                            ideaRepository.Entity.IdeaId = commentRepository.Entity.IdeaId;
                            ideaRepository.LoadByKey();
                            if (ideaRepository.Entity.ContentId.HasValue)
                            {
                                contentId = ideaRepository.Entity.ContentId.Value;
                            }

                            InsertAction(userId, userRelationId.Value, relationId, action, 3, session);
                            InsertAction(userRelationId.Value, userId, relationId, action, 3, session);
                        }

                        break;
                    case "like":
                        ideaRepository.Entity.IdeaId = relationId;
                        ideaRepository.LoadByKey();
                        if (ideaRepository.Entity.ContentId.HasValue)
                        {
                            InsertAction(userId, userRelationId.Value, relationId, action, 3, session);
                            InsertAction(userRelationId.Value, userId, relationId, action, 3, session);

                            contentId = ideaRepository.Entity.ContentId.Value;
                        }

                        break;
                    case "follow":
                        contentId = relationId;
                        break;
                    case "vote":                        
                        object tempId = contentRepository.GetContentIdByAnswerId(relationId);
                        if (tempId != null && int.TryParse(tempId.ToString(), out contentId))
                        {
                            List<Domain.Entities.User> same = userRepository.UserAnswerByAnswerId(relationId, userId);
                            foreach (Domain.Entities.User user in same)
                            {
                                InsertAction(userId, user.UserId.Value, relationId, action, 3, session);
                                InsertAction(user.UserId.Value, userId, relationId, action, 3, session);
                            }
                        }
                        else
                        {
                            contentId = 0;
                        }

                        break;
                }

                if (contentId != 0)
                {
                    // ideas
                    ideaRepository.Entity = new Domain.Entities.Idea();
                    ideaRepository.Entity.ContentId = contentId;
                    List<Domain.Entities.Idea> ideas = ideaRepository.GetAll();
                    IEnumerable<IGrouping<int?, Domain.Entities.Idea>> usersIdeas = ideas.Where(i => i.UserId != userId && i.Active == true).GroupBy(g => g.UserId);
                    foreach (IGrouping<int?, Domain.Entities.Idea> userIdeas in usersIdeas)
                    {
                        InsertAction(userId, userIdeas.Key.Value, relationId, action, 1, session);
                        foreach (Domain.Entities.Idea idea in userIdeas)
                        {
                            InsertAction(userIdeas.Key.Value, userId, idea.IdeaId.Value, "idea", 1, session);
                        }
                    }

                    // comentarios
                    commentRepository.Entity = new Domain.Entities.Comment();
                    commentRepository.Entity.ContentId = contentId;
                    List<Domain.Entities.Comment> comments = commentRepository.GetAll();
                    IEnumerable<IGrouping<int?, Domain.Entities.Comment>> usersComments = comments.Where(c => c.UserId != userId && c.Active == true).GroupBy(g => g.UserId);
                    foreach (IGrouping<int?, Domain.Entities.Comment> userComments in usersComments)
                    {
                        InsertAction(userId, userComments.Key.Value, relationId, action, 1, session);
                        foreach (Domain.Entities.Comment comment in userComments)
                        {
                            InsertAction(userComments.Key.Value, userId, comment.CommentId.Value, "comment", 1, session);
                        }
                    }

                    // likes - new sp needed
                    List<Domain.Entities.User> likes = userRepository.UserLikeByContentId(contentId, userId);                    
                    foreach (Domain.Entities.User like in likes)
                    {
                        InsertAction(userId, like.UserId.Value, relationId, action, 1, session);
                        InsertAction(like.UserId.Value, userId, like.Age.Value, "like", 1, session);
                    }

                    // followers
                    followerRepository.Entity = new Domain.Entities.ChaellengeFollower();
                    followerRepository.Entity.ChallengeId = contentId;
                    List<Domain.Entities.ChaellengeFollower> followers = followerRepository.GetAll();
                    foreach (Domain.Entities.ChaellengeFollower follower in followers)
                    {
                        InsertAction(userId, follower.UserId.Value, relationId, action, 1, session);
                        InsertAction(follower.UserId.Value, userId, contentId, "follow", 1, session);
                    }

                    // votes - new sp needed => answerId is int the age variable for this action
                    List<Domain.Entities.User> answers = userRepository.UserAnswerByContentId(contentId, userId);
                    foreach (Domain.Entities.User answer in answers)
                    {
                        InsertAction(userId, answer.UserId.Value, relationId, action, 1, session);
                        InsertAction(answer.UserId.Value, userId, answer.Age.Value, "vote", 1, session);
                    }

                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch 
            { 
            }
            finally
            {
                if (result)
                {
                    ////session.Commit();
                }
                else
                {
                    ////session.RollBack();
                }
            }

            return result;
        }

        /// <summary>
        /// save the user relation
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="userRelationId">user relation id</param>
        /// <param name="relationId">relation id</param>
        /// <param name="action">text action</param>
        /// <param name="value">relation value</param>
        /// <param name="session">SQL session</param>
        /// <returns>true if the relation was created false if not</returns>
        private static bool InsertAction(int userId, int userRelationId, int relationId, string action, int value, ISession session)
        {
            bool result = false;
            UserRelationRepository relation = new UserRelationRepository(session);

            relation.Entity.UserId = userId;
            relation.Entity.RelationId = relationId;
            relation.Entity.Action = action;
            relation.Entity.UserRelationId = userRelationId;
            if (!relation.Exist())
            {                
                relation.Entity.Value = value;
                relation.Entity.CreationDate = DateTime.Now;
                relation.Insert();
                result = true;
            }

            return result;
        }
    }
}
