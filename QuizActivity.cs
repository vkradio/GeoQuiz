using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace GeoQuiz
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class QuizActivity : AppCompatActivity
    {
        Button trueButton;
        Button falseButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_quiz);

            trueButton = FindViewById<Button>(Resource.Id.TrueButton);
            falseButton = FindViewById<Button>(Resource.Id.FalseButton);

            trueButton.Click += (sender, e) =>
            {
                Toast
                    .MakeText(this, Resource.String.CorrectToast, ToastLength.Short)
                    .Show();
            };

            falseButton.Click += (sender, e) =>
            {
                Toast
                    .MakeText(this, Resource.String.IncorrectToast, ToastLength.Short)
                    .Show();
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}