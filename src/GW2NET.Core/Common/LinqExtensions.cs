// <copyright file="LinqExtensions.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        public static IEnumerable<T> SymmetricExcept<T>(this IEnumerable<T> seq1, IEnumerable<T> seq2)
        {
            HashSet<T> hashSet = new HashSet<T>(seq1);
            hashSet.SymmetricExceptWith(seq2);
            return hashSet.Select(x => x);
        }
    }
}
