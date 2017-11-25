﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmClientVS.Domain.IRepository;

namespace SwarmClientVS.Domain.Service
{
    public class SessionInputModelSimple
    {
        public BindingList<SessionGridViewItemModel> Task { get; set; } = new BindingList<SessionGridViewItemModel>();
        public string Project { get; set; }
    }
}
