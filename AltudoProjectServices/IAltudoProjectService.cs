using AltudoProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltudoProjectServices
{
    public interface IAltudoProjectService
    {
        Task<AltudoModel> processaURL(string url);
    }
}
