using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKYPE4COMLib;

namespace Paywall.SkypeAutomation.Core.SkypeApi
{
  public class SkypeAPI
  {
    private string _lastID = string.Empty;
    private SKYPE4COMLib.Skype _skype;
    private DateTime _timeReference;
    private string _currentUser = string.Empty;

    // SUMMARY: Construct
    public SkypeAPI()
    {
      this._skype = new SKYPE4COMLib.Skype();
      this._skype.Attach(7, false);
      this._timeReference = DateTime.Now;
      this._currentUser = this._skype.CurrentUser.Handle;
    }

    public string LastMessageID(string conversationName)
    {
      int limit = 1000;
      int i = 0;

      if (conversationName.Equals("#monkeys.paywall.bot/$*T;905"))
        i = 0;

      ChatMessageCollection messages = this._skype.Messages;
      foreach(ChatMessage cm in messages)
      {
        if (cm.ChatName.Equals(conversationName))
          return cm.Id.ToString();
        if (i > limit)
          return string.Empty;
        i++;
      }
      return string.Empty;
    }

    public string LastMessageID()
    {
      ChatMessageCollection messages = this._skype.Messages;
      foreach (ChatMessage cm in messages)
        return cm.Id.ToString();
      return string.Empty;
    }

    public List<SkypeMessage> GetMessages(string messageID, string conversationName)
    {
      int limit = 150;
      int i = 0;
      List<SkypeMessage> result = new List<SkypeMessage>();

      ChatMessageCollection messages = this._skype.Messages;
      foreach (ChatMessage cm in messages)
      {
        //if (!cm.FromDisplayName.Equals(conversationName))
        //  continue;
        if (!cm.ChatName.Equals(conversationName))
          continue;
        if (cm.Id <= Int32.Parse(messageID))
          return result;
        if (i > limit)
          return result;

        if(!cm.Sender.Handle.Equals(this._currentUser))
          result.Add(new SkypeMessage()
          {
            ChatMessage = cm,
            Message = cm.Body,
            Sender = cm.Sender.FullName,
            Date = cm.Timestamp
          });

        i++;
      }
      return result;
    }

    public List<SkypeMessage> GetLastMessages(int count)
    {
      int limit = count;
      int i = 0;
      List<SkypeMessage> result = new List<SkypeMessage>();

      ChatMessageCollection messages = this._skype.Messages;
      foreach (ChatMessage cm in messages)
      {
        if (i > limit)
          return result;

        result.Add(new SkypeMessage()
        {
          ChatMessage = cm,
          Message = cm.Body,
          Sender = cm.Sender.FullName,
          Date = cm.Timestamp
        });

        i++;
      }
      return result;
    }
  }
}
