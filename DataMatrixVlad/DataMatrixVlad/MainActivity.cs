using System;
using Android.App;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using ZXing;
using ZXing.Mobile;

namespace DataMatrixVadim
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = Android.Views.SoftInput.AdjustResize)]
    public class MainActivity : AppCompatActivity
    {
        private EditText inputedText;
        private TextView textView;
        private Button convertAction;
        private ImageView viewForImage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            viewForImage = FindViewById<ImageView>(Resource.Id.viewForImage);

            convertAction = FindViewById<Button>(Resource.Id.convertAction);
            convertAction.Click += ConvertAction;
            textView = FindViewById<TextView>(Resource.Id.textView);
            inputedText = FindViewById<EditText>(Resource.Id.inputedText);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }


        private void ConvertAction(object sender, EventArgs eventArgs)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(inputedText.Text);
            string result = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(bytes);

            if (string.Equals(inputedText.Text, result) && !string.IsNullOrEmpty(inputedText.Text))
            {
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.DATA_MATRIX,
                    Options = new ZXing.Datamatrix.DatamatrixEncodingOptions
                    {
                        Width = 100,
                        Height = 100,
                        Margin = 30
                    }
                };
                textView.Text = "";

                var image = writer.Write(inputedText.Text);
                viewForImage.SetImageBitmap(image);
            }
            else
                textView.Text = "Неправильный формат";

            var inputManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputManager.HideSoftInputFromWindow(convertAction.WindowToken, HideSoftInputFlags.None);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

