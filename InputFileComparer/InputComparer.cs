﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystemAssembly.Interfaces;

namespace InputFileComparer
{
    public class InputComparer
    {
        private  IFileSystem _fileSystem;
        public InputComparer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void Process(string[] args)
        {
            if (args != null && args.Length == 1)
            {
                var config = ConfigurationReader.GetConfiguration(_fileSystem, args[0]);
                new ConfigurationValidator(_fileSystem).ValidateConfiguration(config);
                var previousFileLineCollection = new ComparableLineCollection(config, _fileSystem);
                previousFileLineCollection.ParseLines(_fileSystem.ReadFile(config.FirstFile), config.FirstFileHeaderLine);
                var currentFileLineCollection = new FileLineCollection(config, _fileSystem);
                currentFileLineCollection.ParseLines(_fileSystem.ReadFile(config.SecondFile), config.SecondFileHeaderLine??config.FirstFileHeaderLine);
                currentFileLineCollection.FilterComparableLines(previousFileLineCollection);
                _fileSystem.WriteFile(config.OutputFileName, currentFileLineCollection.GetOutputLines());
            }
            else 
            {
                throw new ArgumentException("The InputFileComparer must be called with the full name to a configurationfile as the argument.");
            }

        }


    }
}