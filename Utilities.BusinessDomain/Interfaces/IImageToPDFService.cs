using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.BusinessDomain.Interfaces
{
    public interface IImageToPDFService
    {
        Task<string> ConvertImageToPDF(string image);
    }
}
