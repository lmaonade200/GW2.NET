// <copyright file="AuthenticatedMessageHandler.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Messages
{
    using System.Net.Http;

    public class AuthenticatedMessageHandler : DelegatingHandler
    {
        public AuthenticatedMessageHandler(HttpMessageHandler innerHandler)
        {
            this.InnerHandler = innerHandler;
        }

        // ToDo: Implement Authentification
    }
}
