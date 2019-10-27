using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CommandEngine.Controls.Droid
{
    internal class MaxHeightScrollView: ScrollView
    {
        public int? MaxHeight { get; set; }

        protected MaxHeightScrollView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MaxHeightScrollView(Context context) : base(context)
        {
        }

        public MaxHeightScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public MaxHeightScrollView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public MaxHeightScrollView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            try
            {
                var heightSize = MeasureSpec.GetSize(heightMeasureSpec);
                if (MaxHeight != null
                    && heightSize > MaxHeight)
                {
                    heightSize = MaxHeight.Value;
                }
                heightMeasureSpec = MeasureSpec.MakeMeasureSpec(heightSize, MeasureSpecMode.AtMost);
                LayoutParameters.Height = heightSize;
            }
            catch (Exception e)
            {
                Log.Error("MaxHeightScrollView", "onMeasure - Error forcing height", e);
            }
            finally
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }
    }
}