using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SnapIn.Wrapper
{
    // COM INTEROP HELPER
    // demonstrates how to use COM interop to interact with the MMC snap-in
    // In a real migration scenario, you use these pattern to:
    // 1. activate exusting registerd snap-in via COM component
    // 2. call method throigu com interface
    // 3. handle com error codes
    // 4. manage com object lifetimes
    public static class InteropHelper
    {
        public static object? CreateComObject(string clsid)
        {
            try
            {
                var type = Type.GetTypeFromCLSID(new Guid(clsid));
                if (type == null)
                {
                    Console.WriteLine($"Failed to get type from CLSID: {clsid}");
                    return null;
                }
                return Activator.CreateInstance(type);
            }
            catch (COMException ex)
            {
                Console.WriteLine($"COM error creating object with CLSID {clsid}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error creating object with CLSID {clsid}: {ex.Message}");
                return null;
            }
        }

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

        public static void ReleaseComObject(object comObject)
        {
            try
            {
                if (comObject != null && Marshal.IsComObject(comObject))
                {
                    Marshal.ReleaseComObject(comObject);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error releasing COM object: {ex.Message}");
            }
        }

        public static string DescribeHResult(int hresult) => hresult switch
        {
            0x00000000 => "S_OK (Operation successful)",
            0x00000001 => "S_FALSE (Operation successful but returned false)",
            unchecked((int)0x80070005) => "Access Denied",
            unchecked((int)0x80004002) => "No such interface supported",
            unchecked((int)0x80040154) => "Class not registered",
            _ => $"Unknown COM error: 0x{hresult:X}"
        };
    }
}
