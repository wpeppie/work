using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using FileSystemAssembly.Interfaces;
using FileSystemAssembly.Helpers;
using System.IO;

namespace InputFileComparer
{
    public class ConfigurationValidator
    {
        private IFileSystem _fileSystem;
        public ConfigurationValidator(IFileSystem fileSystem = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public void ValidateConfiguration(Config config)
        {
            CheckMandatoryStringCollection(() => config.KeyColumns);
            CheckMandatoryStringCollection(() => config.ComparisonColumns);
            CheckMandatoryFileExists(() => config.FirstFile);
            CheckMandatoryFileExists(() => config.SecondFile);
            CheckMandatoryString(() => config.DateColumnName);
            CheckMandatoryString(() => config.ColumnsSeparator);
            CheckMandatoryFolderExists(() => config.OutputFileName);
            CheckMandatoryInt(() => config.FirstFileHeaderLine);
            config.SecondFileHeaderLine = config.SecondFileHeaderLine 
                ?? config.FirstFileHeaderLine;
            
        }

        private void CheckMandatoryStringCollection(Expression<Func<string[]>> collection)
        {
            MemberExpression body = collection.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)collection.Body;
                body = ubody.Operand as MemberExpression;
            }
            var compiledCollection = collection.Compile()();
            if (compiledCollection == null || compiledCollection.Length == 0)
            {
                throw new ArgumentException("Configuration " + body.Member.Name + " must contain at least 1 string element");
            }
        }

        private void CheckMandatoryString(Expression<Func<string>> value)
        {
            MemberExpression body = value.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)value.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (string.IsNullOrEmpty(value.Compile()()))
            {
                throw new ArgumentException("Configuration " + body.Member.Name + " must be specified");
            }

        }

        private void CheckMandatoryFileExists(Expression<Func<string>> value)
        {
            CheckMandatoryString(value);
            MemberExpression body = value.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)value.Body;
                body = ubody.Operand as MemberExpression;
            }
            if (!_fileSystem.FileExists(value.Compile()()))
            {
                throw new ArgumentException("Configuration " + body.Member.Name + " does not exist");
            }


        }

        private void CheckMandatoryFolderExists(Expression<Func<string>> value)
        {
            CheckMandatoryString(value);
            MemberExpression body = value.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)value.Body;
                body = ubody.Operand as MemberExpression;
            }
            FileInfo fi = new FileInfo(value.Compile()());
            if (!_fileSystem.FolderExists(fi.DirectoryName))
            {
                throw new ArgumentException("Configuration " + body.Member.Name + " does not exist");
            }


        }
        private void CheckMandatoryInt(Expression<Func<int?>> value)
        {
            MemberExpression body = value.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)value.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (value.Compile()() == null)
            {
                throw new ArgumentException("Configuration " + body.Member.Name + " must be specified");
            }

        }


    }
}
