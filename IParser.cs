using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace streamscraper
{
    public interface IParser
    {
        Task<string> ParseAsync(string uri);
    }
}
