﻿// ----------------------------------------------------------------------------------
//
// Copyright 2011 Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.CloudService.Cmdlet
{
    using System.Management.Automation;
    using System.Security.Permissions;
    using System.Text;
    using AzureTools;
    using Cmdlets.Common;
    using Microsoft.Samples.WindowsAzure.ServiceManagement;
    using Model;
    using Properties;

    /// <summary>
    /// Runs the service in the emulator
    /// </summary>
    [Cmdlet(VerbsLifecycle.Start, "AzureEmulator")]
    public class StartAzureEmulatorCommand : CmdletBase<IServiceManagement>
    {
        [Parameter(Mandatory = false)]
        [Alias("ln")]
        public SwitchParameter Launch { get; set; }

        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        public AzureService StartAzureEmulatorProcess(string rootPath)
        {
            string standardOutput;
            string standardError;

            StringBuilder message = new StringBuilder();
            AzureService service = new AzureService(rootPath ,null);
            
            WriteVerbose(string.Format(Resources.CreatingPackageMessage, "local"));
            service.CreatePackage(DevEnv.Local, out standardOutput, out standardError);
            
            WriteVerbose(Resources.StartingEmulator);
            service.StartEmulator(Launch.ToBool(), out standardOutput, out standardError);
            
            WriteVerbose(standardOutput);
            WriteVerbose(Resources.StartedEmulator);
            SafeWriteOutputPSObject(
                service.GetType().FullName,
                Parameters.ServiceName, service.ServiceName,
                Parameters.RootPath, service.Paths.RootPath);

            return service;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override void ExecuteCmdlet()
        {
            AzureTool.Validate();
            base.ExecuteCmdlet();
            StartAzureEmulatorProcess(base.GetServiceRootPath());
        }
    }
}