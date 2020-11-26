using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        /// <summary>
        /// The people repository
        /// </summary>
        private readonly IMongoRepository<Person> _peopleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleController"/> class.
        /// </summary>
        /// <param name="peopleRepository">The people repository.</param>
        public SampleController(IMongoRepository<Person> peopleRepository)
        {
            _peopleRepository = peopleRepository;
        }

        /// <summary>
        /// Adds the person.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        [HttpPost("registerPerson")]
        public async Task AddPerson(string firstName, string lastName)
        {
            var person = new Person()
            {
                FirstName = firstName,
                LastName = lastName
            };

            await _peopleRepository.InsertOneAsync(person);
        }

        /// <summary>
        /// Updates the person.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="id">The identifier.</param>
        [HttpPut("registerPerson/{id}")]
        public async Task UpdatePerson(string firstName, string lastName, string id)
        {
            var per = await _peopleRepository.FindByIdAsync(id);
            per.FirstName = firstName;
            per.LastName = lastName;

            await _peopleRepository.ReplaceOneAsync(per);
        }

        /// <summary>
        /// Gets the people data.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getPeopleData")]
        public IEnumerable<string> GetPeopleData()
        {
            var people = _peopleRepository.FilterBy(
                filter => filter.FirstName != "test",
                projection => projection.FirstName
            );
            return people;
        }

        /// <summary>
        /// Gets the people data.
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public IEnumerable<Person> GetPeopleDatas()
        {
            var people = _peopleRepository.AsQueryable();
            return people;
        }

        /// <summary>
        /// Gets the people data.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("getPeopleData/{id}")]
        public async Task<IEnumerable<Person>> DeleteAsync(string id)
        {
            var per = await _peopleRepository.FindByIdAsync(id);
            await _peopleRepository.DeleteByIdAsync(id);
            return new List<Person>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Css.Api.Core.Models.Domain.BaseDocument" />
    [BsonCollection("people")]
    public class Person : BaseDocument
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime BirthDate { get; set; }
    }
}
