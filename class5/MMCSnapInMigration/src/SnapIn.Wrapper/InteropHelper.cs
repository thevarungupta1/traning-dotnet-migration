using System.Runtime.InteropServices;

namespace SnapIn.Wrapper;

// ============================================================
// COM INTEROP HELPER
// ============================================================
// Demonstrates how you would use COM interop to instantiate a
// legacy snap-in by its CLSID (Class ID) — the way mmc.exe
// originally loaded snap-ins.
//
// In a real migration scenario, you'd use these patterns to:
//   1. Activate existing registered COM components
//   2. Call methods through COM interfaces
//   3. Handle COM error codes (HRESULTs)
//   4. Manage COM object lifetimes
// ============================================================

public static class InteropHelper
{
    /// <summary>
    /// Demonstrates creating a COM object by CLSID.
    /// This is how mmc.exe loaded snap-ins from the registry:
    ///   HKLM\SOFTWARE\Microsoft\MMC\SnapIns\{CLSID}
    /// </summary>
    public static object? CreateComObject(string clsid)
    {
        try
        {
            var type = Type.GetTypeFromCLSID(new Guid(clsid));
            if (type == null)
            {
                Console.WriteLine($"  COM type not found for CLSID: {clsid}");
                return null;
            }
            return Activator.CreateInstance(type);
        }
        catch (COMException ex)
        {
            Console.WriteLine($"  COM Error (0x{ex.HResult:X8}): {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Error creating COM object: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Checks if a COM class is registered on this machine.
    /// MMC snap-ins register under:
    ///   HKLM\SOFTWARE\Classes\CLSID\{GUID}
    /// </summary>
    public static bool IsComClassRegistered(string clsid)
    {
        try
        {
            var type = Type.GetTypeFromCLSID(new Guid(clsid), throwOnError: false);
            return type != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Safely releases a COM object to prevent leaks.
    /// In .NET Framework, COM interop required explicit release.
    /// </summary>
    public static void ReleaseCom(object? comObject)
    {
        if (comObject != null && Marshal.IsComObject(comObject))
        {
            Marshal.ReleaseComObject(comObject);
        }
    }

    /// <summary>
    /// Demonstrates checking HRESULT values.
    /// COM methods return HRESULT codes instead of throwing exceptions.
    /// </summary>
    public static string DescribeHResult(int hresult) => hresult switch
    {
        0x00000000 => "S_OK — Success",
        0x00000001 => "S_FALSE — Success with info",
        unchecked((int)0x80004001) => "E_NOTIMPL — Not implemented",
        unchecked((int)0x80004002) => "E_NOINTERFACE — No such interface",
        unchecked((int)0x80004003) => "E_POINTER — Invalid pointer",
        unchecked((int)0x80004005) => "E_FAIL — Unspecified failure",
        unchecked((int)0x80070005) => "E_ACCESSDENIED — Access denied",
        unchecked((int)0x8007000E) => "E_OUTOFMEMORY — Out of memory",
        _ => $"Unknown HRESULT: 0x{hresult:X8}"
    };
}
