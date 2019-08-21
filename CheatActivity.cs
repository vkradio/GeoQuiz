using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace GeoQuiz
{
    [Activity(Label = "CheatActivity", Theme = "@style/AppTheme")]
    public class CheatActivity : AppCompatActivity
    {
        const string C_EXTRA_ANSWER_IS_TRUE = "com.bignerdranch.android.geoquiz.answer_is_true";
        const string C_EXTRA_ANSWER_SHOWN = "com.bignerdranch.android.geoquiz.answer_shown";

        bool answerIsTrue;
        TextView answerTextView;
        Button showAnswerButton;

        void SetAnswerShownResult(bool isAnswerShown)
        {
            var data = new Intent();
            data.PutExtra(C_EXTRA_ANSWER_SHOWN, isAnswerShown);
            SetResult(Result.Ok, data);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cheat);

            answerIsTrue = Intent.GetBooleanExtra(C_EXTRA_ANSWER_IS_TRUE, default);
            answerTextView = FindViewById<TextView>(Resource.Id.AnswerTextView);
            showAnswerButton = FindViewById<Button>(Resource.Id.ShowAnswerButton);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                showAnswerButton.Click += (sender, e) =>
                {
                    answerTextView.SetText(answerIsTrue ? Resource.String.true_button : Resource.String.false_button);
                    SetAnswerShownResult(true);

                    var cx = showAnswerButton.Width / 2;
                    var cy = showAnswerButton.Height / 2;
                    var radius = (float)showAnswerButton.Width;
                    var anim = ViewAnimationUtils.CreateCircularReveal(showAnswerButton, cx, cy, radius, 0f);
                    anim.AnimationEnd += (animSender, animE) => showAnswerButton.Visibility = ViewStates.Invisible;
                    anim.Start();
                };
            }
            else
            {
                showAnswerButton.Visibility = ViewStates.Invisible;
            }
        }

        public static Intent NewIntent(Context packageContext, bool answerIsTrue)
        {
            var intent = new Intent(packageContext, typeof(CheatActivity));
            intent.PutExtra(C_EXTRA_ANSWER_IS_TRUE, answerIsTrue);
            return intent;
        }

        public static bool WasAnswerShown(Intent result) => result.GetBooleanExtra(C_EXTRA_ANSWER_SHOWN, default);
    }
}