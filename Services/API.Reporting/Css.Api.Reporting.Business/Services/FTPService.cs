using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.Enums;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Services
{
	/// <summary>
	/// The service for all FTP operations
	/// </summary>
    public class FTPService : IFTPService
    {
		#region Private Properties
		
		/// <summary>
		/// The corresponding mapper object of the mapper json
		/// </summary>
		private readonly MapperSettings _mapper;

		/// <summary>
		/// The encrypter object used for encryption and decryption operations
		/// </summary>
		private readonly IEncrypter _encrypter;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor to initialize all the properties
		/// </summary>
		/// <param name="mapper"></param>
		/// <param name="encrypter"></param>
		public FTPService(IOptions<MapperSettings> mapper, IEncrypter encrypter)
		{
			_mapper = mapper.Value;
			_encrypter = encrypter;
		}
		#endregion

		#region Public Methods

		/// <summary>
		/// The method reads data using the key to map to the FTP service present in the mapper json
		/// </summary>
		/// <param name="key">The 'Key' field defined in the mapper json</param>
		/// <returns>A list of instances of DataFeed for all files present in import directory</returns>
		public async Task<List<DataFeed>> Read(string key)
		{
			List<DataFeed> feeds = new List<DataFeed>();
			
			string remoteDirectory = GetDirectory(key, ProcessType.Import) ;
			string connString = GetConnectionString(key, ProcessType.Import);
			var accessor = await _encrypter.Encrypt(connString);

			using (SftpClient sftp = CreateConnection(connString))
			{
				sftp.Connect();
				var files = sftp.ListDirectory(remoteDirectory)
						.Where(x => x.IsRegularFile)
						.OrderBy(x => x.Attributes.LastWriteTime)
						.ToList();

				foreach(var file in files)
                {
					DataFeed feed = new DataFeed();
					feed.Path = file.FullName;
					feed.Content = sftp.ReadAllBytes(file.FullName);
					feed.Accessor = accessor;
					feeds.Add(feed);
				}
				sftp.Disconnect();
				if(feeds.Count == 0)
                {
					throw new FileNotFoundException(Messages.EmptyFTPDirectory);
                }
			}
			return feeds;
		}

		/// <summary>
		/// Moves the processed file to the corresponding completed folder within the FTP
		/// </summary>
		/// <param name="feed">The instance of DataFeed which completed processing</param>
		public async Task MoveToProcessedFolder(DataFeed feed)
		{
			string connString = await _encrypter.Decrypt(feed.Accessor);
			string path = GetDestinationPath(feed.Path);
			string destPath = String.Format(path, "Processed");
			MoveDirectory(connString, feed.Path, destPath);
			
			if(!string.IsNullOrWhiteSpace(feed.Metadata))
            {
				string pendingFilePath = GetPartialFileDestinationPath(String.Format(path, "Pending"));
				await CreateFile(connString, pendingFilePath, feed.Metadata);
			}
		}

		/// <summary>
		/// Moves the unprocessed file to the corresponding failed folder within the FTP
		/// </summary>
		/// <param name="feed">The instance of DataFeed which failed to process</param>
		public async Task MoveToFailedFolder(DataFeed feed)
		{
			string connString = await _encrypter.Decrypt(feed.Accessor);
			string destPath = string.Format(GetDestinationPath(feed.Path), "Failed");
			MoveDirectory(connString, feed.Path, destPath);
			string errorFilePath = GetErrorFileDestinationPath(destPath);
			await CreateFile(connString, errorFilePath, feed.Metadata);
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// This method creates an object of SftpClient
		/// </summary>
		/// <param name="connectionString">The SFTP connection string</param>
		/// <returns>An instance of SftpClient</returns>
		private SftpClient CreateConnection(string connectionString)
        {
			Dictionary<string, string> conn = GetConnectionProperties(connectionString);
			string host = conn["host"];
			string username = conn["username"];
			string password = conn["password"];

			return new SftpClient(host, username, password);
		}

		/// <summary>
		///  Reads the corresponding SFTP server details from the mapper json based on the key and the process type
		/// </summary>
		/// <param name="key">The 'Key' field defined in the mapper json</param>
		/// <param name="process">The ProcessType - either of Import/Export</param>
		/// <returns>A string</returns>
		private string GetConnectionString(string key, ProcessType process)
		{
			string connStr = string.Empty;
			string globalConnStr = _mapper.GlobalSettings.FTPServer;
			MapperIndividualSettings settings;

			if (process == ProcessType.Import)
			{
				settings = _mapper.Imports.FirstOrDefault(x => x.Key.Equals(key));
			}
			else if (process == ProcessType.Export)
			{
				settings = _mapper.Exports.FirstOrDefault(x => x.Key.Equals(key));
			}
            else
            {
				throw new MappingException(string.Format(Messages.FTPMappingNotFound, key, process));
			}

			if (settings == null)
            {
				throw new MappingException(string.Format(Messages.FTPMappingNotFound, key, process));
			}

			connStr = settings.FTPServer;

			if (string.IsNullOrWhiteSpace(settings.FTPServer))
			{
				connStr = globalConnStr;
			}

			if (string.IsNullOrWhiteSpace(connStr))
			{
				throw new InvalidSourceException(string.Format(Messages.InvalidFTPHost, key));
			}

			return connStr;
		}

		/// <summary>
		///  Generates the SFTP connection dictionary using the input connection string
		/// </summary>
		/// <param name="connectionString">The SFTP connection string</param>
		/// <returns>An instance of Dictionary containing keys - 'host','username','password' and their corresponding string values</returns>
		private Dictionary<string,string> GetConnectionProperties(string connectionString)
        {
			Dictionary<string, string> connectionProperties = new Dictionary<string, string>()
			{
				{ "host" , "" },
				{ "username" , "" },
				{ "password" , "" },
			};

			//var validFTPWithIPRegex = @"^(sftp|ftp):\/\/(\w*):([@#$_a-zA-Z0-9]*)@((([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])+){1})$";
			var validFTPHostRegex = @"^(sftp|ftp):\/\/(?'user'\w*):(?'pwd'[@#$_a-zA-Z0-9]*)@(?'host'(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9]))$";

			Regex rg = new Regex(validFTPHostRegex);
			Match match = rg.Match(connectionString);
			var matchGroups = match.Groups.Values.ToList();
			connectionProperties["host"] = matchGroups.Where(x => x.Name == "host").Select(x => x.Value).FirstOrDefault();
			connectionProperties["username"] = matchGroups.Where(x => x.Name == "user").Select(x => x.Value).FirstOrDefault();
			connectionProperties["password"] = matchGroups.Where(x => x.Name == "pwd").Select(x => x.Value).FirstOrDefault();
			return connectionProperties;
        }

		/// <summary>
		/// Finds the FTP directory using the key, process type and the mapper json
		/// </summary>
		/// <param name="key"></param>
		/// <param name="process"></param>
		/// <returns>A string with the complete path of the directory</returns>
		private string GetDirectory(string key, ProcessType process)
        {
			string dir = string.Empty;
			string globalDir = string.Empty;
			
			MapperIndividualSettings settings;
			if(process == ProcessType.Import)
            {
				settings = _mapper.Imports.FirstOrDefault(x => x.Key.Equals(key));
				globalDir = string.Format(_mapper.GlobalSettings.FTPInbox,key);
            }
			else if(process == ProcessType.Export)
            {
				settings = _mapper.Exports.FirstOrDefault(x => x.Key.Equals(key));
				globalDir = string.Format(_mapper.GlobalSettings.FTPOutbox,key);
			}
            else
            {
				throw new MappingException(Messages.InvalidOperation);
            }

			if (settings == null)
            {
				throw new MappingException(string.Format(Messages.MappingNotFound, key));
            }

			dir = settings.FTPFolder;

			if(string.IsNullOrWhiteSpace(settings.FTPFolder))
            {
				dir = globalDir;
            }

			if (string.IsNullOrWhiteSpace(dir))
            {
				throw new InvalidSourceException(Messages.EmptyFTPDirectory);
			}
			return dir;
		}

		/// <summary>
		/// Retreive the generic destination path using the source path 
		/// </summary>
		/// <param name="sourcePath"></param>
		/// <returns>Path in the format '{{sourceDirectory}}/{0}/[CurrentUTCTimestamp]_{{sourceFileName}}'</returns>
		private string GetDestinationPath(string sourcePath)
        {
			int index = sourcePath.LastIndexOf("/");
			string fileName = sourcePath.Substring(index + 1);
			return sourcePath.Substring(0, index) + @"/{0}/" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + fileName;
		}

		/// <summary>
		/// Retreive the error file path using the failed folder path
		/// </summary>
		/// <param name="sourcePath"></param>
		/// <returns>{{sourcePathWithoutext}}_error.txt'</returns>
		private string GetErrorFileDestinationPath(string sourcePath)
        {
			int index = sourcePath.LastIndexOf(".xml");
			string file = sourcePath.Substring(0, index);
			return file + "_error.txt";
        }

		/// <summary>
		///  Retreive the error file path using the processed folder path
		/// </summary>
		/// <param name="sourcePath"></param>
		/// <returns>{{sourcePathWithoutext}}_pending.txt'</returns>
		private string GetPartialFileDestinationPath(string sourcePath)
		{
			int index = sourcePath.LastIndexOf(".xml");
			string file = sourcePath.Substring(0, index);
			return file + "_partial.txt";
		}

		/// <summary>
		/// This method moves the file from source to destination within the SFTP
		/// </summary>
		/// <param name="connectionString">The SFTP connection string</param>
		/// <param name="oldPath">The complete old path of the file including the file name</param>
		/// <param name="newPath">The complete new path of the file including the file name</param>
		private void MoveDirectory(string connectionString, string oldPath, string newPath)
        {
			using (SftpClient sftp = CreateConnection(connectionString))
			{
				sftp.Connect();
				sftp.RenameFile(oldPath, newPath);
				sftp.Disconnect();
			}
		}

		/// <summary>
		/// This method generates the error file 
		/// </summary>
		/// <param name="connectionString">The SFTP connection string</param>
		/// <param name="destPath">The complete path of the file to be created</param>
		/// <param name="content">The file contents</param>
		/// <returns></returns>
		private async Task CreateFile(string connectionString, string destPath, string content)
        {
			using (SftpClient sftp = CreateConnection(connectionString))
			{
				sftp.Connect();
				using( var streamWriter = sftp.CreateText(destPath))
                {
					await streamWriter.WriteLineAsync(content);
					streamWriter.Close();
                }
				sftp.Disconnect();
			}
		}
        #endregion
    }
}
