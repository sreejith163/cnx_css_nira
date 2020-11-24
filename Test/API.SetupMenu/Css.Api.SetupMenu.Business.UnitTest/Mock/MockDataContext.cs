using Css.Api.SetupMenu.Models.Domain;
using Css.Api.SetupMenu.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Css.Api.SetupMenu.Business.UnitTest.Mock
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
        }.AsQueryable();

        public IQueryable<SkillTag> skillTagsDB = new List<SkillTag>()
        {
        }.AsQueryable();

        public IQueryable<AgentAdmin> agentAdminsDB = new List<AgentAdmin>()
        {
        }.AsQueryable();

        public IQueryable<OperationHour> operationHoursDB = new List<OperationHour>()
        {
        }.AsQueryable();

        public IQueryable<Timezone> timezonesDB = new List<Timezone>()
        {
            new Timezone { Id = 1, Name= "Dateline Standard Time", DisplayName = "(UTC-12:00) International Date Line West", Abbreviation = "DST", Offset = -12 },
            new Timezone { Id = 2, Name= "Hawaiian Standard Time", DisplayName = "(UTC-10:00) Hawaii", Abbreviation = "HST", Offset = -10 },
            new Timezone { Id = 3, Name= "Alaskan Standard Time", DisplayName = "(UTC-09:00) Alaska", Abbreviation = "AKDT", Offset = -8 }
        }.AsQueryable();

        public IQueryable<OperationHourOpenType> operationHourOpenTypesDB = new List<OperationHourOpenType>()
        {
        }.AsQueryable();

        public IQueryable<AgentGroupDetail> agentGroupDetailsDB = new List<AgentGroupDetail>()
        {
        }.AsQueryable();

        public IQueryable<AgentDetail> agentDetailsDB = new List<AgentDetail>()
        {
        }.AsQueryable();

        #region Methods

        /// <summary>
        /// Intializes the mock data.
        /// </summary>
        /// <returns></returns>
        public Mock<SetupMenuContext> IntializeMockData()
        {
            Mock<SetupMenuContext> mockSetupMenuContext = new Mock<SetupMenuContext>();
            var mockAgentAdmin = new Mock<DbSet<AgentAdmin>>();
            mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.Provider).Returns(agentAdminsDB.Provider);
            mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.Expression).Returns(agentAdminsDB.Expression);
            mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.ElementType).Returns(agentAdminsDB.ElementType);
            mockAgentAdmin.As<IQueryable<AgentAdmin>>().Setup(m => m.GetEnumerator()).Returns(agentAdminsDB.GetEnumerator());

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

            mockSetupMenuContext.Setup(c => c.AgentAdmin).Returns(mockAgentAdmin.Object);
            mockSetupMenuContext.Setup(c => c.AgentDetail).Returns(mockAgentDetail.Object);
            mockSetupMenuContext.Setup(c => c.AgentGroupDetail).Returns(mockAgentGroupDetail.Object);
            mockSetupMenuContext.Setup(c => c.AgentSchedulingGroup).Returns(mockEAgentSchedulingGroup.Object);
            mockSetupMenuContext.Setup(c => c.Client).Returns(mockClient.Object);
            mockSetupMenuContext.Setup(c => c.ClientLobGroup).Returns(mockClientLobGroup.Object);
            mockSetupMenuContext.Setup(c => c.OperationHour).Returns(mockOperationHour.Object);
            mockSetupMenuContext.Setup(c => c.OperationHourOpenType).Returns(mockOperationHourOpenType.Object);
            mockSetupMenuContext.Setup(c => c.SkillGroup).Returns(mockSkillGroup.Object);
            mockSetupMenuContext.Setup(c => c.SkillTag).Returns(mockSkillTag.Object);
            mockSetupMenuContext.Setup(c => c.Timezone).Returns(mockTimezone.Object);

            return mockSetupMenuContext;
        }

        #endregion
    }
}
