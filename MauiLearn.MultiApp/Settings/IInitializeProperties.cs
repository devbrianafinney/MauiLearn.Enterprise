using MauiLearn.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MauiDbLocation = Microsoft.Maui.Storage.FileSystem;
//want EF later...
namespace MauiLearn.MultiApp.Settings
{
    using System.Collections.Generic;

    public interface IInitializeProperties
    {
        string Key { get; }
        string Value { get; }
    }


    public static class InitializePropertiesProvider
    {
        // Public accessor: callers only see IInitializeProperties
        public static List<IInitializeProperties> GetInitialProperties()
        {
            return new List<IInitializeProperties>
            {
                new InitializeProperty("NowConnectionString", "Data Source=mauilearn.db3"), //BEAUTIFUL!!!!!! :-) Learn more now! ?null possibles too
                new InitializePropertyRecord("AppDeviceName", "Windows"),
                new InitializePropertyRecord("ConnectionStringInitial", Path.Combine(FileSystem.AppDataDirectory, "mauilearn.db3")),
                new InitializePropertyRecord("ConnectionString", Path.Combine(MauiDbLocation.AppDataDirectory, "mauilearn.db3")),
                new InitializePropertyRecord("EntireFileLocation", FileSystem.AppDataDirectory),
                new InitializePropertyRecord("ApplicationFileLocation", MauiDbLocation.AppDataDirectory),
                new InitializePropertyRecord("EnvironmentSpecialFolderApplicationData", Environment.SpecialFolder.ApplicationData.ToString()),
                new InitializePropertyRecord("AppCoreDb", "mauilearn.db3"),
                new InitializePropertyRecord("DbExtension", ".db3"),
                new InitializePropertyRecord("DbLoggingExtension", ".-wal"),
                new InitializePropertyRecord("DbCacheExtension", ".-smh")
            };
        }

        // Private concrete class implementation
        private class InitializeProperty : IInitializeProperties
        {
            public string Key { get; init; }
            public string Value { get; init; }

            public InitializeProperty(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }

        /* ***************************************************************************************
         * --------------------------------------------------------------------------------------
         *          Private record implementation (immutable, value equality)
         * --------------------------------------------------------------------------------------
         * **************************************************************************************/
        private record InitializePropertyRecord(string Key, string Value) : IInitializeProperties;
    }
}
