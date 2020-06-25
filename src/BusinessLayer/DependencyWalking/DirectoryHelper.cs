using System;
using Microsoft.Extensions.Logging;

using MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking.Interfaces;

namespace MyCompany.MyExamples.ProjectParser.BusinessLayer.DependencyWalking
{
    public class DirectoryHelper : IDirectoryHelper
    {
        public const string ErrorMessageILoggerFactoryIsNull = "ILoggerFactory is null";

        private readonly ILogger<DirectoryHelper> logger;
        
        public DirectoryHelper(ILoggerFactory loggerFactory)
        {
            if (null == loggerFactory)
            {
                throw new ArgumentNullException(ErrorMessageILoggerFactoryIsNull, (Exception)null);
            }

            this.logger = loggerFactory.CreateLogger<DirectoryHelper>();
        }

        public string GetRootDirectory()
        {
            string rootDirectory = string.Empty;
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HOME")))
            {
                /* running in azure */
                rootDirectory = Environment.GetEnvironmentVariable("HOME") + "\\site\\wwwroot";
            }
            else
            {
                /* in visual studio, local debugging */
                rootDirectory = ".";
            }
            
            if (null != this.logger)
            {
                this.logger.LogInformation(string.Format("RootDirectory set to '{0}'", rootDirectory));
            }

            return rootDirectory;
        }

        public string GetRootDirectoryAndCombine(string suffixPath)
        {
            string rootDirectory = this.GetRootDirectory();
            ////string returnValue = System.IO.Path.Combine(rootDirectory, suffixPath);
            string returnValue = rootDirectory + suffixPath;
            return returnValue;
        }

        private void Info(string msg)
        {
            if (null != this.logger)
            {
                this.logger.LogInformation(msg);
            }
        }
    }
}
