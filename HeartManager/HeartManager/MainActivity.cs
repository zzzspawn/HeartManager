using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Wearable.Activity;
//using Java.Interop;
using Android.Views.Animations;

//especially needed for communication
using Android.Gms.Common.Apis;
using Android.Gms.Wearable;
using Android.Gms.Common;
using System.Linq;
using Object = Java.Lang.Object;


namespace HeartManager
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]

    

    public class MainActivity : WearableActivity
    {
        private GoogleApiClient _client;
        const string _syncPath = "/WearHM/Data";
        private HHDataManager _dataManager;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            //textView = FindViewById<TextView>(Resource.Id.text);
            _dataManager = new HHDataManager(this, syncPath: _syncPath);

            

            SetContentView(Resource.Layout.activity_main);

            Button button = FindViewById<Button>(Resource.Id.mybutton);

            button.Click += delegate
            {
                SendData();
            };
            


            SetAmbientEnabled();
        }

        public void SendData()
        {

            _dataManager.SendData();

            //try
            //{
            //    var request = PutDataMapRequest.Create(_syncPath);
            //    var map = request.DataMap;
            //    map.PutString("Message", "Petter says Hello from Wearable!");
            //    map.PutLong("UpdatedAt", DateTime.UtcNow.Ticks);
            //    WearableClass.DataApi.PutDataItem(_client, request.AsPutDataRequest());
            //}
            //finally
            //{
            //    _dataManager.Disconnect();
            //}
        }

        protected override void OnStart()
        {
            base.OnStart();
            _dataManager.Connect();
        }

        

    }

    public class HHDataManager : GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener//, IDataApiDataListener
    {
        public GoogleApiClient client;
        private string _syncPath;
        public HHDataManager(Context context, string syncPath)
        {
            this._syncPath = syncPath;


            client = new GoogleApiClient.Builder(context)
                .AddApi(p0: WearableClass.API)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .Build();

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IntPtr Handle { get; }
        public void OnConnected(Bundle connectionHint)
        {
            //WearableClass.DataApi.AddListener(client, this);
            //PutDataMapRequest putDataMapRequest = ev.ToPutDataMapRequest();
            //await WearableClass.DataApi.PutDataItemAsync(client,
            //    putDataMapRequest.AsPutDataRequest());
        }

        public void OnConnectionSuspended(int cause)
        {
            Android.Util.Log.Error("HeartManager_Error", "GMSonnection suspended " + cause);
            WearableClass.DataApi.RemoveListener(client, this);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Android.Util.Log.Error("HeartManager_Error", "GMSonnection failed " + result.ErrorCode);
        }

        public void Connect()
        {
            client.Connect();
        }

        public void Disconnect()
        {
            client.Disconnect();
        }


        public void OnDataChanged(DataEventBuffer dataEvents)
        {
            var dataEvent = Enumerable.Range(0, dataEvents.Count)
                .Select(i => dataEvents.Get(i).JavaCast<IDataEvent>())
                .FirstOrDefault(x => x.Type == DataEvent.TypeChanged && x.DataItem.Uri.Path.Equals(_syncPath));
            if (dataEvent == null)
            {
                return;
            }

            //do stuffs here 
        }


        public void SendData(List<string> data)
        {
            
        }
    }
}


