using CAF.Core.Extensions;

using System.Collections.Generic;

namespace CAF.Core.ViewModel.User.Response
{
    public class DashboardContainer
    {
        public string Title { get; set; }
        public List<DashboardContainerItem> Items { get; set; } = new List<DashboardContainerItem>();
    }
    public class DashboardContainerItem
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public DashboardContainerItemType Type { get; set; }
        public string Icon { get; set; }
        public string Count { get; set; }


        public DashboardContainerItem()
        {

        }

        public DashboardContainerItem(string title, string count, string icon, string subTitle)
        {
            Title = title;
            Icon = icon;
            Count = count;
            if (subTitle.Length < 70)
            {
                SubTitle = subTitle.AppendWordWithCount(" ", (70 - subTitle.Length));
            }
            else
                SubTitle = subTitle;
        }
    }
    public enum DashboardContainerItemType : int
    {
        Counter = 0,
        Process = 1
    }
}
