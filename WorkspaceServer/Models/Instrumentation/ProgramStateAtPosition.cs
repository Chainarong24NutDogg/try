﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WorkspaceServer.Models.Execution;
using WorkspaceServer.Servers.Roslyn.Instrumentation.Contract;

namespace WorkspaceServer.Models.Instrumentation
{
    public class ProgramStateAtPositionArray : IRunResultFeature
    {
        [JsonProperty("instrumentation")]
        public IReadOnlyCollection<ProgramStateAtPosition> ProgramStates { get; set; }

        public ProgramStateAtPositionArray(IReadOnlyCollection<string> programStates)
        {
            ProgramStates = programStates.Select(JsonConvert.DeserializeObject<ProgramStateAtPosition>).ToArray();
        }

        public void Apply(RunResult result)
        {
            result.AddProperty("instrumentation", ProgramStates);
        }

    }
    //public class ProgramStateAtPosition 
    //{
    //    [JsonProperty("filePosition")]
    //    public FilePosition FilePosition { get; set; }

    //    [JsonProperty("stackTrace")]
    //    public string StackTrace { get; set; }

    //    [JsonProperty("locals")]
    //    public VariableInfo[] Locals { get; set; }

    //    [JsonProperty("parameters")]
    //    public VariableInfo[] Parameters { get; set; }

    //    [JsonProperty("fields")]
    //    public VariableInfo[] Fields { get; set; }

    //    [JsonProperty("output")]
    //    public DeclarationLocation Output { get; set; }
     
    //}

    //public partial class VariableInfo
    //{
    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("value")]
    //    public string Value { get; set; }

    //    [JsonProperty("declaredAt")]
    //    public DeclarationLocation DeclaredAt { get; set; }
    //}

    //public partial class DeclarationLocation
    //{
    //    [JsonProperty("start")]
    //    public long Start { get; set; }

    //    [JsonProperty("end")]
    //    public long End { get; set; }
    //}

    //public partial class FilePosition
    //{
    //    [JsonProperty("line")]
    //    public long Line { get; set; }

    //    [JsonProperty("character")]
    //    public long Character { get; set; }

    //    [JsonProperty("file")]
    //    public string File { get; set; }
    //}
}

