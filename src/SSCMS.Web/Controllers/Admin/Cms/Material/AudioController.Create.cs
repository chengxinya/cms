using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Configuration;
using SSCMS.Enums;
using SSCMS.Models;
using SSCMS.Utils;
using SSCMS.Core.Utils;

namespace SSCMS.Web.Controllers.Admin.Cms.Material
{
    public partial class AudioController
    {
        [RequestSizeLimit(long.MaxValue)]
        [HttpPost, Route(Route)]
        public async Task<ActionResult<MaterialAudio>> Create([FromQuery] CreateRequest request, [FromForm] IFormFile file)
        {
            if (!await _authManager.HasSitePermissionsAsync(request.SiteId,
                MenuUtils.SitePermissions.MaterialAudio))
            {
                return Unauthorized();
            }

            var site = await _siteRepository.GetAsync(request.SiteId);

            if (file == null)
            {
                return this.Error(Constants.ErrorUpload);
            }

            var fileName = Path.GetFileName(file.FileName);

            var fileType = PathUtils.GetExtension(fileName);
            if (!_pathManager.IsAudioExtensionAllowed(site, fileType))
            {
                return this.Error(Constants.ErrorAudioExtensionAllowed);
            }
            if (!_pathManager.IsAudioSizeAllowed(site, file.Length))
            {
                return this.Error(Constants.ErrorAudioSizeAllowed);
            }

            var materialFileName = PathUtils.GetMaterialFileName(fileName);
            var virtualDirectoryPath = PathUtils.GetMaterialVirtualDirectoryPath(UploadType.Audio);

            var directoryPath = PathUtils.Combine(_settingsManager.WebRootPath, virtualDirectoryPath);
            var filePath = PathUtils.Combine(directoryPath, materialFileName);

            await _pathManager.UploadAsync(file, filePath);

            var audio = new MaterialAudio
            {
                GroupId = request.GroupId,
                Title = PathUtils.RemoveExtension(fileName),
                FileType = fileType.ToUpper().Replace(".", string.Empty),
                Url = PageUtils.Combine(virtualDirectoryPath, materialFileName)
            };

            await _materialAudioRepository.InsertAsync(audio);

            return audio;
        }
    }
}
