using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace RMStore.Infrastructure
{
    public interface IScopeInformation
    {
        Dictionary<string, string> HostScopeInfo { get; }

        Dictionary<string, string> GetUserScopeInfo(ClaimsPrincipal user);
    }

    public class ScopeInformation : IScopeInformation
    {
        public Dictionary<string, string> HostScopeInfo { get; }

        public ScopeInformation()
        {
            HostScopeInfo = new Dictionary<string, string>()
            {
                {"MachineName", Environment.MachineName },
                {"EntryPoint", Assembly.GetEntryAssembly().GetName().Name }
            };
        }

        public Dictionary<string, string> GetUserScopeInfo(ClaimsPrincipal user)
        {
            var maskedEmail = MaskEmailAddress(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);
            var userDict = new Dictionary<string, string>
            {
                {"UserId", user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value },
                {"UserName", user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value },
                {"Email", maskedEmail }
            };
            return userDict;
        }

        private string MaskEmailAddress(string emailAddress)
        {
            var atIndex = emailAddress?.IndexOf('@');
            if (atIndex > 1)
            {
                return string.Format("{0}{1}***{2}", emailAddress[0], emailAddress[1],
                    emailAddress.Substring(atIndex.Value));
            }
            return emailAddress;
        }
    }
}
