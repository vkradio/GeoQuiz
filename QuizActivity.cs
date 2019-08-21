using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Util;
using Android.Content;

namespace GeoQuiz
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class QuizActivity : AppCompatActivity
    {
        readonly Question[] questionBank = new Question[]
        {
            new Question(Resource.String.question_australia, true),
            new Question(Resource.String.question_oceans, true),
            new Question(Resource.String.question_mideast, false),
            new Question(Resource.String.question_africa, false),
            new Question(Resource.String.question_americas, true),
            new Question(Resource.String.question_asia, true)
        };

        const string C_LOG_TAG = nameof(QuizActivity);
        const string C_KEY_INDEX = "index";
        const int C_REQUEST_CODE_CHEAT = 0;

        Button trueButton;
        Button falseButton;
        Button nextButton;
        Button cheatButton;
        TextView questionTextView;
        int currentIndex;
        bool isCheater;

        void UpdateQuestion()
        {
            var question = questionBank[currentIndex].TextResId;
            questionTextView.SetText(question);
        }

        void CheckAnswer(bool userPressedTrue)
        {
            var answerIsTrue = questionBank[currentIndex].AnswerTrue;

            var messageResId = userPressedTrue == answerIsTrue ? Resource.String.CorrectToast : Resource.String.IncorrectToast;

            if (isCheater)
                messageResId = Resource.String.JudgmentToast;

            Toast.MakeText(this, messageResId, ToastLength.Short).Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Log.Debug(C_LOG_TAG, "OnCreate(Bundle) called");
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_quiz);

            currentIndex = savedInstanceState?.GetInt(C_KEY_INDEX, default) ?? default;

            trueButton = FindViewById<Button>(Resource.Id.TrueButton);
            falseButton = FindViewById<Button>(Resource.Id.FalseButton);
            nextButton = FindViewById<Button>(Resource.Id.NextButton);
            cheatButton = FindViewById<Button>(Resource.Id.CheatButton);
            questionTextView = FindViewById<TextView>(Resource.Id.QuestionTextView);
            UpdateQuestion();

            trueButton.Click += (sender, e) => CheckAnswer(true);

            falseButton.Click += (sender, e) => CheckAnswer(false);

            nextButton.Click += (sender, e) =>
            {
                currentIndex = (currentIndex + 1) % questionBank.Length;
                isCheater = false;
                UpdateQuestion();
            };

            cheatButton.Click += (sender, e) =>
            {
                bool answerIsTrue = questionBank[currentIndex].AnswerTrue;

                using (var intent = CheatActivity.NewIntent(this, answerIsTrue))
                    StartActivityForResult(intent, C_REQUEST_CODE_CHEAT);
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            Log.Debug(C_LOG_TAG, "OnStart() called");
        }

        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug(C_LOG_TAG, "OnResume() called");
        }

        protected override void OnPause()
        {
            base.OnPause();
            Log.Debug(C_LOG_TAG, "OnPause() called");
        }

        protected override void OnStop()
        {
            base.OnStop();
            Log.Debug(C_LOG_TAG, "OnStop() called");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(C_LOG_TAG, "OnDestroy() called");
        }

        protected override void OnSaveInstanceState(Bundle savedInstanceState)
        {
            base.OnSaveInstanceState(savedInstanceState);
            Log.Info(C_LOG_TAG, nameof(OnSaveInstanceState));
            savedInstanceState.PutInt(C_KEY_INDEX, currentIndex);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode != Result.Ok)
                return;

            if (requestCode == C_REQUEST_CODE_CHEAT)
            {
                if (data == null)
                    return;
                isCheater = CheatActivity.WasAnswerShown(data);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}