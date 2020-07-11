using System;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;

namespace CommandEngine.Controls.Droid
{
    public class HistoryMessageViewHolder : RecyclerView.ViewHolder
    {
        public TextView MessageText { get; }
        public HistoryMessageViewHolder(View view) : base(view)
        {
            MessageText = view.FindViewById<TextView>(Resource.Id.comander_message_text);
        }
    }
}
