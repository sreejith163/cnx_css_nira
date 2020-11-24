using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Css.Api.Admin.Business.UnitTest.Mock
{
    /// <summary>
    /// MockDataContext
    /// </summary>
    public class MockDataContext
    {
        public IQueryable<SchedulingCode> schedulingCodesDB = new List<SchedulingCode>()
        {
             new SchedulingCode { Id = 1, RefId = 1, Description = "test1", PriorityNumber = 1, IconId = 1, CreatedBy = "admin",
                                  CreatedDate = DateTime.UtcNow },
             new SchedulingCode { Id = 2, RefId = 1, Description = "test2", PriorityNumber = 2, IconId = 2, CreatedBy = "admin",
                                  CreatedDate = DateTime.UtcNow },
             new SchedulingCode { Id = 3, RefId = 1, Description = "test3", PriorityNumber = 3, IconId = 3, CreatedBy = "admin",
                                  CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public IQueryable<SchedulingTypeCode> schedulingTypeCodes = new List<SchedulingTypeCode>()
        {
        }.AsQueryable();

        public IQueryable<SchedulingStatus> schedulingStatusesDB = new List<SchedulingStatus>()
        {
        }.AsQueryable();

        public IQueryable<SchedulingCodeType> schedulingCodeTypesDB = new List<SchedulingCodeType>()
        {
            new SchedulingCodeType { Id = 1, Description = "test1", Value = "A" },
            new SchedulingCodeType { Id = 2, Description = "test2", Value = "B" },
            new SchedulingCodeType { Id = 3, Description = "test3", Value = "C" },
            new SchedulingCodeType { Id = 4, Description = "test4", Value = "D" }
        }.AsQueryable();

        public IQueryable<SchedulingCodeIcon> schedulingCodeIconsDB = new List<SchedulingCodeIcon>()
        {
            new SchedulingCodeIcon { Id = 1, Value = "1F30D", Description = "Earth Globe Europe-Africa" },
            new SchedulingCodeIcon { Id = 1, Value = "1F347", Description = "Grapes" },
            new SchedulingCodeIcon { Id = 1, Value = "1F383", Description = "Jack-O-Lantern" },
            new SchedulingCodeIcon { Id = 1, Value = "1F3C1", Description = "Chequered Flag" },
            new SchedulingCodeIcon { Id = 1, Value = "1F3E7", Description = "Automated Teller Machine" }
        }.AsQueryable();

        public IQueryable<LanguageTranslation> languageTranslationsDB = new List<LanguageTranslation>()
        {
        }.AsQueryable();

        public IQueryable<CssVariable> cssVariablesDB = new List<CssVariable>()
        {
        }.AsQueryable();

        public IQueryable<CssMenu> cssMenusDB = new List<CssMenu>()
        {
        }.AsQueryable();

        public IQueryable<CssLanguage> cssLanguagesDB = new List<CssLanguage>()
        {
        }.AsQueryable();

        public IQueryable<AgentCategory> agentCategorysDB = new List<AgentCategory>()
        {
        }.AsQueryable();

        public IQueryable<AgentCategoryDataType> agentCategoryDataTypesDB = new List<AgentCategoryDataType>()
        {
        }.AsQueryable();

        #region Methods

        /// <summary>
        /// Intializes the mock data.
        /// </summary>
        /// <returns></returns>
        public Mock<AdminContext> IntializeMockData()
        {
            Mock<AdminContext> mockAdminContext = new Mock<AdminContext>();

            var mockAgentCategory = new Mock<DbSet<AgentCategory>>();
            mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.Provider).Returns(agentCategorysDB.Provider);
            mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.Expression).Returns(agentCategorysDB.Expression);
            mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.ElementType).Returns(agentCategorysDB.ElementType);
            mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.GetEnumerator()).Returns(agentCategorysDB.GetEnumerator());

            var mockAgentCategoryDataType = new Mock<DbSet<AgentCategoryDataType>>();
            mockAgentCategoryDataType.As<IQueryable<AgentCategoryDataType>>().Setup(m => m.Provider).Returns(agentCategoryDataTypesDB.Provider);
            mockAgentCategoryDataType.As<IQueryable<AgentCategoryDataType>>().Setup(m => m.Expression).Returns(agentCategoryDataTypesDB.Expression);
            mockAgentCategoryDataType.As<IQueryable<AgentCategoryDataType>>().Setup(m => m.ElementType).Returns(agentCategoryDataTypesDB.ElementType);
            mockAgentCategoryDataType.As<IQueryable<AgentCategoryDataType>>().Setup(m => m.GetEnumerator()).Returns(agentCategoryDataTypesDB.GetEnumerator());

            var mockCssLanguage = new Mock<DbSet<CssLanguage>>();
            mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.Provider).Returns(cssLanguagesDB.Provider);
            mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.Expression).Returns(cssLanguagesDB.Expression);
            mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.ElementType).Returns(cssLanguagesDB.ElementType);
            mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.GetEnumerator()).Returns(cssLanguagesDB.GetEnumerator());

            var mockCssMenu = new Mock<DbSet<CssMenu>>();
            mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.Provider).Returns(cssMenusDB.Provider);
            mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.Expression).Returns(cssMenusDB.Expression);
            mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.ElementType).Returns(cssMenusDB.ElementType);
            mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.GetEnumerator()).Returns(cssMenusDB.GetEnumerator());

            var mockCssVariable = new Mock<DbSet<CssVariable>>();
            mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.Provider).Returns(cssVariablesDB.Provider);
            mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.Expression).Returns(cssVariablesDB.Expression);
            mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.ElementType).Returns(cssVariablesDB.ElementType);
            mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.GetEnumerator()).Returns(cssVariablesDB.GetEnumerator());

            var mockLanguageTranslation = new Mock<DbSet<LanguageTranslation>>();
            mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.Provider).Returns(languageTranslationsDB.Provider);
            mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.Expression).Returns(languageTranslationsDB.Expression);
            mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.ElementType).Returns(languageTranslationsDB.ElementType);
            mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.GetEnumerator()).Returns(languageTranslationsDB.GetEnumerator());

            var mockSchedulingCode = new Mock<DbSet<SchedulingCode>>();
            mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.Provider).Returns(schedulingCodesDB.Provider);
            mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.Expression).Returns(schedulingCodesDB.Expression);
            mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.ElementType).Returns(schedulingCodesDB.ElementType);
            mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.GetEnumerator()).Returns(schedulingCodesDB.GetEnumerator());

            var mockSchedulingCodeIcon = new Mock<DbSet<SchedulingCodeIcon>>();
            mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Provider).Returns(schedulingCodeIconsDB.Provider);
            mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Expression).Returns(schedulingCodeIconsDB.Expression);
            mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.ElementType).Returns(schedulingCodeIconsDB.ElementType);
            mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.GetEnumerator()).Returns(schedulingCodeIconsDB.GetEnumerator());

            var mockSchedulingCodeType = new Mock<DbSet<SchedulingCodeType>>();
            mockSchedulingCodeType.As<IQueryable<SchedulingCodeType>>().Setup(m => m.Provider).Returns(schedulingCodeTypesDB.Provider);
            mockSchedulingCodeType.As<IQueryable<SchedulingCodeType>>().Setup(m => m.Expression).Returns(schedulingCodeTypesDB.Expression);
            mockSchedulingCodeType.As<IQueryable<SchedulingCodeType>>().Setup(m => m.ElementType).Returns(schedulingCodeTypesDB.ElementType);
            mockSchedulingCodeType.As<IQueryable<SchedulingCodeType>>().Setup(m => m.GetEnumerator()).Returns(schedulingCodeTypesDB.GetEnumerator());

            var mockSchedulingStatus = new Mock<DbSet<SchedulingStatus>>();
            mockSchedulingStatus.As<IQueryable<SchedulingStatus>>().Setup(m => m.Provider).Returns(schedulingStatusesDB.Provider);
            mockSchedulingStatus.As<IQueryable<SchedulingStatus>>().Setup(m => m.Expression).Returns(schedulingStatusesDB.Expression);
            mockSchedulingStatus.As<IQueryable<SchedulingStatus>>().Setup(m => m.ElementType).Returns(schedulingStatusesDB.ElementType);
            mockSchedulingStatus.As<IQueryable<SchedulingStatus>>().Setup(m => m.GetEnumerator()).Returns(schedulingStatusesDB.GetEnumerator());

            var mockSchedulingTypeCode = new Mock<DbSet<SchedulingTypeCode>>();
            mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.Provider).Returns(schedulingTypeCodes.Provider);
            mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.Expression).Returns(schedulingTypeCodes.Expression);
            mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.ElementType).Returns(schedulingTypeCodes.ElementType);
            mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.GetEnumerator()).Returns(schedulingTypeCodes.GetEnumerator());

            mockAdminContext.Setup(c => c.AgentCategory).Returns(mockAgentCategory.Object);
            mockAdminContext.Setup(c => c.AgentCategoryDataType).Returns(mockAgentCategoryDataType.Object);
            mockAdminContext.Setup(c => c.CssLanguage).Returns(mockCssLanguage.Object);
            mockAdminContext.Setup(c => c.CssMenu).Returns(mockCssMenu.Object);
            mockAdminContext.Setup(c => c.CssVariable).Returns(mockCssVariable.Object);
            mockAdminContext.Setup(c => c.LanguageTranslation).Returns(mockLanguageTranslation.Object);
            mockAdminContext.Setup(c => c.SchedulingCode).Returns(mockSchedulingCode.Object);
            mockAdminContext.Setup(c => c.SchedulingCodeIcon).Returns(mockSchedulingCodeIcon.Object);
            mockAdminContext.Setup(c => c.SchedulingCodeType).Returns(mockSchedulingCodeType.Object);
            mockAdminContext.Setup(c => c.SchedulingStatus).Returns(mockSchedulingStatus.Object);
            mockAdminContext.Setup(c => c.SchedulingTypeCode).Returns(mockSchedulingTypeCode.Object);

            return mockAdminContext;
        }

        #endregion
    }
}
