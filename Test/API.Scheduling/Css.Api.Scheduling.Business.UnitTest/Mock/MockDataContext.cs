using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Css.Api.Scheduling.Business.UnitTest.Mock
{
    /// <summary>
    /// MockDataContext
    /// </summary>
    public class MockDataContext
    {
        public IQueryable<Client> clientsDB = new List<Client>()
        {
            new Client() { Id = 1, RefId = 1, Name= "A", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 2, RefId = 2, Name= "B", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 3, RefId = 3, Name= "C", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 4, RefId = 4, Name= "D", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 5, RefId = 5, Name= "E", CreatedBy = "Admin", CreatedDate = DateTime.Now }
        }.AsQueryable();

        public IQueryable<AgentSchedulingGroup> agentSchedulingGroupsDB = new List<AgentSchedulingGroup>()
        {
        }.AsQueryable();

        public IQueryable<ClientLobGroup> clientLobGroupsDB = new List<ClientLobGroup>()
        {
            new ClientLobGroup{ Id = 1, ClientId = 1, FirstDayOfWeek = 1, Name = "A", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 2, ClientId = 2, FirstDayOfWeek = 1, Name = "B", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 3, ClientId = 3, FirstDayOfWeek = 1, Name = "C", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public IQueryable<SkillGroup> skillGroupsDB = new List<SkillGroup>()
        {
            new SkillGroup{ Id=1, ClientId=1, ClientLobGroupId=1, Name="group1", TimezoneId=1, CreatedDate=DateTime.UtcNow,CreatedBy="admin" },
            new SkillGroup{ Id=2, ClientId=1, ClientLobGroupId=1, Name="group2", TimezoneId=1, CreatedDate=DateTime.UtcNow,CreatedBy="admin" }
        }.AsQueryable();

        public IQueryable<SkillTag> skillTagsDB = new List<SkillTag>()
        {
            new SkillTag { Id = 1, ClientId = 1, ClientLobGroupId = 3, SkillGroupId = 1, Name="Skill1", CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new SkillTag { Id = 2, ClientId = 1, ClientLobGroupId = 3, SkillGroupId = 1, Name="Skill2", CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new SkillTag { Id = 3, ClientId = 1, ClientLobGroupId = 3, SkillGroupId = 1, Name="Skill3", CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public IQueryable<AgentAdmin> agentAdminsDB = new List<AgentAdmin>()
        {
        }.AsQueryable();

        public IQueryable<AgentSchedulingDetail> agentSchedulingDetailsDB = new List<AgentSchedulingDetail>()
        {
        }.AsQueryable();

        public IQueryable<OperationHour> operationHoursDB = new List<OperationHour>()
        {
            new OperationHour{ Id = 1, Day = 1, From = "9AM", SkillTagId = 1, To = "9PM", OperationHourOpenTypeId = 1}
        }.AsQueryable();

        public IQueryable<SchedulingCode> schedulingCodesDB = new List<SchedulingCode>()
        {
             new SchedulingCode { Id = 1, RefId = 1, Description = "test1", PriorityNumber = 1, IconId = 1, CreatedBy = "admin",
                                  CreatedDate = DateTime.UtcNow },
             new SchedulingCode { Id = 2, RefId = 1, Description = "test2", PriorityNumber = 2, IconId = 2, CreatedBy = "admin",
                                  CreatedDate = DateTime.UtcNow },
             new SchedulingCode { Id = 3, RefId = 1, Description = "test3", PriorityNumber = 3, IconId = 3, CreatedBy = "admin",
                                  CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public IQueryable<Timezone> timezonesDB = new List<Timezone>()
        {
            new Timezone { Id = 1, Name= "Dateline Standard Time", DisplayName = "(UTC-12:00) International Date Line West", Abbreviation = "DST", Offset = -12 },
            new Timezone { Id = 2, Name= "Hawaiian Standard Time", DisplayName = "(UTC-10:00) Hawaii", Abbreviation = "HST", Offset = -10 },
            new Timezone { Id = 3, Name= "Alaskan Standard Time", DisplayName = "(UTC-09:00) Alaska", Abbreviation = "AKDT", Offset = -8 }
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

        public IQueryable<OperationHourOpenType> operationHourOpenTypesDB = new List<OperationHourOpenType>()
        {
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

        public IQueryable<AgentSchedulingChart> agentSchedulingChartsDB = new List<AgentSchedulingChart>()
        {
        }.AsQueryable();

        public IQueryable<AgentCategory> agentCategorysDB = new List<AgentCategory>()
        {
        }.AsQueryable();

        public IQueryable<AgentGroupDetail> agentGroupDetailsDB = new List<AgentGroupDetail>()
        {
        }.AsQueryable();

        public IQueryable<AgentDetail> agentDetailsDB = new List<AgentDetail>()
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
        public Mock<SchedulingContext> IntializeMockData()
        {
            Mock<SchedulingContext> mockSchedulingContext = new Mock<SchedulingContext>();
            var mockAgentAdmin = new Mock<DbSet<AgentAdmin>>();
            mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.Provider).Returns(agentAdminsDB.Provider);
            mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.Expression).Returns(agentAdminsDB.Expression);
            mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.ElementType).Returns(agentAdminsDB.ElementType);
            mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.GetEnumerator()).Returns(agentAdminsDB.GetEnumerator());

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

            var mockAgentDetail = new Mock<DbSet<AgentDetail>>();
            mockAgentDetail.As<IQueryable<AgentDetail>>().Setup(m => m.Provider).Returns(agentDetailsDB.Provider);
            mockAgentDetail.As<IQueryable<AgentDetail>>().Setup(m => m.Expression).Returns(agentDetailsDB.Expression);
            mockAgentDetail.As<IQueryable<AgentDetail>>().Setup(m => m.ElementType).Returns(agentDetailsDB.ElementType);
            mockAgentDetail.As<IQueryable<AgentDetail>>().Setup(m => m.GetEnumerator()).Returns(agentDetailsDB.GetEnumerator());

            var mockAgentGroupDetail = new Mock<DbSet<AgentGroupDetail>>();
            mockAgentGroupDetail.As<IQueryable<AgentGroupDetail>>().Setup(m => m.Provider).Returns(agentGroupDetailsDB.Provider);
            mockAgentGroupDetail.As<IQueryable<AgentGroupDetail>>().Setup(m => m.Expression).Returns(agentGroupDetailsDB.Expression);
            mockAgentGroupDetail.As<IQueryable<AgentGroupDetail>>().Setup(m => m.ElementType).Returns(agentGroupDetailsDB.ElementType);
            mockAgentGroupDetail.As<IQueryable<AgentGroupDetail>>().Setup(m => m.GetEnumerator()).Returns(agentGroupDetailsDB.GetEnumerator());

            var mockAgentSchedulingChart = new Mock<DbSet<AgentSchedulingChart>>();
            mockAgentSchedulingChart.As<IQueryable<AgentSchedulingChart>>().Setup(m => m.Provider).Returns(agentSchedulingChartsDB.Provider);
            mockAgentSchedulingChart.As<IQueryable<AgentSchedulingChart>>().Setup(m => m.Expression).Returns(agentSchedulingChartsDB.Expression);
            mockAgentSchedulingChart.As<IQueryable<AgentSchedulingChart>>().Setup(m => m.ElementType).Returns(agentSchedulingChartsDB.ElementType);
            mockAgentSchedulingChart.As<IQueryable<AgentSchedulingChart>>().Setup(m => m.GetEnumerator()).Returns(agentSchedulingChartsDB.GetEnumerator());

            var mockAgentSchedulingDetail = new Mock<DbSet<AgentSchedulingDetail>>();
            mockAgentSchedulingDetail.As<IQueryable<AgentSchedulingDetail>>().Setup(m => m.Provider).Returns(agentSchedulingDetailsDB.Provider);
            mockAgentSchedulingDetail.As<IQueryable<AgentSchedulingDetail>>().Setup(m => m.Expression).Returns(agentSchedulingDetailsDB.Expression);
            mockAgentSchedulingDetail.As<IQueryable<AgentSchedulingDetail>>().Setup(m => m.ElementType).Returns(agentSchedulingDetailsDB.ElementType);
            mockAgentSchedulingDetail.As<IQueryable<AgentSchedulingDetail>>().Setup(m => m.GetEnumerator()).Returns(agentSchedulingDetailsDB.GetEnumerator());

            var mockEAgentSchedulingGroup = new Mock<DbSet<AgentSchedulingGroup>>();
            mockEAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.Provider).Returns(agentSchedulingGroupsDB.Provider);
            mockEAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.Expression).Returns(agentSchedulingGroupsDB.Expression);
            mockEAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.ElementType).Returns(agentSchedulingGroupsDB.ElementType);
            mockEAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.GetEnumerator()).Returns(agentSchedulingGroupsDB.GetEnumerator());

            var mockClient = new Mock<DbSet<Client>>();
            mockClient.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(clientsDB.Provider);
            mockClient.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(clientsDB.Expression);
            mockClient.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(clientsDB.ElementType);
            mockClient.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(clientsDB.GetEnumerator());

            var mockClientLobGroup = new Mock<DbSet<ClientLobGroup>>();
            mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Provider).Returns(clientLobGroupsDB.Provider);
            mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Expression).Returns(clientLobGroupsDB.Expression);
            mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.ElementType).Returns(clientLobGroupsDB.ElementType);
            mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.GetEnumerator()).Returns(clientLobGroupsDB.GetEnumerator());

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

            var mockOperationHour = new Mock<DbSet<OperationHour>>();
            mockOperationHour.As<IQueryable<OperationHour>>().Setup(m => m.Provider).Returns(operationHoursDB.Provider);
            mockOperationHour.As<IQueryable<OperationHour>>().Setup(m => m.Expression).Returns(operationHoursDB.Expression);
            mockOperationHour.As<IQueryable<OperationHour>>().Setup(m => m.ElementType).Returns(operationHoursDB.ElementType);
            mockOperationHour.As<IQueryable<OperationHour>>().Setup(m => m.GetEnumerator()).Returns(operationHoursDB.GetEnumerator());

            var mockOperationHourOpenType = new Mock<DbSet<OperationHourOpenType>>();
            mockOperationHourOpenType.As<IQueryable<OperationHourOpenType>>().Setup(m => m.Provider).Returns(operationHourOpenTypesDB.Provider);
            mockOperationHourOpenType.As<IQueryable<OperationHourOpenType>>().Setup(m => m.Expression).Returns(operationHourOpenTypesDB.Expression);
            mockOperationHourOpenType.As<IQueryable<OperationHourOpenType>>().Setup(m => m.ElementType).Returns(operationHourOpenTypesDB.ElementType);
            mockOperationHourOpenType.As<IQueryable<OperationHourOpenType>>().Setup(m => m.GetEnumerator()).Returns(operationHourOpenTypesDB.GetEnumerator());

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

            var mockSkillGroup = new Mock<DbSet<SkillGroup>>();
            mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.Provider).Returns(skillGroupsDB.Provider);
            mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.Expression).Returns(skillGroupsDB.Expression);
            mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.ElementType).Returns(skillGroupsDB.ElementType);
            mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.GetEnumerator()).Returns(skillGroupsDB.GetEnumerator());

            var mockSkillTag = new Mock<DbSet<SkillTag>>();
            mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.Provider).Returns(skillTagsDB.Provider);
            mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.Expression).Returns(skillTagsDB.Expression);
            mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.ElementType).Returns(skillTagsDB.ElementType);
            mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.GetEnumerator()).Returns(skillTagsDB.GetEnumerator());

            var mockTimezone = new Mock<DbSet<Timezone>>();
            mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Provider).Returns(timezonesDB.Provider);
            mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Expression).Returns(timezonesDB.Expression);
            mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.ElementType).Returns(timezonesDB.ElementType);
            mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.GetEnumerator()).Returns(timezonesDB.GetEnumerator());

            mockSchedulingContext.Setup(c => c.AgentAdmin).Returns(mockAgentAdmin.Object);
            mockSchedulingContext.Setup(c => c.AgentCategory).Returns(mockAgentCategory.Object);
            mockSchedulingContext.Setup(c => c.AgentCategoryDataType).Returns(mockAgentCategoryDataType.Object);
            mockSchedulingContext.Setup(c => c.AgentDetail).Returns(mockAgentDetail.Object);
            mockSchedulingContext.Setup(c => c.AgentGroupDetail).Returns(mockAgentGroupDetail.Object);
            mockSchedulingContext.Setup(c => c.AgentSchedulingChart).Returns(mockAgentSchedulingChart.Object);
            mockSchedulingContext.Setup(c => c.AgentSchedulingDetail).Returns(mockAgentSchedulingDetail.Object);
            mockSchedulingContext.Setup(c => c.AgentSchedulingGroup).Returns(mockEAgentSchedulingGroup.Object);
            mockSchedulingContext.Setup(c => c.Client).Returns(mockClient.Object);
            mockSchedulingContext.Setup(c => c.ClientLobGroup).Returns(mockClientLobGroup.Object);
            mockSchedulingContext.Setup(c => c.CssLanguage).Returns(mockCssLanguage.Object);
            mockSchedulingContext.Setup(c => c.CssMenu).Returns(mockCssMenu.Object);
            mockSchedulingContext.Setup(c => c.CssVariable).Returns(mockCssVariable.Object);
            mockSchedulingContext.Setup(c => c.LanguageTranslation).Returns(mockLanguageTranslation.Object);
            mockSchedulingContext.Setup(c => c.OperationHour).Returns(mockOperationHour.Object);
            mockSchedulingContext.Setup(c => c.OperationHourOpenType).Returns(mockOperationHourOpenType.Object);
            mockSchedulingContext.Setup(c => c.SchedulingCode).Returns(mockSchedulingCode.Object);
            mockSchedulingContext.Setup(c => c.SchedulingCodeIcon).Returns(mockSchedulingCodeIcon.Object);
            mockSchedulingContext.Setup(c => c.SchedulingCodeType).Returns(mockSchedulingCodeType.Object);
            mockSchedulingContext.Setup(c => c.SchedulingStatus).Returns(mockSchedulingStatus.Object);
            mockSchedulingContext.Setup(c => c.SchedulingTypeCode).Returns(mockSchedulingTypeCode.Object);
            mockSchedulingContext.Setup(c => c.SkillGroup).Returns(mockSkillGroup.Object);
            mockSchedulingContext.Setup(c => c.SkillTag).Returns(mockSkillTag.Object);
            mockSchedulingContext.Setup(c => c.Timezone).Returns(mockTimezone.Object);

            return mockSchedulingContext;
        }

        #endregion
    }
}
