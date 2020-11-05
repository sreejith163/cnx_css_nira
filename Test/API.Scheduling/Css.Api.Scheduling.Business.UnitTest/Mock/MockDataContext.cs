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
        public static IQueryable<Client> clientsDB = new List<Client>()
        {
            new Client() { Id = 1, RefId = 1, Name= "A", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 2, RefId = 2, Name= "B", CreatedBy = "Admin", CreatedDate = DateTime.Now },
            new Client() { Id = 3, RefId = 3, Name= "C", CreatedBy = "Admin", CreatedDate = DateTime.Now }
        }.AsQueryable();

        public static IQueryable<AgentSchedulingGroup> agentSchedulingGroupsDB = new List<AgentSchedulingGroup>()
        {
        }.AsQueryable();

        public static IQueryable<ClientLobGroup> clientLobGroupsDB = new List<ClientLobGroup>()
        {
            new ClientLobGroup{ Id = 1, ClientId = 1, FirstDayOfWeek = 1, Name = "A", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 2, ClientId = 2, FirstDayOfWeek = 1, Name = "B", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 3, ClientId = 3, FirstDayOfWeek = 1, Name = "C", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public static IQueryable<SkillGroup> skillGroupsDB = new List<SkillGroup>()
        {
        }.AsQueryable();

        public static IQueryable<SkillTag> skillTagsDB = new List<SkillTag>()
        {
        }.AsQueryable();

        public static IQueryable<AgentAdmin> agentAdminsDB = new List<AgentAdmin>()
        {
        }.AsQueryable();

        public static IQueryable<AgentSchedulingDetail> agentSchedulingDetailsDB = new List<AgentSchedulingDetail>()
        {
        }.AsQueryable();

        public static IQueryable<OperationHour> operationHoursDB = new List<OperationHour>()
        {
        }.AsQueryable();

        public static IQueryable<SchedulingCode> schedulingCodesDB = new List<SchedulingCode>()
        {
             new SchedulingCode { Id = 1, RefId = 1, Description = "test1", PriorityNumber = 1, EmployeeId = 1, IconId = 1, CreatedBy = "admin", 
                                  CreatedDate = DateTime.UtcNow },
             new SchedulingCode { Id = 2, RefId = 1, Description = "test2", PriorityNumber = 2, EmployeeId = 1, IconId = 2, CreatedBy = "admin",
                                  CreatedDate = DateTime.UtcNow },
             new SchedulingCode { Id = 3, RefId = 1, Description = "test3", PriorityNumber = 3, EmployeeId = 1, IconId = 3, CreatedBy = "admin",
                                  CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public static IQueryable<Timezone> timezonesDB = new List<Timezone>()
        {
            new Timezone { Id = 1, Name= "Dateline Standard Time", DisplayName = "(UTC-12:00) International Date Line West", Abbreviation = "DST", Offset = -12 },
            new Timezone { Id = 2, Name= "Hawaiian Standard Time", DisplayName = "(UTC-10:00) Hawaii", Abbreviation = "HST", Offset = -10 },
            new Timezone { Id = 3, Name= "Alaskan Standard Time", DisplayName = "(UTC-09:00) Alaska", Abbreviation = "AKDT", Offset = -8 }
        }.AsQueryable();

        public static IQueryable<SchedulingTypeCode> schedulingTypeCodes = new List<SchedulingTypeCode>()
        {
        }.AsQueryable();

        public static IQueryable<SchedulingStatus> schedulingStatusesDB = new List<SchedulingStatus>()
        {
        }.AsQueryable();

        public static IQueryable<SchedulingCodeType> schedulingCodeTypesDB = new List<SchedulingCodeType>()
        {
            new SchedulingCodeType { Id = 1, Description = "test1", Value = "A" },
            new SchedulingCodeType { Id = 2, Description = "test2", Value = "B" },
            new SchedulingCodeType { Id = 3, Description = "test3", Value = "C" },
            new SchedulingCodeType { Id = 4, Description = "test4", Value = "D" }
        }.AsQueryable();

        public static IQueryable<SchedulingCodeIcon> schedulingCodeIconsDB = new List<SchedulingCodeIcon>()
        {
            new SchedulingCodeIcon { Id = 1, Value = "1F30D", Description = "Earth Globe Europe-Africa" },
            new SchedulingCodeIcon { Id = 1, Value = "1F347", Description = "Grapes" },
            new SchedulingCodeIcon { Id = 1, Value = "1F383", Description = "Jack-O-Lantern" },
            new SchedulingCodeIcon { Id = 1, Value = "1F3C1", Description = "Chequered Flag" },
            new SchedulingCodeIcon { Id = 1, Value = "1F3E7", Description = "Automated Teller Machine" }
        }.AsQueryable();

        public static IQueryable<OperationHourOpenType> operationHourOpenTypesDB = new List<OperationHourOpenType>()
        {
        }.AsQueryable();

        public static IQueryable<LanguageTranslation> languageTranslationsDB = new List<LanguageTranslation>()
        {
        }.AsQueryable();

        public static IQueryable<CssVariable> cssVariablesDB = new List<CssVariable>()
        {
        }.AsQueryable();

        public static IQueryable<CssMenu> cssMenusDB = new List<CssMenu>()
        {
        }.AsQueryable();

        public static IQueryable<CssLanguage> cssLanguagesDB = new List<CssLanguage>()
        {
        }.AsQueryable();

        public static IQueryable<AgentSchedulingChart> agentSchedulingChartsDB = new List<AgentSchedulingChart>()
        {
        }.AsQueryable();

        public static IQueryable<AgentCategory> agentCategorysDB = new List<AgentCategory>()
        {
        }.AsQueryable();

        public static IQueryable<AgentGroupDetail> agentGroupDetailsDB = new List<AgentGroupDetail>()
        {
        }.AsQueryable();

        public static IQueryable<AgentDetail> agentDetailsDB = new List<AgentDetail>()
        {
        }.AsQueryable();

        public static IQueryable<AgentCategoryDataType> agentCategoryDataTypesDB = new List<AgentCategoryDataType>()
        {
        }.AsQueryable();

        #region Methods

        /// <summary>
        /// Intializes the mock data.
        /// </summary>
        /// <param name="mockDatabaseData">if set to <c>true</c> [mock database data].</param>
        public static Mock<SchedulingContext> IntializeMockData(bool mockDatabaseData)
        {
            Mock<SchedulingContext> mockSchedulingContext = new Mock<SchedulingContext>();
            if (mockDatabaseData)
            {
                var mockAgentAdmin = new Mock<DbSet<AgentAdmin>>();
                mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.Provider).Returns(MockDataContext.agentAdminsDB.Provider);
                mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.Expression).Returns(MockDataContext.agentAdminsDB.Expression);
                mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.ElementType).Returns(MockDataContext.agentAdminsDB.ElementType);
                mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.agentAdminsDB.GetEnumerator());

                var mockAgentCategory = new Mock<DbSet<AgentCategory>>();
                mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.Provider).Returns(MockDataContext.agentCategorysDB.Provider);
                mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.Expression).Returns(MockDataContext.agentCategorysDB.Expression);
                mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.ElementType).Returns(MockDataContext.agentCategorysDB.ElementType);
                mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.agentCategorysDB.GetEnumerator());

                var mockAgentCategoryDataType = new Mock<DbSet<AgentCategoryDataType>>();
                mockAgentCategoryDataType.As<IQueryable<AgentCategoryDataType>>().Setup(m => m.Provider).Returns(MockDataContext.agentCategoryDataTypesDB.Provider);
                mockAgentCategoryDataType.As<IQueryable<AgentCategoryDataType>>().Setup(m => m.Expression).Returns(MockDataContext.agentCategoryDataTypesDB.Expression);
                mockAgentCategoryDataType.As<IQueryable<AgentCategoryDataType>>().Setup(m => m.ElementType).Returns(MockDataContext.agentCategoryDataTypesDB.ElementType);
                mockAgentCategoryDataType.As<IQueryable<AgentCategoryDataType>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.agentCategoryDataTypesDB.GetEnumerator());

                var mockAgentDetail = new Mock<DbSet<AgentDetail>>();
                mockAgentDetail.As<IQueryable<AgentDetail>>().Setup(m => m.Provider).Returns(MockDataContext.agentDetailsDB.Provider);
                mockAgentDetail.As<IQueryable<AgentDetail>>().Setup(m => m.Expression).Returns(MockDataContext.agentDetailsDB.Expression);
                mockAgentDetail.As<IQueryable<AgentDetail>>().Setup(m => m.ElementType).Returns(MockDataContext.agentDetailsDB.ElementType);
                mockAgentDetail.As<IQueryable<AgentDetail>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.agentDetailsDB.GetEnumerator());

                var mockAgentGroupDetail = new Mock<DbSet<AgentGroupDetail>>();
                mockAgentGroupDetail.As<IQueryable<AgentGroupDetail>>().Setup(m => m.Provider).Returns(MockDataContext.agentGroupDetailsDB.Provider);
                mockAgentGroupDetail.As<IQueryable<AgentGroupDetail>>().Setup(m => m.Expression).Returns(MockDataContext.agentGroupDetailsDB.Expression);
                mockAgentGroupDetail.As<IQueryable<AgentGroupDetail>>().Setup(m => m.ElementType).Returns(MockDataContext.agentGroupDetailsDB.ElementType);
                mockAgentGroupDetail.As<IQueryable<AgentGroupDetail>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.agentGroupDetailsDB.GetEnumerator());

                var mockAgentSchedulingChart = new Mock<DbSet<AgentSchedulingChart>>();
                mockAgentSchedulingChart.As<IQueryable<AgentSchedulingChart>>().Setup(m => m.Provider).Returns(MockDataContext.agentSchedulingChartsDB.Provider);
                mockAgentSchedulingChart.As<IQueryable<AgentSchedulingChart>>().Setup(m => m.Expression).Returns(MockDataContext.agentSchedulingChartsDB.Expression);
                mockAgentSchedulingChart.As<IQueryable<AgentSchedulingChart>>().Setup(m => m.ElementType).Returns(MockDataContext.agentSchedulingChartsDB.ElementType);
                mockAgentSchedulingChart.As<IQueryable<AgentSchedulingChart>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.agentSchedulingChartsDB.GetEnumerator());

                var mockAgentSchedulingDetail = new Mock<DbSet<AgentSchedulingDetail>>();
                mockAgentSchedulingDetail.As<IQueryable<AgentSchedulingDetail>>().Setup(m => m.Provider).Returns(MockDataContext.agentSchedulingDetailsDB.Provider);
                mockAgentSchedulingDetail.As<IQueryable<AgentSchedulingDetail>>().Setup(m => m.Expression).Returns(MockDataContext.agentSchedulingDetailsDB.Expression);
                mockAgentSchedulingDetail.As<IQueryable<AgentSchedulingDetail>>().Setup(m => m.ElementType).Returns(MockDataContext.agentSchedulingDetailsDB.ElementType);
                mockAgentSchedulingDetail.As<IQueryable<AgentSchedulingDetail>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.agentSchedulingDetailsDB.GetEnumerator());

                var mockEAgentSchedulingGroup = new Mock<DbSet<AgentSchedulingGroup>>();
                mockEAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.Provider).Returns(MockDataContext.agentSchedulingGroupsDB.Provider);
                mockEAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.Expression).Returns(MockDataContext.agentSchedulingGroupsDB.Expression);
                mockEAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.ElementType).Returns(MockDataContext.agentSchedulingGroupsDB.ElementType);
                mockEAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.agentSchedulingGroupsDB.GetEnumerator());

                var mockClient = new Mock<DbSet<Client>>();
                mockClient.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(MockDataContext.clientsDB.Provider);
                mockClient.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(MockDataContext.clientsDB.Expression);
                mockClient.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(MockDataContext.clientsDB.ElementType);
                mockClient.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.clientsDB.GetEnumerator());

                var mockClientLobGroup = new Mock<DbSet<ClientLobGroup>>();
                mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Provider).Returns(MockDataContext.clientLobGroupsDB.Provider);
                mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Expression).Returns(MockDataContext.clientLobGroupsDB.Expression);
                mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.ElementType).Returns(MockDataContext.clientLobGroupsDB.ElementType);
                mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.clientLobGroupsDB.GetEnumerator());

                var mockCssLanguage = new Mock<DbSet<CssLanguage>>();
                mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.Provider).Returns(MockDataContext.cssLanguagesDB.Provider);
                mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.Expression).Returns(MockDataContext.cssLanguagesDB.Expression);
                mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.ElementType).Returns(MockDataContext.cssLanguagesDB.ElementType);
                mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.cssLanguagesDB.GetEnumerator());

                var mockCssMenu = new Mock<DbSet<CssMenu>>();
                mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.Provider).Returns(MockDataContext.cssMenusDB.Provider);
                mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.Expression).Returns(MockDataContext.cssMenusDB.Expression);
                mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.ElementType).Returns(MockDataContext.cssMenusDB.ElementType);
                mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.cssMenusDB.GetEnumerator());

                var mockCssVariable = new Mock<DbSet<CssVariable>>();
                mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.Provider).Returns(MockDataContext.cssVariablesDB.Provider);
                mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.Expression).Returns(MockDataContext.cssVariablesDB.Expression);
                mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.ElementType).Returns(MockDataContext.cssVariablesDB.ElementType);
                mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.cssVariablesDB.GetEnumerator());

                var mockLanguageTranslation = new Mock<DbSet<LanguageTranslation>>();
                mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.Provider).Returns(MockDataContext.languageTranslationsDB.Provider);
                mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.Expression).Returns(MockDataContext.languageTranslationsDB.Expression);
                mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.ElementType).Returns(MockDataContext.languageTranslationsDB.ElementType);
                mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.languageTranslationsDB.GetEnumerator());

                var mockOperationHour = new Mock<DbSet<OperationHour>>();
                mockOperationHour.As<IQueryable<OperationHour>>().Setup(m => m.Provider).Returns(MockDataContext.operationHoursDB.Provider);
                mockOperationHour.As<IQueryable<OperationHour>>().Setup(m => m.Expression).Returns(MockDataContext.operationHoursDB.Expression);
                mockOperationHour.As<IQueryable<OperationHour>>().Setup(m => m.ElementType).Returns(MockDataContext.operationHoursDB.ElementType);
                mockOperationHour.As<IQueryable<OperationHour>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.operationHoursDB.GetEnumerator());

                var mockOperationHourOpenType = new Mock<DbSet<OperationHourOpenType>>();
                mockOperationHourOpenType.As<IQueryable<OperationHourOpenType>>().Setup(m => m.Provider).Returns(MockDataContext.operationHourOpenTypesDB.Provider);
                mockOperationHourOpenType.As<IQueryable<OperationHourOpenType>>().Setup(m => m.Expression).Returns(MockDataContext.operationHourOpenTypesDB.Expression);
                mockOperationHourOpenType.As<IQueryable<OperationHourOpenType>>().Setup(m => m.ElementType).Returns(MockDataContext.operationHourOpenTypesDB.ElementType);
                mockOperationHourOpenType.As<IQueryable<OperationHourOpenType>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.operationHourOpenTypesDB.GetEnumerator());

                var mockSchedulingCode = new Mock<DbSet<SchedulingCode>>();
                mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.Provider).Returns(MockDataContext.schedulingCodesDB.Provider);
                mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.Expression).Returns(MockDataContext.schedulingCodesDB.Expression);
                mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.ElementType).Returns(MockDataContext.schedulingCodesDB.ElementType);
                mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.schedulingCodesDB.GetEnumerator());

                var mockSchedulingCodeIcon = new Mock<DbSet<SchedulingCodeIcon>>();
                mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Provider).Returns(MockDataContext.schedulingCodeIconsDB.Provider);
                mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Expression).Returns(MockDataContext.schedulingCodeIconsDB.Expression);
                mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.ElementType).Returns(MockDataContext.schedulingCodeIconsDB.ElementType);
                mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.schedulingCodeIconsDB.GetEnumerator());

                var mockSchedulingCodeType = new Mock<DbSet<SchedulingCodeType>>();
                mockSchedulingCodeType.As<IQueryable<SchedulingCodeType>>().Setup(m => m.Provider).Returns(MockDataContext.schedulingCodeTypesDB.Provider);
                mockSchedulingCodeType.As<IQueryable<SchedulingCodeType>>().Setup(m => m.Expression).Returns(MockDataContext.schedulingCodeTypesDB.Expression);
                mockSchedulingCodeType.As<IQueryable<SchedulingCodeType>>().Setup(m => m.ElementType).Returns(MockDataContext.schedulingCodeTypesDB.ElementType);
                mockSchedulingCodeType.As<IQueryable<SchedulingCodeType>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.schedulingCodeTypesDB.GetEnumerator());

                var mockSchedulingStatus = new Mock<DbSet<SchedulingStatus>>();
                mockSchedulingStatus.As<IQueryable<SchedulingStatus>>().Setup(m => m.Provider).Returns(MockDataContext.schedulingStatusesDB.Provider);
                mockSchedulingStatus.As<IQueryable<SchedulingStatus>>().Setup(m => m.Expression).Returns(MockDataContext.schedulingStatusesDB.Expression);
                mockSchedulingStatus.As<IQueryable<SchedulingStatus>>().Setup(m => m.ElementType).Returns(MockDataContext.schedulingStatusesDB.ElementType);
                mockSchedulingStatus.As<IQueryable<SchedulingStatus>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.schedulingStatusesDB.GetEnumerator());

                var mockSchedulingTypeCode = new Mock<DbSet<SchedulingTypeCode>>();
                mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.Provider).Returns(MockDataContext.schedulingTypeCodes.Provider);
                mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.Expression).Returns(MockDataContext.schedulingTypeCodes.Expression);
                mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.ElementType).Returns(MockDataContext.schedulingTypeCodes.ElementType);
                mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.schedulingTypeCodes.GetEnumerator());

                var mockSkillGroup = new Mock<DbSet<SkillGroup>>();
                mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.Provider).Returns(MockDataContext.skillGroupsDB.Provider);
                mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.Expression).Returns(MockDataContext.skillGroupsDB.Expression);
                mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.ElementType).Returns(MockDataContext.skillGroupsDB.ElementType);
                mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.skillGroupsDB.GetEnumerator());

                var mockSkillTag = new Mock<DbSet<SkillTag>>();
                mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.Provider).Returns(MockDataContext.skillTagsDB.Provider);
                mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.Expression).Returns(MockDataContext.skillTagsDB.Expression);
                mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.ElementType).Returns(MockDataContext.skillTagsDB.ElementType);
                mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.skillTagsDB.GetEnumerator());

                var mockTimezone = new Mock<DbSet<Timezone>>();
                mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Provider).Returns(MockDataContext.timezonesDB.Provider);
                mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Expression).Returns(MockDataContext.timezonesDB.Expression);
                mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.ElementType).Returns(MockDataContext.timezonesDB.ElementType);
                mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.timezonesDB.GetEnumerator());

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
            }

            return mockSchedulingContext;
        }

        #endregion
    }
}
