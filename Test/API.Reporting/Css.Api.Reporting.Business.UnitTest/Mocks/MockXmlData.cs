using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Business.UnitTest.Mocks
{
    /// <summary>
    /// A helper class to generate mock data for all xml services
    /// </summary>
    public class MockXmlData
    {
        #region Private Properties
        /// <summary>
        /// The list of agents with valid information
        /// </summary>
        private UDWAgentList SuccessAgentList;

        /// <summary>
        /// The list of agents containing some invalid data
        /// </summary>
        private UDWAgentList PartialAgentList;

        /// <summary>
        /// A valid xml string 
        /// </summary>
        private string UDWSuccessXml;

        /// <summary>
        /// A valid xml string with some unexpected data
        /// </summary>
        private string UDWPartialXml;

        /// <summary>
        /// An invalid xml string
        /// </summary>
        private string UDWFailedXml;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize the xml data
        /// </summary>
        public void InitializeData()
        {
            InitializeAgentsData();
            UDWSuccessXml = @"<?xml version='1.0' encoding='ISO-8859-1' ?><agentList><newAgent><sso>F1.L1@concentrix.com</sso><UUID>75869015f3e34bff85ee65a77bc992d5</UUID><firstName>F1</firstName><lastName>L1</lastName><mu>341561</mu><startID>154794</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000001</ssn></newAgent><newAgent><sso>F2.L2@concentrix.com</sso><UUID>f9d2c357d7624b86ae50c9701af2327d</UUID><firstName>F2</firstName><lastName>L2</lastName><mu>338384</mu><startID>171934</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000002</ssn></newAgent><newAgent><sso>F3.L3@concentrix.com</sso><UUID>ef221673cda94f59b26f313e99c3e5ca</UUID><firstName>F3</firstName><lastName>L3</lastName><mu>302699</mu><startID>211697</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000003</ssn></newAgent><newAgent><sso>F4.L4@concentrix.com</sso><UUID>0ad4e0372d4645d3ad451978fd24c1ca</UUID><firstName>F4</firstName><lastName>L4</lastName><mu>380195</mu><startID>170854</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000004</ssn></newAgent><newAgent><sso>F5.L5@concentrix.com</sso><UUID>095ddb59eaa64d57b76b2e1dddc7aed0</UUID><firstName>F5</firstName><lastName>L5</lastName><mu>397594</mu><startID>138897</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000005</ssn></newAgent><newAgent><sso>F6.L6@concentrix.com</sso><UUID>e3610603f46e4e1faab44fbffe1b63df</UUID><firstName>F6</firstName><lastName>L6</lastName><mu>353639</mu><startID>129470</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000006</ssn></newAgent><newAgent><sso>F7.L7@concentrix.com</sso><UUID>84d1cbcb1f4f44538d453b81edd9170f</UUID><firstName>F7</firstName><lastName>L7</lastName><mu>346098</mu><startID>134102</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000007</ssn></newAgent><newAgent><sso>F8.L8@concentrix.com</sso><UUID>7c811f79351d44f68f090886ee3dbcde</UUID><firstName>F8</firstName><lastName>L8</lastName><mu>325642</mu><startID>192836</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000008</ssn></newAgent><newAgent><sso>F9.L9@concentrix.com</sso><UUID>ead1aaca0858489c9a05281743b5a6b1</UUID><firstName>F9</firstName><lastName>L9</lastName><mu>372104</mu><startID>168096</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000009</ssn></newAgent><newAgent><sso>F10.L10@concentrix.com</sso><UUID>85bd9b4e66a94737afa35f2e198984a7</UUID><firstName>F10</firstName><lastName>L10</lastName><mu>307240</mu><startID>185433</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000010</ssn></newAgent><changedAgent><sso>F11.L11@concentrix.com</sso><UUID>b86cdfce31d640f99a67bb9df93251e6</UUID><firstName>F11</firstName><lastName>L11</lastName><mu>592417</mu><startID>917247</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000011</id></changedAgent><changedAgent><sso>F12.L12@concentrix.com</sso><UUID>25b01c438375416fbdd0c2880d67415c</UUID><firstName>F12</firstName><lastName>L12</lastName><mu>514566</mu><startID>920833</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000012</id></changedAgent><changedAgent><sso>F13.L13@concentrix.com</sso><UUID>81434aab147a4cb5b0d1a3642a1b44b8</UUID><firstName>F13</firstName><lastName>L13</lastName><mu>554775</mu><startID>918184</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000013</id></changedAgent><changedAgent><sso>F14.L14@concentrix.com</sso><UUID>32df74ceb6be4732b600dbd6eaf700d8</UUID><firstName>F14</firstName><lastName>L14</lastName><mu>516529</mu><startID>919346</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000014</id></changedAgent><changedAgent><sso>F15.L15@concentrix.com</sso><UUID>04520cc04fde4f838ab9006e47327653</UUID><firstName>F15</firstName><lastName>L15</lastName><mu>520370</mu><startID>917436</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000015</id></changedAgent></agentList>";
            UDWPartialXml = @"<?xml version='1.0' encoding='ISO-8859-1' ?><agentList><newAgent><sso>F1.L1@concentrix.com</sso><UUID>75869015f3e34bff85ee65a77bc992d5</UUID><firstName>F1</firstName><lastName>L1</lastName><mu>341561</mu><startID>154794</startID><agentStartDate><day>4</day><month>13</month><year>2021</year></agentStartDate><senDate><day>4</day><month>13</month><year>2021</year></senDate><ssn>1000001</ssn></newAgent><newAgent><sso>F2.L2@concentrix.com</sso><UUID>f9d2c357d7624b86ae50c9701af2327d</UUID><firstName>F2</firstName><lastName>L2</lastName><mu>338384</mu><startID>171934</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000002</ssn></newAgent><newAgent><sso>F3.L3@concentrix.com</sso><UUID>ef221673cda94f59b26f313e99c3e5ca</UUID><firstName>F3</firstName><lastName>L3</lastName><mu>302699</mu><startID>211697</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000003</ssn></newAgent><newAgent><sso>F4.L4@concentrix.com</sso><UUID>0ad4e0372d4645d3ad451978fd24c1ca</UUID><firstName>F4</firstName><lastName>L4</lastName><mu>380195</mu><startID>170854</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000004</ssn></newAgent><newAgent><sso>F5.L5@concentrix.com</sso><UUID>095ddb59eaa64d57b76b2e1dddc7aed0</UUID><firstName>F5</firstName><lastName>L5</lastName><mu>397594</mu><startID>138897</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000005</ssn></newAgent><newAgent><sso>F6.L6@concentrix.com</sso><UUID>e3610603f46e4e1faab44fbffe1b63df</UUID><firstName>F6</firstName><lastName>L6</lastName><mu>353639</mu><startID>129470</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000006</ssn></newAgent><newAgent><sso>F7.L7@concentrix.com</sso><UUID>84d1cbcb1f4f44538d453b81edd9170f</UUID><firstName>F7</firstName><lastName>L7</lastName><mu>346098</mu><startID>134102</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000007</ssn></newAgent><newAgent><sso>F8.L8@concentrix.com</sso><UUID>7c811f79351d44f68f090886ee3dbcde</UUID><firstName>F8</firstName><lastName>L8</lastName><mu>325642</mu><startID>192836</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000008</ssn></newAgent><newAgent><sso>F9.L9@concentrix.com</sso><UUID>ead1aaca0858489c9a05281743b5a6b1</UUID><firstName>F9</firstName><lastName>L9</lastName><mu>372104</mu><startID>168096</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000009</ssn></newAgent><newAgent><sso>F10.L10@concentrix.com</sso><UUID>85bd9b4e66a94737afa35f2e198984a7</UUID><firstName>F10</firstName><lastName>L10</lastName><mu>307240</mu><startID>185433</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><ssn>1000010</ssn></newAgent><changedAgent><sso>F11.L11@concentrix.com</sso><UUID>b86cdfce31d640f99a67bb9df93251e6</UUID><firstName>F11</firstName><lastName>L11</lastName><mu>592417</mu><startID>917247</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000011</id></changedAgent><changedAgent><sso>F12.L12@concentrix.com</sso><UUID>25b01c438375416fbdd0c2880d67415c</UUID><firstName>F12</firstName><lastName>L12</lastName><mu>514566</mu><startID>920833</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000012</id></changedAgent><changedAgent><sso>F13.L13@concentrix.com</sso><UUID>81434aab147a4cb5b0d1a3642a1b44b8</UUID><firstName>F13</firstName><lastName>L13</lastName><mu>554775</mu><startID>918184</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000013</id></changedAgent><changedAgent><sso>F14.L14@concentrix.com</sso><UUID>32df74ceb6be4732b600dbd6eaf700d8</UUID><firstName>F14</firstName><lastName>L14</lastName><mu>516529</mu><startID>919346</startID><agentStartDate><day>4</day><month>1</month><year>2021</year></agentStartDate><senDate><day>4</day><month>1</month><year>2021</year></senDate><id>1000014</id></changedAgent><changedAgent><sso>F15.L15@concentrix.com</sso><UUID>04520cc04fde4f838ab9006e47327653</UUID><firstName>F15</firstName><lastName>L15</lastName><mu>520370</mu><startID>917436</startID><agentStartDate><day>4</day><month>13</month><year>2021</year></agentStartDate><senDate><day>4</day><month>13</month><year>2021</year></senDate><id>1000015</id></changedAgent></agentList>";
            UDWFailedXml = @"<?xml version='1.0' encoding='ISO-8859-1' ?><agentList><newAgent>";
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// A generic method to return data based on status and the class of T
        /// </summary>
        /// <typeparam name="T">A class</typeparam>
        /// <param name="status">A value of the enum ProcessStatus</param>
        /// <returns>Instance of T</returns>
        public T GetData<T>(int status = (int)ProcessStatus.Success)
        {
            if (typeof(T).Equals(typeof(UDWAgentList)))
            {
                UDWAgentList data = SuccessAgentList;
                if(status == (int)ProcessStatus.Partial)
                {
                    data = PartialAgentList;
                }
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// A generic method to return an xml data string based on status and the class of T
        /// </summary>
        /// <typeparam name="T">A class</typeparam>
        /// <param name="status">A value of the enum ProcessStatus</param>
        /// <returns>An xml string</returns>
        public string GetString<T>(int status = (int)ProcessStatus.Success)
            where T : class
        {
            string data = string.Empty;
            if(typeof(T).Equals(typeof(UDWAgentList)))
            {
                switch(status)
                {
                    case (int)ProcessStatus.Partial:
                        data = UDWPartialXml;
                        break;
                    case (int)ProcessStatus.Failed:
                        data = UDWFailedXml;
                        break;
                    default:
                        data = UDWSuccessXml;
                        break;
                }
            }

            return data;
        }

        /// <summary>
        /// A generic method to return bytes data based on status and the class of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="status">A value of the enum ProcessStatus</param>
        /// <returns>A byte array</returns>
        public byte[] GetBytes<T>(int status = (int)ProcessStatus.Success)
            where T : class
        {
            if (typeof(T).Equals(typeof(UDWAgentList)))
            {
                string data;
                switch (status)
                {
                    case (int)ProcessStatus.Partial:
                        data = UDWPartialXml;
                        break;
                    case (int)ProcessStatus.Failed:
                        data = UDWFailedXml;
                        break;
                    default:
                        data = UDWSuccessXml;
                        break;
                }
                return Encoding.Default.GetBytes(data);
            }

            throw new InvalidOperationException();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// A method to initialize all agent mock data
        /// </summary>
        private void InitializeAgentsData()
        {
            SuccessAgentList = new UDWAgentList()
            {
                NewAgents = new List<UDWAgent>(),
                ChangedAgents = new List<UDWAgentUpdate>()
            };
            PartialAgentList = new UDWAgentList()
            {
                NewAgents = new List<UDWAgent>(),
                ChangedAgents = new List<UDWAgentUpdate>()
            };
            
            for (int i = 1; i<=10; i++)
            {
                UDWAgent sAgent = new UDWAgent();
                UDWAgent pAgent = new UDWAgent();

                sAgent.AgentStartDate = new UDWAgentDate()
                {
                    Day = DateTime.UtcNow.Day,
                    Month = DateTime.UtcNow.Month,
                    Year = DateTime.UtcNow.Year
                };
                pAgent.AgentStartDate = new UDWAgentDate() 
                { 
                    Day = DateTime.UtcNow.Day,
                    Month = i == 1 ? DateTime.UtcNow.Month : 13, 
                    Year = DateTime.UtcNow.Year 
                };

                sAgent.FirstName = string.Concat("FS", i);
                sAgent.LastName = string.Concat("LS", i);
                sAgent.MU = new Random().Next(300000, 400000).ToString();
                pAgent.FirstName = string.Concat("FP", i);
                pAgent.LastName = string.Concat("LP", i);
                pAgent.MU = new Random().Next(400000, 500000).ToString();

                sAgent.SenDate = new UDWAgentDate() 
                {
                    Day = DateTime.UtcNow.Day,
                    Month = DateTime.UtcNow.Month,
                    Year = DateTime.UtcNow.Year
                };
                pAgent.SenDate = new UDWAgentDate()
                {
                    Day = DateTime.UtcNow.Day,
                    Month = i == 1 ? DateTime.UtcNow.Month : 13,
                    Year = DateTime.UtcNow.Year
                };

                sAgent.SSN = 1000000 + i;
                sAgent.SSO = string.Concat(sAgent.FirstName, ".", sAgent.LastName, "@concentrix.com");
                sAgent.StartId = new Random().Next(111000, 222000);
                sAgent.UUID = Guid.NewGuid().ToString().Replace("-", "");
                pAgent.SSN = 2000000 + i;
                pAgent.SSO = string.Concat(pAgent.FirstName, ".", pAgent.LastName, "@concentrix.com");
                pAgent.StartId = new Random().Next(311000, 422000);
                pAgent.UUID = Guid.NewGuid().ToString().Replace("-", "");

                SuccessAgentList.NewAgents.Add(sAgent);
                PartialAgentList.NewAgents.Add(pAgent);
            }
            

            for (int i = 11; i <= 15; i++)
            {
                UDWAgentUpdate sAgent = new UDWAgentUpdate();
                UDWAgentUpdate pAgent = new UDWAgentUpdate();

                sAgent.AgentStartDate = new UDWAgentDate() 
                {
                    Day = DateTime.UtcNow.Day,
                    Month = DateTime.UtcNow.Month,
                    Year = DateTime.UtcNow.Year
                };
                pAgent.AgentStartDate = new UDWAgentDate()
                {
                    Day = DateTime.UtcNow.Day,
                    Month = i == 15 ? DateTime.UtcNow.Month : 13,
                    Year = DateTime.UtcNow.Year
                };

                sAgent.FirstName = string.Concat("UFS", i);
                sAgent.LastName = string.Concat("ULS", i);
                sAgent.MU = new Random().Next(500000, 600000).ToString();
                pAgent.FirstName = string.Concat("UFP", i);
                pAgent.LastName = string.Concat("ULP", i);
                pAgent.MU = new Random().Next(600000, 700000).ToString();

                sAgent.SenDate = new UDWAgentDate() 
                {
                    Day = DateTime.UtcNow.Day,
                    Month = DateTime.UtcNow.Month,
                    Year = DateTime.UtcNow.Year
                };
                pAgent.SenDate = new UDWAgentDate()
                {
                    Day = DateTime.UtcNow.Day,
                    Month = i == 15 ? DateTime.UtcNow.Month : 13,
                    Year = DateTime.UtcNow.Year
                };

                sAgent.SSN = 1000000 + i;
                sAgent.SSO = string.Concat(sAgent.FirstName, ".", sAgent.LastName, "@concentrix.com");
                sAgent.StartId = new Random().Next(811000, 822000);
                sAgent.UUID = Guid.NewGuid().ToString().Replace("-", "");
                pAgent.SSN = 2000000 + i;
                pAgent.SSO = string.Concat(pAgent.FirstName, ".", pAgent.LastName, "@concentrix.com");
                pAgent.StartId = new Random().Next(911000, 922000);
                pAgent.UUID = Guid.NewGuid().ToString().Replace("-", "");

                SuccessAgentList.ChangedAgents.Add(sAgent);
                PartialAgentList.ChangedAgents.Add(pAgent);
            }
            #endregion
        }
    }
}
