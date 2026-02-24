using System.Runtime.InteropServices;

namespace Legacy.Snapln
{
    public interface ISnapInComponent
    {
        string GetDisplayName();
        string GetDescription();
        string GetVersion();
        string[] GetNodeNames();
        object[] GetNodeItems(string nodeName);
        int GetItemCount(string nodeName);
    }
}
