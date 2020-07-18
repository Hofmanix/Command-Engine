using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace CommandEngine.Controls.Droid
{
    class HistoryMessagesAdapter : RecyclerView.Adapter
    {
        private readonly List<CommandText> _messages;

        public HistoryMessagesAdapter(List<CommandText> messages)
        {
            _messages = messages;
        }
        public override int ItemCount => _messages.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var historyMessageHolder = holder as HistoryMessageViewHolder;
            historyMessageHolder.MessageText.Text = _messages[position].Text;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var messageTextView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.commander_message, parent, false);
            var holder = new HistoryMessageViewHolder(messageTextView);
            return holder;
        }
    }
}