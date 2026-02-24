using System.Runtime.InteropServices;

namespace Legacy.SnapIn;

// ============================================================
// COM INTERFACE — The MMC Snap-in Contract
// ============================================================
// Traditional MMC snap-ins were COM components that implemented
// specific interfaces (IComponentData, IComponent, ISnapinAbout).
//
// This interface simulates that pattern. In real MMC snap-ins,
// you'd implement Microsoft.ManagementConsole interfaces.
//
// Key COM Interop attributes explained:
//   [ComVisible(true)]  — Exposes this type to COM clients
//   [Guid("...")]       — Every COM type needs a unique GUID
//   [InterfaceType(...)] — Controls how the interface is exposed
// ============================================================

[ComVisible(true)]
[Guid("A1B2C3D4-E5F6-4A5B-8C9D-0E1F2A3B4C5D")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISnapInComponent
{
    string GetDisplayName();
    string GetDescription();
    string GetVersion();
    string[] GetNodeNames();
    object[] GetNodeItems(string nodeName);
    int GetItemCount(string nodeName);
}
