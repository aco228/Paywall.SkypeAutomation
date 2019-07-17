using Paywall.SkypeAutomation.Core.SkypeApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.SkypeProcess
{
  public class SkypeProcessManager
  {
    private Process _process = null;
    private IntPtr? _searchPointer = null;
    private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
    private const uint WM_SETTEXT = 0x000C;
    private const uint WM_KEYDOWN = 0x100;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, Int32 wParam, Int32 lParam);

    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    public SkypeProcessManager()
    {

      foreach(Process p in Process.GetProcesses())
        try
        {
          if(p.MainModule.ModuleName.ToLower().Equals("skype.exe"))
          {
            this._process = p;
            break;
          }
        }
        catch { }

      //this._process = (from p in Process.GetProcesses() where p.MainModule.ModuleName.ToLower().Contains("skype.exe") /* p.MainWindowTitle.Contains("Skype™")*/ select p).FirstOrDefault();
      if (this._process == null)
        throw new ArgumentException("Could not find SKYPE process. Maybe skype is closed!?");

      this._searchPointer = this.GetClassPointer("TSearchControl", "TAccessibleEdit");
      int a = 0;
    }

    private List<IntPtr> _EnumChildrenHandles(IntPtr handle)
    {
      try
      {
        List<IntPtr> _childrenHandlesList = new List<IntPtr>();
        GCHandle _gcchildrenHandlesList = GCHandle.Alloc(_childrenHandlesList);
        IntPtr _pchildrenHandlesList = GCHandle.ToIntPtr(_gcchildrenHandlesList);
        try
        {
          EnumWindowProc _childProc = new EnumWindowProc(_EnumWindow);
          EnumChildWindows(handle, _childProc, _pchildrenHandlesList);
        }
        finally
        {
          _gcchildrenHandlesList.Free();
        }
        return _childrenHandlesList;
      }
      catch { return null; }
    }

    private bool _EnumWindow(IntPtr hWnd, IntPtr lParam)
    {
      try
      {
        GCHandle _childrenHandles = GCHandle.FromIntPtr(lParam);
        if (_childrenHandles == null || _childrenHandles.Target == null)
          return false;
        List<IntPtr> _childrenHandlesList = _childrenHandles.Target as List<IntPtr>;
        _childrenHandlesList.Add(hWnd);
        return true;
      }
      catch { return false; }
    }

    // SUMMARY: Conversaation send message
    public SkypeProcessResponse Send(SkypeConversationBase conversation, String message)
    {
      String objclassname = "TChatRichEdit";
      try
      {
        if (conversation.GetPointer().HasValue)
        {
          SendMessage(conversation.SendPointer.Value, WM_SETTEXT, IntPtr.Zero, message);
          SendMessage(conversation.SendPointer.Value, WM_KEYDOWN, 13, 0);

          return new SkypeProcessResponse(true, string.Format("Message has been sent to '{0}': '{1}'", conversation.ConversationName, message));
        }

        List<IntPtr> l = _EnumChildrenHandles(this._process.MainWindowHandle);
        bool found = false;

        foreach (IntPtr hndl in l)
        {
          StringBuilder getWindowTextStringBuilder = new StringBuilder(255);
          GetWindowText(hndl, getWindowTextStringBuilder, getWindowTextStringBuilder.Capacity);

          if (getWindowTextStringBuilder.ToString().Contains(conversation.ConversationName))
          { found = true; continue; }

          StringBuilder stringBuilder = new StringBuilder(255);
          GetClassName(hndl, stringBuilder, stringBuilder.Capacity);

          if (stringBuilder.ToString() == objclassname && found)
          {
            SendMessage(hndl, WM_SETTEXT, IntPtr.Zero, message);
            SendMessage(hndl, WM_KEYDOWN, 13, 0);

            return new SkypeProcessResponse(true, string.Format("Message has been sent to '{0}': '{1}'", conversation.ConversationName, message));
          }
        }

        return new SkypeProcessResponse(false, "Conversation could not be found");
      }
      catch (Exception e)
      {
        return new SkypeProcessResponse(false, "FATAL", e.Message);
      }
    }


    // SUMMARY: Get specified pointer
    public IntPtr? GetSenderPointer(string conversation, String objclassname = "TChatRichEdit")
    {
      try
      {
        List<IntPtr> l = _EnumChildrenHandles(this._process.MainWindowHandle);
        bool found = false;

        foreach (IntPtr hndl in l)
        {
          StringBuilder getWindowTextStringBuilder = new StringBuilder(255);
          GetWindowText(hndl, getWindowTextStringBuilder, getWindowTextStringBuilder.Capacity);
          
          if (getWindowTextStringBuilder.ToString().Contains(conversation))
          {
            found = true;
            continue;
          }

          StringBuilder stringBuilder = new StringBuilder(255);
          GetClassName(hndl, stringBuilder, stringBuilder.Capacity);

          if (stringBuilder.ToString() == objclassname && found)
            return hndl;
        }

        return null;
      }
      catch (Exception e)
      {
        return null;
      }
    }

    // SUMMARY: Get unspecified pointer
    public IntPtr? GetClassPointer(string pointerParent, string pointerName)
    {
      try
      {
        List<IntPtr> ptrs = _EnumChildrenHandles(this._process.MainWindowHandle);
        bool found = false;

        foreach(IntPtr ptr in ptrs)
        {
          StringBuilder stringBuilder = new StringBuilder(255);
          GetClassName(ptr, stringBuilder, stringBuilder.Capacity);

          string ptr_name = stringBuilder.ToString();

          if(ptr_name.Equals(pointerParent))
          {
            found = true;
            continue;
          }

          if (ptr_name.Equals(pointerName) && found)
            return ptr;
        }

        return null;
      }
      catch
      {
        return null;
      }
    }

    // SUMMARY: Check if conversation exists in proceses
    public bool ConversationExists(string conversationName)
    {
      try
      {
        List<IntPtr> l = _EnumChildrenHandles(this._process.MainWindowHandle);
        foreach (IntPtr hndl in l)
        {
          StringBuilder getWindowTextStringBuilder = new StringBuilder(255);
          GetWindowText(hndl, getWindowTextStringBuilder, getWindowTextStringBuilder.Capacity);
          if (getWindowTextStringBuilder.ToString().Equals(conversationName))
            return true;
        }
        return false;
      }
      catch
      {
        return false;
      }
    }

    // SUMMARY: Open specific conversation
    public void OpenConversation(string conversationName)
    {
      if (!this._searchPointer.HasValue)
        return;

      SendMessage(this._searchPointer.Value, WM_SETTEXT, IntPtr.Zero, conversationName);
      SendMessage(this._searchPointer.Value, WM_KEYDOWN, 13, 0);
    }

  }
}
