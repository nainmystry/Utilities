using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.BusinessDomain.Interfaces;

namespace Utilities.BusinessDomain.Services
{
    public class ImageToPDFService : IImageToPDFService
    {
        public ImageToPDFService()
        {

        }

        public async Task<string> ConvertImageToPDF(string image)
        {
            throw new NotImplementedException();
        }
    }
}
