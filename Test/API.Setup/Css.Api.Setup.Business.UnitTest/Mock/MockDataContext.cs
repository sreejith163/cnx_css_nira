using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Repository.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Css.Api.Setup.Business.UnitTest.Mock
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

        public IQueryable<ClientLobGroup> clientLobGroupsDB = new List<ClientLobGroup>()
        {
            new ClientLobGroup{ Id = 1, ClientId = 1, FirstDayOfWeek = 1, Name = "A", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 2, ClientId = 2, FirstDayOfWeek = 1, Name = "B", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 3, ClientId = 3, FirstDayOfWeek = 1, Name = "C", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 4, ClientId = 1, FirstDayOfWeek = 1, Name = "D", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new ClientLobGroup{ Id = 5, ClientId = 2, FirstDayOfWeek = 1, Name = "E", TimezoneId = 1, RefId = 1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public IQueryable<SkillGroup> skillGroupsDB = new List<SkillGroup>()
        {
             new SkillGroup { Id = 1, RefId = 1, Name = "skillGroup1", ClientId=1, ClientLobGroupId=1,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new SkillGroup { Id = 2, RefId = 1, Name = "skillGroup2",  ClientId=1, ClientLobGroupId=2,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new SkillGroup { Id = 3, RefId = 1, Name = "skillGroup3",  ClientId=1, ClientLobGroupId=1,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public IQueryable<SkillTag> skillTagsDB = new List<SkillTag>()
        {
            new SkillTag { Id = 1, RefId = 1, Name = "skillTag1", ClientId=1, ClientLobGroupId=1, SkillGroupId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new SkillTag { Id = 2, RefId = 1, Name = "skillTag2",  ClientId=1, ClientLobGroupId=2,SkillGroupId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new SkillTag { Id = 3, RefId = 1, Name = "skillTag3",  ClientId=1, ClientLobGroupId=1,SkillGroupId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();

        public IQueryable<AgentSchedulingGroup> agentSchedulingGroupsDB = new List<AgentSchedulingGroup>()
        {
             new AgentSchedulingGroup { Id = 1, RefId = 1, Name = "agentSchedulingGroup1", ClientId=1, ClientLobGroupId=1, SkillGroupId=1, SkillTagId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new AgentSchedulingGroup { Id = 2, RefId = 1, Name = "agentSchedulingGroup2",  ClientId=1, ClientLobGroupId=2,SkillGroupId=1, SkillTagId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new AgentSchedulingGroup { Id = 3, RefId = 1, Name = "agentSchedulingGroup3",  ClientId=1, ClientLobGroupId=1,SkillGroupId=1, SkillTagId=1,
                   CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        }.AsQueryable();
                
        public IQueryable<OperationHour> operationHoursDB = new List<OperationHour>()
        {
            new OperationHour { Id = 1, SkillGroupId=1, Day=0, OperationHourOpenTypeId=1 },
            new OperationHour { Id = 1, SkillGroupId=1, Day=1, OperationHourOpenTypeId=1 },
            new OperationHour { Id = 1, SkillGroupId=1, Day=2, OperationHourOpenTypeId=1 },
            new OperationHour { Id = 1, SkillGroupId=1, Day=3, OperationHourOpenTypeId=1 },
            new OperationHour { Id = 1, SkillGroupId=1, Day=4, OperationHourOpenTypeId=1 },
            new OperationHour { Id = 1, SkillGroupId=1, Day=5, OperationHourOpenTypeId=1 },
            new OperationHour { Id = 1, SkillGroupId=1, Day=6, OperationHourOpenTypeId=1 }
        }.AsQueryable();

        public IQueryable<Timezone> timezonesDB = new List<Timezone>()
        {
            new Timezone { Id = 1, Name= "Dateline Standard Time", DisplayName = "(UTC-12:00) International Date Line West", Abbreviation = "DST", UtcOffset = new TimeSpan(0,-12,0,0,0) },
            new Timezone { Id = 2, Name= "Hawaiian Standard Time", DisplayName = "(UTC-10:00) Hawaii", Abbreviation = "HST", UtcOffset = new TimeSpan(0,-10,0,0,0) },
            new Timezone { Id = 3, Name= "Alaskan Standard Time", DisplayName = "(UTC-09:00) Alaska", Abbreviation = "AKDT", UtcOffset = new TimeSpan(0,-9,0,0,0) }
        }.AsQueryable();

        public IQueryable<OperationHourOpenType> operationHourOpenTypesDB = new List<OperationHourOpenType>()
        {
        }.AsQueryable();

        #region Methods

        /// <summary>
        /// Intializes the mock data.
        /// </summary>
        /// <returns></returns>
        public Mock<SetupContext> IntializeMockData()
        {
            Mock<SetupContext> mockSchedulingContext = new Mock<SetupContext>();

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

            mockSchedulingContext.Setup(c => c.AgentSchedulingGroup).Returns(mockEAgentSchedulingGroup.Object);
            mockSchedulingContext.Setup(c => c.Client).Returns(mockClient.Object);
            mockSchedulingContext.Setup(c => c.ClientLobGroup).Returns(mockClientLobGroup.Object);
            mockSchedulingContext.Setup(c => c.OperationHour).Returns(mockOperationHour.Object);
            mockSchedulingContext.Setup(c => c.OperationHourOpenType).Returns(mockOperationHourOpenType.Object);
            mockSchedulingContext.Setup(c => c.SkillGroup).Returns(mockSkillGroup.Object);
            mockSchedulingContext.Setup(c => c.SkillTag).Returns(mockSkillTag.Object);
            mockSchedulingContext.Setup(c => c.Timezone).Returns(mockTimezone.Object);

            return mockSchedulingContext;
        }

        #endregion
    }
}
