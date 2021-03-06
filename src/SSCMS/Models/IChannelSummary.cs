using System;
using System.Collections.Generic;

namespace SSCMS.Models
{
    public interface IChannelSummary
    {
        int Id { get; set; }
        string ChannelName { get; set; }
        int ParentId { get; set; }
        List<int> ParentsPath { get; set; }
        string IndexName { get; set; }
        string ContentModelPluginId { get; set; }
        string TableName { get; set; }
        int Taxis { get; set; }
        DateTime? AddDate { get; set; }
    }
}
