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
        private readonly List<string> _messages;

        public HistoryMessagesAdapter(List<string> messages)
        {
            _messages = messages;
        }
        public override int ItemCount => _messages.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var historyMessageHolder = holder as HistoryMessageViewHolder;
            historyMessageHolder.MessageText.Text = _messages[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var messageTextView = new TextView(parent.Context);
            var holder = new HistoryMessageViewHolder(messageTextView);
            return holder;
        }
    }

    public class HistoryMessageViewHolder : RecyclerView.ViewHolder
    {
        public TextView MessageText { get; }
        public HistoryMessageViewHolder(TextView view) : base(view)
        {
            MessageText = view;
        }
    }
}