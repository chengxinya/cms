using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Utils;

namespace SSCMS.Web.Controllers.Admin
{
    public partial class RedirectController
    {
        [HttpPost, Route(Route)]
        public async Task<ActionResult<StringResult>> Submit([FromBody] SubmitRequest request)
        {
            var site = await _siteRepository.GetAsync(request.SiteId);
            var url = string.Empty;

            if (request.SiteId > 0 && request.ChannelId > 0 && request.ContentId > 0)
            {
                var channelInfo = await _channelRepository.GetAsync(request.ChannelId);
                url = await _pathManager.GetContentUrlAsync(site, channelInfo, request.ContentId, request.IsLocal);
            }
            else if (request.SiteId > 0 && request.ChannelId > 0)
            {
                var channelInfo = await _channelRepository.GetAsync(request.ChannelId);
                url = await _pathManager.GetChannelUrlAsync(site, channelInfo, request.IsLocal);
            }
            else if (request.SiteId > 0 && request.FileTemplateId > 0)
            {
                url = await _pathManager.GetFileUrlAsync(site, request.FileTemplateId, request.IsLocal);
            }
            else if (request.SiteId > 0 && request.SpecialId > 0)
            {
                url = await _pathManager.GetSpecialUrlAsync(site, request.SpecialId, request.IsLocal);
            }
            else if (request.SiteId > 0)
            {
                var channelInfo = await _channelRepository.GetAsync(request.SiteId);
                url = await _pathManager.GetChannelUrlAsync(site, channelInfo, request.IsLocal);
            }

            //if (site.IsSeparatedWeb)
            //{
            //    if (siteId > 0 && channelId > 0 && contentId > 0)
            //    {
            //        url = PreviewApi.GetContentUrl(siteId, channelId, contentId);
            //    }
            //    else if (siteId > 0 && channelId > 0)
            //    {
            //        url = PreviewApi.GetChannelUrl(siteId, channelId);
            //    }
            //    else if (siteId > 0 && templateId > 0)
            //    {
            //        url = PreviewApi.GetFileUrl(siteId, templateId);
            //    }
            //    else if (siteId > 0)
            //    {
            //        url = PreviewApi.GetSiteUrl(siteId);
            //    }
            //}
            //else
            //{

            //}

            if (string.IsNullOrEmpty(url) || StringUtils.EqualsIgnoreCase(url, PageUtils.UnClickableUrl))
            {
                if (request.SiteId == 0)
                {
                    request.SiteId = await _siteRepository.GetIdByIsRootAsync();
                }
                if (request.SiteId != 0)
                {
                    site = await _siteRepository.GetAsync(request.SiteId);

                    url = site.IsSeparatedWeb
                        ? _pathManager.GetPreviewSiteUrl(request.SiteId)
                        : await _pathManager.GetWebUrlAsync(site);
                }
                else
                {
                    url = "/";
                }
            }

            return new StringResult
            {
                Value = url
            };
        }
    }
}
