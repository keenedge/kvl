using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

using Zone.Models;

namespace Zone.Services
{
    /// <summary/>
    public interface IConfigRepoService
    {
        /// <summary/>
        string RootPath { get; set; }
        /// <summary/>
        string PresetsPath { get; }
        /// <summary/>
        string RoutesPath { get; }
        /// <summary/>
        string AudioActionsPath { get; }
        /// <summary/>
        T LoadFile<T>( string filePath);
        /// <summary/>
        Object LoadFile( string filePath);

    }
}