using Paywall.SkypeAutomation.Core.Events;
using Paywall.SkypeAutomation.Core.SkypeCommand;
using Paywall.SkypeAutomation.Implementations.SkypeApi;
using Paywall.SkypeAutomation.Implementations.SkypeCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paywall.SkypeAutomation.Core.SkypeApi
{
  public abstract class SkypeConversationBase
  {
    private string _key = string.Empty;
    protected string _messageID = string.Empty;
    private string _chatName = string.Empty;
    private string _conversationName = string.Empty;
    private string _description = string.Empty;
    private DateTime _lastCalled;
    private DateTime _lastRefreshTime;
    private DateTime _refreshDelay;
    private int _refreshedTimes = 0;
    private bool _nonExecutable = true;
    private EventTrigger _eventTrigger = null;
    private SkypeCommandSetBase _commandSet = null;
    private IntPtr? _sendPointer = null;

    public string Key { get { return this._key; } }
    public string ConversationName { get { return this._conversationName; } }     // for sending message
    public string ChatName { get { return this._chatName; } }                     // for receiveing messages
    public string Description { get { return this._description; } }
    public SkypeConversionManager Parent { get { return SkypeConversionManager.Current; } }
    public IntPtr? SendPointer { get { return this._sendPointer; } }
    protected EventTrigger Trigger { get { return this._eventTrigger; } set { this._eventTrigger = value; ; } }
    protected SkypeCommandSetBase CommandSet { get { return this._commandSet; } set { this._commandSet = value; } }

    protected bool NonExecutable { get { return this._nonExecutable; } set { this._nonExecutable = value; } }

    public SkypeConversationBase(string key, string conversionName, string chatName, string description = "")
    {
      this._key = key;
      this._conversationName = conversionName;
      this._chatName = chatName;
      this._description = description;
      this._commandSet = new DefaultCommandSet();

      this._sendPointer = Program.SkypeProcess.GetSenderPointer(this.ConversationName);
      this._messageID = this.Parent.SkypeAPI.LastMessageID();
      if (string.IsNullOrEmpty(this._messageID))
        throw new ArgumentException("Could not find last message ID of this conversation '" + this.ConversationName + "'");
      this._lastCalled = DateTime.Now;
    }

    public void Call(DateTime time)
    {
      if(this._eventTrigger != null && this._lastCalled.AddMilliseconds(this._eventTrigger.Miliseconds) > time)
        return;

      if (!this._nonExecutable)
        return;

      this._lastCalled = DateTime.Now;
      this.Execute();
    }

    public IntPtr? GetPointer()
    {
      if (Program.SkypeProcess.ConversationExists(this.ConversationName))
        return this._sendPointer;

      Program.SkypeProcess.OpenConversation(this.ConversationName);
      this._sendPointer = Program.SkypeProcess.GetSenderPointer(this.ConversationName);
      return this._sendPointer;
    }


    // SUMMARY: From Skype API get unread messages
    public List<SkypeMessage> GetMessages()
    {
      List<SkypeMessage> messages = this.Parent.SkypeAPI.GetMessages(this._messageID, this._chatName);
      this._messageID = messages != null && messages.Count > 0 ? messages.LastOrDefault().ChatMessage.Id.ToString() : this._messageID;
      return messages;
    }

    protected abstract void Execute();
  }
}
