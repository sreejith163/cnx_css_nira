using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Setup.UnitTest.Mock
{
    public class MockSkillTagData
    {
        /// <summary>
        /// The SkillTags
        /// </summary>
        private List<SkillTag> skillTagsDB = new List<SkillTag>()
        {
             new SkillTag { Id = 1, RefId = 1, Name = "skillTag1", ClientId=1, ClientLobGroupId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new SkillTag { Id = 2, RefId = 1, Name = "skillTag2",  ClientId=1, ClientLobGroupId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new SkillTag { Id = 3, RefId = 1, Name = "skillTag3",  ClientId=1, ClientLobGroupId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        };

        /// <summary>
        /// Gets the SkillTags.
        /// </summary>
        /// <param name="SkillTagParameters">The SkillTag parameters.</param>
        /// <returns></returns>
        public CSSResponse GetSkillTags(SkillTagQueryParameter queryParameters)
        {
            var skillTags = skillTagsDB.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize);
            return new CSSResponse(skillTags, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        /// <returns></returns>
        public CSSResponse GetSkillTag(SkillTagIdDetails skillTagId)
        {
            var skillTag = skillTagsDB.Where(x => x.Id == skillTagId.SkillTagId && x.IsDeleted == false).FirstOrDefault();
            return skillTag != null ? new CSSResponse(skillTag, HttpStatusCode.OK) : new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the skill tag.
        /// </summary>
        /// <param name="createSkillTag">The create skill tag.</param>
        /// <returns></returns>
        public CSSResponse CreateSkillTag(CreateSkillTag createSkillTag)
        {
            if (skillTagsDB.Exists(x => x.IsDeleted == false && x.Name == createSkillTag.Name))
            {
                return new CSSResponse($"Skill Group with name '{createSkillTag.Name}' already exists.", HttpStatusCode.Conflict);
            }

            SkillTag skillTag = new SkillTag()
            {
                Id = 4,
                RefId = createSkillTag.RefId,
                CreatedBy = createSkillTag.CreatedBy,
                Name = createSkillTag.Name,
                SkillGroupId = createSkillTag.SkillGroupId,
                CreatedDate = DateTime.UtcNow
            };

            skillTagsDB.Add(skillTag);

            return new CSSResponse(new SkillTagIdDetails { SkillTagId = skillTag.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <param name="updateSkillTag">The update skill tag.</param>
        /// <returns></returns>
        public CSSResponse UpdateSkillTag(SkillTagIdDetails skillTagIdDetails, UpdateSkillTag updateSkillTag)
        {
            if (!skillTagsDB.Exists(x => x.IsDeleted == false && x.Id == skillTagIdDetails.SkillTagId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (skillTagsDB.Exists(x => x.IsDeleted == false && x.Name == updateSkillTag.Name && x.Id != skillTagIdDetails.SkillTagId))
            {
                return new CSSResponse($"Skill Group with name '{updateSkillTag.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var skillTag = skillTagsDB.Where(x => x.IsDeleted == false && x.Id == skillTagIdDetails.SkillTagId).FirstOrDefault();
            skillTag.Name = updateSkillTag.Name;
            skillTag.SkillGroupId = updateSkillTag.SkillGroupId;
            skillTag.ModifiedBy = updateSkillTag.ModifiedBy;
            skillTag.ModifiedDate = DateTime.UtcNow;

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the skill tags.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        public CSSResponse DeleteSkillTags(SkillTagIdDetails skillTagIdDetails)
        {
            if (!skillTagsDB.Exists(x => x.IsDeleted == false && x.Id == skillTagIdDetails.SkillTagId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillTag = skillTagsDB.Where(x => x.IsDeleted == false && x.Id == skillTagIdDetails.SkillTagId).FirstOrDefault();
            skillTagsDB.Remove(skillTag);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}