using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.UnitTest.Services
{
    /// <summary>
    /// The unit testing class for UDWAgentList XMLParser
    /// </summary>
    public class UDWAgentParserShould : XMLParserTests<UDWAgentList>
    {
        #region Private Properties
        
        /// <summary>
        /// The mock xml data
        /// </summary>
        private readonly MockXmlData _data;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UDWAgentParserShould()
        {
            _data = new MockXmlData();
            _data.InitializeData();
        }
        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Method to input the sample for serialization
        /// </summary>
        /// <returns></returns>
        protected override string CreateDeserializationSample() => _data.GetString<UDWAgentList>();

        /// <summary>
        /// Method to input the sample for deserialization
        /// </summary>
        /// <returns></returns>
        protected override UDWAgentList CreateSerializationSample() => _data.GetData<UDWAgentList>();
        #endregion
    }
}
