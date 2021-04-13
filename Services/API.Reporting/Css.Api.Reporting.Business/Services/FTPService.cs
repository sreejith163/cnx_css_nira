using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request;
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
		/// The FTP options
		/// </summary>
		private FTPOptions _options;
		#endregion

		#region Public Methods

		/// <summary>
		/// The method to set the FTP options
		/// </summary>
		/// <param name="options"></param>
		public void Set(Dictionary<string, string> options)
        {
			_options = new FTPOptions();
			SetConnectionProperties(options["FTPServer"]);
			_options.Inbox = options["FTPInbox"];
			_options.Outbox = options["FTPOutbox"];
		}

		/// <summary>
		/// A helper method to determine the destination folder
		/// </summary>
		/// <param name="startDateTime"></param>
		/// <param name="endDateTime"></param>
		/// <param name="recentlyUpdated"></param>
		/// <returns></returns>
		public string GetDestinationFolder(DateTime startDateTime, DateTime endDateTime, bool? recentlyUpdated = null)
        {
			if ((endDateTime - startDateTime).TotalDays == 2 && recentlyUpdated.HasValue && recentlyUpdated.Value)
				return "CSSIntraDayFiles/";
			else if(startDateTime == endDateTime || (endDateTime - startDateTime).TotalDays == 2)
				return "CSS1DayFiles/";
			else if ((endDateTime - startDateTime).TotalDays == 13)
				return "CSS14DayFiles/";
			else 
				return string.Empty;
		}

		/// <summary>
		/// The method to generate the filename for export employees
		/// </summary>
		/// <param name="agentSchedulingGroup"></param>
		/// <returns></returns>
		public string GetEmployeesFileName(string agentSchedulingGroup)
        {
			return string.Join("/", "CSSEmpIDFiles", string.Join("_", "CSS", agentSchedulingGroup) + ".txt");
        }

		/// <summary>
		/// The method reads data using the key to map to the FTP service present in the mapper json
		/// </summary>
		/// <param name="key">The 'Key' field defined in the mapper json</param>
		/// <returns>A list of instances of DataFeed for all files present in import directory</returns>
		public List<DataFeed> Read()
		{
			List<DataFeed> feeds = new List<DataFeed>();

			string inbox = _options.Inbox;

			using (SftpClient sftp = CreateClient())
			{
				sftp.Connect();
				var files = sftp.ListDirectory(inbox)
						.Where(x => x.IsRegularFile)
						.OrderBy(x => x.Attributes.LastWriteTime)
						.ToList();

				foreach(var file in files)
                {
					DataFeed feed = new DataFeed();
					feed.Feeder = file.FullName;
					feed.Content = sftp.ReadAllBytes(file.FullName);
					feeds.Add(feed);
				}
				sftp.Disconnect();
				sftp.Dispose();
				if(feeds.Count == 0)
                {
					throw new FileNotFoundException(Messages.EmptyFTPDirectory);
                }
			}
			return feeds;
		}

		/// <summary>
		/// The method to write data to FTP Outbox for the mapped context
		/// </summary>
		/// <param name="filename">The name of the destination file</param>
		/// <param name="contents">The content of the file</param>
		/// <returns>True if successful</returns>
		public bool Write(string filename, string contents)
		{
			string outbox = _options.Outbox;
			using (SftpClient sftp = CreateClient())
			{
				sftp.Connect();
				try
				{
					sftp.DeleteFile(string.Join('/', outbox, filename));
				}
				catch
				{

				}
				sftp.WriteAllText(string.Join('/', outbox, filename), contents);
				sftp.Disconnect();
				sftp.Dispose();
			}
			return true;
		}

		/// <summary>
		/// Moves the processed file to the corresponding completed folder within the FTP
		/// </summary>
		/// <param name="feed">The instance of DataFeed which completed processing</param>
		public async Task MoveToProcessedFolder(DataFeed feed)
		{
			string path = await Task.FromResult(GetDestinationPath(feed.Feeder));
			string destPath = String.Format(path, "Processed");
			MoveDirectory(feed.Feeder, destPath);
		}

		/// <summary>
		/// Moves the unprocessed data to the corresponding folder within the FTP
		/// </summary>
		/// <param name="feed">The instance of DataFeed which completed processing</param>
		public async Task MoveToUnprocessedFolder(DataFeed feed)
        {
			string path = GetDestinationPath(feed.Feeder);
			string filePath = String.Format(path, "Processed");
			string destPath = GetPartialFileDestinationPath(String.Format(path, "Pending"));
			MoveDirectory(feed.Feeder, filePath);
			await CreateFile(destPath, feed.Metadata);
			feed.Metadata = string.Format("Unprocessed data - {0}", destPath);
		}

		/// <summary>
		/// Moves the unprocessed file to the corresponding failed folder within the FTP
		/// </summary>
		/// <param name="feed">The instance of DataFeed which failed to process</param>
		public async Task MoveToFailedFolder(DataFeed feed)
		{
			string destPath = string.Format(GetDestinationPath(feed.Feeder), "Failed");
			MoveDirectory(feed.Feeder, destPath);
			string errorFilePath = GetErrorFileDestinationPath(destPath);
			await CreateFile(errorFilePath, feed.Metadata);
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// This method creates an object of SftpClient
		/// </summary>
		/// <param name="connectionString">The SFTP connection string</param>
		/// <returns>An instance of SftpClient</returns>
		private SftpClient CreateClient()
        {
			return new SftpClient(_options.Server, _options.Username, _options.Password);
		}

		/// <summary>
		/// Sets the connection properties using the input connection string
		/// </summary>
		/// <param name="connectionString">The SFTP connection string</param>
		private void SetConnectionProperties(string connectionString)
        {
			//var validFTPWithIPRegex = @"^(sftp|ftp):\/\/(\w*):([@#$_a-zA-Z0-9]*)@((([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])+){1})$";
			var validFTPHostRegex = @"^(sftp|ftp):\/\/(?'user'\w*):(?'pwd'[@#^%$\(\)\[\],:;_a-zA-Z0-9]*)@(?'host'(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9]))$";

			Regex rg = new Regex(validFTPHostRegex);
			Match match = rg.Match(connectionString);
			var matchGroups = match.Groups.Values.ToList();
			_options.Server = matchGroups.Where(x => x.Name == "host").Select(x => x.Value).FirstOrDefault();
			_options.Username = matchGroups.Where(x => x.Name == "user").Select(x => x.Value).FirstOrDefault();
			_options.Password = matchGroups.Where(x => x.Name == "pwd").Select(x => x.Value).FirstOrDefault();
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
			int index = sourcePath.LastIndexOf(".");
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
			int index = sourcePath.LastIndexOf(".");
			string extension = sourcePath.Substring(index);
			string file = sourcePath.Substring(0, index);
			return file + "_pending" + extension;
		}

		/// <summary>
		/// This method moves the file from source to destination within the SFTP
		/// </summary>
		/// <param name="connectionString">The SFTP connection string</param>
		/// <param name="oldPath">The complete old path of the file including the file name</param>
		/// <param name="newPath">The complete new path of the file including the file name</param>
		private void MoveDirectory(string oldPath, string newPath)
        {
			using (SftpClient sftp = CreateClient())
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
		private async Task CreateFile(string destPath, string content)
        {
			using (SftpClient sftp = CreateClient())
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
