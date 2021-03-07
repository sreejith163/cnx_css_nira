using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Setup.UnitTest.Mock
{
    public class MockSkillGroupData
    {
        /// <summary>
        /// The SkillGroups
        /// </summary>
        private List<SkillGroup> skillGroupsDB = new List<SkillGroup>()
        {
             new SkillGroup { Id = 1, RefId = 1, Name = "skillGroup1", ClientId=1, ClientLobGroupId=1,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new SkillGroup { Id = 2, RefId = 1, Name = "skillGroup2",  ClientId=1, ClientLobGroupId=1,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new SkillGroup { Id = 3, RefId = 1, Name = "skillGroup3",  ClientId=1, ClientLobGroupId=1,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        };

        /// <summary>
        /// Gets the SkillGroups.
        /// </summary>
        /// <param name="SkillGroupParameters">The SkillGroup parameters.</param>
        /// <returns></returns>
        public CSSResponse GetSkillGroups(SkillGroupQueryParameter queryParameters)
        {
            var skillGroups = skillGroupsDB.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize);
            return new CSSResponse(skillGroups, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the skill group.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <returns></returns>
        public CSSResponse GetSkillGroup(SkillGroupIdDetails skillGroupId)
        {
            var skillGroup = skillGroupsDB.Where(x => x.Id == skillGroupId.SkillGroupId && x.IsDeleted == false).FirstOrDefault();
            return skillGroup != null ? new CSSResponse(skillGroup, HttpStatusCode.OK) : new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the skill group.
        /// </summary>
        /// <param name="createSkillGroup">The create skill group.</param>
        /// <returns></returns>
        public CSSResponse CreateSkillGroup(CreateSkillGroup createSkillGroup)
        {
            if (skillGroupsDB.Exists(x => x.IsDeleted == false && x.Name == createSkillGroup.Name))
            {
                return new CSSResponse($"Skill Group with name '{createSkillGroup.Name}' already exists.", HttpStatusCode.Conflict);
            }

            SkillGroup skillGroup = new SkillGroup()
            {
                Id = 4,
                RefId = createSkillGroup.RefId,
                CreatedBy = createSkillGroup.CreatedBy,
                Name = createSkillGroup.Name,
                ClientLobGroupId = createSkillGroup.ClientLobGroupId,
                FirstDayOfWeek = createSkillGroup.FirstDayOfWeek,
                TimezoneId = createSkillGroup.TimezoneId,
                CreatedDate = DateTime.UtcNow
            };

            skillGroupsDB.Add(skillGroup);

            return new CSSResponse(new SkillGroupIdDetails { SkillGroupId = skillGroup.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the skill group.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <param name="updateSkillGroup">The update skill group.</param>
        /// <returns></returns>
        public CSSResponse UpdateSkillGroup(SkillGroupIdDetails skillGroupIdDetails, UpdateSkillGroup updateSkillGroup)
        {
            if (!skillGroupsDB.Exists(x => x.IsDeleted == false && x.Id == skillGroupIdDetails.SkillGroupId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (skillGroupsDB.Exists(x => x.IsDeleted == false && x.Name == updateSkillGroup.Name && x.Id != skillGroupIdDetails.SkillGroupId))
            {
                return new CSSResponse($"Skill Group with name '{updateSkillGroup.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var skillGroup = skillGroupsDB.Where(x => x.IsDeleted == false && x.Id == skillGroupIdDetails.SkillGroupId).FirstOrDefault();
            skillGroup.Name = updateSkillGroup.Name;
            skillGroup.ClientLobGroupId = updateSkillGroup.ClientLobGroupId;
            skillGroup.ModifiedBy = updateSkillGroup.ModifiedBy;
            skillGroup.ModifiedDate = DateTime.UtcNow;
            skillGroup.FirstDayOfWeek = updateSkillGroup.FirstDayOfWeek;
            skillGroup.TimezoneId = updateSkillGroup.TimezoneId;

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the skill groups.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        public CSSResponse DeleteSkillGroups(SkillGroupIdDetails skillGroupIdDetails)
        {
            if (!skillGroupsDB.Exists(x => x.IsDeleted == false && x.Id == skillGroupIdDetails.SkillGroupId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillGroup = skillGroupsDB.Where(x => x.IsDeleted == false && x.Id == skillGroupIdDetails.SkillGroupId).FirstOrDefault();
            skillGroupsDB.Remove(skillGroup);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}