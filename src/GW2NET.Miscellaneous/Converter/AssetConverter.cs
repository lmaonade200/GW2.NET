// <copyright file="AssetConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous.Converter
{
    using System;

    using GW2NET.Common;
    using GW2NET.Files;
    using GW2NET.Miscellaneous.ApiModels;

    /// <summary>Converts a <see cref="FileDataContract"/> to an <see cref="Asset"/>.</summary>
    public sealed class AssetConverter : IConverter<FileDataContract, Asset>
    {
        /// <inheritdoc />
        public Asset Convert(FileDataContract value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Asset asset = new Asset
            {
                Identifier = value.Id,
            };

            Uri icon;
            if (Uri.TryCreate(value.Icon, UriKind.Absolute, out icon))
            {
                asset.IconFileUrl = icon;

                // Split the path into segments
                // Format: /file/{signature}/{identifier}.{extension}
                string[] segments = icon.LocalPath.Split('.')[0].Split('/');
                if (segments.Length >= 3 && segments[2] != null)
                {
                    asset.FileSignature = segments[2];
                }

                int iconFileId;
                if (segments.Length >= 4 && int.TryParse(segments[3], out iconFileId))
                {
                    asset.FileId = iconFileId;
                }
            }

            return asset;
        }
    }
}