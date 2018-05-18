package com.UQAC.OceanEmpire;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.location.ActivityRecognition;
import com.unity3d.player.*;
import android.app.Activity;
import android.app.PendingIntent;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.graphics.PixelFormat;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.security.keystore.KeyGenParameterSpec;
import android.security.keystore.KeyProperties;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.util.Base64;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.security.KeyStore;
import java.security.PrivateKey;
import java.util.Calendar;
import java.util.Enumeration;

import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;

public class UnityPlayerActivity extends Activity implements GoogleApiClient.ConnectionCallbacks, GoogleApiClient.OnConnectionFailedListener
{
    protected UnityPlayer mUnityPlayer = null; // don't change the name of this variable; referenced from native code

    static public UnityPlayerActivity myInstance = null;

    public GoogleApiClient mApiClient;

    public SecretKey key;

    public KeyStore ks;

    public char[] password = "1234567890".toCharArray();

    public void SendMessageToUnity(String msg){
        Log.e("ANDROID-UNITY","SENDING MSG TO UNITY");
        mUnityPlayer.UnitySendMessage("GoogleActivities(Clone)", "ReceiveAndroidMessage", msg);
    }

    void GenerateKey(){
        try {
            // Get and Convert the Key
            key = KeyGenerator.getInstance("AES").generateKey();

            SaveKey();
        }
        catch(Exception ex){
            ex.printStackTrace();
        }
    }

    void SaveKey(){
        try{
            // Save my secret key
            KeyStore.SecretKeyEntry secretKeyEntry = new KeyStore.SecretKeyEntry(key);
            ks.setEntry("SecretKeyAlias", secretKeyEntry,null);

            // Save the keystore
            FileOutputStream fos = new FileOutputStream(this.getFilesDir().getAbsolutePath() + "/OEKeyStore");
            ks.store(fos, password);
        }
        catch(Exception ex){
            ex.printStackTrace();
        }
    }

    void LoadKey(){
        try{
            // Load Keystore
            FileInputStream fis = new FileInputStream(this.getFilesDir().getAbsolutePath() + "/OEKeyStore");
            ks.load(fis, password);

            // Load the secret key
            KeyStore.SecretKeyEntry secretKeyEntry = (KeyStore.SecretKeyEntry)ks.getEntry("SecretKeyAlias",null);
            key = secretKeyEntry.getSecretKey();
        }
        catch(Exception ex){
            ex.printStackTrace();
        }
    }

    // Setup activity layout
    @Override protected void onCreate (Bundle savedInstanceState)
    {
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        super.onCreate(savedInstanceState);

        getWindow().setFormat(PixelFormat.RGBX_8888); // <--- This makes xperia play happy

        try{
            // Get Keystore
            ks = KeyStore.getInstance(KeyStore.getDefaultType());

            SharedPreferences wmbPreference = PreferenceManager.getDefaultSharedPreferences(this);
            boolean isFirstRun = wmbPreference.getBoolean("FIRSTRUN", true);
            if (isFirstRun)
            {
                String dir = getFilesDir().getAbsolutePath();
                File fileToDelete = new File(dir, "activities.txt");
                boolean successful = fileToDelete.delete();
                SharedPreferences.Editor editor = wmbPreference.edit();
                editor.putBoolean("FIRSTRUN", false);
                editor.commit();

                ks.load(null, password);
                GenerateKey();
            } else {
                LoadKey();
            }
        }
        catch(Exception ex){
            ex.printStackTrace();
        }

        mUnityPlayer = new UnityPlayer(this);
        setContentView(mUnityPlayer);
        mUnityPlayer.requestFocus();

        mApiClient = new GoogleApiClient.Builder(this)
                .addApi(ActivityRecognition.API)
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
                .build();

        ActivityDetection.secretKey = key;
        Log.e("KEY : ",Base64.encodeToString(key.getEncoded(),Base64.DEFAULT));
        SendMessageToUnity(Base64.encodeToString(key.getEncoded(),Base64.DEFAULT));

        mApiClient.connect();

        myInstance = this;
    }

    @Override protected void onNewIntent(Intent intent)
    {
        // To support deep linking, we need to make sure that the client can get access to
        // the last sent intent. The clients access this through a JNI api that allows them
        // to get the intent set on launch. To update that after launch we have to manually
        // replace the intent with the one caught here.
        setIntent(intent);
    }

    // Quit Unity
    @Override protected void onDestroy ()
    {
        mUnityPlayer.quit();
        myInstance = null;
        super.onDestroy();
    }

    // Pause Unity
    @Override protected void onPause()
    {
        super.onPause();
        mUnityPlayer.pause();
    }

    // Resume Unity
    @Override protected void onResume()
    {
        super.onResume();
        mUnityPlayer.resume();
    }

    // Low Memory Unity
    @Override public void onLowMemory()
    {
        super.onLowMemory();
        mUnityPlayer.lowMemory();
    }

    // Trim Memory Unity
    @Override public void onTrimMemory(int level)
    {
        super.onTrimMemory(level);
        if (level == TRIM_MEMORY_RUNNING_CRITICAL)
        {
            mUnityPlayer.lowMemory();
        }
    }

    // This ensures the layout will be correct.
    @Override public void onConfigurationChanged(Configuration newConfig)
    {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    // Notify Unity of the focus change.
    @Override public void onWindowFocusChanged(boolean hasFocus)
    {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override public boolean dispatchKeyEvent(KeyEvent event)
    {
        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }
    /*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }

    @Override
    public void onConnected(@Nullable Bundle bundle) {
        Intent intent = new Intent( this, ActivityDetection.class );
        PendingIntent pendingIntent = PendingIntent.getService( this, 0, intent, PendingIntent.FLAG_UPDATE_CURRENT );
        ActivityRecognition.ActivityRecognitionApi.requestActivityUpdates( mApiClient, 3000, pendingIntent );

    }

    @Override
    public void onConnectionSuspended(int i) {

    }

    @Override
    public void onConnectionFailed(@NonNull ConnectionResult connectionResult) {

    }
}
