﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmClientVS.Domain.IRepository
{
    public interface IRepository<T> where T : IData
    {
        void GenerateIdentifier();
        void Save(T dataModel);
        T Get<T>();
    }
}