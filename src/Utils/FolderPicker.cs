using System;
using System.Runtime.InteropServices;

namespace PilotLookUp.Utils
{
    internal static class FolderPicker
    {
        public enum PickResult
        {
            Ok,
            Cancelled,
            Failed
        }

        public static PickResult PickFolder(IntPtr ownerHandle, string title, out string folderPath, string initialFolder = null)
        {
            folderPath = null;

            IFileOpenDialog dialog = null;
            IShellItem initialFolderItem = null;
            IShellItem resultItem = null;
            try
            {
                dialog = (IFileOpenDialog)new FileOpenDialog();

                dialog.GetOptions(out var options);
                dialog.SetOptions(options | FOS.FOS_PICKFOLDERS | FOS.FOS_FORCEFILESYSTEM | FOS.FOS_PATHMUSTEXIST);

                if (!string.IsNullOrWhiteSpace(title))
                    dialog.SetTitle(title);

                if (!string.IsNullOrWhiteSpace(initialFolder))
                {
                    var riid = typeof(IShellItem).GUID;
                    var hrFolder = SHCreateItemFromParsingName(initialFolder, IntPtr.Zero, ref riid, out initialFolderItem);
                    if (hrFolder == 0 && initialFolderItem != null)
                        dialog.SetFolder(initialFolderItem);
                }

                var hr = dialog.Show(ownerHandle);
                if (hr == HRESULT.ERROR_CANCELLED)
                    return PickResult.Cancelled;
                if (hr != 0)
                    return PickResult.Failed;

                dialog.GetResult(out resultItem);
                resultItem.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out var pszString);
                try
                {
                    folderPath = Marshal.PtrToStringUni(pszString);
                    return !string.IsNullOrWhiteSpace(folderPath) ? PickResult.Ok : PickResult.Failed;
                }
                finally
                {
                    if (pszString != IntPtr.Zero)
                        Marshal.FreeCoTaskMem(pszString);
                }
            }
            catch
            {
                return PickResult.Failed;
            }
            finally
            {
                if (resultItem != null)
                    Marshal.FinalReleaseComObject(resultItem);
                if (initialFolderItem != null)
                    Marshal.FinalReleaseComObject(initialFolderItem);
                if (dialog != null)
                    Marshal.FinalReleaseComObject(dialog);
            }
        }

        private static class HRESULT
        {
            public const int ERROR_CANCELLED = unchecked((int)0x800704C7);
        }

        [Flags]
        private enum FOS : uint
        {
            FOS_PICKFOLDERS = 0x00000020,
            FOS_FORCEFILESYSTEM = 0x00000040,
            FOS_PATHMUSTEXIST = 0x00000800
        }

        private enum SIGDN : uint
        {
            SIGDN_FILESYSPATH = 0x80058000
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        private static extern int SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            IntPtr pbc,
            ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        [ComImport]
        [Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7")]
        private class FileOpenDialog
        {
        }

        [ComImport]
        [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            void BindToHandler(IntPtr pbc, ref Guid bhid, ref Guid riid, out IntPtr ppv);
            void GetParent(out IShellItem ppsi);
            void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
            void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
            void Compare(IShellItem psi, uint hint, out int piOrder);
        }

        [ComImport]
        [Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileDialog
        {
            [PreserveSig] int Show(IntPtr parent);
            void SetFileTypes(uint cFileTypes, IntPtr rgFilterSpec);
            void SetFileTypeIndex(uint iFileType);
            void GetFileTypeIndex(out uint piFileType);
            void Advise(IntPtr pfde, out uint pdwCookie);
            void Unadvise(uint dwCookie);
            void SetOptions(FOS fos);
            void GetOptions(out FOS pfos);
            void SetDefaultFolder(IShellItem psi);
            void SetFolder(IShellItem psi);
            void GetFolder(out IShellItem ppsi);
            void GetCurrentSelection(out IShellItem ppsi);
            void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);
            void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
            void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
            void GetResult(out IShellItem ppsi);
            void AddPlace(IShellItem psi, uint fdap);
            void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
            void Close(int hr);
            void SetClientGuid(ref Guid guid);
            void ClearClientData();
            void SetFilter(IntPtr pFilter);
        }

        [ComImport]
        [Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileOpenDialog : IFileDialog
        {
            void GetResults(out IntPtr ppenum);
            void GetSelectedItems(out IntPtr ppsai);
        }
    }
}
