﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SwarmClientVS.Domain.IRepository;

namespace SwarmClientVS.Domain.DataModel
{
    public class SessionInputData : IData
    {
        public List<ProjectInputData> ProjectInput { get; set; } = new List<ProjectInputData>();
        public DeveloperData Developer { get; set; }
    }
}
